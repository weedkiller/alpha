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
    public class CustomerRepository : BaseRepository
    {
        public Customer GetCustomerById(int customerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Customer_Get", CustomerMapper.Mapper, customerId);

            return GetFirstElement(data);
        }

        public void AsociateCustomer(int UserId, int Customer, int Month, int Year)
        {
            var command = Db.GetStoredProcCommand("pub_CustomerUser_Asign");
            Db.AddInParameter(command, "@UserId", DbType.Int32, UserId);
            Db.AddInParameter(command, "@CustomerId", DbType.Int32, Customer);
            Db.AddInParameter(command, "@Month", DbType.Int32, Month);
            Db.AddInParameter(command, "@Year", DbType.Int32, Year);
            Db.ExecuteScalar(command);
        }
        public IEnumerable<Customer> GetCustomers()
        {
            var data = Db.ExecuteSprocAccessor("pub_Customer_GetList", CustomerMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<CustomerSummary> GetCustomersSummary()
        {
            var data = Db.ExecuteSprocAccessor("pub_CustomerSummary_GetList", GenericMapper<CustomerSummary>.Mapper);

            return data.ToList();
        }
        public IEnumerable<BaseEntityText> GetPortfolioCustomerResume()
        {
            var data = Db.ExecuteSprocAccessor("pub_CustomersUserResume_GET", GenericMapper<BaseEntityText>.Mapper);

            return data.ToList();
        }

        public IEnumerable<BaseEntityText> GetPortfolioCustomerResumeNext()
        {
            var data = Db.ExecuteSprocAccessor("pub_CustomersUserResumeNext_GET", GenericMapper<BaseEntityText>.Mapper);

            return data.ToList();
        }
        
        public IEnumerable<Customer> GetCustomersByStatusAndZone(string Status, string Zones, string Payment)
        {
            var data = Db.ExecuteSprocAccessor("pub_CustomersByStatusAndZone_GetList", CustomerMapper.Mapper, Status, Zones, Payment);

            return data.ToList();
        }
        public IEnumerable<CustomerPortfolio> GetPortfolioCustomer(string Status, string Zones, string Payment, string UserId, int Year, int Month)
        {
            var data = Db.ExecuteSprocAccessor("pub_CustomersForUser_GetList", CustomerPortfolioMapper.Mapper, Status, Zones, Payment, UserId, Year, Month);
            return data.ToList();
        }

        public CustomerPortfolioTotal GetPortfolioCustomerTotal(string Status, string Zones, string Payment, string UserId, int Year, int Month)
        {
            var data = Db.ExecuteSprocAccessor("pub_CustomersForUserTotal_GetList", CustomerPortfolioTotalMapper.Mapper, Status, Zones, Payment, UserId, Year, Month);
            return GetFirstElement(data);
        }

        public IEnumerable<CustomerPortfolio> GetPortfolioCustomerZone(string Province, string Citys, int Year, int Month, string UserId)
        {
            var data = Db.ExecuteSprocAccessor("pub_CustomersForUserByZone_GetList", CustomerPortfolioMapper.Mapper, Province, Citys, Year, Month, UserId);
            return data.ToList();
        }

        public IEnumerable<InstallmentReport> GetInstallmentReport(int Installment)
        {
            var data = Db.ExecuteSprocAccessor("pub_InstallmentReport_GetList", InstallmentReportMapper.Mapper, Installment);
            return data.ToList();
        }

        public CustomerPortfolioTotal GetPortfolioCustomerTotalZone(string Province, string Citys, int Year, int Month, string UserId)
        {
            var data = Db.ExecuteSprocAccessor("pub_CustomersForUserTotalByZone_GetList", CustomerPortfolioTotalMapper.Mapper, Province, Citys, Year, Month, UserId);
            return GetFirstElement(data);
        }

        public IEnumerable<Customer> GetCustomersForUser(string Status, string Zones, string Payment)
        {
            var data = Db.ExecuteSprocAccessor("pub_CustomersForUser_GetList", CustomerMapper.Mapper, Status, Zones, Payment);

            return data.ToList();
        }
        
        public Customer GetCustomerByDNI(string DNI)
        {
            var data = Db.ExecuteSprocAccessor("pub_CustomersByDNI_GetList", CustomerMapper.Mapper, DNI);

            return GetFirstElement(data);
        }

        public void Delete(int customerId)
        {
            var command = Db.GetStoredProcCommand("pub_Customer_Delete");
            Db.AddInParameter(command, "@CustomerId", DbType.Int32, customerId);
            Db.ExecuteScalar(command);
        }

        public int AddCustomer(Customer customer)
        {
            var command = Db.GetStoredProcCommand("pub_Customer_Insert");

            Db.AddInParameter(command, "@IdentificationType", DbType.Int32, customer.IdentificationType);
            Db.AddInParameter(command, "@IdentificationNumber", DbType.String, customer.IdentificationNumber);
            Db.AddInParameter(command, "@Name", DbType.String, customer.Name);
            Db.AddInParameter(command, "@Surname", DbType.String, customer.Surname);
            Db.AddInParameter(command, "@Birthdate", DbType.DateTime, customer.Birthdate);
            Db.AddInParameter(command, "@MaritalStatus", DbType.Int32, customer.MaritalStatus);
            Db.AddInParameter(command, "@Address", DbType.String, customer.Address);
            Db.AddInParameter(command, "@CityId", DbType.Int32, customer.CityId);
            Db.AddInParameter(command, "@State", DbType.String, customer.State);
            Db.AddInParameter(command, "@ZipCode", DbType.String, customer.ZipCode);
            Db.AddInParameter(command, "@Phone", DbType.String, customer.Phone);
            Db.AddInParameter(command, "@CellPhone", DbType.String, customer.CellPhone);
            Db.AddInParameter(command, "@Email", DbType.String, customer.Email);
            Db.AddInParameter(command, "@Occupation", DbType.String, customer.Occupation);
            Db.AddInParameter(command, "@ContactHours", DbType.String, customer.ContactHours);
            Db.AddInParameter(command, "@SpouseIdentificationType", DbType.Int32, customer.SpouseIdentificationType);
            Db.AddInParameter(command, "@SpouseIdentificationNumber", DbType.String, customer.SpouseIdentificationNumber);
            Db.AddInParameter(command, "@SpouseName", DbType.String, customer.SpouseName);
            Db.AddInParameter(command, "@SpouseSurname", DbType.String, customer.SpouseSurname);
            Db.AddInParameter(command, "@StatusClient", DbType.Int32, customer.StatusClient);
            Db.AddInParameter(command, "@CUIT", DbType.String, customer.CUIT);
            Db.AddInParameter(command, "@AddressDetail", DbType.String, customer.AddressDetail);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void Update(Customer customer)
        {
            var command = Db.GetStoredProcCommand("pub_Customer_Update");

            Db.AddInParameter(command, "@CustomerId", DbType.Int32, customer.CustomerId);
            Db.AddInParameter(command, "@IdentificationType", DbType.Int32, customer.IdentificationType);
            Db.AddInParameter(command, "@IdentificationNumber", DbType.String, customer.IdentificationNumber);
            Db.AddInParameter(command, "@Name", DbType.String, customer.Name);
            Db.AddInParameter(command, "@Surname", DbType.String, customer.Surname);
            Db.AddInParameter(command, "@Birthdate", DbType.DateTime, customer.Birthdate);
            Db.AddInParameter(command, "@MaritalStatus", DbType.Int32, customer.MaritalStatus);
            Db.AddInParameter(command, "@Address", DbType.String, customer.Address);
            Db.AddInParameter(command, "@CityId", DbType.Int32, customer.CityId);
            Db.AddInParameter(command, "@State", DbType.String, customer.State);
            Db.AddInParameter(command, "@ZipCode", DbType.String, customer.ZipCode);
            Db.AddInParameter(command, "@Phone", DbType.String, customer.Phone);
            Db.AddInParameter(command, "@CellPhone", DbType.String, customer.CellPhone);
            Db.AddInParameter(command, "@Email", DbType.String, customer.Email);
            Db.AddInParameter(command, "@Occupation", DbType.String, customer.Occupation);
            Db.AddInParameter(command, "@ContactHours", DbType.String, customer.ContactHours);
            Db.AddInParameter(command, "@SpouseIdentificationType", DbType.Int32, customer.SpouseIdentificationType);
            Db.AddInParameter(command, "@SpouseIdentificationNumber", DbType.String, customer.SpouseIdentificationNumber);
            Db.AddInParameter(command, "@SpouseName", DbType.String, customer.SpouseName);
            Db.AddInParameter(command, "@SpouseSurname", DbType.String, customer.SpouseSurname);
            Db.AddInParameter(command, "@StatusClient", DbType.Int32, customer.StatusClient);
            Db.AddInParameter(command, "@CUIT", DbType.String, customer.CUIT);
            Db.AddInParameter(command, "@AddressDetail", DbType.String, customer.AddressDetail);

            Db.ExecuteScalar(command);
        }

        public void UpdateCustomerStatus(int customerId, int Status)
        {
            var command = Db.GetStoredProcCommand("pub_CustomerStatus_Update");

            Db.AddInParameter(command, "@CustomerId", DbType.Int32, customerId);
            Db.AddInParameter(command, "@StatusClient", DbType.Int32, Status);


            Db.ExecuteScalar(command);
        }

        public IEnumerable<IdentificationType> GetIdentificationTypes()
        {
            var data = Db.ExecuteSprocAccessor("pub_IdentificationType_GetList", IdentificationTypeMapper.Mapper);

            return data.ToList();
        }

        public IdentificationType GetIdentificationType(int idIdentification)
        {
            var data = Db.ExecuteSprocAccessor("pub_IdentificationType_Get", IdentificationTypeMapper.Mapper, idIdentification);

            return GetFirstElement(data);
        }

        public IEnumerable<MaritalStatus> GetMaritalStatus()
        {
            var data = Db.ExecuteSprocAccessor("pub_MaritalStatus_GetList", MaritalStatusMapper.Mapper);

            return data.ToList();
        }

        public MaritalStatus GetMaritalStatusById(int idMaritalStatus)
        {
            var data = Db.ExecuteSprocAccessor("pub_MaritalStatus_Get", MaritalStatusMapper.Mapper, idMaritalStatus);

            return GetFirstElement(data);
        }

        public IEnumerable<StatusClient> GetStatusClient()
        {
            var data = Db.ExecuteSprocAccessor("pub_StatusClient_GetList", StatusClientMapper.Mapper);

            return data.ToList();
        }
    }

}
