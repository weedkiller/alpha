using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


namespace Multigroup.Services.Shared
{
    public class EmployeeService
    {
        private EmployeeRepository _employeeRepository;
        private UserRepository _userService;
        private UserTypeService _userTypeService;
        private SalaryAllocationRepository _salaryAllocationRepository;

        public EmployeeService()
        {
            _employeeRepository = new EmployeeRepository();
            _userService = new UserRepository();
            _userTypeService = new UserTypeService();
            _salaryAllocationRepository = new SalaryAllocationRepository();
        }


        public Provider GetProvidersById(int providerId)
        {
            return _employeeRepository.GetProviderById(providerId);
        }

        public IEnumerable<Provider> GetProviders()
        {
            return _employeeRepository.GetProviders();
        }

        public bool ExisteProvider(string document)
        {
            IEnumerable<Provider> providers = _employeeRepository.GetProviders();
            bool existe = false;

            foreach (Provider pro in providers)
            {
                if (pro.Document == document && pro.IsEmployee == true)
                    existe = true;
            }

            return existe;
        }

        public bool NoExisteUser(string document, string ProviderType)
        {

            bool existe = true;
            UserType userType = _userTypeService.GetUserType(int.Parse(ProviderType));

            IEnumerable<User> usuarios = _userService.GetUsers();

            foreach (User user in usuarios)
            {
                if (user.IdentificationNumber == document)
                    existe = false;
            }

            if (existe && !userType.IsCommission)
                existe = false;

            return existe;
        }

        public bool ExisteProviderUpdate(string document, int providerId)
        {
            IEnumerable<Provider> providers = _employeeRepository.GetProviders();
            bool existe = false;

            foreach (Provider pro in providers)
            {
                if (pro.Document == document && pro.IsEmployee == true && providerId != pro.ProviderId)
                    existe = true;
            }

            return existe;
        }

        public Provider GetProviderByDocument(string document)
        {
            IEnumerable<Provider> providers = _employeeRepository.GetProviders();

            foreach (Provider pro in providers)
            {
                if (pro.Document == document && pro.IsEmployee == true)
                    return pro;
            }
            return null;
        }

        public Provider GetProviderByUserId(int userId)
        {
            IEnumerable<Provider> providers = _employeeRepository.GetProviders();

            foreach (Provider pro in providers)
            {
                if (pro.UserId == userId && pro.IsEmployee == true)
                    return pro;
            }
            return null;
        }

        public IEnumerable<ProviderSummary> GetProvidersSummary(int? SelectedProviderType, string DateFrom, string DateTo, string BalanceFrom, string BalanceTo, string active)
        {
            if (SelectedProviderType == null)
                SelectedProviderType = 0;
            if (DateFrom == "")
                DateFrom = "01/01/1900";
            if (DateTo == "")
                DateTo = "01/01/2100";
            if (BalanceFrom == "")
                BalanceFrom = "-100000000";
            if (BalanceTo == "")
                BalanceTo = "100000000";
            return _employeeRepository.GetProvidersSummary(SelectedProviderType.Value.ToString(), DateFrom, DateTo, BalanceFrom, BalanceTo, active);
        }

        public IEnumerable<ProviderType> GetProviderTypes()
        {
            return _employeeRepository.GetProviderTypes();
        }

        public IEnumerable<EmployeeCurrentAcount> GetEmployeeCurrentAcount(string Providers, string BalanceFrom, string BalanceTo, string NotImput)
        {
            return _employeeRepository.GetEmployeeCurrentAcount(Providers, BalanceFrom, BalanceTo, NotImput);
        }


        public void AddProvider(Provider Provider)
        {
            _employeeRepository.AddProvider(Provider);
            if (Provider.ProviderId != 0)
            {
                User user = _userService.GetUser(Provider.UserId);
                user.UserType = Provider.ProviderTypeId;
                _userService.Update(user);
            }
        }

        public void UpdateProvider(Provider Provider)
        {


            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _employeeRepository.UpdateProvider(Provider);
                    if (Provider.ProviderId != 0)
                    {
                        User user = _userService.GetUser(Provider.UserId);
                        user.UserType = Provider.ProviderTypeId;
                        _userService.Update(user);

                        if (Provider.Active == false)
                        {
                            IEnumerable<SalaryAllocation> imputaciones = _salaryAllocationRepository.GetSalaryAllocationsByEmployee(Provider.ProviderId);
                            foreach (SalaryAllocation impu in imputaciones)
                            {
                                impu.EndDate = DateTime.Now.AddMonths(-1);
                                _salaryAllocationRepository.UpdateSalaryAllocation(impu);

                            }
                        }
                    }

                    scope.Complete();

                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public IEnumerable<EmployeeCurrentAcountDetail> GetEmployeeCurrentAcountDetail(int providerId)
        {
            return _employeeRepository.GetEmployeeCurrentAcountDetail(providerId);
        }
    }
}
