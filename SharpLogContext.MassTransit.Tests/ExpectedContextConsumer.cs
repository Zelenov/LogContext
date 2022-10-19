using System.Threading.Tasks;

using MassTransit;

using NUnit.Framework;

namespace SharpLogContext.MassTransit.Tests
{
    class ExpectedContextConsumer :
        IConsumer<IExpectedContext>
    {
        public async Task Foo()
        {
            LogContext.Current.Add(TestMassTransit.Pair1);
            await Task.CompletedTask;
        }
        public async Task Consume(ConsumeContext<IExpectedContext> context)
        {
            await Foo();
            LogContext.Current.Add(TestMassTransit.Pair2);
            var actualValues = LogContext.Current.GetValues();
            var expectedValues = context.Message.ExpectedValues;
            CollectionAssert.AreEquivalent(expectedValues, actualValues);
            var e = TestMassTransit.Events[context.Message.EventIndex];
            e.Set();
        }
    }
}