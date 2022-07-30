using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared.Mappers;
using Multigroup.Repositories.Shared.Resources;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;


namespace Multigroup.Repositories.Shared
{
    public class ContractRepository : BaseRepository
    {
        public Contract GetContractById(int contractId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Contract_Get", ContractMapper.Mapper, contractId);

            return GetFirstElement(data);
        }

        public Contract GetContractWebById(int contractId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractWeb_Get", ContractMapper.Mapper, contractId);

            return GetFirstElement(data);
        }

        public QuotationContract GetQuotationContract(int contractId)
        {
            var data = Db.ExecuteSprocAccessor("pub_QuotationContract_Get", GenericMapper<QuotationContract>.Mapper, contractId);

            return GetFirstElement(data);
        }

        public Contract GetContractWebWithStatusById(int contractId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractWebWithStatus_Get", ContractMapper.Mapper, contractId);

            return GetFirstElement(data);
        }

        public Contract GetContractWebSignById(int contractId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractWebSign_Get", ContractMapper.Mapper, contractId);

            return GetFirstElement(data);
        }

        public Contract GetContractByCustomerId(int customerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractByCustomerId_Get", ContractMapper.Mapper, customerId);

            return GetFirstElement(data);
        }

        public Contract GetContractWebByCustomerId(int customerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractWebByCustomerId_Get", ContractMapper.Mapper, customerId);

            return GetFirstElement(data);
        }


        public ContractResume GetContractResume(int contractId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractResume_GetList", GenericMapper<ContractResume>.Mapper, contractId);

            return GetFirstElement(data);
        }

        public ContractAmount GetAmountByContractId(int contractId, string PaymentDate)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractAmountByContractId_Get", GenericMapper<ContractAmount>.Mapper, contractId, PaymentDate);

            return GetFirstElement(data);
        }

        public CuotasContrato GetCuotasContratoByNumero(int numero)
        {
            var data = Db.ExecuteSprocAccessor("pub_CuotasContratoByNumero_Get", GenericMapper<CuotasContrato>.Mapper, numero);

            return GetFirstElement(data);
        }

        public void AdjudicationControl()
        {
            var command = Db.GetStoredProcCommand("pub_adjudicationControl_update");
            Db.ExecuteScalar(command);
        }

        public void ContractStatusControl()
        {
            var command = Db.GetStoredProcCommand("pub_ContractStatusControl_update");
            Db.ExecuteScalar(command);
        }

        public void ExpiredInstallment()
        {
            var command = Db.GetStoredProcCommand("pub_ExpiredInstallment_update");
            Db.ExecuteScalar(command);
        }

        public void ExpiredVisits()
        {
            var command = Db.GetStoredProcCommand("pub_ExpiredVisits_update");
            Db.ExecuteScalar(command);
        }

        public void Control()
        {
            var command = Db.GetStoredProcCommand("pub_Control_insert");
            Db.ExecuteScalar(command);
        }

        public void ContractStatusControlRegular(int ContractId, DateTime PaymentDate)
        {
            var command = Db.GetStoredProcCommand("pub_ContractStatusControlregular_update");
            Db.AddInParameter(command, "@ContractId", DbType.Int32, ContractId);
            Db.AddInParameter(command, "@PaymentDate", DbType.DateTime, PaymentDate);
            Db.ExecuteScalar(command);
        }

        public void GenerateInterests(int ContractId, int InstallmentId)
        {
            var command = Db.GetStoredProcCommand("pub_GenerateInterests_insert");
            Db.AddInParameter(command, "@ContractId", DbType.Int32, ContractId);
            Db.AddInParameter(command, "@InstallmentId", DbType.Int32, InstallmentId);

            Db.ExecuteScalar(command);
        }

        public IEnumerable<InstallmentInsterestsSummary> GetInstallmentInsterestsByContractId(int ContractId)
        {
            var data = Db.ExecuteSprocAccessor("pub_InstallmentInterestsByContractId_Get", GenericMapper<InstallmentInsterestsSummary>.Mapper, ContractId);

            return data.ToList();
        }

        public IEnumerable<StatusReport> GetStatusReport(string _selectedStatus, string DateFrom, string DateTo)
        {
            var data = Db.ExecuteSprocAccessor("pub_StatusReport_GetList", GenericMapper<StatusReport>.Mapper, _selectedStatus, DateTo, DateFrom);

            return data.ToList();
        }

        public IEnumerable<SupervisorReport> GetSupervisorReport(string desde, string hasta, string ContractType)
        {
            var data = Db.ExecuteSprocAccessor("pub_SupervisorReport_Get", GenericMapper<SupervisorReport>.Mapper, desde, hasta, ContractType);

            return data.ToList();
        }

