using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Uas.BaseStation.ControlApp
{
    public class UwpAppConfig : IAppConfig
    {

        public UwpAppConfig()
        {
            WebAddress = "https://www.IoTAppStudio.com";
        }

        public PlatformTypes PlatformType { get => PlatformTypes.WindowsUWP; }

        public Environments Environment { get; set; }

        public string WebAddress { get; set; }

        public string AppName => "UAV Base Station";

        public string AppLogo => "appicon.png";

        public string CompanyLogo => "slsys.png";
#if DEBUG
        public bool EmitTestingCode => true;
#else
        public bool EmitTestingCode => true;
#endif

        public string AppId => "BCD0A4DF7F8449E693DA66A1205ADDB4";
        public string ClientType => "mobileapp";

        public VersionInfo Version { get; set; }

        public string CompanyName => "Software Logistics, LLC";

        public string CompanySiteLink => "https://support.nuviot.com/help.html#/information/company";

        public string AppDescription => "IoT Device Manager is a free app provided by Software Logistics, LLC that will all you to manage and monitor devices from device repositores created with IoT App Studio.";

        public string TermsAndConditionsLink => "https://app.termly.io/document/terms-of-use-for-saas/90eaf71a-610a-435e-95b1-c94b808f8aca";

        public string PrivacyStatementLink => "https://app.termly.io/document/privacy-policy-for-website/f0b67cde-2a08-4fe8-a35e-5e4571545d00";

        public AuthTypes AuthType => AuthTypes.User;

        public string APIToken => "";

        public string InstanceId { get; set; }
        public string InstanceAuthKey { get; set; }
        public string DeviceId { get; set; }
        public string DeviceRepoId { get; set; }
    }
}

