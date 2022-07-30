
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Multigroup.Services.Shared;
using Multigroup.Portal.Models.Shared;

namespace Multigroup.Portal.Helpers
{
    public class CollectionHelper
    {
        private UserService _userService;
        private CustomerService _customerService;
        private CityService _cityService;
        private ContractService _contractService;
        private AgencyService _agencyService;
        private MPSellerAccountService _mpSellerAccountService;
        private ArticleService _articleService;
        private ProviderService _providerService;
        private PurchaseRequestService _purchaseRequestService;
        private PartnerService _partnerService;
        private CashierService _cashierService;
        private SpendingService _spendingService;
        private ContactCallService _contactCallService;

        public CollectionHelper()
        {
            _userService = new UserService();
            _customerService = new CustomerService();
            _cityService = new CityService();
            _contractService = new ContractService();
            _agencyService = new AgencyService();
            _mpSellerAccountService = new MPSellerAccountService();
            _articleService = new ArticleService();
            _providerService = new ProviderService();
            _purchaseRequestService = new PurchaseRequestService();
            _partnerService = new PartnerService();
            _cashierService = new CashierService();
            _spendingService = new SpendingService();
            _contactCallService = new ContactCallService();
        }

        public SingleSelectVm GetUsersSingleSelectVm()
        {
            var users = _userService.GetUsers();
            var ddlUsers = new SingleSelectVm();
            var items = users.Select(x => new Item { Id = x.UserId, Name = x.Surname + " " + x.Names });
            ddlUsers.ListItems = new SelectList(items, "Id", "Name");

            return ddlUsers;
        }

        public SingleSelectVm GetUsersAuthorizedSingleSelectVm(double userId)
        {
            var users = _userService.GetUsersAuthorized(userId);
            var ddlUsers = new SingleSelectVm();
            var items = users.Select(x => new Item { Id = x.UserId, Name = x.Surname + " " + x.Names });
            ddlUsers.ListItems = new SelectList(items, "Id", "Name");

            return ddlUsers;
        }

        public SingleSelectVm GetProviderIdsSingleSelectVm()
        {
            var providers = _providerService.GetProvidersWithId();
            var ddlProviders = new SingleSelectVm();
            var items = providers.Select(x => new Item { Id = x.ProviderId, Name = x.Name });
            ddlProviders.ListItems = new SelectList(items, "Id", "Name");

            return ddlProviders;
        }

        public SingleSelectVm GetPurchaseDocumentIdsSingleSelectVm()
        {
            var purdocuments = _spendingService.GetPurchaseDocuments();
            var ddlPurdocuments = new SingleSelectVm();
            var items = purdocuments.Select(x => new Item { Id = x.PurchaseDocumentId, Name = x.Name });
            ddlPurdocuments.ListItems = new SelectList(items, "Id", "Name");

            return ddlPurdocuments;
        }

        public SingleSelectVm GetPurchasePerceptionIdsSingleSelectVm()
        {
            var purperceptions = _spendingService.GetPurchasePerceptions();
            var ddlPurperceptions = new SingleSelectVm();
            var items = purperceptions.Select(x => new Item { Id = x.PurchasePerceptionId, Name = x.Name });
            ddlPurperceptions.ListItems = new SelectList(items, "Id", "Name");

            return ddlPurperceptions;
        }

        public SingleSelectVm GetEmployeesIdsSingleSelectVm()
        {
            var providers = _providerService.GetEmployeesWithId();
            var ddlProviders = new SingleSelectVm();
            var items = providers.Select(x => new Item { Id = x.ProviderId, Name = x.Name });
            ddlProviders.ListItems = new SelectList(items, "Id", "Name");

            return ddlProviders;
        }

        public SingleSelectVm GetProvidersByIdSingleSelectVm(int id)
        {
            var providers = _providerService.GetProvidersWithId();
            providers = providers.Where(x => x.ProviderId == id);
            var ddlProviders = new SingleSelectVm();
            var items = providers.Select(x => new Item { Id = x.ProviderId, Name = x.Name });
            ddlProviders.ListItems = new SelectList(items, "Id", "Name");

            return ddlProviders;
        }

