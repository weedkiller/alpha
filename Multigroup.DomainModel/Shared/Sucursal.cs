using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class Sucursal
    {
        public int Sucursal_Id { get; set; } 
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int Encargado_Id { get; set; }
        public int Ciudad { get; set; }

    }

    public class SucursalSummary
    {
        public int Sucursal_Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Encargado { get; set; }
        public string Ciudad { get; set; }

    }

}
