using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MassTransit.Testing;
using NUnit.Framework;

namespace SharpLogContext.MassTransit.Tests
{
    [TestFixture]
    [SingleThreaded]
    public class TestMassTransit
    {
        public static KeyValuePair<string, object> Pair1 = new KeyValuePair<string, object>("1", 1);
        public static KeyValuePair<string, object> Pair2 = new KeyValuePair<string, object>("2", 2);
        [Test]
        public async Task MassTransit_Consume_WithoutLogContext_Returns1Pair()
        {
            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer<ExpectedContextConsumer>();
            await harness.Start();
            try
            {
                var e = new ManualResetEvent(false);
                await harness.InputQueueSendEndpoint.Send<IExpectedContext>(new
                {
                    ExpectedValues = new[] { Pair2 },
                    EventIndex = 0
                });
                Assert.IsTrue(Events[0].WaitOne(2000));

            }
            finally
            {
                await harness.Stop();
            }
        }

        public static ManualResetEvent[] Events =
            new[] {new ManualResetEvent(false), new ManualResetEvent(false)};
        [Test]
        public async Task MassTransit_Consume_WithLogContext_Returns2Pairs()
        {
            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer<ExpectedContextConsumer>();
            await harness.Start();  
            harness.BusControl.AddLogContext();
            try
            {
                await harness.InputQueueSendEndpoint.Send<IExpectedContext>(new
                {
                    ExpectedValues = new[] { Pair1, Pair2 },
                    EventIndex = 1
                });
                Assert.IsTrue(Events[1].WaitOne(2000));

            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}