        public IEnumerable<PortfolioReport> GetPortfolioReport(string fecha, string user)
        {
            var data = Db.ExecuteSprocAccessor("pub_PortfolioReport_Get", GenericMapper<PortfolioReport>.Mapper, fecha, user);

            return data.ToList();
        }

        public IEnumerable<StatusContractHistory> GetStatusContractHistoryByContractId(int ContractId)
        {
            var data = Db.ExecuteSprocAccessor("pub_StatusContractHistoryByContractId_Get", GenericMapper<StatusContractHistory>.Mapper, ContractId);

            return data.ToList();
        }


        public Installment GetInstallmentById(int installmentId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Installment_Get", GenericMapper<Installment>.Mapper, installmentId);

            return GetFirstElement(data);
        }

        public Installment GetInstallmentsByContractIdNumber(int contractId, int number)
        {
            var data = Db.ExecuteSprocAccessor("pub_InstallmentByContractIdNumber_Get", GenericMapper<Installment>.Mapper, contractId, number);

            return GetFirstElement(data);
        }

        public IEnumerable<PaymentAllocation> GetPaymentAllocationByInstallmentId(int installmentId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentAllocationByInstallmentId_Get", GenericMapper<PaymentAllocation>.Mapper, installmentId);

            return data.ToList();
        }

        public IEnumerable<Contract> GetContracts()
        {
            var data = Db.ExecuteSprocAccessor("pub_Contract_GetList", ContractMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<Contract> GetContractsMigracion()
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractMigracion_GetList", ContractMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<Contract> GetContractByDNI(int DNI)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractByDNI_GetList", ContractMapper.Mapper, DNI);

            return data.ToList();
        }

        public Installment GetInstallmentByContract(int ContractId)
        {
            var data = Db.ExecuteSprocAccessor("pub_InstallmentByContract_Get", GenericMapper<Installment>.Mapper, ContractId);

            return GetFirstElement(data);
        }

        public IEnumerable<Contract> GetContractByNumber(string number)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractByNumber_GetList", ContractMapper.Mapper, number);

