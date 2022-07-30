using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class Agency
    {
        public int AgencyId { get; set; }
        public string Description { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public int ManagerId { get; set; }
        public User Manager { get; set; }
        public int Type { get; set; }
    }

    public class AgencyType
    {
        public int AgencyTypeId { get; set; }
        public string Description { get; set; }
    }

    public class Cartera
    {
        public int Numero { get; set; }
        public string Nombre { get; set; }
    }

    public class CarteraFinal
    {
        public int Total { get; set; }
        public int TotalCucu { get; set; }
        public int TotalMaxi { get; set; }
        public double MontoCucu { get; set; }
        public double MontoMaxi { get; set; }
        public string ContratosMaxi { get; set; }
        public string ContratosCucu { get; set; }
        
    }
}
