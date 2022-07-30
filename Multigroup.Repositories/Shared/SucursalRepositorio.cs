using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared.Mappers;
using Multigroup.Repositories.Shared.Resources;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;

using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared
{
    public class SucursalRepositorio : BaseRepository
    {
        public Sucursal GetSucursalById(int sucursalId)
        {
            var data = Db.ExecuteSprocAccessor(StoredProcedures.pub_Sucursal_Get, SucursalMapper.Mapper, sucursalId);

            return GetFirstElement(data);
        }

        public IEnumerable<Sucursal> GetSucursales()
        {
            var data = Db.ExecuteSprocAccessor(StoredProcedures.pub_Sucursal_GetList, SucursalMapper.Mapper);

            return data;
        }


        public int InsertSucursal(Sucursal sucursal)
        {
            var command = Db.GetStoredProcCommand(StoredProcedures.pub_Sucursal_Insert);

            Db.AddInParameter(command, "@Nombre", DbType.String, sucursal.Nombre);
            Db.AddInParameter(command, "@Direccion", DbType.String, sucursal.Direccion);
            Db.AddInParameter(command, "@Telefono", DbType.String, sucursal.Telefono);
            Db.AddInParameter(command, "@EncargadoId", DbType.Int32, sucursal.Encargado_Id);
            Db.AddInParameter(command, "@CiudadId", DbType.Int32, sucursal.Ciudad);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }
        public void DeleteSucursal(int sucursalId)
        {
            var command = Db.GetStoredProcCommand(StoredProcedures.pub_Sucursal_Delete);

            Db.AddInParameter(command, "@SucursalId", DbType.Int32, sucursalId);

            Db.ExecuteScalar(command);
        }

        public void UpdateSucursal(Sucursal sucursal)
        {
            var command = Db.GetStoredProcCommand(StoredProcedures.pub_Sucursal_Update);

            Db.AddInParameter(command, "@SucursalId", DbType.Int32, sucursal.Sucursal_Id);
            Db.AddInParameter(command, "@Nombre", DbType.String, sucursal.Nombre);
            Db.AddInParameter(command, "@Direccion", DbType.String, sucursal.Direccion);
            Db.AddInParameter(command, "@Telefono", DbType.String, sucursal.Telefono);
            Db.AddInParameter(command, "@EncargadoId", DbType.Int32, sucursal.Encargado_Id);
            Db.AddInParameter(command, "@CiudadId", DbType.Int32, sucursal.Ciudad);

            Db.ExecuteScalar(command);
        }



    }
}
