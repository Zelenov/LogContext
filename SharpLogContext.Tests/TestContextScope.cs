using System.Collections.Generic;
using NUnit.Framework;

namespace SharpLogContext.Tests
{
    [TestFixture]
    public class TestContextScope
    {

        [Test]
        public void CreateScope_ReturnsInitialValues()
        {
            var context = LogContext.CreateNewLogContext();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);
            var pair2A = new KeyValuePair<string, object>("2", 20);
            context.AttachValue(pair1);
            context.AttachValue(pair2);
            using (context.CreateScope(pair2A))
            {
                CollectionAssert.AreEquivalent(new []{pair1, pair2A}, context.GetValues());
            }
            CollectionAssert.AreEquivalent(new[] { pair1, pair2 }, context.GetValues());
        }
        [Test]
        public void CreateScope_RemoveInsideScope_ReturnsInitialValues()
        {
            var context = LogContext.CreateNewLogContext();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);
            var pair2A = new KeyValuePair<string, object>("2", 20);
            context.AttachValue(pair1);
            context.AttachValue(pair2);
            using (context.CreateScope(pair2A))
            {
                context.RemoveKey(pair2.Key);
                CollectionAssert.AreEquivalent(new []{pair1}, context.GetValues());
            }
            CollectionAssert.AreEquivalent(new[] { pair1, pair2 }, context.GetValues());
        }
        [Test]
        public void CreateScope_AttachInsideScope_ReturnsInitialValues()
        {
            var context = LogContext.CreateNewLogContext();
            var pair1 = new KeyValuePair<string, object>("1", 1);
            var pair2 = new KeyValuePair<string, object>("2", 2);
            var pair3 = new KeyValuePair<string, object>("3", 3);
            context.AttachValue(pair1);
            using (context.CreateScope(pair2))
            {
                context.AttachValue(pair3);
                CollectionAssert.AreEquivalent(new[] {pair1, pair2, pair3}, context.GetValues());
            }
            CollectionAssert.AreEquivalent(new[] { pair1, pair3 }, context.GetValues());
        }

    }
}