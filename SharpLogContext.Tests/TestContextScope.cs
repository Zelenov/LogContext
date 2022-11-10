using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

namespace SharpLogContext.Tests
{
    [TestFixture]
    public class TestContextScope
    {
        [SetUp]
        public void SetUp()
        {
            LogContext.Release();
        }

        [Test]
        public void CreateScope_ReturnsInitialValues()
        {
            var context = LogContext.Initialize();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);
            var pair2A = new KeyValuePair<string, object>("2", 20);
            context.Add(pair1);
            context.Add(pair2);
            using (context.CreateScope(pair2A))
            {
                CollectionAssert.AreEquivalent(new []{pair1, pair2A}, context.GetValues());
            }
            CollectionAssert.AreEquivalent(new[] { pair1, pair2 }, context.GetValues());
        }

        [Test]
        public void CreateScope_RemoveInsideScope_ReturnsAllValues()
        {
            var context = LogContext.Initialize();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);
            var pair2A = new KeyValuePair<string, object>("2", 20);
            context.Add(pair1);
            context.Add(pair2);
            using (var scope = context.CreateScope(pair2A))
            {
                scope.Remove(pair2.Key);
                CollectionAssert.AreEquivalent(new[] { pair1, pair2 }, context.GetValues());
            }
            CollectionAssert.AreEquivalent(new[] { pair1, pair2 }, context.GetValues());
        }

        [Test]
        public void CreateScope_RemoveOutsideScope_ReturnsAllValues()
        {
            var context = LogContext.Initialize();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);
            var pair2A = new KeyValuePair<string, object>("2", 20);
            context.Add(pair1);
            context.Add(pair2);
            using (context.CreateScope(pair2A))
            {
                context.Remove(pair2.Key);
                CollectionAssert.AreEquivalent(new[] { pair1, pair2A }, context.GetValues());
            }
            CollectionAssert.AreEquivalent(new[] { pair1 }, context.GetValues());
        }

        [Test]
        public void CreateScope_RemoveAll_RemovesAllValues()
        {
            var context = LogContext.Initialize();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);
            var pair2A = new KeyValuePair<string, object>("2", 20);
            context.Add(pair1);
            context.Add(pair2);
            using (var scope = context.CreateScope(pair2A))
            {
                scope.Remove(pair2.Key);
                context.Remove(pair2.Key);
                CollectionAssert.AreEquivalent(new[] { pair1 }, context.GetValues());
            }
            CollectionAssert.AreEquivalent(new[] { pair1 }, context.GetValues());
        }

        [Test]
        public void CreateScope_AttachInsideScope_ReturnsValues()
        {
            var context = LogContext.Initialize();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);
            var pair3 = new KeyValuePair<string, object>("3", 3);
            context.Add(pair1);
            using (context.CreateScope(pair2))
            {
                context.Add(pair3);
                CollectionAssert.AreEquivalent(new[] {pair1, pair2, pair3}, context.GetValues());
            }
            CollectionAssert.AreEquivalent(new[] { pair1, pair3 }, context.GetValues());
        }

        [Test]
        public async Task CreateScope_MultipleTasks_DoNotClash()
        {
            var context = LogContext.Initialize();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);
            context.Add(pair1);

            var barrier = new Barrier(3);

            async Task Task1()
            {
                var pair3 = new KeyValuePair<string, object>("3", 3);
                await Task.Yield();

                using (context.CreateScope(pair3))
                {
                    barrier.SignalAndWait();
                    CollectionAssert.AreEquivalent(new[] { pair1, pair2, pair3 }, context.GetValues());
                }
            }

            async Task Task2()
            {
                var pair4 = new KeyValuePair<string, object>("4", 4);
                await Task.Yield();

                using (context.CreateScope(pair4))
                {
                    barrier.SignalAndWait();
                    CollectionAssert.AreEquivalent(new[] { pair1, pair2, pair4 }, context.GetValues());
                }
            }

            async Task Task3()
            {
                await Task.Yield();
                context.Add(pair2);
                barrier.SignalAndWait();
            }

            var tasks = new[] { Task1(), Task2(), Task3() };
            await Task.WhenAll(tasks);

            CollectionAssert.AreEquivalent(new[] { pair1, pair2 }, context.GetValues());
        }

        [Test]
        public async Task AsyncLocal_WorksAsExpected()
        {
            var a = new AsyncLocal<int>
            {
                Value = 0,
            };


            var barrier = new Barrier(2);
            async Task Task1()
            {
                await Task.Yield();
                a.Value = 1;
                barrier.SignalAndWait();
                Assert.AreEqual(1, a.Value);
            }

            async Task Task2()
            {
                await Task.Yield();
                a.Value = 2;
                barrier.SignalAndWait();
                Assert.AreEqual(2, a.Value);
            }
            

            var tasks = new[] { Task1(), Task2() };
            await Task.WhenAll(tasks);

            Assert.AreEqual(0, a.Value);
        }
    }
}