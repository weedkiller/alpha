using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Threading.Tasks;
using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;

namespace Multigroup.Services.Shared
{
    public class SucursalService
    {
        private SucursalRepositorio _sucursalRepositorio;

        public SucursalService()
        {
            _sucursalRepositorio = new SucursalRepositorio();
        }

        public Sucursal GetSucursal(int sucursalId)
        {
            return _sucursalRepositorio.GetSucursalById(sucursalId);
        }

        public IEnumerable<Sucursal> getSucursales()
        {
            return _sucursalRepositorio.GetSucursales();
        }


        public int InsertSucursal(Sucursal sucursal)
        {
            try
            {
                return _sucursalRepositorio.InsertSucursal(sucursal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSucursal(Sucursal sucursal)
        {
            try
            {
                _sucursalRepositorio.UpdateSucursal(sucursal);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteSucursal(int sucursalId)
        {
            _sucursalRepositorio.DeleteSucursal(sucursalId);
        }

    }
}