        public SingleSelectVm GetSpendingsByProviderSingleSelectVm(int id)
        {
            var spendings = _spendingService.GetSpendingsByProvider(id);
            var ddlSpendings = new SingleSelectVm();
            var items = spendings.Select(x => new Item { Id = x.SpendingId, Name = x.Description });
            ddlSpendings.ListItems = new SelectList(items, "Id", "Name");

            return ddlSpendings;
        }

        public SingleSelectVm GetSpendingsByEmployeeSingleSelectVm(int id)
        {
            var spendings = _spendingService.GetSpendingsByEmployee(id);
            var ddlSpendings = new SingleSelectVm();
            var items = spendings.Select(x => new Item { Id = x.SpendingId, Name = x.Description });
            ddlSpendings.ListItems = new SelectList(items, "Id", "Name");

            return ddlSpendings;
        }

        public SingleSelectVm GetEmployeesByIdSingleSelectVm(int id)
        {
            var providers = _providerService.GetEmployeesWithId();
            providers = providers.Where(x => x.ProviderId == id);
            var ddlProviders = new SingleSelectVm();
            var items = providers.Select(x => new Item { Id = x.ProviderId, Name = x.Name });
            ddlProviders.ListItems = new SelectList(items, "Id", "Name");

            return ddlProviders;
        }

        public SingleSelectVm GetUrgencySingleSelectVm()
        {
            var urgencies = _purchaseRequestService.GetUrgencys();
            var ddlUrgencies = new SingleSelectVm();
            var items = urgencies.Select(x => new Item { Id = x.UrgencyId, Name = x.Description });
            ddlUrgencies.ListItems = new SelectList(items, "Id", "Name");

            return ddlUrgencies;
        }

        public SingleSelectVm GetArticleSingleSelectVm()
        {
            var articles = _articleService.GetArticles();
            var ddlArticles = new SingleSelectVm();
            var items = articles.Select(x => new Item { Id = x.ArticleId, Name = x.Name });
            ddlArticles.ListItems = new SelectList(items, "Id", "Name");

            return ddlArticles;
        }

        public SingleSelectVm GetArticleByHeadingSingleSelectVm(int headingId)
        {
            var articles = _articleService.GetArticlesSummary(headingId);
            var ddlArticles = new SingleSelectVm();
            var items = articles.Select(x => new Item { Id = x.ArticleId, Name = x.Name });
            ddlArticles.ListItems = new SelectList(items, "Id", "Name");

            return ddlArticles;
        }

        public SingleSelectVm GetProviderAndEmployeesSingleSelectVm()
        {
            var providers = _providerService.GetProvidersAndEmployeesWithId();
            var ddlProviders = new SingleSelectVm();
            var items = providers.Select(x => new Item { Id = x.ProviderId, Name = x.Name });
            ddlProviders.ListItems = new SelectList(items, "Id", "Name");

            return ddlProviders;
        }

        public SingleSelectVm GetHeadingsSingleSelectVm()
        {
            var headings = _articleService.GetHeadings();
            var ddlHeadings = new SingleSelectVm();
            var items = headings.Select(x => new Item { Id = x.HeadingId, Name = x.Name });
            ddlHeadings.ListItems = new SelectList(items, "Id", "Name");

            return ddlHeadings;
        }

        public SingleSelectVm GetProviderTypesSingleSelectVm()
        {
            var providerTypes = _providerService.GetProviderTypes();
            var ddlProviderTypes = new SingleSelectVm();
            var items = providerTypes.Select(x => new Item { Id = x.ProviderTypeId, Name = x.Name });
            ddlProviderTypes.ListItems = new SelectList(items, "Id", "Name");

            return ddlProviderTypes;
        }

        public SingleSelectVm GetIVAPositionsSingleSelectVm()
        {
            var IVAPosition = _providerService.GetIVAPositions();
            var ddlIVAPositions = new SingleSelectVm();
            var items = IVAPosition.Select(x => new Item { Id = x.IVAPositionId, Name = x.Description });
            ddlIVAPositions.ListItems = new SelectList(items, "Id", "Name");

            return ddlIVAPositions;
        }