            return data.ToList();
        }

        public IEnumerable<Installment> GetInstallments()
        {
            var data = Db.ExecuteSprocAccessor("pub_Installment_GetList", GenericMapper<Installment>.Mapper);

            return data.ToList();
        }

        public IEnumerable<Installment> GetInstallmentsByContractId(int contractId)
        {
            var data = Db.ExecuteSprocAccessor("pub_InstallmentByContractId_GetList", GenericMapper<Installment>.Mapper, contractId);

            return data.ToList();
        }

        public IEnumerable<ContractType> GetContractTypes()
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractType_GetList", GenericMapper<ContractType>.Mapper);

            return data.ToList();
        }

        public IEnumerable<PaymentPlace> GetPaymentPlaces()
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentPlace_GetList", GenericMapper<PaymentPlace>.Mapper);

            return data.ToList();
        }

        public IEnumerable<ContractSummary> GetContractByFilter(string status, string statusDetail, string clients, string contractType, int? agency, string desde, string hasta)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractByFilter_GetList", ContractSummaryMapper.Mapper, status, statusDetail, clients, contractType, agency, desde, hasta);

            return data.ToList();
        }

        public IEnumerable<ContractSummary> GetContractByFilterTMK(string status, string statusDetail, string clients, string contractType, int? agency, string desde, string hasta, int userId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractByFilterTMK_GetList", ContractSummaryMapper.Mapper, status, statusDetail, clients, contractType, agency, desde, hasta, userId);

            return data.ToList();
        }

        public IEnumerable<ContractCuotas> GetContractCuotas(int contractId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractCuotas_GetList", ContractCuotasMapper.Mapper, contractId);

            return data.ToList();
        }

        public IEnumerable<ContractSummary> GetContractWebByFilter(string status, string clients, string desde, string hasta, int userId, int supervisor, int gerente)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractWebByFilter_GetList", ContractSummaryMapper.Mapper, status, clients, desde, hasta, userId, supervisor, gerente);

            return data.ToList();
        }

        public ContractSummaryTotal GetContractWebByFilterTotal(string status, string clients, string desde, string hasta, int userId, int supervisor, int gerente)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractWebByFilterTotal_GetList", ContractSummaryTotalMapper.Mapper, status, clients, desde, hasta, userId, supervisor, gerente);

            return GetFirstElement(data);
        }

        public void Delete(int contractId)
        {
            var command = Db.GetStoredProcCommand("pub_Contract_Delete");
            Db.AddInParameter(command, "@ContractId", DbType.Int32, contractId);
            Db.ExecuteScalar(command);
        }

        public void DeleteWeb(int contractId)
        {
            var command = Db.GetStoredProcCommand("pub_ContractWeb_Delete");
            Db.AddInParameter(command, "@ContractId", DbType.Int32, contractId);
            Db.ExecuteScalar(command);
        }
        public void ConfirmContracts(int contractId)
        {
            //TODO hacer el SP para pasar el contrato y actualizar el estado del web
            var command = Db.GetStoredProcCommand("pub_ConfirmContract_Update");
            Db.AddInParameter(command, "@ContractId", DbType.Int32, contractId);
            Db.ExecuteScalar(command);
        }

        public void DeleteInstallment(int contractId)
        {
            var command = Db.GetStoredProcCommand("pub_Installment_Delete");
            Db.AddInParameter(command, "@ContractId", DbType.Int32, contractId);
            Db.ExecuteScalar(command);
        }

        public void DeleteInterestById(int InterestId)
        {
            var command = Db.GetStoredProcCommand("pub_InstallmentInterests_Delete");
            Db.AddInParameter(command, "@installmentInterestsId", DbType.Int32, InterestId);
            Db.ExecuteScalar(command);
        }

        public void UpdateInstallments(int id, int InstallmentNumber)
        {
            var command = Db.GetStoredProcCommand("pub_UpdateInstallments");
            Db.AddInParameter(command, "@ContractId", DbType.Int32, id);
            Db.AddInParameter(command, "@InstallmentNumber", DbType.Int32, InstallmentNumber);
            Db.ExecuteScalar(command);
        }
        public void DeleteInterestByContract(int contractId)
        {
            var command = Db.GetStoredProcCommand("pub_InstallmentInterests_DeleteByContract");
            Db.AddInParameter(command, "@ContractId", DbType.Int32, contractId);
            Db.ExecuteScalar(command);
        }

        public int AddInstallmentInsterests(InstallmentInsterests installmentInsterests)
        {
            var command = Db.GetStoredProcCommand("pub_InstallmentInterests_Insert");

            Db.AddInParameter(command, "@ContractId", DbType.Int32, installmentInsterests.ContractId);
            Db.AddInParameter(command, "@InstallmentId", DbType.Int32, installmentInsterests.InstallmentId);
            Db.AddInParameter(command, "@PaymentId", DbType.Int32, installmentInsterests.PaymentId);
            Db.AddInParameter(command, "@Amount", DbType.Double, installmentInsterests.Amount);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int UpdateCuotasContrato(CuotasContrato cuotasContrato)
        {
            var command = Db.GetStoredProcCommand("pub_CuotasContrato_Update");

            Db.AddInParameter(command, "@CuotasContratoId", DbType.Int32, cuotasContrato.CuotasContratoId);
            Db.AddInParameter(command, "@Cuotas", DbType.Int32, cuotasContrato.Cuotas);
            Db.AddInParameter(command, "@Migrado", DbType.Int32, cuotasContrato.Migrado);
            Db.AddInParameter(command, "@NumeroContrato", DbType.Int32, cuotasContrato.NumeroContrato);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int UpdateInstallmentInsterests(InstallmentInsterestsSummary installmentInsterests)
        {
            var command = Db.GetStoredProcCommand("pub_InstallmentInterests_Update");

            Db.AddInParameter(command, "@InstallmentInterestsId", DbType.Int32, installmentInsterests.InstallmentInterestsId);
            Db.AddInParameter(command, "@ContractId", DbType.Int32, installmentInsterests.ContractId);
            Db.AddInParameter(command, "@InstallmentId", DbType.Int32, installmentInsterests.InstallmentId);
            Db.AddInParameter(command, "@PaymentId", DbType.Int32, installmentInsterests.PaymentId);
            Db.AddInParameter(command, "@Amount", DbType.Double, installmentInsterests.Amount);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdateInstallmentAmount(int contractId, double amount)
        {
            var command = Db.GetStoredProcCommand("pub_InstallmentAmount_Update");

            Db.AddInParameter(command, "@ContractId", DbType.Int32, contractId);
            Db.AddInParameter(command, "@Amount", DbType.Double, amount);

            Db.ExecuteScalar(command);
        }

        public void UpdateInstallmentDate(int contractId, int cuota)
        {
            var command = Db.GetStoredProcCommand("pub_Installment_UpdateDate");

            Db.AddInParameter(command, "@ContractId", DbType.Int32, contractId);
            Db.AddInParameter(command, "@Installment", DbType.Int32, cuota);

            Db.ExecuteScalar(command);
        }

        public int AddContract(Contract contract, int user)
        {
            var command = Db.GetStoredProcCommand("pub_Contract_Insert");

            Db.AddInParameter(command, "@AdmissionDate", DbType.DateTime, contract.AdmissionDate);
            Db.AddInParameter(command, "@AdvancesAmount", DbType.Double, contract.AdvancesAmount);
            Db.AddInParameter(command, "@AgencyId", DbType.Int32, contract.AgencyId);
            Db.AddInParameter(command, "@CustomerId", DbType.Int32, contract.CustomerId);
            Db.AddInParameter(command, "@Discount", DbType.Int32, contract.Discount);
            Db.AddInParameter(command, "@FirstAdvanceAmount", DbType.Double, contract.FirstAdvanceAmount);
            Db.AddInParameter(command, "@GoodRequested", DbType.String, contract.GoodRequested);
            Db.AddInParameter(command, "@GoodRequestedColour", DbType.String, contract.GoodRequestedColour);
            Db.AddInParameter(command, "@Number", DbType.Int32, contract.Number);
            Db.AddInParameter(command, "@Observations", DbType.String, contract.Observations);
            Db.AddInParameter(command, "@PaymentDateId", DbType.Int32, contract.PaymentDateId);
            Db.AddInParameter(command, "@RegistrationDate", DbType.DateTime, contract.RegistrationDate);
            Db.AddInParameter(command, "@SellerId", DbType.Int32, contract.SellerId);
            Db.AddInParameter(command, "@SupervisorId", DbType.Int32, contract.SupervisorId);
            Db.AddInParameter(command, "@ManagerId", DbType.Int32, contract.ManagerId);
            Db.AddInParameter(command, "@StatusDetail", DbType.Int32, contract.StatusDetail);
            Db.AddInParameter(command, "@Status", DbType.Int32, contract.Status);
            Db.AddInParameter(command, "@PaperStatus", DbType.Int32, contract.PaperStatus);
            Db.AddInParameter(command, "@SubscriptionAmount", DbType.Double, contract.SubscriptionAmount);
            Db.AddInParameter(command, "@VehicleBrand", DbType.String, contract.VehicleBrand);
            Db.AddInParameter(command, "@VehicleMileage", DbType.Int32, contract.VehicleMileage);
            Db.AddInParameter(command, "@VehicleModel", DbType.String, contract.VehicleModel);
            Db.AddInParameter(command, "@VehicleRegistrationPlate", DbType.String, contract.VehicleRegistrationPlate);
            Db.AddInParameter(command, "@FirstAdvanceDiscount", DbType.Int32, contract.FirstAdvanceDiscount);
            Db.AddInParameter(command, "@SecondAdvanceDiscount", DbType.Int32, contract.SecondAdvanceDiscount);
            Db.AddInParameter(command, "@ThirdAdvanceDiscount", DbType.Int32, contract.ThirdAdvanceDiscount);
            Db.AddInParameter(command, "@PaymentPreference", DbType.Int32, contract.PaymentPreference);
            Db.AddInParameter(command, "@PaymentPlace", DbType.Int32, contract.PaymentPlace);
            Db.AddInParameter(command, "@ContractType", DbType.Int32, contract.ContractType);
            Db.AddInParameter(command, "@Expenses", DbType.Double, contract.Expenses);
            Db.AddInParameter(command, "@Total", DbType.Double, contract.Total);
            Db.AddInParameter(command, "@Salary", DbType.Double, contract.Salary);
            Db.AddInParameter(command, "@Code", DbType.String, contract.Code);
            Db.AddInParameter(command, "@Vehicle", DbType.String, contract.Vehicle);
            Db.AddInParameter(command, "@Money", DbType.String, contract.Money);
            Db.AddInParameter(command, "@Description", DbType.String, contract.Description);
            Db.AddInParameter(command, "@Product", DbType.String, contract.Product);
            Db.AddInParameter(command, "@Measure", DbType.String, contract.Measure);
            Db.AddInParameter(command, "@Advance", DbType.Double, contract.Advance);
            Db.AddInParameter(command, "@UserId", DbType.Int32, user);


            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int AddContractWeb(Contract contract, int user)
        {
            var command = Db.GetStoredProcCommand("pub_ContractWeb_Insert");

            Db.AddInParameter(command, "@AdmissionDate", DbType.DateTime, contract.AdmissionDate);
            Db.AddInParameter(command, "@AdvancesAmount", DbType.Double, contract.AdvancesAmount);
            Db.AddInParameter(command, "@AgencyId", DbType.Int32, contract.AgencyId);
            Db.AddInParameter(command, "@CustomerId", DbType.Int32, contract.CustomerId);
            Db.AddInParameter(command, "@Discount", DbType.Int32, contract.Discount);
            Db.AddInParameter(command, "@FirstAdvanceAmount", DbType.Double, contract.FirstAdvanceAmount);
            Db.AddInParameter(command, "@GoodRequested", DbType.String, contract.GoodRequested);
            Db.AddInParameter(command, "@GoodRequestedColour", DbType.String, contract.GoodRequestedColour);
            Db.AddInParameter(command, "@Number", DbType.Int32, contract.Number);
            Db.AddInParameter(command, "@Observations", DbType.String, contract.Observations);
            Db.AddInParameter(command, "@PaymentDateId", DbType.Int32, contract.PaymentDateId);
            Db.AddInParameter(command, "@RegistrationDate", DbType.DateTime, contract.RegistrationDate);
            Db.AddInParameter(command, "@SellerId", DbType.Int32, user);
            Db.AddInParameter(command, "@SupervisorId", DbType.Int32, contract.SupervisorId);
            Db.AddInParameter(command, "@ManagerId", DbType.Int32, contract.ManagerId);
            Db.AddInParameter(command, "@StatusDetail", DbType.Int32, contract.StatusDetail);
            Db.AddInParameter(command, "@Status", DbType.Int32, contract.Status);
            Db.AddInParameter(command, "@PaperStatus", DbType.Int32, contract.PaperStatus);
            Db.AddInParameter(command, "@SubscriptionAmount", DbType.Double, contract.SubscriptionAmount);
            Db.AddInParameter(command, "@VehicleBrand", DbType.String, contract.VehicleBrand);
            Db.AddInParameter(command, "@VehicleMileage", DbType.Int32, contract.VehicleMileage);
            Db.AddInParameter(command, "@VehicleModel", DbType.String, contract.VehicleModel);
            Db.AddInParameter(command, "@VehicleRegistrationPlate", DbType.String, contract.VehicleRegistrationPlate);
            Db.AddInParameter(command, "@FirstAdvanceDiscount", DbType.Int32, contract.FirstAdvanceDiscount);
            Db.AddInParameter(command, "@SecondAdvanceDiscount", DbType.Int32, contract.SecondAdvanceDiscount);
            Db.AddInParameter(command, "@ThirdAdvanceDiscount", DbType.Int32, contract.ThirdAdvanceDiscount);
            Db.AddInParameter(command, "@PaymentPreference", DbType.Int32, contract.PaymentPreference);
            Db.AddInParameter(command, "@PaymentPlace", DbType.Int32, contract.PaymentPlace);
            Db.AddInParameter(command, "@ContractType", DbType.Int32, contract.ContractType);
            Db.AddInParameter(command, "@Expenses", DbType.Double, contract.Expenses);
            Db.AddInParameter(command, "@Total", DbType.Double, contract.Total);
            Db.AddInParameter(command, "@Salary", DbType.Double, contract.Salary);
            Db.AddInParameter(command, "@Code", DbType.String, contract.Code);
            Db.AddInParameter(command, "@Vehicle", DbType.String, contract.Vehicle);
            Db.AddInParameter(command, "@Money", DbType.String, contract.Money);
            Db.AddInParameter(command, "@Description", DbType.String, contract.Description);
            Db.AddInParameter(command, "@Product", DbType.String, contract.Product);
            Db.AddInParameter(command, "@Measure", DbType.String, contract.Measure);
            Db.AddInParameter(command, "@Advance", DbType.Double, contract.Advance);
            Db.AddInParameter(command, "@UserId", DbType.Int32, user);


            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int AddInstallment(int contrato, int numero, double monto, int? periodoPago, int fecha)
        {
            var command = Db.GetStoredProcCommand("pub_Installment_Insert");

            Db.AddInParameter(command, "@ContractId", DbType.Int32, contrato);
            Db.AddInParameter(command, "@Number", DbType.Int32, numero);
            Db.AddInParameter(command, "@InitialDate", DbType.Int32, fecha);
            Db.AddInParameter(command, "@DueDate", DbType.Int32, fecha);
            Db.AddInParameter(command, "@OriginalAmount", DbType.Double, monto);
            Db.AddInParameter(command, "@PaymentDate", DbType.Int32, periodoPago);
            Db.AddInParameter(command, "@Status", DbType.String, 7);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int AddQuotaionContract(QuotationContract quotation)
        {
            var command = Db.GetStoredProcCommand("pub_QuotationContract_Insert");

            Db.AddInParameter(command, "@ContractId", DbType.Int32, quotation.ContractId);
            Db.AddInParameter(command, "@Quotation", DbType.Double, quotation.Quotation);
            Db.AddInParameter(command, "@QuotationDolar", DbType.Double, quotation.QuotationDolar);
            Db.AddInParameter(command, "@QuotationDate", DbType.DateTime, quotation.QuotationDate);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void AddStatusContractHistory(int ContractId, int UserId, int StatusOld, int StatusNew, string Observations, DateTime Date)
        {
            var command = Db.GetStoredProcCommand("pub_StatusContractHistory_Insert");

            Db.AddInParameter(command, "@ContractId", DbType.Int32, ContractId);
            Db.AddInParameter(command, "@StatusOld", DbType.Int32, StatusOld);
            Db.AddInParameter(command, "@StatusNew", DbType.Int32, StatusNew);
            Db.AddInParameter(command, "@Observations", DbType.String, Observations);
            Db.AddInParameter(command, "@UserId", DbType.Double, UserId);
            Db.AddInParameter(command, "@Date", DbType.DateTime, Date);

            Db.ExecuteScalar(command);
        }

        public void UpdateInstallment(Installment cuota)
        {
            var command = Db.GetStoredProcCommand("pub_Installment_Update");

            Db.AddInParameter(command, "@InstallmentId", DbType.Int32, cuota.InstallmentId);
            Db.AddInParameter(command, "@ContractId", DbType.Int32, cuota.ContractId);
            Db.AddInParameter(command, "@Number", DbType.Int32, cuota.Number);
            Db.AddInParameter(command, "@InitialDate", DbType.DateTime, cuota.InitialDate);
            Db.AddInParameter(command, "@DueDate", DbType.DateTime, cuota.DueDate);
            Db.AddInParameter(command, "@OriginalAmount", DbType.Double, cuota.OriginalAmount);
            Db.AddInParameter(command, "@PaymentDate", DbType.DateTime, cuota.PaymentDate);
            Db.AddInParameter(command, "@Status", DbType.String, cuota.Status);

            Db.ExecuteScalar(command);

        }

        public int AddPaymentAllocation(PaymentAllocation pago)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentAllocation_Insert");

            Db.AddInParameter(command, "@PaymentId", DbType.Int32, pago.PaymentId);
            Db.AddInParameter(command, "@InstallmentId", DbType.Int32, pago.InstallmentId);
            Db.AddInParameter(command, "@amount", DbType.Double, pago.amount);
            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int AddPaymentAdvance(int CustomerId, int ContractId, double Amount, int PaymentId)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentAdvance_Insert");

            Db.AddInParameter(command, "@CustomerId", DbType.Int32, CustomerId);
            Db.AddInParameter(command, "@ContractId", DbType.Int32, ContractId);
            Db.AddInParameter(command, "@Amount", DbType.Double, Amount);
            Db.AddInParameter(command, "@PaymentId", DbType.Int32, PaymentId);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int AddSuscription(int CustomerId, int ContractId, double Amount, int PaymentId)
        {
            var command = Db.GetStoredProcCommand("pub_Suscription_Insert");

            Db.AddInParameter(command, "@CustomerId", DbType.Int32, CustomerId);
            Db.AddInParameter(command, "@ContractId", DbType.Int32, ContractId);
            Db.AddInParameter(command, "@Amount", DbType.Double, Amount);
            Db.AddInParameter(command, "@PaymentId", DbType.Int32, PaymentId);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int AddBonification(int CustomerId, int ContractId, double Amount, int PaymentId)
        {
            var command = Db.GetStoredProcCommand("pub_Bonification_Insert");

            Db.AddInParameter(command, "@CustomerId", DbType.Int32, CustomerId);
            Db.AddInParameter(command, "@ContractId", DbType.Int32, ContractId);
            Db.AddInParameter(command, "@Amount", DbType.Double, Amount);
            Db.AddInParameter(command, "@PaymentId", DbType.Int32, PaymentId);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void Update(Contract contract)
        {
            var command = Db.GetStoredProcCommand("pub_Contract_Update");

            Db.AddInParameter(command, "@ContractId", DbType.Int32, contract.ContractId);
            Db.AddInParameter(command, "@AdmissionDate", DbType.DateTime, contract.AdmissionDate);
            Db.AddInParameter(command, "@AdvancesAmount", DbType.Double, contract.AdvancesAmount);
            Db.AddInParameter(command, "@AgencyId", DbType.Int32, contract.AgencyId);
            Db.AddInParameter(command, "@CustomerId", DbType.Int32, contract.CustomerId);
            Db.AddInParameter(command, "@Discount", DbType.Int32, contract.Discount);
            Db.AddInParameter(command, "@FirstAdvanceAmount", DbType.Double, contract.FirstAdvanceAmount);
            Db.AddInParameter(command, "@GoodRequested", DbType.String, contract.GoodRequested);
            Db.AddInParameter(command, "@GoodRequestedColour", DbType.String, contract.GoodRequestedColour);
            Db.AddInParameter(command, "@Number", DbType.Int32, contract.Number);
            Db.AddInParameter(command, "@Observations", DbType.String, contract.Observations);
            Db.AddInParameter(command, "@PaymentDateId", DbType.Int32, contract.PaymentDateId);
            Db.AddInParameter(command, "@RegistrationDate", DbType.DateTime, contract.RegistrationDate);
            Db.AddInParameter(command, "@SellerId", DbType.Int32, contract.SellerId);
            Db.AddInParameter(command, "@SupervisorId", DbType.Int32, contract.SupervisorId);
            Db.AddInParameter(command, "@ManagerId", DbType.Int32, contract.ManagerId);
            Db.AddInParameter(command, "@StatusDetail", DbType.Int32, contract.StatusDetail);
            Db.AddInParameter(command, "@Status", DbType.Int32, contract.Status);
            Db.AddInParameter(command, "@PaperStatus", DbType.Int32, contract.PaperStatus);
            Db.AddInParameter(command, "@SubscriptionAmount", DbType.Double, contract.SubscriptionAmount);
            Db.AddInParameter(command, "@VehicleBrand", DbType.String, contract.VehicleBrand);
            Db.AddInParameter(command, "@VehicleMileage", DbType.Int32, contract.VehicleMileage);
            Db.AddInParameter(command, "@VehicleModel", DbType.String, contract.VehicleModel);
            Db.AddInParameter(command, "@VehicleRegistrationPlate", DbType.String, contract.VehicleRegistrationPlate);
            Db.AddInParameter(command, "@FirstAdvanceDiscount", DbType.Int32, contract.FirstAdvanceDiscount);
            Db.AddInParameter(command, "@SecondAdvanceDiscount", DbType.Int32, contract.SecondAdvanceDiscount);
            Db.AddInParameter(command, "@ThirdAdvanceDiscount", DbType.Int32, contract.ThirdAdvanceDiscount);
            Db.AddInParameter(command, "@PaymentPreference", DbType.Int32, contract.PaymentPreference);
            Db.AddInParameter(command, "@PaymentPlace", DbType.Int32, contract.PaymentPlace);
            Db.AddInParameter(command, "@ContractType", DbType.Int32, contract.ContractType);
            Db.AddInParameter(command, "@Expenses", DbType.Double, contract.Expenses);
            Db.AddInParameter(command, "@Total", DbType.Double, contract.Total);
            Db.AddInParameter(command, "@Salary", DbType.Double, contract.Salary);
            Db.AddInParameter(command, "@Code", DbType.String, contract.Code);
            Db.AddInParameter(command, "@Vehicle", DbType.String, contract.Vehicle);
            Db.AddInParameter(command, "@Money", DbType.String, contract.Money);
            Db.AddInParameter(command, "@Description", DbType.String, contract.Description);
            Db.AddInParameter(command, "@Product", DbType.String, contract.Product);
            Db.AddInParameter(command, "@Measure", DbType.String, contract.Measure);
            Db.AddInParameter(command, "@Advance", DbType.Double, contract.Advance);

            Db.ExecuteScalar(command);
        }

        public void UpdateWeb(Contract contract)
        {
            var command = Db.GetStoredProcCommand("pub_ContractWeb_Update");

            Db.AddInParameter(command, "@ContractId", DbType.Int32, contract.Number);
            Db.AddInParameter(command, "@Number", DbType.Int32, contract.Number);
            Db.ExecuteScalar(command);
        }

        public IEnumerable<ContractFile> GetContractDocuments(int contractId)
        {
            return Db.ExecuteSprocAccessor("pub_ContractFile_GetList", ContractFileMapper.Mapper, contractId).ToList();
        }

        public IEnumerable<ContractFile> GetContractWebDocuments(int contractId)
        {
            return Db.ExecuteSprocAccessor("pub_ContractFileWeb_GetList", ContractFileMapper.Mapper, contractId).ToList();
        }

        public void DeleteContractFileWeb(int contractId)
        {
            var command = Db.GetStoredProcCommand("pub_ContractFileWeb_Delete");
            Db.AddInParameter(command, "@ContractId", DbType.Int32, contractId);
            Db.ExecuteScalar(command);
        }

        public void UpdateContractStatus(int contractId, int status, int statusDetail, int PaperStatus)
        {
            var command = Db.GetStoredProcCommand("pub_ContractStatus_Update");

            Db.AddInParameter(command, "@ContractId", DbType.Int32, contractId);

            Db.AddInParameter(command, "@StatusDetail", DbType.Int32, statusDetail);
            Db.AddInParameter(command, "@Status", DbType.Int32, status);
            Db.AddInParameter(command, "@PaperStatus", DbType.Int32, PaperStatus);

            Db.ExecuteScalar(command);
        }

        public void UpdateContractWebStatus(int contractId)
        {
            var command = Db.GetStoredProcCommand("pub_ContractWebStatus_Update");

            Db.AddInParameter(command, "@ContractId", DbType.Int32, contractId);

            Db.ExecuteScalar(command);
        }

        public IEnumerable<IdentificationType> GetIdentificationTypes()
        {
            var data = Db.ExecuteSprocAccessor("pub_IdentificationType_GetList", IdentificationTypeMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<MaritalStatus> GetMaritalStatus()
        {
            var data = Db.ExecuteSprocAccessor("pub_MaritalStatus_GetList", MaritalStatusMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<PaymentDate> GetPaymentDate()
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentDate_GetList", PaymentDateMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<PaymentMethod> GetPaymentMethod()
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentMethod_GetList", GenericMapper<PaymentMethod>.Mapper);

            return data.ToList();
        }

        public IEnumerable<PaymentMethod> GetPaymentMethodByPP(int PaymentPreferenceId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentMethodByPP_GetList", GenericMapper<PaymentMethod>.Mapper, PaymentPreferenceId);

            return data.ToList();
        }

        public IEnumerable<StatusContract> GetStatusContract(string type)
        {
            var data = Db.ExecuteSprocAccessor("pub_StatusContract_GetList", StatusContractMapper.Mapper, type);

            return data;
        }

        public StatusContract GetStatusContractById(int idStatus)
        {
            var data = Db.ExecuteSprocAccessor("pub_StatusContract_Get", StatusContractMapper.Mapper, idStatus);

            return GetFirstElement(data);
        }

        public IEnumerable<ContractType> GetContractType()
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractType_GetList", GenericMapper<ContractType>.Mapper);

            return data;
        }

        public IEnumerable<ContractChart> GetContractChart(string agencychart)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContractChart_GetList", ContractChartMapper.Mapper, agencychart);

            return data;
        }

        public IEnumerable<CustomerDebt> GetCustomerDebt()
        {
            var data = Db.ExecuteSprocAccessor("pub_CustomerDebt_GetList", GenericMapper<CustomerDebt>.Mapper);

            return data;
        }

        public IEnumerable<PaymentPreference> GetPaymentPreference()
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentPreference_GetList", GenericMapper<PaymentPreference>.Mapper);

            return data;
        }

        public PaymentPreference GetPaymentPreferenceById(int paymentPreferenceId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentPreference_Get", GenericMapper<PaymentPreference>.Mapper, paymentPreferenceId);

            return GetFirstElement(data);
        }

        public void InsertDocument(string path, HttpPostedFileBase file, int contractId)
        {
            try
            {
                var command = Db.GetStoredProcCommand("pub_ContractFile_Insert");

                Db.AddInParameter(command, "@ContractId", DbType.Int32, contractId);
                Db.AddInParameter(command, "@Path", DbType.String, path);
                Db.AddInParameter(command, "@Name", DbType.String, file.FileName);
                Db.AddInParameter(command, "@ContentLength", DbType.Int32, file.ContentLength);

                Db.ExecuteScalar(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertDocumentWeb(string path, HttpPostedFileBase file, int contractId)
        {
            try
            {
                var command = Db.GetStoredProcCommand("pub_ContractFileWeb_Insert");

                Db.AddInParameter(command, "@ContractId", DbType.Int32, contractId);
                Db.AddInParameter(command, "@Path", DbType.String, path);
                Db.AddInParameter(command, "@Name", DbType.String, file.FileName);
                Db.AddInParameter(command, "@ContentLength", DbType.Int32, file.ContentLength);

                Db.ExecuteScalar(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<InstallmentReminderSummary> GetInstallmentsBeforeStart(int daysToStart)
        {
            return Db.ExecuteSprocAccessor("pub_InstallmentBeforeStart_GetList", GenericMapper<InstallmentReminderSummary>.Mapper, daysToStart).ToList();
        }

        public IEnumerable<InstallmentReminderSummary> GetInstallmentsBeforeDue(int daysToDue)
        {
            return Db.ExecuteSprocAccessor("pub_InstallmentBeforeDueDate_GetList", GenericMapper<InstallmentReminderSummary>.Mapper, daysToDue).ToList();
        }

        public IEnumerable<InstallmentReminderSummary> GetInstallmentsAfterDue(int daysAfterDue)
        {
            return Db.ExecuteSprocAccessor("pub_InstallmentAfterDueDate_GetList", GenericMapper<InstallmentReminderSummary>.Mapper, daysAfterDue).ToList();
        }

        public Payment GetLastPayment(int contractId)
        {
            var data = Db.ExecuteSprocAccessor("pub_LastPaymentByContract_Get", GenericMapper<Payment>.Mapper, contractId);

            return GetFirstElement(data);
        }

    }

}
