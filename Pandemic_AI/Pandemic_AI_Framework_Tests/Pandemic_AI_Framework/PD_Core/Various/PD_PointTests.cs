using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pandemic_AI_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic_AI_Framework.Tests
{
    [TestClass()]
    public class PD_PointTests
    {
        [TestMethod()]
        public void EqualityWithSelf_Test()
        {
            PD_Point p = new PD_Point(0, 0);

            Assert.IsTrue(p.Equals(p) == true);
        }

        //[TestMethod()]
        //public void ComparisonWithSelf_Test()
        //{
        //    PD_Point p = new PD_Point(0, 0);

        //    Assert.IsTrue(p == p);
        //}

        [TestMethod()]
        public void EqualityWithSame_Test()
        {
            PD_Point p1a = new PD_Point(0, 0);
            PD_Point p1b = new PD_Point(0, 0);

            Assert.IsTrue(p1a.Equals(p1b) == true);
        }

        [TestMethod()]
        public void ComparisonWithSame_Test()
        {
            PD_Point p1a = new PD_Point(0, 0);
            PD_Point p1b = new PD_Point(0, 0);

            Assert.IsTrue(p1a == p1b);
        }

        [TestMethod()]
        public void EqualityWithOther_Test()
        {
            PD_Point p1 = new PD_Point(0, 0);
            PD_Point p2 = new PD_Point(1, 1);

            Assert.IsTrue(p1.Equals(p2) == false);
        }

        [TestMethod()]
        public void ComparisonWithOther_Test()
        {
            PD_Point p1 = new PD_Point(0, 0);
            PD_Point p2 = new PD_Point(1, 1);

            Assert.IsTrue(p1 != p2);
        }
    }
}