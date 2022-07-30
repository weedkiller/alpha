using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Multigroup.DomainModel.Shared
{
    public class PostedFileWrapper
    {
        public string FileName { get; set; }
        public Stream InputStream { get; set; }
        public string Path { get; set; }
    }
}
