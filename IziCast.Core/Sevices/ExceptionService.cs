using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using System.Linq;
using IziCast.Core.Sevices.Interfaces;

namespace IziCast.Core.Sevices
{
    public class ExceptionService : IExceptionService
    {
        public void TrackException(Exception exception, IDictionary<string, string> details = null)
        {
            if (details == null || !details.Any())
                details = null;

            Crashes.TrackError(exception, details);
        }
    }
}