        public SingleSelectVm GetUsersAgencySingleSelectVm()
        {
            var users = _userService.GetUsers();
            users = users.Where(x => x.UserType == 3);
            var ddlUsers = new SingleSelectVm();
            var items = users.Select(x => new Item { Id = x.UserId, Name = x.Surname + " " + x.Names });
            ddlUsers.ListItems = new SelectList(items, "Id", "Name");

            return ddlUsers;
        }

        public SingleSelectVm GetUsersByTypeSingleSelectVm(int type)
        {
            var users = _userService.GetUsers();
            users = users.Where(x => x.UserType == type);
            var ddlUsers = new SingleSelectVm();
            var items = users.Select(x => new Item { Id = x.UserId, Name = x.Surname + " " + x.Names });
            ddlUsers.ListItems = new SelectList(items, "Id", "Name");

            return ddlUsers;
        }

        public SingleSelectVm GetContactCallStatusSingleSelectVm()
        {
            var states = _contactCallService.GetContactCallStates();
            var ddlStates = new SingleSelectVm();
            var items = states.Select(x => new Item { Id = x.ContactCallStateId, Name = x.Description });
            ddlStates.ListItems = new SelectList(items, "Id", "Name");

            return ddlStates;
        }

        public SingleSelectVm GetTypesAgencySingleSelectVm()
        {
            var types = _agencyService.GetAgencyTypes();
            var ddlTypes = new SingleSelectVm();
            var items = types.Select(x => new Item { Id = x.AgencyTypeId, Name = x.Description });
            ddlTypes.ListItems = new SelectList(items, "Id", "Name");

            return ddlTypes;
        }

        public SingleSelectVm GetMPSellerAccountSingleSelectVm()
        {
            var mpSellerAccount = _mpSellerAccountService.GetMPSellerAccounts();
            var ddlMPSellerAccount = new SingleSelectVm();
            var items = mpSellerAccount.Select(x => new Item { Id = x.MPSellerAccountId, Name = x.Token.ToString() });
            ddlMPSellerAccount.ListItems = new SelectList(items, "Id", "Name");

            return ddlMPSellerAccount;
        }

        public SingleSelectVm GetIdentificationTypeSingleSelectVm()
        {
            var identificationType = _customerService.GetIdentificationTypes();
            var ddlIdentificationType = new SingleSelectVm();
            var items = identificationType.Select(x => new Item { Id = x.IdentificationTypeId, Name = x.Description });
            ddlIdentificationType.ListItems = new SelectList(items, "Id", "Name");

            return ddlIdentificationType;
        }

        public SingleSelectVm GetUserTypeSingleSelectVm()
        {
            var userType = _userService.GetUserTypes();
            var ddlUserType = new SingleSelectVm();
            var items = userType.Select(x => new Item { Id = x.UserTypeId, Name = x.Description });
            ddlUserType.ListItems = new SelectList(items, "Id", "Name");

            return ddlUserType;
        }

        public SingleSelectVm GetCashierSingleSelectVm()
        {
            var cashier = _cashierService.GetCashiers();
            cashier = cashier.Where(x => x.Active == true);
            var ddlCashier = new SingleSelectVm();
            var items = cashier.Select(x => new Item { Id = x.CashierId, Name = x.Name });
            ddlCashier.ListItems = new SelectList(items, "Id", "Name");

            return ddlCashier;
        }


        public SingleSelectVm GetMaritalStatusSingleSelectVm()
        {
            var maritalStatus = _customerService.GetMaritalStatus();
            var dllMaritalStatus = new SingleSelectVm();
            var items = maritalStatus.Select(x => new Item { Id = x.MaritalStatusId, Name = x.Description });
            dllMaritalStatus.ListItems = new SelectList(items, "Id", "Name");

            return dllMaritalStatus;
        }

