using System;
using System.Collections.Generic;

namespace IziCast.Core.Sevices.Interfaces
{
    public interface IExceptionService
    {
        void TrackException(Exception exception, IDictionary<string, string> details = null);
    }
}
