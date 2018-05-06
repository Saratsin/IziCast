using System;
using System.Collections.Generic;

namespace IziCast.Core.Services.Interfaces
{
    public interface IExceptionService
    {
        void TrackException(Exception exception, IDictionary<string, string> details = null);
    }
}
