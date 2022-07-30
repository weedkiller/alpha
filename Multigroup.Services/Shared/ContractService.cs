using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Web;
using Multigroup.Common.PDF;
using System.Configuration;
using System.Transactions;


namespace Multigroup.Services.Shared
{
    public class ContractService
    {
        private ContractRepository _contractRepository;
        private ContractPaperRepository _contractPaperRepository;
        private CustomerRepository _customerRepository;
        private PaymentRepository _paymentRepository;
        private PdfGeneratorHelper _pdfGeneratorHelper;
        private ReceipService _receipService;

        private String clientesTotal;
        private String estadosTotal;
        private String estadosDetalleTotal;
        private String contractTypeTotal;
        private bool primero;
        private NotificationService _notificationService;
        private EmailService _emailService;



        public ContractService()
        {
            _contractRepository = new ContractRepository();
            _notificationService = new NotificationService();
            _contractPaperRepository = new ContractPaperRepository();
            _paymentRepository = new PaymentRepository();
            _customerRepository = new CustomerRepository();
            _pdfGeneratorHelper = new PdfGeneratorHelper();
            _receipService = new ReceipService();
            _emailService = new EmailService();

            primero = true;
            clientesTotal = "";
            estadosTotal = "";
            estadosDetalleTotal = "";
            contractTypeTotal = "";
        }

        public Contract GetContract(int ContractId)
        {
            return _contractRepository.GetContractById(ContractId);
        }

        public Contract GetContractWeb(int ContractId)
        {
            return _contractRepository.GetContractWebById(ContractId);
        }

        public QuotationContract GetQuotationContract(int ContractId)
        {
            return _contractRepository.GetQuotationContract(ContractId);
        }

        public ContractResume GetContractResume(int ContractId)
        {
            return _contractRepository.GetContractResume(ContractId);
        }

        public string GetAmountByContractId(int ContractId, string PaymentDate)
        {
            return _contractRepository.GetAmountByContractId(ContractId, PaymentDate).Amount.ToString();
        }

        public IEnumerable<Contract> GetContractByDNI(int DNI)
        {
            return _contractRepository.GetContractByDNI(DNI);
        }

        public Installment GetInstallmentByContract(int ContractId)
        {
            return _contractRepository.GetInstallmentByContract(ContractId);
        }

        public IEnumerable<Contract> GetContractByNumber(string number)
        {
            return _contractRepository.GetContractByNumber(number);
        }

        public Contract GetContractsById(int contractId)
        {
            return _contractRepository.GetContractById(contractId);
        }

        public IEnumerable<Contract> GetContracts()
        {
            return _contractRepository.GetContracts();
        }

        public bool ExisteContract(int number)
        {
            IEnumerable<Contract> contracts = _contractRepository.GetContracts();
            bool existe = false;

            foreach (Contract con in contracts)
            {
                if (con.Number == number && con.ContractType != 3)
                    existe = true;
            }

            return existe;
        }

        public bool ExisteContractHouse(int number)
        {
            IEnumerable<Contract> contracts = _contractRepository.GetContracts();
            bool existe = false;

            foreach (Contract con in contracts)
            {
                if (con.Number == number && con.ContractType == 3)
                    existe = true;
            }

            return existe;
        }

        public bool ExisteContractPaper(int number)
        {
            IEnumerable<ContractPaper> contracts = _contractPaperRepository.GetContractPapers();
            bool existe = false;

            foreach (ContractPaper con in contracts)
            {
                if (con.Number == number && con.ContractType != 3)
                    existe = true;
            }

            return existe;
        }

        public bool ExisteContractPaperHouse(int number)
        {
            IEnumerable<ContractPaper> contracts = _contractPaperRepository.GetContractPapers();
            bool existe = false;

            foreach (ContractPaper con in contracts)
            {
                if (con.Number == number && con.ContractType == 3)
                    existe = true;
            }

            return existe;
        }

        public IEnumerable<ContractSummary> GetContractByFilter(string status, string statusDetail, string clients, string contractType, int? agency, string desde, string hasta)
        {
            if (agency == null)
                agency = 0;
            if (desde == "")
                desde = "01/01/1900";
            if (hasta == "")
                hasta = "01/01/2100";
            return _contractRepository.GetContractByFilter(status, statusDetail, clients, contractType, agency, desde, hasta);
        }

