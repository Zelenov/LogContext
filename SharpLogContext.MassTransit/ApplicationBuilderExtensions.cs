using System;
using System.Collections.Generic;
using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Builder;

namespace SharpLogContext.MassTransit
{
    public static class BusControlExtensions
    {
        public static IBusControl AddLogContext(this IBusControl busControl)
        {
            var observer = new ReceiveObserver();
            busControl.ConnectReceiveObserver(observer);
            return busControl;
        }
    }
}