        public SingleSelectVm GetCitySingleSelectVm()
        {
            var city = _cityService.GetCitys();
            var dllCity = new SingleSelectVm();
            var items = city.Select(x => new Item { Id = x.CityId, Name = x.Name });
            dllCity.ListItems = new SelectList(items, "Id", "Name");

            return dllCity;
        }

        public SingleSelectVm GetStateSingleSelectVm()
        {
            var state = _cityService.GetStates();
            var dllState = new SingleSelectVm();
            var items = state.Select(x => new Item { Id = x.StateId, Name = x.Name });
            dllState.ListItems = new SelectList(items, "Id", "Name");

            return dllState;
        }

        public SingleSelectVm GetSellerSingleSelectVm()
        {
            var seller = _userService.GetSellers();
            var dllSeller = new SingleSelectVm();
            var items = seller.Select(x => new Item { Id = x.UserId, Name = x.Surname + " " + x.Names });
            dllSeller.ListItems = new SelectList(items, "Id", "Name");

            return dllSeller;
        }

        public SingleSelectVm GetStatusClientSingleSelectVm()
        {
            var statusClient = _customerService.GetStatusClient();
            var ddlStatusClient = new SingleSelectVm();
            var items = statusClient.Select(x => new Item { Id = x.StatusClientId, Name = x.Description });
            ddlStatusClient.ListItems = new SelectList(items, "Id", "Name");

            return ddlStatusClient;
        }

        public SingleSelectVm GetPaymentDateSingleSelectVm()
        {
            var paymentDate = _contractService.GetPaymentDate();
            var ddlPaymentDate = new SingleSelectVm();
            var items = paymentDate.Select(x => new Item { Id = x.PaymentDateId, Name = x.Description });
            ddlPaymentDate.ListItems = new SelectList(items, "Id", "Name");

            return ddlPaymentDate;
        }

        public SingleSelectVm GetPaymentMethodSingleSelectVm()
        {
            var paymentMethod = _contractService.GetPaymentMethod();
            var ddlPaymentMethod = new SingleSelectVm();
            var items = paymentMethod.Select(x => new Item { Id = x.PaymentMethodId, Name = x.Name });
            ddlPaymentMethod.ListItems = new SelectList(items, "Id", "Name");

            return ddlPaymentMethod;
        }

        public SingleSelectVm GetPaymentMethodByPPSingleSelectVm(int PaymetPreferenceId)
        {
            var paymentMethod = _contractService.GetPaymentMethodByPP(PaymetPreferenceId);
            var ddlPaymentMethod = new SingleSelectVm();
            var items = paymentMethod.Select(x => new Item { Id = x.PaymentMethodId, Name = x.Name });
            ddlPaymentMethod.ListItems = new SelectList(items, "Id", "Name");

            return ddlPaymentMethod;
        }

        public SingleSelectVm GetPaymentMethodNCSingleSelectVm()
        {
            var paymentMethod = _contractService.GetPaymentMethod();
            paymentMethod = paymentMethod.Where(x => x.Consolidated == false);
            var ddlPaymentMethod = new SingleSelectVm();
            var items = paymentMethod.Select(x => new Item { Id = x.PaymentMethodId, Name = x.Name });
            ddlPaymentMethod.ListItems = new SelectList(items, "Id", "Name");

            return ddlPaymentMethod;
        }

        public SingleSelectVm GetAgencySingleSelectVm()
        {
            var agency = _agencyService.GetAgencys();
            var ddlAgency = new SingleSelectVm();
            var items = agency.Select(x => new Item { Id = x.AgencyId, Name = x.Description });
            ddlAgency.ListItems = new SelectList(items, "Id", "Name");

            return ddlAgency;
        }

        public SingleSelectVm GetPaymentPreferenceSingleSelectVm()
        {
            var paymentPreference = _contractService.GetPaymentPreference();
            var ddlPaymentPreference = new SingleSelectVm();
            var items = paymentPreference.Select(x => new Item { Id = x.PaymentPreferenceId, Name = x.Description });
            ddlPaymentPreference.ListItems = new SelectList(items, "Id", "Name");

            return ddlPaymentPreference;
        }

