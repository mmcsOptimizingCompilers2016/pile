using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptimizingCompilers2016.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizingCompilers2016.Library.Tests
{
    [TestClass()]
    public class BaseBlockDividerTests
    {
        [TestMethod()]
        public void divideTest()
        {
            string s = "hello";
            Assert.AreEqual(s, "hello");
            //throw new NotImplementedException();
        }
    }
}