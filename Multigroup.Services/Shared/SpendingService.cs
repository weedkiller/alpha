using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Configuration;
using Multigroup.Common.PDF;

namespace Multigroup.Services.Shared
{
    public class SpendingService
    {
        private SpendingRepository _spendingRepository;
        private ArticleRepository _articleRepository;
        private PaymentMethodsRepository _paymentMethodRepository;
        private CashierRepository _cashierRepository;
        private UserRepository _userRepository;
        private CashCycleRepository _cashCycleRepository;
        private ProviderRepository _providerRepository;
        private MovementRepository _movementRepository;
        private SpendingPaymentRepository _spendingPaymentRepository;
        private PurchasePerceptionRepository _purchasePerceptionRepository;
        private PdfGeneratorHelper _pdfGeneratorHelper;

        public SpendingService()
        {
            _spendingRepository = new SpendingRepository();
            _articleRepository = new ArticleRepository();
            _paymentMethodRepository = new PaymentMethodsRepository();
            _cashierRepository = new CashierRepository();
            _userRepository = new UserRepository();
            _cashCycleRepository = new CashCycleRepository();
            _providerRepository = new ProviderRepository();
            _movementRepository = new MovementRepository();
            _spendingPaymentRepository = new SpendingPaymentRepository();
            _purchasePerceptionRepository = new PurchasePerceptionRepository();
            _pdfGeneratorHelper = new PdfGeneratorHelper();
        }


        public Spending GetSpendingsById(int spendingId)
        {
            Spending gasto = _spendingRepository.GetSpendingById(spendingId);

            List<SpendingDetail> Details = new List<SpendingDetail>();

            List<SpendingPaymentMethod> payments = new List<SpendingPaymentMethod>();

            foreach (SpendingDetail item in _spendingRepository.GetSpendingDetails(gasto.SpendingId))
            {
                SpendingDetail detalle = _spendingRepository.GetSpendingDetail(item.SpendingDetailId);
                if (detalle.ArticleId != (int)ArticlesEspecial.Salary)
                    detalle.Article = _articleRepository.GetArticleById(detalle.ArticleId).Name;
                if (detalle.ExpenseAuthorizationId.HasValue)
                    detalle.Origin = "Autorización de Gasto " + detalle.ExpenseAuthorizationId.Value;
                else if (detalle.ScheduledExpenseId.HasValue)
                    detalle.Origin = "Gasto Programado " + detalle.ScheduledExpenseId.Value;
                else if (detalle.PurchaseRequestId.HasValue)
                    detalle.Origin = "Solicitud de Compra " + detalle.PurchaseRequestId.Value;
                else
                    detalle.Origin = "Independendiente";
                Details.Add(detalle);
            }

            foreach (SpendingPaymentMethod item in _spendingRepository.GetSpendingPaymentMethods(gasto.SpendingId))
            {
                SpendingPaymentMethod detalle = _spendingRepository.GetSpendingPaymentMethod(item.SpendingPaymentMethodId);
                detalle.PaymentMethod = _paymentMethodRepository.GetPaymentMethodsById(detalle.PaymentMethodId).Name;
                payments.Add(detalle);
            }

            gasto.Details = Details;
            gasto.PaymentMethods = payments;

            return gasto;
        }

        public IEnumerable<Spending> GetSpendings()
        {
            return _spendingRepository.GetSpendings();
        }

        public IEnumerable<Spending> GetSpendingsByProvider(int providerId)
        {
            return _spendingRepository.GetSpendingsByProvider(providerId);
        }

        public IEnumerable<Spending> GetSpendingsByEmployee(int providerId)
        {
            return _spendingRepository.GetSpendingsByEmployee(providerId);
        }

        public IEnumerable<SpendingPaymentMethod> GetSpendingPaymentMethods(int spendingId)
        {
            return _spendingRepository.GetSpendingPaymentMethods(spendingId);
        }

