using System.Collections.Generic;

namespace SharpLogContext.MassTransit.Tests
{
    public interface IExpectedContext
    {
        KeyValuePair<string, object>[] ExpectedValues { get; set; }
        int EventIndex { get; set; }
    }
}