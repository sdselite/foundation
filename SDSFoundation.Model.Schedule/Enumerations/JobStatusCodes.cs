using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSFoundation.Model.Schedule.Enumerations
{
    public enum JobStatusCodes
    {
        None = 0,
        Queued = 1,
        InProcess = 2,
        Completed = 3,
        Abended = 4,
        Failed = 5
    }
}
