using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharpLogContext.Tests
{
    [TestFixture]
    [SingleThreaded]
    public class TestInitialization
    {
        [SetUp]
        public void SetUp()
        {
            LogContext.Release();
        }

        [Test]
        public void Init_Add_Init_Add_Returns2()
        {
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);

            LogContext.Initialize();
            LogContext.Current.Add(pair1);

            LogContext.Initialize();
            LogContext.Current.Add(pair2);

            var actualValues = LogContext.Current.GetValues();
            var expectedValues = new[] {pair1, pair2};
            CollectionAssert.AreEquivalent(expectedValues, actualValues);
        }

        [Test]
        public void Add_Init_Add_Init_Add_Returns3()
        {
            Assert.False(LogContext.IsInitialized);
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);
            var pair3 = new KeyValuePair<string, object>("3", 3);

            LogContext.Current.Add(pair1);

            LogContext.Initialize();
            LogContext.Current.Add(pair2);

            LogContext.Initialize();
            LogContext.Current.Add(pair3);

            var actualValues = LogContext.Current.GetValues();
            var expectedValues = new[] { pair1, pair2, pair3 };
            CollectionAssert.AreEquivalent(expectedValues, actualValues);
        }
    }
}