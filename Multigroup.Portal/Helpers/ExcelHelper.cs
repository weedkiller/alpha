using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Multigroup.Portal.Helpers
{
    public class ExcelHelper
    {
        public static Byte[] BuildDocument(string user, string password, string reportUrl)
        {
            NetworkCredential nwc = new NetworkCredential(user, password);
            WebClient client = new WebClient();
            client.Credentials = nwc;
            
            return client.DownloadData(reportUrl);
        }
    }
}