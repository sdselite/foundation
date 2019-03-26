using System;
using System.Collections.Generic;
using System.Text;

namespace SDSFoundation.Model.Schedule.NetStandard.Enumerations
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
