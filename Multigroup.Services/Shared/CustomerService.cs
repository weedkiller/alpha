using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Services.Shared
{
    public class CustomerService
    {
        private bool primero;
        private CustomerRepository _customerRepository;
        private CityRepository _cityRepository;
        private String zonasTotal;
        private String estadosTotal;
        private String paymentTotal;


        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
            _cityRepository = new CityRepository();
            primero = true;
            zonasTotal = "";
            estadosTotal = "";
            paymentTotal = "";
        }

        public Customer GetCustomer(int CustomerId)
        {
            return _customerRepository.GetCustomerById(CustomerId);
        }

        public Customer GetCustomersById(int customerId)
        {
            return _customerRepository.GetCustomerById(customerId);
        }
        public void AsociateCustomer(int UserId, int customer, int Month, int Year)
        {
            _customerRepository.AsociateCustomer(UserId, customer, Month, Year);
        }
        public IEnumerable<CustomerPortfolio> GetPortfolioCustomer(List<string> status, List<string> zones, List<string> paymentDate, List<string> UserId, int Year, int Month)
        {
            var _status = (status[0] != "" && status.Count() > 0) ? String.Join(",", status) : "0";
            var _zones = (zones[0] != "" && zones.Count() > 0) ? String.Join(",", zones) : "0";
            var _paymentDate = (paymentDate[0] != "" && paymentDate.Count() > 0) ? String.Join(",", paymentDate) : "0";
            var _userId = (UserId[0] != "" && UserId.Count() > 0) ? String.Join(",", UserId) : "0";
            return _customerRepository.GetPortfolioCustomer(_status.Replace("2", "4").Replace("3", "22"), _zones, _paymentDate, _userId, Year, Month);
        }

        public CustomerPortfolioTotal GetPortfolioCustomerTotal(List<string> status, List<string> zones, List<string> paymentDate, List<string> UserId, int Year, int Month)
        {
            var _status = (status[0] != "" && status.Count() > 0) ? String.Join(",", status) : "0";
            var _zones = (zones[0] != "" && zones.Count() > 0) ? String.Join(",", zones) : "0";
            var _paymentDate = (paymentDate[0] != "" && paymentDate.Count() > 0) ? String.Join(",", paymentDate) : "0";
            var _userId = (UserId[0] != "" && UserId.Count() > 0) ? String.Join(",", UserId) : "0";
            return _customerRepository.GetPortfolioCustomerTotal(_status, _zones, _paymentDate, _userId, Year, Month);
        }

        public IEnumerable<CustomerPortfolio> GetPortfolioCustomerZone(int? province, List<string> citys, int Year, int Month, List<string> UserId)
        {
            if (province == null)
                province = 0;
            var _citys = (citys[0] != "" && citys.Count() > 0) ? String.Join(",", citys) : "0";
            var _userId = (UserId[0] != "" && UserId.Count() > 0) ? String.Join(",", UserId) : "0";
            return _customerRepository.GetPortfolioCustomerZone(province.ToString(), _citys, Year, Month, _userId);
        }

        public IEnumerable<InstallmentReport> GetInstallmentReport(int Installment)
        {
            return _customerRepository.GetInstallmentReport(Installment);
        }

        public CustomerPortfolioTotal GetPortfolioCustomerTotalZone(int? province, List<string> citys, int Year, int Month, List<string> UserId)
        {
            if (province == null)
                province = 0;
            var _citys = (citys[0] != "" && citys.Count() > 0) ? String.Join(",", citys) : "0";
            var _userId = (UserId[0] != "" && UserId.Count() > 0) ? String.Join(",", UserId) : "0";
            return _customerRepository.GetPortfolioCustomerTotalZone(province.ToString(), _citys, Year, Month, _userId);
        }

        public IEnumerable<BaseEntityText> GetPortfolioCustomerResume()
        {
            return _customerRepository.GetPortfolioCustomerResume();
        }

        public IEnumerable<BaseEntityText> GetPortfolioCustomerResumeNext()
        {
            return _customerRepository.GetPortfolioCustomerResumeNext();
        }
        public IEnumerable<Customer> GetCustomers()
        {
            return _customerRepository.GetCustomers();
        }

        public IEnumerable<Customer> GetCustomersComplete()
        {
            List<Customer> Clientes = new List<Customer>();
            foreach(Customer item in _customerRepository.GetCustomers())
            {
                item.Name = item.Surname + " " + item.Name + " ( " + item.IdentificationNumber + ")";
                Clientes.Add(item);
            }

            return Clientes;
        }

        public IEnumerable<CustomerSummary> GetCustomersSummary()
        {
            return _customerRepository.GetCustomersSummary();
        }

        public IEnumerable<Customer> GetCustomersByStatusAndZone(List<int> status, List<int> zones)
        {
            primero = true;
            status.ForEach(x => ConcatenarStatus(Int32.Parse(x.ToString())));
            primero = true;
            zones.ForEach(x => ConcatenarZones(Int32.Parse(x.ToString())));
            return _customerRepository.GetCustomersByStatusAndZone(estadosTotal, zonasTotal, "0");
        }

        public IEnumerable<Customer> GetCustomersForUser(List<int> status, List<int> zones, List<int> payment)
        {
            primero = true;
            status.ForEach(x => ConcatenarStatus(Int32.Parse(x.ToString())));
            primero = true;
            zones.ForEach(x => ConcatenarZones(Int32.Parse(x.ToString())));
            primero = true;
            payment.ForEach(x => ConcatenarPayment(Int32.Parse(x.ToString())));
            return _customerRepository.GetCustomersForUser(estadosTotal, zonasTotal, paymentTotal);
        }

        private void ConcatenarStatus(int aux)
        {
            if (primero == true)
            {
                estadosTotal = "" + aux;
                primero = false;
            }
            else
                estadosTotal = estadosTotal + "," + aux;
        }

        private void ConcatenarZones(int aux)
        {
            if (primero == true)
            {
                zonasTotal = "" + aux;
                primero = false;
            }
            else
                zonasTotal = zonasTotal + "," + aux;
        }

        private void ConcatenarPayment(int aux)
        {
            if (primero == true)
            {
                paymentTotal = "" + aux;
                primero = false;
            }
            else
                paymentTotal = paymentTotal + "," + aux;
        }

        public int AddCustomer(Customer customer)
        {
            customer.StatusClient = 1;
            customer.State = _cityRepository.GetState(Int32.Parse(customer.State)).Name;
            return _customerRepository.AddCustomer(customer);
        }

        public bool ExisteCustomerByDNI(String IdentificationNumber)
        {
            Customer clientes =_customerRepository.GetCustomerByDNI(IdentificationNumber);
            
            bool existe = false;
            if (clientes != null)
            {
                existe = true;
            }
            return existe;
        }

        public Customer GetCustomerByDNI(String IdentificationNumber)
        {
          return _customerRepository.GetCustomerByDNI(IdentificationNumber);
        }
        

        public void DeleteCustomers(int userId)
        {
            _customerRepository.Delete(userId);
        }

        public void UpdateCustomers(Customer customer)
        {
            customer.State = _cityRepository.GetState(Int32.Parse(customer.State)).Name;
            _customerRepository.Update(customer);
        }

        public void UpdateCustomerStatus(int customerId, int Status)
        {
            _customerRepository.UpdateCustomerStatus(customerId, Status);
        }

        public IEnumerable<IdentificationType> GetIdentificationTypes()
        {
            return _customerRepository.GetIdentificationTypes();
        }

        public IdentificationType GetIdentificationType(int idIdentification)
        {
            return _customerRepository.GetIdentificationType(idIdentification);
        }

        public IEnumerable<MaritalStatus> GetMaritalStatus()
        {
            return _customerRepository.GetMaritalStatus();
        }

        public MaritalStatus GetMaritalStatusById(int idMaritalStatus)
        {
            return _customerRepository.GetMaritalStatusById(idMaritalStatus);
        }

        public IEnumerable<StatusClient> GetStatusClient()
        {
            return _customerRepository.GetStatusClient();
        }
    }
}
