using System;
using System.Collections.Generic;
using System.Text;

namespace SDSFoundation.Model.Security.Policies
{
    public class PasswordPolicySettings
    {
        public bool DevicePasswordRequiresLowercase { get; set; }

        public bool DevicePasswordRequiresUppercase { get; set; }

        public bool DevicePasswordRequiresNumeric { get; set; }

        public bool DevicePasswordRequiresSpecial { get; set; }

        public int DevicePasswordMinimumLength { get; set; }
    }
}
