using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSFoundation.Model.Schedule.Enumerations
{
    public enum ScheduleTypes
    {
        None,
        Block,
        BlockLive,
        BlockReview,
        BlockArchive,
        JobExecution,
        Notice,
        LimitedAccess,
        Maintenance,
        Offline,
        Online
    }
}
