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
    public class SalaryAllocationService
    {
        private SalaryAllocationRepository _salaryAllocationRepository;
        private SpendingRepository _spendingRepository;
        private ProviderRepository _providerRepository;

        public SalaryAllocationService()
        {
            _salaryAllocationRepository = new SalaryAllocationRepository();
            _spendingRepository = new SpendingRepository();
            _providerRepository = new ProviderRepository();
        }


        public SalaryAllocation GetSalaryAllocationsById(int salaryAllocationId)
        {
            return _salaryAllocationRepository.GetSalaryAllocationById(salaryAllocationId);
        }

        public IEnumerable<SalaryAllocation> GetSalaryAllocations()
        {
            return _salaryAllocationRepository.GetSalaryAllocations();
        }

        public IEnumerable<SalaryAllocationSummary> GetSalaryAllocationsSummary(int? SelectedProvider, string DateFromStart, string DateToStart, string DateFromEnd, string DateToEnd, string AmountFrom, string AmountTo, string Authorized, int? AuthorizedUser)
        {
            if (SelectedProvider == null)
                SelectedProvider = 0;
            if (AuthorizedUser == null)
                AuthorizedUser = 0;
            if (DateFromStart == "")
                DateFromStart = "01/01/1900";
            if (DateToStart == "")
                DateToStart = "01/01/2100";
            if (DateFromEnd == "")
                DateFromEnd = "01/01/1900";
            if (DateToEnd == "")
                DateToEnd = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            return _salaryAllocationRepository.GetSalaryAllocationsSummary(SelectedProvider.Value.ToString(), DateFromStart, DateToStart, DateFromEnd, DateToEnd, AmountFrom, AmountTo, Authorized, AuthorizedUser.Value.ToString());
        }

        public IEnumerable<SalaryAllocationSummary> GetSalarySettlementSummary(int? SelectedProvider, string Period)
        {
            if (SelectedProvider == null)
                SelectedProvider = 0;
            if (Period == "")
                Period = "0";
            return _salaryAllocationRepository.GetSalarySettlementSummary(SelectedProvider.Value.ToString(), Period);
        }

        public IEnumerable<SalaryAllocationSummary> GetGenerateSalarySettlementSummary(string Period)
        {
            try
            {
                string[] aux = Period.Split('-');
                if (aux.Length == 2)
                {
                    string Month = aux[0];
                    string Year = "20" + aux[1];
                    DateTime DatePeriod = new DateTime(int.Parse(Year), int.Parse(Month), 1);
                    return _salaryAllocationRepository.GetGenerateSalarySettlementSummary(Period, DatePeriod.ToShortDateString());
                }
                else
                    return _salaryAllocationRepository.GetGenerateSalarySettlementSummary(Period, "Mal");
            }
            catch (Exception ex)
            {
                return _salaryAllocationRepository.GetGenerateSalarySettlementSummary(Period, "Mal");
            }
        }

        public void AddSalaryAllocation(SalaryAllocation SalaryAllocation)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    int scheduled = _salaryAllocationRepository.AddSalaryAllocation(SalaryAllocation);
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

        public void DeleteSalaryAllocation(int salaryAllocationId)
        {
            _salaryAllocationRepository.DeleteSalaryAllocation(salaryAllocationId);
        }

        public void UpdateSalaryAllocation(SalaryAllocation SalaryAllocation)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _salaryAllocationRepository.UpdateSalaryAllocation(SalaryAllocation);
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

        public void GenerateSettlement(List<Int32> ListaIds, string period, int userId, string description)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    foreach (int id in ListaIds)
                    {
                        SalaryAllocation sueldo = _salaryAllocationRepository.GetSalaryAllocationById(id);
                        Spending gasto = new Spending();
                        gasto.Amount = sueldo.Amount;
                        gasto.ProveedorId = sueldo.ProveedorId;
                        gasto.SpendingTypeId = (int)SpendingType.Salary;
                        gasto.ExecutionDate = DateTime.Now;
                        gasto.ExpirationDate = DateTime.Now;
                        gasto.Balance = sueldo.Amount;
                        gasto.Period = period;
                        gasto.UserId = userId;
                        gasto.Description = description;

                        int idGasto = _spendingRepository.AddSpending(gasto);

                        SpendingDetail detalle = new SpendingDetail();
                        detalle.ArticleId = (int)ArticlesEspecial.Salary;
                        detalle.SpendingId = idGasto;
                        detalle.Price = sueldo.Amount;
                        detalle.Total = sueldo.Amount;
                        detalle.Quantity = 1;
                        detalle.SalaryImputationId = id;

                        _spendingRepository.AddSpendingDetail(detalle);

                        sueldo.LastMonth = period;
                        _salaryAllocationRepository.UpdateSalaryAllocation(sueldo);

                        Provider provider = _providerRepository.GetProviderById(sueldo.ProveedorId);
                        provider.Balance = provider.Balance + sueldo.Amount;
                        _providerRepository.UpdateProvider(provider);


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
    }
}