        public IEnumerable<ContractSummary> GetContractByFilterTMK(string status, string statusDetail, string clients, string contractType, int? agency, string desde, string hasta, int userId)
        {
            if (agency == null)
                agency = 0;
            if (desde == "")
                desde = "01/01/1900";
            if (hasta == "")
                hasta = "01/01/2100";
            return _contractRepository.GetContractByFilterTMK(status, statusDetail, clients, contractType, agency, desde, hasta, userId);
        }

        public IEnumerable<ContractCuotas> GetContractCuotas(int contractId)
        {
            return _contractRepository.GetContractCuotas(contractId);
        }

        public IEnumerable<ContractSummary> GetContractWebByFilter(string status, string clients, string desde, string hasta, int userId, int supervisor, int gerente)
        {
            if (desde == "")
                desde = "01/01/1900";
            if (hasta == "")
                hasta = "01/01/2100";
            return _contractRepository.GetContractWebByFilter(status, clients, desde, hasta, userId, supervisor, gerente);
        }

        public ContractSummaryTotal GetContractWebByFilterTotal(string status, string clients, string desde, string hasta, int userId, int supervisor, int gerente)
        {
            if (desde == "")
                desde = "01/01/1900";
            if (hasta == "")
                hasta = "01/01/2100";
            return _contractRepository.GetContractWebByFilterTotal(status, clients, desde, hasta, userId, supervisor, gerente);
        }
        private void ConcatenarContractTypes(int aux)
        {
            if (primero == true)
            {
                contractTypeTotal = "" + aux;
                primero = false;
            }
            else
                contractTypeTotal = contractTypeTotal + "," + aux;
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

        private void ConcatenarStatusDetail(int aux)
        {
            if (primero == true)
            {
                estadosDetalleTotal = "" + aux;
                primero = false;
            }
            else
                estadosDetalleTotal = estadosDetalleTotal + "," + aux;
        }

        private void ConcatenarClientes(int aux)
        {
            if (primero == true)
            {
                clientesTotal = "" + aux;
                primero = false;
            }
            else
                clientesTotal = clientesTotal + "," + aux;
        }

        public void AddContract(Contract contract, int user)
        {
            contract.Status = 2;
            contract.StatusDetail = 1;
            contract.PaperStatus = 3;
            int contrato = _contractRepository.AddContract(contract, user);
            if (contract.ContractType == 3)
            {
                Payment payment = new Payment();
                                payment.ReceipNumber = contract.Number.ToString();
                                payment.Amount = contract.Total;
                                payment.PaymentDate = contract.RegistrationDate;
                                payment.type = "5";
                                payment.PaymentMethod = "1";
                                var idPago = _paymentRepository.AddPayment(payment, user);
                                Receip oReceip = _receipService.GetReceipByNumber(payment.ReceipNumber);
                                if (oReceip != null)
                                _receipService.UpdateReceipStatus(oReceip.ReceipId, 15, null);
                                _contractRepository.AddSuscription(contract.CustomerId, contrato, contract.Total, idPago);

            }

            IEnumerable<ContractPaper> contracts = _contractPaperRepository.GetContractPapers();

            foreach (ContractPaper con in contracts)
            {
                if (con.Number == contract.Number && contract.ContractType == con.ContractType)
                    _contractPaperRepository.UpdateContractPaperStatus(con.PaperContractId, 15, con.User);
            }
        }

        public int AddContractWeb(Contract contract, int user, QuotationContract quotation)
        {
            contract.Status = 2;
            contract.StatusDetail = 1;
            contract.PaperStatus = 3;

            Contract contratoViejo = _contractRepository.GetContractWebByCustomerId(contract.CustomerId);
            if (contratoViejo == null)
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    try
                    {
                        int contrato = _contractRepository.AddContractWeb(contract, user);
                        contract.Number = contrato;
                        contract.ContractId = contrato;
                        _contractRepository.UpdateWeb(contract);
                        quotation.ContractId = contrato;
                        _contractRepository.AddQuotaionContract(quotation);
                        _notificationService.NotifyNewContract(contract);
                        scope.Complete();
                        return contrato;
                    }
                    catch (Exception ex)
                    {
                        Transaction.Current.Rollback();
                        scope.Dispose();
                        throw ex;
                    }
                }
            }
            else return contratoViejo.ContractId;
        }

        public void NotifySignContract(int contractId, int customerId, User user, string htmlContract)
        {
           //Generamos el pdf y lo guardamos
            Byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(htmlContract, PdfSharp.PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
            }

            var path = pathContract(contractId);
            File.WriteAllBytes(path, res);
            IEnumerable<ContractFile> documentos = GetContractWebDocuments(contractId);
            _notificationService.NotifySignContract(path, customerId, contractId, documentos, user);

        }


