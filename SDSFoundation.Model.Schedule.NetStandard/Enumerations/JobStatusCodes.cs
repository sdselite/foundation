﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SDSFoundation.Model.Schedule.NetStandard.Enumerations
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
