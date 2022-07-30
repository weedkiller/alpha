using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Multigroup.Services.Shared
{
    public class ProviderService
    {
        private ProviderRepository _providerRepository;

        public ProviderService()
        {
            _providerRepository = new ProviderRepository();
        }


        public Provider GetProvidersById(int providerId)
        {
            return _providerRepository.GetProviderById(providerId);
        }

        public IEnumerable<Provider> GetProviders()
        {
            return _providerRepository.GetProviders();
        }

        public IEnumerable<Provider> GetProvidersWithId()
        {
            List<Provider> proveedoresCompleto = new List<Provider>();

            IEnumerable<Provider> proveedores = _providerRepository.GetProviders();

            foreach (Provider proveedor in proveedores)
            {
                if (proveedor.IsEmployee == false)
                {
                    proveedor.Name = proveedor.Name + " - " + proveedor.ProviderId;
                    proveedoresCompleto.Add(proveedor);
                }
            }

            return proveedoresCompleto;
        }

        public IEnumerable<Provider> GetEmployeesWithId()
        {
            List<Provider> proveedoresCompleto = new List<Provider>();

            IEnumerable<Provider> proveedores = _providerRepository.GetProviders();

            foreach (Provider proveedor in proveedores)
            {
                if (proveedor.IsEmployee == true)
                {
                    proveedor.Name = proveedor.Name + " - " + proveedor.ProviderId;
                    proveedoresCompleto.Add(proveedor);
                }
            }

            return proveedoresCompleto;
        }

        public IEnumerable<Provider> GetProvidersAndEmployeesWithId()
        {
            List<Provider> proveedoresCompleto = new List<Provider>();

            IEnumerable<Provider> proveedores = _providerRepository.GetProviders();

            foreach (Provider proveedor in proveedores)
            {
                proveedor.Name = proveedor.Name + " - " + proveedor.ProviderId;
                proveedoresCompleto.Add(proveedor);
            }

            return proveedoresCompleto;
        }

        public bool ExisteProvider(string document)
        {
            IEnumerable<Provider> providers = _providerRepository.GetProviders();
            bool existe = false;

            foreach (Provider pro in providers)
            {
                if (pro.Document == document && pro.IsEmployee == false)
                    existe = true;
            }

            return existe;
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
            return _providerRepository.GetProvidersSummary(SelectedProviderType.Value.ToString(), DateFrom, DateTo, BalanceFrom, BalanceTo, active);
        }

        public IEnumerable<ProviderType> GetProviderTypes()
        {
            return _providerRepository.GetProviderTypes();
        }

        public IEnumerable<IVAPosition> GetIVAPositions()
        {
            return _providerRepository.GetIVAPositions();
        }

        public void AddProvider(Provider Provider)
        {
            _providerRepository.AddProvider(Provider);
        }

        public void UpdateProvider(Provider Provider)
        {
            _providerRepository.UpdateProvider(Provider);
        }


        public IEnumerable<ProviderCurrentAcount> GetProviderCurrentAcount(string Providers, string BalanceFrom, string BalanceTo, string NotImput)
        {
            return _providerRepository.GetProviderCurrentAcount(Providers, BalanceFrom, BalanceTo, NotImput);
        }

        public IEnumerable<ProviderCurrentAcountDetail> GetProviderCurrentAcountDetail(int providerId)
        {
            return _providerRepository.GetProviderCurrentAcountDetail(providerId);
        }
    }
}