        public void UpdateContractStatus(int contractId, int status, int statusDetail, int PaperStatus, string startMonth)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _contractRepository.UpdateContractStatus(contractId, status, statusDetail, PaperStatus);

                    Contract contract = _contractRepository.GetContractById(contractId);
                    IEnumerable<Installment> cuotas = _contractRepository.GetInstallmentsByContractId(contractId);


                    int comienzo = Math.Abs((Convert.ToDateTime(startMonth).Month - DateTime.Now.Month) + 12 * (Convert.ToDateTime(startMonth).Year - DateTime.Now.Year));
                    if (status == 18 && cuotas.Count() == 0)
                    {
                        _contractRepository.AddInstallment(contract.ContractId, 1, contract.FirstAdvanceAmount, contract.PaymentDateId, comienzo);
                        int numero = 2;
                        for (int i = comienzo + 1; i < comienzo + 36; i++)
                        {
                            _contractRepository.AddInstallment(contract.ContractId, numero, contract.AdvancesAmount, contract.PaymentDateId, i);
                            numero++;
                        }
                    }
                    if (status == 2 && cuotas.Count() > 0)
                    {
                        _contractRepository.DeleteInstallment(contract.ContractId);
                    }

                    if (status == 18)
                        ScoringContractMail(contract.ContractId);
                    else if (status == 21)
                        _notificationService.NotifyCancelation(contract.CustomerId);
                    else if (status == 24)
                        _notificationService.NotifyDebtCancelation(contract.CustomerId);
                    else if (status == 20)
                        _notificationService.NotifyAdjudication(contract.CustomerId);
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

        public void ScoringContractMail(int contractId)
        {
            Contract contrato = _contractRepository.GetContractById(contractId);
            _notificationService.NotifyScoringContract(contrato);
        }

        public void UpdateContractWebStatus(int contractId)
        {
            _contractRepository.UpdateContractWebStatus(contractId);
        }

        public PaymentPreference GetPaymentPreferenceById(int PaymentPreferenceId)
        {
            return _contractRepository.GetPaymentPreferenceById(PaymentPreferenceId);
        }

        public bool IsContractSign(int contractId)
        {
            Contract contrato = _contractRepository.GetContractWebSignById(contractId);
            if (contrato != null)
                return true;
            else
                return false;
        }

