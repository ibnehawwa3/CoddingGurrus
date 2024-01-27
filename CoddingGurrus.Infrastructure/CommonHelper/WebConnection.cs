using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Infrastructure.CommonHelper
{
    internal class WebConnection : WebClient
    {
        internal int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri Address)
        {
            WebRequest WebReq = base.GetWebRequest(Address);
            WebReq.Timeout = Timeout * 1000; // Seconds

            return WebReq;
        }
    }
}
