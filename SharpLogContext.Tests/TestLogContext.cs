using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharpLogContext.Tests
{
    [TestFixture]
    [SingleThreaded]
    public class TestLogContext
    {
        [SetUp]
        public void SetUp()
        {
            LogContext.Release();
        }

        [Test]
        public async Task Add_AsyncTask_ReturnsInitial()
        {
            LogContext.Initialize();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);

            async Task Foo()
            {
                await Task.CompletedTask;
                LogContext.Current.Add(pair2);
                await Task.CompletedTask;
            }

            LogContext.Current.Add(pair1);
            await Foo();
            var actualValues = LogContext.Current.GetValues();
            var expectedValues = new[] {pair1, pair2};
            CollectionAssert.AreEquivalent(expectedValues, actualValues);
        }

        [Test]
        public async Task Add_AsyncTaskOuterScope_DoesntGoToOuterScope()
        {
            LogContext.Release();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);

            async Task Foo()
            {
                await Task.CompletedTask;
                LogContext.Current.Add(pair2);
                await Task.CompletedTask;
            }

            await Foo();
            LogContext.Current.Add(pair1);
            var actualValues = LogContext.Current.GetValues();
            var expectedValues = new[] {pair1};
            CollectionAssert.AreEquivalent(expectedValues, actualValues);
        }

        [Test]
        public void Add_AsyncTaskLevels_DoesntGoToOuterScope()
        {
            LogContext.Release();
            var pair1 = new KeyValuePair<string, object>("1", 1);

            async Task Thing1()
            {
                await Thing2();
                CollectionAssert.AreEquivalent(Array.Empty<KeyValuePair<string, object>>(), LogContext.Current.GetValues());
            }

            async Task Thing2()
            {
                SetValue();
                CollectionAssert.AreEquivalent(new[] {pair1}, LogContext.Current.GetValues());
                await Task.CompletedTask;
            }

            void SetValue()
            {
                LogContext.Current.Add(pair1);
            }

            Thing1().GetAwaiter().GetResult();

        }

        [Test]
        public void Add_AsyncVoid_ReturnsInitial()
        {
            LogContext.Initialize();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);

            async void Foo()
            {
                await Task.CompletedTask;
                LogContext.Current.Add(pair2);
                await Task.CompletedTask;
            }

            LogContext.Current.Add(pair1);
            Foo();
            var actualValues = LogContext.Current.GetValues();
            var expectedValues = new[] {pair1, pair2};
            CollectionAssert.AreEquivalent(expectedValues, actualValues);
        }

        [Test]
        public void Add_SameContext_ReturnsInitial()
        {
            var context = LogContext.Initialize();
            var pair = new KeyValuePair<string, object>("1", 1);
            context.Add(pair);
            var actualValues = context.GetValues();
            var expectedValues = new[] {pair};
            Assert.AreEqual(expectedValues, actualValues);
        }

        [Test]
        public void Add_Thread_ReturnsInitial()
        {
            LogContext.Initialize();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);

            var e = new ManualResetEvent(false);
            var threadId1 = Thread.CurrentThread.ManagedThreadId;
            var threadId2 = 0;
            var thread = new Thread(() =>
            {
                threadId2 = Thread.CurrentThread.ManagedThreadId;
                LogContext.Current.Add(pair2);
                e.Set();
            });
            LogContext.Current.Add(pair1);
            thread.Start();
            e.WaitOne();
            var actualValues = LogContext.Current.GetValues();
            var expectedValues = new[] {pair1, pair2};
            CollectionAssert.AreEquivalent(expectedValues, actualValues);
            Assert.AreNotEqual(threadId1, threadId2);
        }

        [Test]
        public async Task Add_ThreadPool_ReturnsInitial()
        {
            LogContext.Initialize();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);

            void Foo()
            {
                LogContext.Current.Add(pair2);
            }

            LogContext.Current.Add(pair1);
            await Task.Factory.StartNew(Foo);
            var actualValues = LogContext.Current.GetValues();
            var expectedValues = new[] {pair1, pair2};
            CollectionAssert.AreEquivalent(expectedValues, actualValues);
        }
    }
}