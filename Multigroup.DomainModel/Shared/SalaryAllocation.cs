using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class SalaryAllocation
    {
        public int SalaryAllocationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProveedorId { get; set; }
        public string Description { get; set; }
        public int UserAuthorizedId { get; set; }
        public bool IsAuthorized { get; set; }
        public double Amount { get; set; }
        public string LastMonth { get; set; }
    }

    public class SalaryAllocationSummary
    {
        public int SalaryAllocationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Proveedor { get; set; }
        public string UserAuthorized { get; set; }
        public string Description { get; set; }
        public bool IsAuthorized { get; set; }
        public double Amount { get; set; }
        public string LastMonth { get; set; }
    }
}
