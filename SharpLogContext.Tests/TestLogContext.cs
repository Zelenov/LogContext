using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharpLogContext.Tests
{
    [TestFixture]
    [SingleThreaded]
    public class TestSqlBuilder
    {
        [Test]
        public async Task AttachValue_AsyncTask_ReturnsInitial()
        {
            LogContext.CreateNewLogContext();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);

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
        public async Task AttachValue_AsyncTaskOuterScope_DoesntGoToOuterScope()
        {
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);

            async Task Foo()
            {
                await Task.CompletedTask;
                LogContext.Current.AttachValue(pair2);
                await Task.CompletedTask;
            }

            await Foo();
            LogContext.Current.AttachValue(pair1);
            var actualValues = LogContext.Current.GetValues();
            var expectedValues = new[] {pair1};
            CollectionAssert.AreEquivalent(expectedValues, actualValues);
        }
        [Test]
        public void AttachValue_AsyncTaskLevels_DoesntGoToOuterScope()
        {
            var pair1 = new KeyValuePair<string, object>("1", 1);

            async Task Thing1()
            {
                await Thing2();
                CollectionAssert.AreEquivalent(new KeyValuePair<string, object>[0], LogContext.Current.GetValues());
            }

            async Task Thing2()
            {
                SetValue();
                CollectionAssert.AreEquivalent(new []{pair1}, LogContext.Current.GetValues());
                await Task.CompletedTask;
            }

            void SetValue()
            {
                LogContext.Current.AttachValue(pair1);
            }

            Thing1().GetAwaiter().GetResult();
        }

        [Test]
        public void AttachValue_AsyncVoid_ReturnsInitial()
        {
            LogContext.CreateNewLogContext();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);

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
            var pair = new KeyValuePair<string, object>("1", 1);
            context.AttachValue(pair);
            var actualValues = context.GetValues();
            var expectedValues = new[] {pair};
            Assert.AreEqual(expectedValues, actualValues);
        }

        [Test]
        public void AttachValue_Thread_ReturnsInitial()
        {
            LogContext.CreateNewLogContext();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);

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
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);

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