        public SpendingPaymentMethod GetSpendingPaymentMethodById(int spendingPaymentMethodId)
        {
            return _spendingRepository.GetSpendingPaymentMethod(spendingPaymentMethodId);
        }

        public IEnumerable<SpendingPaymentMethod> GetSpendingPaymentMethodsByPM(int paymentMethodId)
        {
            return _spendingRepository.GetSpendingPaymentMethodsByPM(paymentMethodId);
        }

        public IEnumerable<SpendingSummary> GetSpendingSummary(int? SelectedProvider, int? SelectedUser, string DateFromExecution, string DateToExecution, string DateFromExpiration, string DateToExpiration, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance, string PositiveAll, string Provider)
        {
            if (SelectedProvider == null)
                SelectedProvider = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromExecution == "")
                DateFromExecution = "01/01/1900";
            if (DateToExecution == "")
                DateToExecution = "01/01/2100";
            if (DateFromExpiration == "")
                DateFromExpiration = "01/01/1900";
            if (DateToExpiration == "")
                DateToExpiration = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            if (BalanceFrom == "")
                BalanceFrom = "-100000000";
            if (BalanceTo == "")
                BalanceTo = "100000000";
            if (Receipt == "")
                Receipt = "0";
            return _spendingRepository.GetSpendingsSummary(SelectedProvider.Value.ToString(), SelectedUser.Value.ToString(), DateFromExecution, DateToExecution, DateFromExpiration, DateToExpiration, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance, PositiveAll, Provider);
        }

        public IEnumerable<SpendingSummary> GetSpendingSummaryWithOutImput(int? SelectedProvider, int? SelectedUser, string DateFromExecution, string DateToExecution, string DateFromExpiration, string DateToExpiration, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance, string PositiveAll, string Provider, List<int> SpendingId)
        {
            if (SelectedProvider == null)
                SelectedProvider = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromExecution == "")
                DateFromExecution = "01/01/1900";
            if (DateToExecution == "")
                DateToExecution = "01/01/2100";
            if (DateFromExpiration == "")
                DateFromExpiration = "01/01/1900";
            if (DateToExpiration == "")
                DateToExpiration = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            if (BalanceFrom == "")
                BalanceFrom = "-100000000";
            if (BalanceTo == "")
                BalanceTo = "100000000";
            if (Receipt == "")
                Receipt = "0";

            IEnumerable<SpendingSummary> spendings = _spendingRepository.GetSpendingsSummary(SelectedProvider.Value.ToString(), SelectedUser.Value.ToString(), DateFromExecution, DateToExecution, DateFromExpiration, DateToExpiration, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance, PositiveAll, Provider);

            List<SpendingSummary> spefiltrada = new List<SpendingSummary>();
            bool existe = false;
            foreach (SpendingSummary spe in spendings)
            {
                existe = false;
                foreach (int spen in SpendingId)
                {
                    if (spe.SpendingId == spen)
                        existe = true;
                }
                if (!existe)
                    spefiltrada.Add(spe);
            }