        public void GeneratePaymentHistory(int contractId, int status, int statusDetail, int PaperStatus, string startMonth)
        {


            IEnumerable<Contract> contratos = _contractRepository.GetContractsMigracion();

            foreach (Contract contrato in contratos)
            {
                var cuotas = _contractRepository.GetCuotasContratoByNumero(contrato.Number);
                if (cuotas!= null && !cuotas.Migrado)
                {
                    for (int i = 1; i <= cuotas.Cuotas; i++)
                        {
                            using (var scope = new TransactionScope(TransactionScopeOption.Required))
                            {
                                try
                                {
                                    Installment cuota = _contractRepository.GetInstallmentsByContractIdNumber(contrato.ContractId, i);

                                    Payment payment = new Payment();
                                    payment.Observations = "Historico";
                                    payment.ReceipNumber = "0";
                                    payment.Amount = cuota.OriginalAmount;
                                    payment.PaymentDate = cuota.InitialDate;
                                    payment.type = "1";
                                    payment.PaymentMethod = "1";


                                    if (cuota.Status == "7")
                                    {
                                        var idPago = _paymentRepository.AddPayment(payment, 1);
                                        PaymentAllocation pago = new PaymentAllocation();
                                        pago.InstallmentId = cuota.InstallmentId;
                                        pago.PaymentId = idPago;
                                        pago.amount = cuota.OriginalAmount;
                                        _contractRepository.AddPaymentAllocation(pago);

                                        cuota.Status = "8";
                                        cuota.PaymentDate = cuota.InitialDate;
                                        _contractRepository.UpdateInstallment(cuota);

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
                    cuotas.Migrado = true;
                    _contractRepository.UpdateCuotasContrato(cuotas);
                }
            }
        }

        public void GenerateSuscriptionHistory(int contractId, int status, int statusDetail, int PaperStatus, string startMonth)
        {


            IEnumerable<Contract> contratos = _contractRepository.GetContractsMigracion();

            foreach (Contract contrato in contratos)
            {
                var cuotas = _contractRepository.GetCuotasContratoByNumero(contrato.Number);
                if (cuotas != null && !cuotas.Migrado)
                {
                    for (int i = 1; i <= cuotas.Cuotas; i++)
                    {
                        using (var scope = new TransactionScope(TransactionScopeOption.Required))
                        {
                            try
                            {
                                Payment payment = new Payment();
                                payment.Observations = "Historico";
                                payment.ReceipNumber = "0";
                                payment.Amount = contrato.SubscriptionAmount;
                                payment.PaymentDate = contrato.RegistrationDate;
                                payment.type = "5";
                                payment.PaymentMethod = "1";
                                var idPago = _paymentRepository.AddPayment(payment, 1);
                                _contractRepository.AddSuscription(contrato.CustomerId, contrato.ContractId, contrato.SubscriptionAmount, idPago);

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
                    cuotas.Migrado = true;
                    _contractRepository.UpdateCuotasContrato(cuotas);
                }
            }
        }

        public void GenerateAdvanceHistory(int contractId, int status, int statusDetail, int PaperStatus, string startMonth)
        {


            IEnumerable<Contract> contratos = _contractRepository.GetContractsMigracion();

            foreach (Contract contrato in contratos)
            {
                var cuotas = _contractRepository.GetCuotasContratoByNumero(contrato.Number);
                if (cuotas != null && !cuotas.Migrado)
                {
                    for (int i = 1; i <= cuotas.Cuotas; i++)
                    {
                        using (var scope = new TransactionScope(TransactionScopeOption.Required))
                        {
                            try
                            {
                                Payment payment = new Payment();
                                payment.Observations = "Historico";
                                payment.ReceipNumber = "0";
                                payment.Amount = cuotas.Monto;
                                payment.PaymentDate = DateTime.Now;
                                payment.type = "2";
                                payment.PaymentMethod = "1";
                                var idPago = _paymentRepository.AddPayment(payment, 1);
                                _contractRepository.AddPaymentAdvance(contrato.CustomerId, contrato.ContractId, cuotas.Monto, idPago);

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
                    cuotas.Migrado = true;
                    _contractRepository.UpdateCuotasContrato(cuotas);
                }
            }
        }

        public void GenerateInstallmentHistory(int contractId, int status, int statusDetail, int PaperStatus, string startMonth)
        {


            IEnumerable<Contract> contratos = _contractRepository.GetContractsMigracion();

            foreach (Contract contrato in contratos)
            {

                IEnumerable<Installment> cuotas = _contractRepository.GetInstallmentsByContractId(contrato.ContractId);

                int comienzo = Math.Abs((contrato.RegistrationDate.Value.Month - DateTime.Now.Month) + 12 * (contrato.RegistrationDate.Value.Year - DateTime.Now.Year));

                comienzo = (-1 * comienzo) + 1;

                if (cuotas.Count() == 0)
                {
                    _contractRepository.AddInstallment(contrato.ContractId, 1, contrato.FirstAdvanceAmount, contrato.PaymentDateId, comienzo);
                    int numero = 2;
                    for (int i = comienzo + 1; i < comienzo + 36; i++)
                    {
                        _contractRepository.AddInstallment(contrato.ContractId, numero, contrato.AdvancesAmount, contrato.PaymentDateId, i);
                        numero++;
                    }
                }
            }
        }

        public void AddStatusContractHistory(int ContractId, int UserId, int StatusOld, int StatusNew, string Observations)
        {
            DateTime now = DateTime.Now;
            _contractRepository.AddStatusContractHistory(ContractId, UserId, StatusOld, StatusNew, Observations, now);
        }

        public void UpdateInstallmentDate(int contractId, int cuota)
        {
            _contractRepository.UpdateInstallmentDate(contractId, cuota);
        }

        public IEnumerable<ContractFile> GetContractDocuments(int contractId)
        {
            return _contractRepository.GetContractDocuments(contractId);
        }

        public IEnumerable<ContractFile> GetContractWebDocuments(int contractId)
        {
            return _contractRepository.GetContractWebDocuments(contractId);
        }

        public void UpdateInstallments(int id, int InstallmentNumber)
        {
            _contractRepository.UpdateInstallments(id, InstallmentNumber);
        }

        public IEnumerable<InstallmentInsterestsSummary> GetInstallmentInsterestsByContractId(int contractId)
        {
            return _contractRepository.GetInstallmentInsterestsByContractId(contractId);
        }
        public IEnumerable<StatusContractHistory> GetStatusContractHistoryByContractId(int contractId)
        {
            return _contractRepository.GetStatusContractHistoryByContractId(contractId);
        }
        
        public void DeleteInterestById(int interestId)
        {
            _contractRepository.DeleteInterestById(interestId);
        }

        public void DeleteInterestByContract(int contractId)
        {
            _contractRepository.DeleteInterestByContract(contractId);
        }

        public void DeleteContracts(int userId)
        {
            _contractRepository.Delete(userId);
        }
        public void ConfirmContracts(int contractId)
        {
            _contractRepository.ConfirmContracts(contractId);
        }

        public void DeleteContractWeb(int contractId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Contract contrato = _contractRepository.GetContractWebWithStatusById(contractId);
                    if (contrato.Status == 38)
                    {
                        _notificationService.NotifyCancelationWeb(contractId);
                    }
                    _contractRepository.DeleteWeb(contractId);


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

        public void UpdateContracts(Contract contract)
        {
            Contract contrato = _contractRepository.GetContractById(contract.ContractId);
            contract.CustomerId = contrato.CustomerId;
            _contractRepository.Update(contract);
            if (contrato.AdvancesAmount != contract.AdvancesAmount)
                _contractRepository.UpdateInstallmentAmount(contrato.ContractId, contract.AdvancesAmount);
        }


        public void DeleteLastPayment(int contractId)
        {
            Payment lastPayment = _contractRepository.GetLastPayment(contractId);
            IEnumerable<PaymentAllocation> pagos = _paymentRepository.GetPaymentsAllocationByPaymentId(lastPayment.PaymentId);
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    foreach (PaymentAllocation pago in pagos)
                    {
                        _paymentRepository.DeleteInterestByInstallmentId(pago.InstallmentId);
                        _paymentRepository.DeletePaymentAllocation(pago.PaymentAllocationId);
                        Installment installment = _contractRepository.GetInstallmentById(pago.InstallmentId);
                        IEnumerable<PaymentAllocation> otrosPagos = _contractRepository.GetPaymentAllocationByInstallmentId(pago.InstallmentId);
                        if (otrosPagos.Count() > 0)
                            installment.Status = "17";
                        else
                            installment.Status = "7";
                        _contractRepository.UpdateInstallment(installment);
                    }
                    _paymentRepository.Delete(lastPayment.PaymentId);
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

        public IEnumerable<IdentificationType> GetIdentificationTypes()
        {
            return _contractRepository.GetIdentificationTypes();
        }

        public IEnumerable<PaymentDate> GetPaymentDate()
        {
            return _contractRepository.GetPaymentDate();
        }

        public IEnumerable<PaymentMethod> GetPaymentMethod()
        {
            return _contractRepository.GetPaymentMethod();
        }

        public IEnumerable<PaymentMethod> GetPaymentMethodByPP(int PaymentPreferenceId)
        {
            return _contractRepository.GetPaymentMethodByPP(PaymentPreferenceId);
        }

        public IEnumerable<PaymentPlace> GetPaymentPlaces()
        {
            return _contractRepository.GetPaymentPlaces();
        }

        public IEnumerable<ContractType> GetContractTypes()
        {
            return _contractRepository.GetContractTypes();
        }
        public IEnumerable<StatusContract> GetStatusContract(string type)
        {
            return _contractRepository.GetStatusContract(type);
        }

        public StatusContract GetStatusContract(int idStatus)
        {
            return _contractRepository.GetStatusContractById(idStatus);
        }

        public IEnumerable<PaymentPreference> GetPaymentPreference()
        {
            return _contractRepository.GetPaymentPreference();
        }

        public IEnumerable<StatusContract> GetStatusContractFilter(string type)
        {
            var status = _contractRepository.GetStatusContract(type);
            status = status.Where(x => x.StatusContractId == 11 || x.StatusContractId == 13);
            return status;
        }
        public IEnumerable<Customer> GetCustomer()
        {
            return _customerRepository.GetCustomers();
        }
        public IEnumerable<ContractChart> GetContractChart(string agency)
        {
            return _contractRepository.GetContractChart(agency);
        }

        public IEnumerable<CustomerDebt> GetCustomerDebt()
        {
            return _contractRepository.GetCustomerDebt();
        }

        public IEnumerable<SupervisorReport> GetSupervisorReport(string desde, string hasta, string ContractType)
        {
            if (desde == "")
                desde = "01/01/1900";
            if (hasta == "")
                hasta = "01/01/2100";
            return _contractRepository.GetSupervisorReport(desde, hasta, ContractType);
        }

        public IEnumerable<StatusReport> GetStatusReport(string _selectedStatus, string desde, string hasta)
        {
            return _contractRepository.GetStatusReport(_selectedStatus, desde, hasta);
        }

        public IEnumerable<PortfolioReport> GetPortfolioReport(string fecha, string user)
        {
            return _contractRepository.GetPortfolioReport(fecha, user);
        }

        public void InsertDocument(HttpPostedFileBase file, int actionPlanId, string code)
        {
            try
            {
                var path = SaveAttach(file, actionPlanId, code);
                _contractRepository.InsertDocument(path, file, actionPlanId);
            }
            catch (Exception ex)
            {
            }
        }

        public void InsertDocumentWeb(HttpPostedFileBase file, int actionPlanId, string code)
        {
            try
            {
                var path = SaveAttachWeb(file, actionPlanId, code);
                _contractRepository.InsertDocumentWeb(path, file, actionPlanId);
            }
            catch (Exception ex)
            {
            }
        }

        public void DeleteContractFileWeb(int ContractId)
        {
            _contractRepository.DeleteContractFileWeb(ContractId);
        }

        public string SaveAttach(HttpPostedFileBase file, int actionPlanId, string code)
        {
            var path = ConfigurationManager.AppSettings["ActionPlanDocumentPath"];
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

        public string SaveAttachWeb(HttpPostedFileBase file, int actionPlanId, string code)
        {
            var path = ConfigurationManager.AppSettings["ActionPlanDocumentWebPath"];
            var filePath = System.AppDomain.CurrentDomain.BaseDirectory + path;
            filePath = System.IO.Path.Combine(filePath.ToString(), code);
            path = System.IO.Path.Combine(path.ToString(), code);
            bool isExists = System.IO.Directory.Exists(filePath);

            if (!isExists)
                System.IO.Directory.CreateDirectory(filePath);

            var pathFinal = string.Format("{0}\\{1}", filePath, file.FileName);


            _pdfGeneratorHelper.SaveFileContents(pathFinal, file.InputStream);

            return path;
        }

        public string pathContract(int contractId)
        {
            var path = ConfigurationManager.AppSettings["ContractsPath"];
            var filePath = System.AppDomain.CurrentDomain.BaseDirectory + path;
            filePath = System.IO.Path.Combine(filePath.ToString(), contractId.ToString());
            path = System.IO.Path.Combine(path.ToString(), contractId.ToString());
            bool isExists = System.IO.Directory.Exists(filePath);

            if (!isExists)
                System.IO.Directory.CreateDirectory(filePath);

            var pathFinal = string.Format("{0}\\{1}", filePath, "contract.pdf");
            return pathFinal;
        }

        public void NotifyScoringContract(int contractId, int customerId, User user, string htmlContract)
        {
            //Generamos el pdf y lo guardamos
            Byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(htmlContract, PdfSharp.PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
            }

            var path = pathContractScoring(contractId);
            File.WriteAllBytes(path, res);
            IEnumerable<ContractFile> documentos = GetContractWebDocuments(contractId);
            _notificationService.NotifyScoringContract(path, customerId, contractId, documentos, user);
            Contract contrato = _contractRepository.GetContractByNumber(contractId.ToString()).FirstOrDefault();
            
        }

        public string pathContractScoring(int contractId)
        {
            var path = ConfigurationManager.AppSettings["ContractsPath"];
            var filePath = System.AppDomain.CurrentDomain.BaseDirectory + path;
            filePath = System.IO.Path.Combine(filePath.ToString(), contractId.ToString());
            path = System.IO.Path.Combine(path.ToString(), contractId.ToString());
            bool isExists = System.IO.Directory.Exists(filePath);

            if (!isExists)
                System.IO.Directory.CreateDirectory(filePath);

            var pathFinal = string.Format("{0}\\{1}", filePath, "Scoring.pdf");


            return pathFinal;
        }

        public IEnumerable<InstallmentReminderSummary> GetInstallmentsBeforeStart(int days)
        {
            return _contractRepository.GetInstallmentsBeforeStart(days);
        }

        public IEnumerable<InstallmentReminderSummary> GetInstallmentsBeforeDue(int days)
        {
            return _contractRepository.GetInstallmentsBeforeDue(days);
        }

        public IEnumerable<InstallmentReminderSummary> GetInstallmentsAfterDue(int days)
        {
            return _contractRepository.GetInstallmentsAfterDue(days);
        }

    }
}
