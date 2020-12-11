using MassTransit;

namespace SharpLogContext.MassTransit
{
    public static class BusFactoryConfiguratorExtensions
    {
        public static IBusFactoryConfigurator AddLogContext(this IBusFactoryConfigurator busControl)
        {
            var observer = new LogContextObserver();
            busControl.ConnectReceiveObserver(observer);
            return busControl;
        }
    }
}