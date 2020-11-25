using MassTransit;

namespace SharpLogContext.MassTransit
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
