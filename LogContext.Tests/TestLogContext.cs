using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using NUnit.Framework;

namespace LogContext.Tests
{
    [TestFixture]
    [SingleThreaded]
    public class TestSqlBuilder
    {
        [SetUp]
        public void SetUp()
        {
            Randomizer.Seed = TestContext.CurrentContext.Random;
        }

        [Test]
        public async Task AttachValue_AsyncTask_ReturnsInitial()
        {
            LogContext.CreateNewLogContext();
            var faker = new Faker();
            var pair1 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));
            var pair2 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));

            async Task Foo()
            {
                await Task.CompletedTask;
                LogContext.Current.AttachValue(pair2);
                await Task.CompletedTask;
            }

            LogContext.Current.AttachValue(pair1);
            await Foo();
            var actualValues = LogContext.Current.GetValues();
            var expectedValues = new[] {pair1, pair2};
            CollectionAssert.AreEquivalent(expectedValues, actualValues);
        }

        [Test]
        public void AttachValue_AsyncVoid_ReturnsInitial()
        {
            LogContext.CreateNewLogContext();
            var faker = new Faker();
            var pair1 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));
            var pair2 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));

            async void Foo()
            {
                await Task.CompletedTask;
                LogContext.Current.AttachValue(pair2);
                await Task.CompletedTask;
            }

            LogContext.Current.AttachValue(pair1);
            Foo();
            var actualValues = LogContext.Current.GetValues();
            var expectedValues = new[] {pair1, pair2};
            CollectionAssert.AreEquivalent(expectedValues, actualValues);
        }

        [Test]
        public void AttachValue_SameContext_ReturnsInitial()
        {
            var context = LogContext.CreateNewLogContext();
            var faker = new Faker();
            var pair = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));
            context.AttachValue(pair);
            var actualValues = context.GetValues();
            var expectedValues = new[] {pair};
            Assert.AreEqual(expectedValues, actualValues);
        }

        [Test]
        public void AttachValue_Thread_ReturnsInitial()
        {
            LogContext.CreateNewLogContext();
            var faker = new Faker();
            var pair1 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));
            var pair2 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));

            var e = new ManualResetEvent(false);
            var threadId1 = Thread.CurrentThread.ManagedThreadId;
            var threadId2 = 0;
            var thread = new Thread(() =>
            {
                threadId2 = Thread.CurrentThread.ManagedThreadId;
                LogContext.Current.AttachValue(pair2);
                e.Set();
            });
            LogContext.Current.AttachValue(pair1);
            thread.Start();
            e.WaitOne();
            var actualValues = LogContext.Current.GetValues();
            var expectedValues = new[] {pair1, pair2};
            CollectionAssert.AreEquivalent(expectedValues, actualValues);
            Assert.AreNotEqual(threadId1, threadId2);
        }

        [Test]
        public async Task AttachValue_ThreadPool_ReturnsInitial()
        {
            LogContext.CreateNewLogContext();
            var faker = new Faker();
            var pair1 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));
            var pair2 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));

            void Foo()
            {
                LogContext.Current.AttachValue(pair2);
            }

            LogContext.Current.AttachValue(pair1);
            await Task.Factory.StartNew(Foo);
            var actualValues = LogContext.Current.GetValues();
            var expectedValues = new[] {pair1, pair2};
            CollectionAssert.AreEquivalent(expectedValues, actualValues);
        }
    }
}