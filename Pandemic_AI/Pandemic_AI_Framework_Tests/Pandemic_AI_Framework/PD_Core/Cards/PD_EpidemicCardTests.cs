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
    public class PD_EpidemicCardTests
    {
        [TestMethod()]
        public void EqualityWithSelf_Test()
        {
            PD_EpidemicCard epidemicCard =
                new PD_EpidemicCard(
                    0
                    );

            Assert.IsTrue(epidemicCard.Equals(epidemicCard) == true);
        }

        //[TestMethod()]
        //public void ComparisonWithSelf_Test()
        //{
        //    PD_EpidemicCard epidemicCard =
        //        new PD_EpidemicCard(
        //            0
        //            );

        //    Assert.IsTrue(epidemicCard == epidemicCard);
        //}

        [TestMethod()]
        public void EqualityWithSame_Test()
        {
            PD_EpidemicCard epidemicCard1a =
                new PD_EpidemicCard(
                    0
                    );

            PD_EpidemicCard epidemicCard1b =
                new PD_EpidemicCard(
                    0
                    );

            Assert.IsTrue(epidemicCard1a.Equals(epidemicCard1b) == true);
        }

        [TestMethod()]
        public void ComparisonWithSame_Test()
        {
            PD_EpidemicCard epidemicCard1a =
                new PD_EpidemicCard(
                    0
                    );

            PD_EpidemicCard epidemicCard1b =
                new PD_EpidemicCard(
                    0
                    );

            Assert.IsTrue(epidemicCard1a == epidemicCard1b);
        }

        [TestMethod()]
        public void EqualityWithOther_Test()
        {
            PD_EpidemicCard epidemicCard1 =
                new PD_EpidemicCard(
                    0
                    );

            PD_EpidemicCard epidemicCard2 =
                new PD_EpidemicCard(
                    1
                    );

            Assert.IsTrue(epidemicCard1.Equals(epidemicCard2) == false);
        }

        [TestMethod()]
        public void ComparisonWithOther_Test()
        {
            PD_EpidemicCard epidemicCard1 =
                new PD_EpidemicCard(
                    0
                    );

            PD_EpidemicCard epidemicCard2 =
                new PD_EpidemicCard(
                    1
                    );

            Assert.IsTrue(epidemicCard1 != epidemicCard2);
        }
    }
}