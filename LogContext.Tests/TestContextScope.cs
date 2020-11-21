using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using NUnit.Framework;

namespace LogContext.Tests
{
    [TestFixture]
    public class TestContextScope
    {
        [SetUp]
        public void SetUp()
        {
            Randomizer.Seed = TestContext.CurrentContext.Random;
        }

        [Test]
        public void CreateScope_ReturnsInitialValues()
        {
            var context = LogContext.CreateNewLogContext();
            var faker = new Faker();
            var pair1 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));
            var pair2 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));
            var pair2A = new KeyValuePair<string, object>(pair2.Key, faker.Random.Number(100));
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
            var faker = new Faker();
            var pair1 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));
            var pair2 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));
            var pair2A = new KeyValuePair<string, object>(pair2.Key, faker.Random.Number(100));
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
            var faker = new Faker();
            var pair1 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));
            var pair2 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));
            var pair3 = new KeyValuePair<string, object>(faker.Lorem.Word(), faker.Random.Number(100));
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