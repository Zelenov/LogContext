using MassTransit;

using SharpLogContext.MassTransit;

// ReSharper disable once CheckNamespace
namespace SharpLogContext
{
    public static class BusControlExtensions
    {
        public static IBusControl AddLogContext(this IBusControl busControl)
        {
            var observer = new LogContextObserver();
            busControl.ConnectReceiveObserver(observer);
            return busControl;
        }
    }
}
