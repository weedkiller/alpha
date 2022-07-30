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
    public class VisitRepository : BaseRepository
    {
        public IEnumerable<AssignVisit> GetAssignVisitByZoneAndPaymentDate(int? ZoneId, int? PaymentDateId, int UserId)
        {
            var data = Db.ExecuteSprocAccessor("query_pre_visit", GenericMapper<AssignVisit>.Mapper, ZoneId, PaymentDateId, UserId);

            return data.ToList();
        }
        public void ChangeStatus(int VisitId, int Status)
        {
            var command = Db.GetStoredProcCommand("pub_VisitStatus_Update");
            Db.AddInParameter(command, "@VisitId", DbType.Int32, VisitId);
            Db.AddInParameter(command, "@StatusVisit", DbType.Int32, Status);
            Db.ExecuteScalar(command);
        }
        public IEnumerable<VisitSummary> GetVisitsByFilters(string status, string zones, string collectors, string DateTo, string DateFrom)
        {
            var data = Db.ExecuteSprocAccessor("pub_VisitByFilter_GetList", GenericMapper<VisitSummary>.Mapper, status, zones, collectors, DateTo, DateFrom);

            return data.ToList();
        }

        public Visit GetVisitById(int visitId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Visit_Get", GenericMapper<Visit>.Mapper, visitId);

            return GetFirstElement(data);
        }

        public IEnumerable<AgencyPaymentHistory> GetAgencyPaymentHistoryByCustomer(int customerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_AgencyPaymentHistoryByCustomer_Get", GenericMapper<AgencyPaymentHistory>.Mapper, customerId);

            return data.ToList();
        }

        public IEnumerable<AgencyPaymentHistory> GetAgencyPaymentHistoryByCustomerLastMonth(int customerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_AgencyPaymentHistoryByCustomerLastMonth_Get", GenericMapper<AgencyPaymentHistory>.Mapper, customerId);

            return data.ToList();
        }

        public IEnumerable<AssignPaymentPreferenceCollector> GetAssignPaymentPreferenceSummaryPaymentCollector(string Telemarketer, string Collector, string Zone, string Customer, string DateVisitFrom, string DateVisitTo, string DatePaymentFrom, string DatePaymentTo, string IsSurrender, string IsAssign, string IsRegister)
        {
            var data = Db.ExecuteSprocAccessor("pub_AssignPaymentPreferenceCollector_GetList", GenericMapper<AssignPaymentPreferenceCollector>.Mapper, Telemarketer, Collector, Zone, Customer, DateVisitFrom, DateVisitTo, DatePaymentFrom, DatePaymentTo, IsSurrender, IsAssign, IsRegister);

            return data.ToList();
        }

        public AssignPaymentPreferenceCollector GetAssignPaymentPreferenceCollectorById(int id)
        {
            var data = Db.ExecuteSprocAccessor("pub_AssignPaymentPreferenceCollector_Get", GenericMapper<AssignPaymentPreferenceCollector>.Mapper, id);

            return GetFirstElement(data);
        }

        public InstallmentSurrender GetInstallmentSurrenderById(int id)
        {
            var data = Db.ExecuteSprocAccessor("pub_InstallmentSurrender_Get", GenericMapper<InstallmentSurrender>.Mapper, id);

            return GetFirstElement(data);
        }

        public IEnumerable<CollectorAssigmmentSummary> GetCollectorAssigmmentsByAssignPaymentAssign(int id)
        {
            var data = Db.ExecuteSprocAccessor("pub_CollectorAssigmmentByPaymentAssign_GetList", GenericMapper<CollectorAssigmmentSummary>.Mapper, id);

            return data.ToList();
        }

        public IEnumerable<CollectorState> GetCollectorState()
        {
            var data = Db.ExecuteSprocAccessor("pub_CollectorState_GetList", GenericMapper<CollectorState>.Mapper);

            return data.ToList();
        }

        public IEnumerable<CollectorSurrender> GetCollectorSurrendorByUser(int id)
        {
            var data = Db.ExecuteSprocAccessor("pub_CollectorSurrenderByUser_GetList", GenericMapper<CollectorSurrender>.Mapper, id);

            return data.ToList();
        }

        public IEnumerable<CollectorAdvancement> GetCollectorAdvancementByUser(int id)
        {
            var data = Db.ExecuteSprocAccessor("pub_CollectorAdvancementByUser_GetList", GenericMapper<CollectorAdvancement>.Mapper, id);

            return data.ToList();
        }

        public IEnumerable<InstallmentSurrenderSummary> GetInstallmentSurrenderSummary(string Collector)
        {
            var data = Db.ExecuteSprocAccessor("pub_InstallmentSurrenderSummary_GetList", GenericMapper<InstallmentSurrenderSummary>.Mapper, Collector);

            return data.ToList();
        }
        public void Insert(Visit visit, string observationsCollector, int userId)
        {
            var command = Db.GetStoredProcCommand("pub_Visit_Insert");

            Db.AddInParameter(command, "@CustomerId", DbType.Int32, visit.CustomerId);
            Db.AddInParameter(command, "@ScheduledVisitDate", DbType.DateTime, visit.ScheduledVisitDate);
            Db.AddInParameter(command, "@ConcretedVisitDate", DbType.DateTime, visit.ConcretedVisitDate);
            Db.AddInParameter(command, "@CollectorId", DbType.Int32, visit.CollectorId);
            Db.AddInParameter(command, "@CollectorComission", DbType.Double, visit.CollectorComission);
            Db.AddInParameter(command, "@AmountOwed", DbType.Double, visit.AmountOwed);
            Db.AddInParameter(command, "@AmountCollected", DbType.Double, visit.AmountCollected);
            Db.AddInParameter(command, "@TotalComission", DbType.Double, visit.TotalComission);
            Db.AddInParameter(command, "@Status", DbType.Int32, visit.Status);
            Db.AddInParameter(command, "@CollectorExpensesDesc", DbType.String, visit.CollectorExpensesDesc);
            Db.AddInParameter(command, "@CollectorExpensesAmount", DbType.Int32, visit.CollectorExpensesAmount);
            Db.AddInParameter(command, "@Observations", DbType.String, visit.Observations);
            Db.AddInParameter(command, "@ObservationsCollector", DbType.String, observationsCollector);
            Db.AddInParameter(command, "@UserId", DbType.Int32, userId);

            Db.ExecuteScalar(command);
        }

        public void AddCollectorAssigmment(CollectorAssigmment collector)
        {
            var command = Db.GetStoredProcCommand("pub_CollectorAssigmment_Insert");

            Db.AddInParameter(command, "@AssignPaymentPreferenceId", DbType.Int32, collector.AssignPaymentPreferenceId);
            Db.AddInParameter(command, "@VisitDate", DbType.DateTime, collector.VisitDate);
            Db.AddInParameter(command, "@Date", DbType.DateTime, collector.Date);
            Db.AddInParameter(command, "@UserId", DbType.Int32, collector.UserId);
            Db.AddInParameter(command, "@Active", DbType.Boolean, collector.Active);

            Db.ExecuteScalar(command);
        }

        public void UpdateCollectorAssigmmentStatus(CollectorAssigmment collector)
        {
            var command = Db.GetStoredProcCommand("pub_CollectorAssigmmentStatus_Update");

            Db.AddInParameter(command, "@AssignPaymentPreferenceId", DbType.Int32, collector.AssignPaymentPreferenceId);

            Db.ExecuteScalar(command);
        }

        public void RepeatAsignation(int monthToRepeat, int yearToRepeat, int monthToApply, int yearToApply)
        {
            var command = Db.GetStoredProcCommand("pub_CustomersUser_InsertEqual");

            Db.AddInParameter(command, "@monthToRepeat", DbType.Int32, monthToRepeat);
            Db.AddInParameter(command, "@yearToRepeat", DbType.Int32, yearToRepeat);
            Db.AddInParameter(command, "@monthToApply", DbType.Int32, monthToApply);
            Db.AddInParameter(command, "@yearToApply", DbType.Int32, yearToApply);
            Db.ExecuteScalar(command);
        }

        public void InsertAgencyPayment(AgencyPayment agencyPayment, int userId)
        {
            var command = Db.GetStoredProcCommand("pub_AgencyPayment_Insert");

            Db.AddInParameter(command, "@CustomerId", DbType.Int32, agencyPayment.CustomerId);
            Db.AddInParameter(command, "@ScheduledPaymentDate", DbType.DateTime, agencyPayment.ScheduledPaymentDate);
            Db.AddInParameter(command, "@Observations", DbType.String, agencyPayment.Observations);
            Db.AddInParameter(command, "@UserId", DbType.Int32, userId);

            Db.ExecuteScalar(command);
        }

        public void Update(Visit visit)
        {
            var command = Db.GetStoredProcCommand("pub_Visit_Update");

            Db.AddInParameter(command, "@VisitId", DbType.Int32, visit.VisitId);
            Db.AddInParameter(command, "@CustomerId", DbType.Int32, visit.CustomerId);
            Db.AddInParameter(command, "@ScheduledVisitDate", DbType.DateTime, visit.ScheduledVisitDate);
            Db.AddInParameter(command, "@ConcretedVisitDate", DbType.DateTime, visit.ConcretedVisitDate);
            Db.AddInParameter(command, "@CollectorId", DbType.Int32, visit.CollectorId);
            Db.AddInParameter(command, "@CollectorComission", DbType.Double, visit.CollectorComission);
            Db.AddInParameter(command, "@AmountOwed", DbType.Double, visit.AmountOwed);
            Db.AddInParameter(command, "@AmountCollected", DbType.Double, visit.AmountCollected);
            Db.AddInParameter(command, "@TotalComission", DbType.Double, visit.TotalComission);
            Db.AddInParameter(command, "@Status", DbType.Int32, visit.Status);
            Db.AddInParameter(command, "@CollectorExpensesDesc", DbType.String, visit.CollectorExpensesDesc);
            Db.AddInParameter(command, "@CollectorExpensesAmount", DbType.Int32, visit.CollectorExpensesAmount);
            Db.AddInParameter(command, "@Observations", DbType.String, visit.Observations);
            Db.ExecuteScalar(command);
        }

        public int AddCollectorSurrender(CollectorSurrender rendicion)
        {
            var command = Db.GetStoredProcCommand("pub_CollectorSurrender_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, rendicion.Amount);
            Db.AddInParameter(command, "@Balance", DbType.Double, rendicion.Balance);
            Db.AddInParameter(command, "@Commentary", DbType.String, rendicion.Commentary);
            Db.AddInParameter(command, "@Date", DbType.DateTime, rendicion.Date);
            Db.AddInParameter(command, "@InstallmentQuantity", DbType.Int32, rendicion.InstallmentQuantity);
            Db.AddInParameter(command, "@ReceiptQuantity", DbType.Int32, rendicion.ReceiptQuantity);
            Db.AddInParameter(command, "@UserId", DbType.Int32, rendicion.UserId);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int AddCollectorAdvancement(CollectorAdvancement avance)
        {
            var command = Db.GetStoredProcCommand("pub_CollectorAdvancement_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, avance.Amount);
            Db.AddInParameter(command, "@Balance", DbType.Double, avance.Balance);
            Db.AddInParameter(command, "@Commentary", DbType.String, avance.Commentary);
            Db.AddInParameter(command, "@Date", DbType.DateTime, avance.Date);
            Db.AddInParameter(command, "@Hour", DbType.String, avance.Hour);
            Db.AddInParameter(command, "@CollectorId", DbType.Int32, avance.CollectorId);


            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdateCollectorSurrender(CollectorSurrender rendicion)
        {
            var command = Db.GetStoredProcCommand("pub_CollectorSurrender_Update");

            Db.AddInParameter(command, "@CollectorSurrenderId", DbType.Double, rendicion.CollectorSurrenderId);
            Db.AddInParameter(command, "@Balance", DbType.Double, rendicion.Balance);

            Db.ExecuteScalar(command);
        }

        public void UpdateCollectorAdvancement(CollectorAdvancement avance)
        {
            var command = Db.GetStoredProcCommand("pub_CollectorAdvancement_Update");

            Db.AddInParameter(command, "@CollectorAdvancementId", DbType.Double, avance.CollectorAdvancementId);
            Db.AddInParameter(command, "@Balance", DbType.Double, avance.Balance);

            Db.ExecuteScalar(command);
        }

        public void AddInstallmentSurrender(InstallmentSurrender cuota)
        {
            var command = Db.GetStoredProcCommand("pub_InstallmentSurrender_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, cuota.Amount);
            Db.AddInParameter(command, "@AssignPaymentPreferenceId", DbType.Int32, cuota.AssignPaymentPreferenceId);
            Db.AddInParameter(command, "@CollectorSurrenderId", DbType.Int32, cuota.CollectorSurrenderId);

            Db.ExecuteScalar(command);
        }

        public void AddCollectorSurrenderPM(CollectorSurrenderPM collectorSurrenderPM)
        {
            var command = Db.GetStoredProcCommand("pub_CollectorSurrenderPM_Insert");

            Db.AddInParameter(command, "@CollectorSurrenderId", DbType.Int32, collectorSurrenderPM.CollectorSurrenderId);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, collectorSurrenderPM.PaymentMethodId);
            Db.AddInParameter(command, "@Amount", DbType.Double, collectorSurrenderPM.Amount);
            Db.AddInParameter(command, "@Saving", DbType.Int32, collectorSurrenderPM.Saving);
            Db.AddInParameter(command, "@Cycle", DbType.Double, collectorSurrenderPM.Cycle);

            Db.ExecuteScalar(command);
        }

        public void AddCollectorAdvancementPM(CollectorAdvancementPM collectorAdvancementPM)
        {
            var command = Db.GetStoredProcCommand("pub_CollectorAdvancementPM_Insert");

            Db.AddInParameter(command, "@CollectorAdvancementId", DbType.Int32, collectorAdvancementPM.CollectorAdvancementId);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, collectorAdvancementPM.PaymentMethodId);
            Db.AddInParameter(command, "@Amount", DbType.Double, collectorAdvancementPM.Amount);
            Db.AddInParameter(command, "@Saving", DbType.Int32, collectorAdvancementPM.Saving);
            Db.AddInParameter(command, "@Cycle", DbType.Double, collectorAdvancementPM.Cycle);

            Db.ExecuteScalar(command);
        }
    }

}
