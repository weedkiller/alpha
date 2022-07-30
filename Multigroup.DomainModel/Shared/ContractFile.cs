using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class ContractFile
    {
        public int ContractFileId { get; set; }
        public int ContractId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int ContentLength { get; set; }
    }

    public class SpendingFile
    {
        public int SpendingFileId { get; set; }
        public int SpendingId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int ContentLength { get; set; }
    }
}
