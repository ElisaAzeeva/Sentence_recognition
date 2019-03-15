using CommonLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace LibTests
{
    [TestClass]
    public class RecognitionAPITests
    {
        


        [TestMethod]
        public void GetRunsTest1()
        {
            var sentences = new List<string>
            {
                "abcd",
                "abcd"
            };

            var token = new List<Token>
            {
                new Token(0,1,1,SentenceMembers.Addition)
            };

            var result = RecognitionAPI.GetRuns(new Data(sentences, token),  SentenceMembers.Addition)
                .Select(r => r.Text).ToList();

            List<string> expected = new List<string> {
                "a", "b", "cdabcd"
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetRunsTest2()
        {
            var sentences = new List<string>
            {
                "abcd",
                "abcd"
            };

            var token = new List<Token>
            {
                new Token(0,1,1,SentenceMembers.Addition),
                new Token(1,1,1,SentenceMembers.Addition)
            };

            var result = RecognitionAPI.GetRuns(new Data(sentences, token), SentenceMembers.Addition)
                .Select(r => r.Text).ToList();

            List<string> expected = new List<string> {
                "a", "b", "cda", "b", "cd",
            };

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetRunsTest3()
        {
            var sentences = new List<string>
            {
                "abcd",
                "abcd",
                "abcd",
            };

            var token = new List<Token>
            {
                new Token(0,1,1,SentenceMembers.Addition),
                new Token(1,1,1,SentenceMembers.Addition),
                new Token(2,1,1,SentenceMembers.Addition)

            };

            var result = RecognitionAPI.GetRuns(new Data(sentences, token), SentenceMembers.Addition)
                .Select(r => r.Text).ToList();

            List<string> expected = new List<string> {
                "a", "b", "cda",
                "a", "b", "cda",
                "b", "cd",
            };

            CollectionAssert.AreEqual(expected, result);
        }

    }
}
