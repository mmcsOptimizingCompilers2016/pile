using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryTests.Optimizators;
using OptimizingCompilers2016.Library.InterBlockOptimizators;
using OptimizingCompilers2016.Library;
using OptimizingCompilers2016.Library.Visitors;
using System.Linq;
using System.Collections.Generic;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;

namespace LibraryTests.InterBlockOptimizators
{
    [TestClass]
    public class CommonExpressionsTests
    {
        static readonly string text0 =
      @"
        a = 0 + k;
        b = 1 + k;
        e = 1 * k;
        if b > 0
        {
            c = 1 + k;
        }
        d = k + 1;
       ";
        static readonly string result0 =
          @"
            a = 0 + k;
            tmp = 1 + k;
            b = tmp;
            e = 1 * k;
            if b > 0
            {
                c = tmp;
            }
            d = tmp;
           ";

        static readonly string text1 =
      @"
	    d = a + 2;
	    e = 3 + a;
	    if e > 3
	    {
    		i = 0;
    	}
    	z = a + 3;
    	k = a + 2;
       ";
        static readonly string result1 =
      @"
        t0 = a + 2;
        d = t0;
        t1 = a + 3;
	    e = t1;
	    if e > 3
	    {
    		i = 0;
    	}
    	z = t1;
    	k = t0;
       ";

        static readonly string text2 =
      @"
        if a > 2
        {
            trash0 = a-b;
            i = b + a;
            trash1 = b-a;
        }
        else
        {
            trash0 = a-b;
            j = b + a;
            trash1 = b-a;
        }
        k = b + a;
       ";
        
        static readonly string result2 =
      @"
        if a > 2
        {
            trash0 = a-b;
            v0 = b + a;
            i = v0;
            trash1 = b-a;
        }
        else
        {
            trash0 = a-b;
            v0 = b + a;
            j = v0;
            trash1 = b-a;
        }
        k = v0;
       ";

        static readonly string text3 = @"
        if a > 2
        {
            
            i = b + a;
            if i
            {
                j = a;
            }
                else
            {
                j = b;
            }
        }
        else
        {
            j = b + a;
        }
        if a < 0
        {
            st = st - 1;
        }
        k = b + a;
       ";

        static readonly string result3 = @"
        if a > 2
        {
            v0 = b + a;
            i = v0;
            if i
            {
                j = a;
            }
                else
            {
                j = b;
            }
        }
        else
        {
            vo = b + a;
            j = v0;
        }
        if a < 0
        {
            st = st - 1;
        }
        k = v0;
       ";

        static readonly string text4 =
      @"
	    d = a / 2;
	    if e > 3
	    {
    		k = a / 2;
            a = 0;
    	}
        else
        {
            z = a / 2;
            e = 2 / a;
        }
        q = a / 2;
       ";
        static readonly string result4 =
      @"
        v0 = a / 2;
        d = v0;
	    if e > 3
	    {
    		k = v0;
            a = 0;
    	}
        else
        {
            z = v0;
            e = 2 / a;
        }
        q = a / 2;
       ";

        private ControlFlowGraph getCFG(string text)
        {
            string resultantText = "{ " + text + " }";
            Scanner scanner = new Scanner();
            scanner.SetSource(resultantText, 0);

            Parser parser = new Parser(scanner);
            var b = parser.Parse();
            Assert.IsTrue(b);

            var linearCode = new LinearCodeVisitor();
            parser.root.Accept(linearCode);
            var cfg = BaseBlockDivider.divide(linearCode.code);

            return cfg;
        }

        private void compareCFG(ControlFlowGraph g1, ControlFlowGraph g2)
        {
            var utils = new Optimizators.CommonExpressionsTests();
            var bbs1 = g1.GetVertices().ToList();
            var bbs2 = g2.GetVertices().ToList();
            Assert.AreEqual(bbs1.Count, bbs2.Count);
            var map = new Dictionary<IValue, IValue>();
            for (int i = 0; i < bbs1.Count; ++i)
            {
                utils.compareBBs(bbs1[i], bbs2[i], ref map);
            }
        }

        private void commonTestTemplate(string text, string result)
        {
            var cfg0 = getCFG(text);
            var cse = new CommonExpressions();
            cse.Optimize(cfg0);

            var cfg1 = getCFG(result);

            compareCFG(cfg0, cfg1);
        }

        [TestMethod]
        public void cseIBTest0()
        {
            commonTestTemplate(text0, result0);
        }
        [TestMethod]
        public void cseIBTest1()
        {
            commonTestTemplate(text1, result1);
        }
        [TestMethod]
        public void cseIBTest2()
        {
            commonTestTemplate(text2, result2);
        }
        [TestMethod]
        public void cseIBTest3()
        {
            commonTestTemplate(text3, result3);
        }
        [TestMethod]
        public void cseIBTest4()
        {
            commonTestTemplate(text4, result4);
        }
    }
}
