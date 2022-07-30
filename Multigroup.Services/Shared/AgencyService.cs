using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Services.Shared
{
    public class AgencyService
    {
        private AgencyRepository _agencyRepository;

        public AgencyService()
        {
            _agencyRepository = new AgencyRepository();
        }

        public Agency GetAgency(int AgencyId)
        {
            return _agencyRepository.GetAgencyById(AgencyId);
        }

        public Agency GetAgencysById(int agencyId)
        {
            return _agencyRepository.GetAgencyById(agencyId);
        }

        public IEnumerable<Agency> GetAgencys()
        {
            return _agencyRepository.GetAgencys();
        }

        public CarteraFinal GetCarteraClientes()
        {

            double acumuladoMaxi = 0;
            double acumuladoCucu = 0;
            double acumTotal = 0;
            int cantidadMaxi = 0;
            int cantidadCucu = 0;


            CarteraFinal carFinal = new CarteraFinal();

            string contratosCucu = "";
            string contratosMaxi = "";



            bool cucu = true;



            IEnumerable<Cartera> carteras = _agencyRepository.GetCartera();

            foreach (Cartera cartera in carteras)
            {
                IEnumerable<Contract> contratos  = _agencyRepository.getVistaContratos(cartera.Nombre);

                foreach (Contract contrato in contratos)
                {

                    acumTotal = acumTotal + contrato.AdvancesAmount;
                    if (cucu)
                    {
                        contratosCucu += ", " + contrato.ContractId;
                        acumuladoCucu += contrato.AdvancesAmount;
                        cantidadCucu++;
                        if ((acumuladoCucu / acumTotal * 100) >= 30)
                            cucu = false;
                    }
                    else
                    {
                        contratosMaxi += ", " + contrato.ContractId;
                        acumuladoMaxi+= contrato.AdvancesAmount;
                        cantidadMaxi++;
                        if ((acumuladoMaxi / acumTotal * 100) >= 70)
                            cucu = true;
                    }
                }
            }

            carFinal.ContratosCucu = contratosCucu;
            carFinal.ContratosMaxi = contratosMaxi;
            carFinal.MontoCucu = acumuladoCucu;
            carFinal.MontoMaxi = acumuladoMaxi;
            carFinal.Total = cantidadCucu + cantidadMaxi;
            carFinal.TotalCucu = cantidadCucu;
            carFinal.TotalMaxi = cantidadMaxi;

            return carFinal;
        }

        public IEnumerable<AgencyType> GetAgencyTypes()
        {
            return _agencyRepository.GetAgencyTypes();
        }

        public void AddAgency(Agency agency)
        {
            _agencyRepository.AddAgency(agency);
        }

        public void DeleteAgencys(int agencyId)
        {
            _agencyRepository.Delete(agencyId);
        }

        public void UpdateAgencys(Agency agency)
        {
            _agencyRepository.Update(agency);
        }
    }
}
