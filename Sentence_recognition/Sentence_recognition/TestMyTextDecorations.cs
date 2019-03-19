using System;
using CommonLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace LibTests
{
    [TestClass]
    public class TestMyTextDecorations
    {
        [TestMethod]
        [Description("Test incorrect argument. How code will react when we pass 0 into it." +
            "Expects null result.")]
        public void InccorectTestGetDecorationFromType1()
        {
            Assert.IsNull(MyTextDecorations.GetDecorationFromType(0));
        }

        [TestMethod]
        [Description("Test incorrect argument. How code will react " +
            "when we pass random number into it." +
            "Expects null result.")]
        public void InccorectTestGetDecorationFromType2()
        {
            Assert.IsNull(MyTextDecorations.GetDecorationFromType((SentenceMembers)9));
        }

        [TestMethod]
        [Description("Test incorrect argument. How code will react " +
            "when we pass double flag into it." +
            "Expects null result.")]
        public void InccorectTestGetDecorationFromType3()
        {
            Assert.IsNull(MyTextDecorations
                .GetDecorationFromType(SentenceMembers.Addition|SentenceMembers.Circumstance));
        }

        [TestMethod]
        [Description("No surprise test. Doesn't test correct behavior. " +
            "Expects not null result.")]
        public void TestGetDecorationFromType4()
        {
            Assert.IsNotNull(MyTextDecorations
                .GetDecorationFromType(SentenceMembers.Addition));
            Assert.IsNotNull(MyTextDecorations
                .GetDecorationFromType(SentenceMembers.Circumstance));
            Assert.IsNotNull(MyTextDecorations
                .GetDecorationFromType(SentenceMembers.Definition));
            Assert.IsNotNull(MyTextDecorations
                .GetDecorationFromType(SentenceMembers.Predicate));
            Assert.IsNotNull(MyTextDecorations
                .GetDecorationFromType(SentenceMembers.Subject));
        }
    }
}