            return spefiltrada;
        }

        public IEnumerable<SpendingSummary> GetSpendingSallarySummary(int? SelectedProvider, int? SelectedUser, string DateFromExecution, string DateToExecution, string DateFromExpiration, string DateToExpiration, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance, string PositiveAll, string Provider)
        {
            if (SelectedProvider == null)
                SelectedProvider = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromExecution == "")
                DateFromExecution = "01/01/1900";
            if (DateToExecution == "")
                DateToExecution = "01/01/2100";
            if (DateFromExpiration == "")
                DateFromExpiration = "01/01/1900";
            if (DateToExpiration == "")
                DateToExpiration = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            if (BalanceFrom == "")
                BalanceFrom = "-100000000";
            if (BalanceTo == "")
                BalanceTo = "100000000";
            if (Receipt == "")
                Receipt = "0";
            return _spendingRepository.GetSpendingsSallarySummary(SelectedProvider.Value.ToString(), SelectedUser.Value.ToString(), DateFromExecution, DateToExecution, DateFromExpiration, DateToExpiration, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance, PositiveAll, Provider);
        }

        public IEnumerable<SpendingSummary> GetSpendingSallarySummaryWithOutImput(int? SelectedProvider, int? SelectedUser, string DateFromExecution, string DateToExecution, string DateFromExpiration, string DateToExpiration, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance, string PositiveAll, string Provider, List<int> SpendingId)
        {
            if (SelectedProvider == null)
                SelectedProvider = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromExecution == "")
                DateFromExecution = "01/01/1900";
            if (DateToExecution == "")
                DateToExecution = "01/01/2100";
            if (DateFromExpiration == "")
                DateFromExpiration = "01/01/1900";
            if (DateToExpiration == "")
                DateToExpiration = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            if (BalanceFrom == "")
                BalanceFrom = "-100000000";
            if (BalanceTo == "")
                BalanceTo = "100000000";
            if (Receipt == "")
                Receipt = "0";

            IEnumerable<SpendingSummary> spendings = _spendingRepository.GetSpendingsSallarySummary(SelectedProvider.Value.ToString(), SelectedUser.Value.ToString(), DateFromExecution, DateToExecution, DateFromExpiration, DateToExpiration, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance, PositiveAll, Provider);

            List<SpendingSummary> spefiltrada = new List<SpendingSummary>();
            bool existe = false;
            foreach (SpendingSummary spe in spendings)
            {
                existe = false;
                foreach (int spen in SpendingId)
                {
                    if (spe.SpendingId == spen)
                        existe = true;
                }
                if (!existe)
                    spefiltrada.Add(spe);
            }

            return spefiltrada;
        }

        public IEnumerable<SpendingDetailSummary> GetSpendingDetailSummary(int? SelectedProvider, int? SelectedUser, string DateFromExecution, string DateToExecution, string DateFromExpiration, string DateToExpiration, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance, string PositiveAll, string Provider)
        {
            if (SelectedProvider == null)
                SelectedProvider = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromExecution == "")
                DateFromExecution = "01/01/1900";
            if (DateToExecution == "")
                DateToExecution = "01/01/2100";
            if (DateFromExpiration == "")
                DateFromExpiration = "01/01/1900";
            if (DateToExpiration == "")
                DateToExpiration = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            if (BalanceFrom == "")
                BalanceFrom = "-100000000";
            if (BalanceTo == "")
                BalanceTo = "100000000";
            if (Receipt == "")
                Receipt = "0";
            return _spendingRepository.GetSpendingsDetailSummary(SelectedProvider.Value.ToString(), SelectedUser.Value.ToString(), DateFromExecution, DateToExecution, DateFromExpiration, DateToExpiration, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance, PositiveAll, Provider);
        }

        public SpendingDetail GetSpendingDetail(int spendingDetailId)
        {
            return _spendingRepository.GetSpendingDetail(spendingDetailId);
        }

        public IEnumerable<SpendingDetail> GetSpendingDetails(int spendingId)
        {
            return _spendingRepository.GetSpendingDetails(spendingId);
        }

        public void AddSpending(Spending Spending, Invoice invoice)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Spending.SpendingTypeId = (int)SpendingType.Spending;
                    int expense = _spendingRepository.AddSpending(Spending);
                    invoice.ImputDate = Spending.ExecutionDate;
                    invoice.SpendingId = expense;
                    UserCashier userCashier = _userRepository.GetUserCashierByUserId(Spending.UserId);

                    CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);

                    foreach (SpendingDetail detail in Spending.Details)
                    {
                        detail.SpendingId = expense;
                        _spendingRepository.AddSpendingDetail(detail);
                    }

                    foreach (SpendingPaymentMethod payment in Spending.PaymentMethods)
                    {
                        payment.SpendingId = expense;
                        payment.Saving = userCashier.CashierId.ToString();
                        payment.Cycle = cashCycle.CashCycleId.ToString();
                        _spendingRepository.AddSpendingPaymentMethod(payment);
                    }

                    if (Spending.ProveedorId.HasValue)
                    {
                        Provider provider = _providerRepository.GetProviderById(Spending.ProveedorId.Value);
                        provider.Balance = provider.Balance + Spending.Amount;
                        _providerRepository.UpdateProvider(provider);
                    }

                    if (invoice != null)
                    {
                        _spendingRepository.AddInvoice(invoice);
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

        public void DeleteSpending(int spendingId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    Spending spending = _spendingRepository.GetSpendingById(spendingId);

                    if (spending.ProveedorId.HasValue)
                    {
                        Provider provider = _providerRepository.GetProviderById(spending.ProveedorId.Value);
                        provider.Balance = provider.Balance - spending.Amount;
                        _providerRepository.UpdateProvider(provider);
                    }


                    _spendingRepository.DeleteSpending(spendingId);
                    _spendingRepository.DeleteSpendingDetailBySpendingId(spendingId);
                    _spendingRepository.DeleteSpendingPMBySpendingId(spendingId);
                    _spendingRepository.DeleteInvoiceBySpendingId(spendingId);
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

        public void UpdateSpending(Spending Spending, Invoice invoice)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    UserCashier userCashier = _userRepository.GetUserCashierByUserId(Spending.UserId);

                    CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);

                    Spending old = _spendingRepository.GetSpendingById(Spending.SpendingId);
                    _spendingRepository.UpdateSpending(Spending);
                    _spendingRepository.DeleteInvoiceBySpendingId(Spending.SpendingId);
                    invoice.ImputDate = Spending.ExecutionDate;
                    invoice.SpendingId = Spending.SpendingId;
                    if (Spending.ProveedorId.HasValue)
                    {
                        Provider provider = _providerRepository.GetProviderById(Spending.ProveedorId.Value);
                        provider.Balance = provider.Balance + Spending.Amount - old.Amount;
                        _providerRepository.UpdateProvider(provider);
                    }

                    _spendingRepository.DeleteSpendingDetailBySpendingId(Spending.SpendingId);
                    _spendingRepository.DeleteSpendingPMBySpendingId(Spending.SpendingId);
                    foreach (SpendingDetail detail in Spending.Details)
                    {
                        detail.SpendingId = Spending.SpendingId;
                        _spendingRepository.AddSpendingDetail(detail);
                    }

                    foreach (SpendingPaymentMethod payment in Spending.PaymentMethods)
                    {
                        payment.SpendingId = Spending.SpendingId;
                        payment.Saving = userCashier.CashierId.ToString();
                        payment.Cycle = cashCycle.CashCycleId.ToString();
                        _spendingRepository.AddSpendingPaymentMethod(payment);
                    }

                    if (invoice != null)
                    {
                        _spendingRepository.AddInvoice(invoice);
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

        public void UpdateSpendingShort(Spending Spending)
        {

            _spendingRepository.UpdateSpendingShort(Spending);
        }



        public void AddSpendingDetail(SpendingDetail SpendingDetail)
        {
            _spendingRepository.AddSpendingDetail(SpendingDetail);
        }

        public void DeleteSpendingDetail(int spendingDetailId)
        {
            _spendingRepository.DeleteSpendingDetail(spendingDetailId);
        }

        public void UpdateSpendingDetail(SpendingDetail SpendingDetail)
        {
            _spendingRepository.UpdateSpendingDetail(SpendingDetail);
        }

        public void AddSpendingPaymentMethod(SpendingPaymentMethod SpendingPaymentMethod)
        {
            _spendingRepository.AddSpendingPaymentMethod(SpendingPaymentMethod);
        }

        public void DeleteSpendingPaymentMethod(int spendingPaymentMethodId)
        {
            _spendingRepository.DeleteSpendingPaymentMethod(spendingPaymentMethodId);
        }

        public void UpdateSpendingPaymentMethod(SpendingPaymentMethod SpendingPaymentMethod)
        {
            _spendingRepository.UpdateSpendingPaymentMethod(SpendingPaymentMethod);
        }

        public bool IsEditable(int spendingId)
        {
            bool editable = true;
            foreach (SpendingPaymentMethod pm in _spendingRepository.GetSpendingPaymentMethods(spendingId))
            {
                if (pm.ReconciledId != null)
                    editable = false;

                PaymentMethodVerification verif = _paymentMethodRepository.GetPaymentMethodVerificationBySpending(pm.SpendingPaymentMethodId);
                if (verif != null)
                    editable = false;

                CashCycle caja = _cashCycleRepository.GetCashCycleByCashierId(int.Parse(pm.Saving));

                if (caja.CashCycleId != int.Parse(pm.Cycle))
                    editable = false;

                SpendingPaymentDetail spd = _spendingPaymentRepository.GetSpendingPaymentDetailsBySpendingId(spendingId);
                if (spd != null)
                    editable = false;
            }
            return editable;
        }

        public bool IsImputed(int spendingId)
        {
            bool editable = true;
            foreach (SpendingPaymentMethod pm in _spendingRepository.GetSpendingPaymentMethods(spendingId))
            {
                SpendingPaymentDetail spd = _spendingPaymentRepository.GetSpendingPaymentDetailsBySpendingId(spendingId);
                if (spd != null)
                    editable = false;
            }
            return editable;
        }

        public void UpdateInvoice(Invoice invoice)
        {
            _spendingRepository.UpdateInvoice(invoice);
        }

        public void AddInvoice(Invoice invoice)
        {
            _spendingRepository.AddInvoice(invoice);
        }

        public Invoice GetInvoice(int invoiceId)
        {
            return _spendingRepository.GetInvoice(invoiceId);
        }

        public Invoice GetInvoiceBySpendingId(int spendingId)
        {
            return _spendingRepository.GetInvoiceBySpendingId(spendingId);
        }

        public Invoice GetInvoiceByFilter(string CUIT, int PurchaseDocumentId, string Letter, int Prefix, int Number, int spendingId)
        {
            return _spendingRepository.GetInvoiceByFilter(CUIT, PurchaseDocumentId, Letter, Prefix, Number, spendingId);
        }

        public IEnumerable<Invoice> GetInvoices()
        {
            return _spendingRepository.GetInvoices();
        }

        public IEnumerable<PurchaseDocument> GetPurchaseDocuments()
        {
            return _purchasePerceptionRepository.GetPurchaseDocuments();
        }

        public IEnumerable<PurchasePerception> GetPurchasePerceptions()
        {
            return _purchasePerceptionRepository.GetPurchasePerceptions();
        }

        public IEnumerable<SpendingFile> GetSpendingDocuments(int spendingId)
        {
            return _spendingRepository.GetSpendingDocuments(spendingId);
        }

        public void InsertDocument(HttpPostedFileBase file, int actionPlanId, string code)
        {
            try
            {
                var path = SaveAttach(file, actionPlanId, code);
                _spendingRepository.InsertDocument(path, file, actionPlanId);
            }
            catch (Exception ex)
            {
            }
        }

        public void DeleteSpendingFile(int SpendingId)
        {
            _spendingRepository.DeleteSpendingFile(SpendingId);
        }

        public string SaveAttach(HttpPostedFileBase file, int actionPlanId, string code)
        {
            var path = ConfigurationManager.AppSettings["SpendingDocumentPath"];
            var filePath = System.AppDomain.CurrentDomain.BaseDirectory + path;
            filePath = System.IO.Path.Combine(filePath.ToString(), code);
            path = System.IO.Path.Combine(path.ToString(), code);
            bool isExists = System.IO.Directory.Exists(filePath);

            if (!isExists)
                System.IO.Directory.CreateDirectory(filePath);

            var pathFinal = string.Format("{0}\\{1}", filePath, file.FileName);


            _pdfGeneratorHelper.SaveFileContentsAsync(pathFinal, file.InputStream);

            return path;
        }

    }
}
