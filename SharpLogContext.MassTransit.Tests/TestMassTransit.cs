using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Testing;
using NUnit.Framework;

namespace SharpLogContext.MassTransit.Tests
{
    [TestFixture]
    [SingleThreaded]
    public class TestSqlBuilder
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
                await harness.InputQueueSendEndpoint.Send<IExpectedContext>(new
                {
                    ExpectedValues = new[] {Pair2}
                });

                Assert.That(await consumerHarness.Consumed.Any<IExpectedContext>());

            }
            finally
            {
                await harness.Stop();
            }
        } 
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
                    ExpectedValues = new[] {Pair1, Pair2}
                });

                Assert.That(await consumerHarness.Consumed.Any<IExpectedContext>());

            }
            finally
            {
                await harness.Stop();
            }
        }
    }

    public interface IExpectedContext
    {
        KeyValuePair<string, object>[] ExpectedValues { get; set; }
    }


    class ExpectedContextConsumer :
        IConsumer<IExpectedContext>
    {

        public async Task Foo()
        {
            LogContext.Current.AttachValue(TestSqlBuilder.Pair1);
            await Task.CompletedTask;
        }
        public async Task Consume(ConsumeContext<IExpectedContext> context)
        {
            await Foo();
            LogContext.Current.AttachValue(TestSqlBuilder.Pair2);
            var actualValues = LogContext.Current.GetValues();
            var expectedValues = context.Message.ExpectedValues;
            CollectionAssert.AreEquivalent(expectedValues, actualValues);
        }
    }

}