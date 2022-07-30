using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.Transactions;
using System.IO;
using Multigroup.Common.PDF;
using System.Data;
using ExcelDataReader;

namespace Multigroup.Services.Shared
{
    public class ContactCallService
    {
        private ContactCallRepository _ContactCallRepository;
        private PdfGeneratorHelper _pdfGeneratorHelper;

        public ContactCallService()
        {
            _ContactCallRepository = new ContactCallRepository();
            _pdfGeneratorHelper = new PdfGeneratorHelper();
        }

        public ContactCall GetContactCallsById(int ContactCallId)
        {
            return _ContactCallRepository.GetContactCallById(ContactCallId);
        }

        public IEnumerable<ContactCall> GetContactCalls()
        {
            return _ContactCallRepository.GetContactCalls();
        }

        public IEnumerable<ContactCallSummary> GetContactCallSummaries(int userId)
        {
            return _ContactCallRepository.GetContactCallSummaries(userId);
        }

        public IEnumerable<ContactCallSummary> GetContactCallSummaryByFilter(string states, string dateFrom, string dateTo, string managerId, string supervisorId, string sellerId)
        {
            return _ContactCallRepository.GetContactCallSummaryByFilter(states, dateFrom, dateTo, managerId, supervisorId, sellerId);
        }

        public IEnumerable<ContactCallState> GetContactCallStates()
        {
            return _ContactCallRepository.GetContactCallStates();
        }

        public void AddContactCall(ContactCall ContactCall)
        {
            _ContactCallRepository.AddContactCall(ContactCall);
        }

        public void DeleteContactCalls(int ContactCallId)
        {
            _ContactCallRepository.Delete(ContactCallId);
        }

        public void UpdateContactCalls(ContactCall ContactCall)
        {
            _ContactCallRepository.Update(ContactCall);
        }

        public string InsertExcel(HttpPostedFileBase file)
        {
            try
            {
                var path = SaveAttachExcel(file);
                return path;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void CargarExcel(string ruta)
        {
            try
            {
                using (var stream = File.Open(ruta, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet();
                        // Ejemplos de acceso a datos
                        DataTable table = result.Tables[0];
                        foreach (DataRow row in table.Rows)
                        {
                            ContactCall contacto = new ContactCall();
                            string[] subs = row[0].ToString().Split(' ');
                            string nombre = "";
                            foreach (var sub in subs)
                            {
                                if (!sub.Equals(subs[subs.Count() - 1]))
                                {
                                    nombre = nombre + " " + sub;
                                }                               
                            }
                            contacto.Name = nombre.Trim();
                            contacto.Surname = subs[subs.Count() - 1];
                            contacto.Phone = row[1].ToString();
                            contacto.Vehicle = row[1].ToString();
                            contacto.ContactCallStateId = 1;
                            _ContactCallRepository.AddContactCall(contacto);
                        }                      
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string SaveAttachExcel(HttpPostedFileBase file)
        {
            var path = ConfigurationManager.AppSettings["ExcelPath"];
            var filePath = System.AppDomain.CurrentDomain.BaseDirectory + path;
            filePath = System.IO.Path.Combine(filePath.ToString());
            path = System.IO.Path.Combine(path.ToString());
            bool isExists = System.IO.Directory.Exists(filePath);

            if (!isExists)
                System.IO.Directory.CreateDirectory(filePath);

            var pathFinal = string.Format("{0}\\{1}", filePath, "Datos.xls");


            _pdfGeneratorHelper.SaveFileContents(pathFinal, file.InputStream);

            return pathFinal;
        }
    }
}
