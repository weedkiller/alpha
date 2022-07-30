using Multigroup.DomainModel.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Providers
{
    public class SessionProvider
    {
        public static IEnumerable<PostedFileWrapper> GetPostedFiles()
        {
            var files = UpLoadFiles.Select(x => new PostedFileWrapper
            {
                FileName = x.FileName,
                InputStream = x.InputStream,
            });

            return files;
        }

        public static List<HttpPostedFileBase> UpLoadFiles
        {
            get { return (List<HttpPostedFileBase>)HttpContext.Current.Session["files"]; }
            set { HttpContext.Current.Session["files"] = value; }
        }
    }
}