        public SingleSelectVm GetPaymentPlaceSingleSelectVm()
        {
            var paymentPlace = _contractService.GetPaymentPlaces();
            var ddlPaymentPlace = new SingleSelectVm();
            var items = paymentPlace.Select(x => new Item { Id = x.PaymentPlaceId, Name = x.Description });
            ddlPaymentPlace.ListItems = new SelectList(items, "Id", "Name");

            return ddlPaymentPlace;
        }

        public SingleSelectVm GetContractTypeSingleSelectVm()
        {
            var contractType = _contractService.GetContractTypes();
            contractType = contractType.Where(x => x.ContractTypeId != 3);
            var ddlContractType = new SingleSelectVm();
            var items = contractType.Select(x => new Item { Id = x.ContractTypeId, Name = x.Description });
            ddlContractType.ListItems = new SelectList(items, "Id", "Name");

            return ddlContractType;
        }

        public SingleSelectVm GetAllContractTypeSingleSelectVm()
        {
            var contractType = _contractService.GetContractTypes();
            var ddlContractType = new SingleSelectVm();
            var items = contractType.Select(x => new Item { Id = x.ContractTypeId, Name = x.Description });
            ddlContractType.ListItems = new SelectList(items, "Id", "Name");

            return ddlContractType;
        }
        public MultiSelectVm GetContractTypeMultiSelectVm()
        {
            var contractType = _contractService.GetContractTypes();
            var ddlContractType = new MultiSelectVm();
            var items = contractType.Select(x => new Item { Id = x.ContractTypeId, Name = x.Description });
            ddlContractType.ListItems = new SelectList(items, "Id", "Name");

            return ddlContractType;
        }

        public SingleSelectVm GetStatusSingleSelectVm()
        {
            var status = _contractService.GetStatusContract("Status");
            var ddlStatus = new SingleSelectVm();
            var items = status.Select(x => new Item { Id = x.StatusContractId, Name = x.Description });
            ddlStatus.ListItems = new SelectList(items, "Id", "Name");

            return ddlStatus;
        }

        public SingleSelectVm GetPercentagesSingleSelectVm()
        {
            var percentages = _partnerService.GetPercentageDefinitions();
            var ddlPercentages = new SingleSelectVm();
            var items = percentages.Select(x => new Item { Id = x.PercentageDefinitionId, Name = x.Name });
            ddlPercentages.ListItems = new SelectList(items, "Id", "Name");

            return ddlPercentages;
        }

        public SingleSelectVm GetCustomersSingleSelectVm()
        {
            var customers = _customerService.GetCustomersComplete();
            var ddlCustomers = new SingleSelectVm();
            var items = customers.Select(x => new Item { Id = x.CustomerId, Name = x.Name });
            ddlCustomers.ListItems = new SelectList(items, "Id", "Name");

            return ddlCustomers;
        }

        public SingleSelectVm GetPaperStatusSingleSelectVm()
        {
            var PaperStatus = _contractService.GetStatusContract("PaperStatus");
            var ddlPaperStatus = new SingleSelectVm();
            var items = PaperStatus.Select(x => new Item { Id = x.StatusContractId, Name = x.Description });
            ddlPaperStatus.ListItems = new SelectList(items, "Id", "Name");

            return ddlPaperStatus;
        }

        public SingleSelectVm GetPartnersSingleSelectVm()
        {
            var Partners = _partnerService.GetPartners();
            var ddlPartners = new SingleSelectVm();
            var items = Partners.Select(x => new Item { Id = x.PartnerId, Name = x.Name });
            ddlPartners.ListItems = new SelectList(items, "Id", "Name");

            return ddlPartners;
        }

        public SingleSelectVm GetStatusDetailSingleSelectVm()
        {
            var statusDetail = _contractService.GetStatusContract("StatusDetail");
            var ddlStatusDetail = new SingleSelectVm();
            var items = statusDetail.Select(x => new Item { Id = x.StatusContractId, Name = x.Description });
            ddlStatusDetail.ListItems = new SelectList(items, "Id", "Name");

            return ddlStatusDetail;
        }
    }
}