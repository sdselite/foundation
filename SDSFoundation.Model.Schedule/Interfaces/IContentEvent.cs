using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSFoundation.Model.Schedule.Interfaces
{
    public interface IContentEvent
    {
        string Description { get; set; }
        DateTime? EndDate { get; set; }
        DateTime? StartDate { get; set; }
        int DeviceOrPeripheralNumber { get; set; }
        int ReasonCode { get; set; }
    }
}
