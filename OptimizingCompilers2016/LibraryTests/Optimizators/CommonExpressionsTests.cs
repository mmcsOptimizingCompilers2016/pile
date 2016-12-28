using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptimizingCompilers2016.Library.Optimizators;
using OptimizingCompilers2016.Library;
using OptimizingCompilers2016.Library.LinearCode;
using OptimizingCompilers2016.Library.Visitors;
using System.Collections.Generic;
using OptimizingCompilers2016.Library.ThreeAddressCode.Values;

namespace LibraryTests.Optimizators
{
    
    [TestClass]
    public class CommonExpressionsTests
    {
        static readonly string text0 =
          @"a = a + 3;
            c = a + b;
            d = a + b;
            b = a + 3;
            e = a + b;
            e = a + 3;
            gb1 = a * 3;
            gb2 = a - 3;
           ";
        static readonly string result0 =
          @"a = a + 3;
            v_0 = a + b;
            c = v_0;
            d = v_0;
            v_1 = a + 3;
            b = v_1;
            e = a + b;
            e = v_1;
            gb1 = a * 3;
            gb2 = a - 3;
           ";

        static readonly string text1 =
          @"a = 2 * 3;
            b = 3 * 2;
            a = b / c;
            k = 3 * 2;
            d = c / b;
           ";
        static readonly string result1 =
          @"v0 = 2 * 3;
            a = v0;
            b = v0;
            a = b / c;
            k = v0;
            d = c / b;
           ";
        private BaseBlock getBlock(string text)
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

            return cfg.GetRoot();
        }

        void checkValues(IValue v0, IValue v1, ref Dictionary<IValue, IValue> map)
        {
            if (v0 == null || v1 == null)
            {
                Assert.AreEqual(v0, v1);
                return;
            }
            if (v0 is NumericValue || v1 is NumericValue)
            {
                Assert.AreEqual(v0 is NumericValue, v1 is NumericValue);
                Assert.AreEqual(v0 as NumericValue, v1 as NumericValue);
                return;
            }
            // map : v1 -> v0
            if (!map.ContainsKey(v1))
            {
                map.Add(v1, v0);
                return;
            }
            Assert.AreEqual(v0, map[v1]);
        }

        // used in InterBlock cse
        public void compareBBs(BaseBlock b1, BaseBlock b2, ref Dictionary<IValue, IValue> map)
        {
            Assert.AreEqual(b1.Commands.Count, b2.Commands.Count);
            for (int i = 0; i < b1.Commands.Count; ++i)
            {
                checkValues(b1.Commands[i].Label, b2.Commands[i].Label, ref map);
                checkValues(b1.Commands[i].Destination, b2.Commands[i].Destination, ref map);
                checkValues(b1.Commands[i].LeftOperand, b2.Commands[i].LeftOperand, ref map);
                Assert.AreEqual(b1.Commands[i].Operation, b2.Commands[i].Operation);
                checkValues(b1.Commands[i].RightOperand, b2.Commands[i].RightOperand, ref map);
            }
        }

        private void commonTestTemplate(string text, string result)
        {
            var b1 = getBlock(text);
            CommonExpressions exp = new CommonExpressions();
            Assert.IsTrue(exp.Optimize(b1));

            var b2 = getBlock(result);
            var map = new Dictionary<IValue, IValue>();
            compareBBs(b1, b2, ref map);
        }

        [TestMethod]
        public void cseTest0()
        {
            commonTestTemplate(text0, result0);
        }

        [TestMethod]
        public void cseTest1()
        {
            commonTestTemplate(text1, result1);
        }
    }
}
