using System;
using System.Threading.Tasks;
using MassTransit;
namespace SharpLogContext.MassTransit
{
    internal class LogContextObserver : IReceiveObserver
    {
        public Task PreReceive(ReceiveContext context)
        {
            LogContext.CreateNewLogContext();
            return Task.CompletedTask;
        }

        public Task PostReceive(ReceiveContext context)
        {
            return Task.CompletedTask;
        }

        public Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType)
            where T : class
        {
            return Task.CompletedTask;
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan elapsed, string consumerType, Exception exception) where T : class
        {
            return Task.CompletedTask;
        }

        public Task ReceiveFault(ReceiveContext context, Exception exception)
        {
            return Task.CompletedTask;
        }
    }
}