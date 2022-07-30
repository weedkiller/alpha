using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared.Mappers;
using Multigroup.Repositories.Shared.Resources;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Multigroup.Repositories.Shared
{
    public class PurchasePerceptionRepository : BaseRepository
    {
        public PurchasePerception GetPurchasePerceptionById(int PurchasePerceptionId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PurchasePerception_Get", PurchasePerceptionMapper.Mapper, PurchasePerceptionId);

            return GetFirstElement(data);
        }


        public IEnumerable<PurchasePerception> GetPurchasePerceptions()
        {
            var data = Db.ExecuteSprocAccessor("pub_PurchasePerception_GetList", PurchasePerceptionMapper.Mapper);

            return data.ToList();
        }

        public void DeletePurchasePerception(int PurchasePerceptionId)
        {
            var command = Db.GetStoredProcCommand("pub_PurchasePerception_Delete");
            Db.AddInParameter(command, "@PurchasePerceptionId", DbType.Int32, PurchasePerceptionId);
            Db.ExecuteScalar(command);
        }

        public void AddPurchasePerception(PurchasePerception PurchasePerception)
        {
            var command = Db.GetStoredProcCommand("pub_PurchasePerception_Insert");

            Db.AddInParameter(command, "@Name", DbType.String, PurchasePerception.Name);
            Db.AddInParameter(command, "@Code", DbType.String, PurchasePerception.Code);
            Db.AddInParameter(command, "@Active", DbType.Boolean, PurchasePerception.Active);

            Db.ExecuteScalar(command);
        }

        public void Update(PurchasePerception PurchasePerception)
        {
            var command = Db.GetStoredProcCommand("pub_PurchasePerception_Update");

            Db.AddInParameter(command, "@PurchasePerceptionId", DbType.Int32, PurchasePerception.PurchasePerceptionId);
            Db.AddInParameter(command, "@Name", DbType.String, PurchasePerception.Name);
            Db.AddInParameter(command, "@Code", DbType.String, PurchasePerception.Code);
            Db.AddInParameter(command, "@Active", DbType.Boolean, PurchasePerception.Active);

            Db.ExecuteScalar(command);
        }

        public PurchaseDocument GetPurchaseDocumentById(int PurchaseDocumentId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PurchaseDocument_Get", PurchaseDocumentMapper.Mapper, PurchaseDocumentId);

            return GetFirstElement(data);
        }


        public IEnumerable<PurchaseDocument> GetPurchaseDocuments()
        {
            var data = Db.ExecuteSprocAccessor("pub_PurchaseDocument_GetList", PurchaseDocumentMapper.Mapper);

            return data.ToList();
        }

        public void DeletePurchaseDocument(int PurchaseDocumentId)
        {
            var command = Db.GetStoredProcCommand("pub_PurchaseDocument_Delete");
            Db.AddInParameter(command, "@PurchaseDocumentId", DbType.Int32, PurchaseDocumentId);
            Db.ExecuteScalar(command);
        }

        public void AddPurchaseDocument(PurchaseDocument PurchaseDocument)
        {
            var command = Db.GetStoredProcCommand("pub_PurchaseDocument_Insert");

            Db.AddInParameter(command, "@Name", DbType.String, PurchaseDocument.Name);
            Db.AddInParameter(command, "@Code", DbType.String, PurchaseDocument.Code);
            Db.AddInParameter(command, "@Active", DbType.Boolean, PurchaseDocument.Active);
            Db.AddInParameter(command, "@Subtract", DbType.Boolean, PurchaseDocument.Subtract);

            Db.ExecuteScalar(command);
        }

        public void Update(PurchaseDocument PurchaseDocument)
        {
            var command = Db.GetStoredProcCommand("pub_PurchaseDocument_Update");

            Db.AddInParameter(command, "@PurchasePerceptionId", DbType.Int32, PurchaseDocument.PurchaseDocumentId);
            Db.AddInParameter(command, "@Name", DbType.String, PurchaseDocument.Name);
            Db.AddInParameter(command, "@Code", DbType.String, PurchaseDocument.Code);
            Db.AddInParameter(command, "@Active", DbType.Boolean, PurchaseDocument.Active);
            Db.AddInParameter(command, "@Subtract", DbType.Boolean, PurchaseDocument.Subtract);

            Db.ExecuteScalar(command);
        }
    }
}
