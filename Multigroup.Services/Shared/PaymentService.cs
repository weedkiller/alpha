using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Net.Mail;
using MCC.Common.Notifications;
using System.Configuration;
using System.Web;
using Multigroup.Common.PDF;

namespace Multigroup.Services.Shared
{
    public class PaymentService
    {
        private PaymentRepository _paymentRepository;
        private ContractRepository _contractRepository;
        private CustomerService _customerService;
        private EmailService _emailService;
        private ReceipService _receipService;
        private PdfGeneratorHelper _pdfGeneratorHelper;
        private CashierRepository _cashierRepository;
        private UserRepository _userRepository;
        private CashCycleRepository _cashCycleRepository;

        public PaymentService()
        {
            _customerService = new CustomerService();
            _emailService = new EmailService();
            _paymentRepository = new PaymentRepository();
            _contractRepository = new ContractRepository();
            _receipService = new ReceipService();
            _pdfGeneratorHelper = new PdfGeneratorHelper();
            _cashierRepository = new CashierRepository();
            _userRepository = new UserRepository();
            _cashCycleRepository = new CashCycleRepository();
        }
        public Payment GetPayment(int paymentId)
        {
            return _paymentRepository.GetPaymentById(paymentId);
        }

        public PaymentDate GetPaymentDate(int paymentDateId)
        {
            return _paymentRepository.GetPaymentDateById(paymentDateId);
        }

        public Payment GetPaymentByReceipNumber(string receipNumber, string paymentMethod)
        {
            return _paymentRepository.GetPaymentByReceipNumber(receipNumber, paymentMethod);
        }
        public IEnumerable<PaymentMethod> GetPaymentMethods()
        {
            return _paymentRepository.GetPaymentMethods();
        }
        public IEnumerable<PaymentType> GetPaymentTypes()
        {
            return _paymentRepository.GetPaymentTypes();
        }

        public IEnumerable<Payment> GetPaymentByClientId(int customerId)
        {
            return _paymentRepository.GetPaymentTypes(customerId);
        }

        public IEnumerable<PaymentResume> GetPaymentResume(int contractId)
        {
            return _paymentRepository.GetPaymentResume(contractId);
        }

        public IEnumerable<AssignState> GetAssignStates()
        {
            return _paymentRepository.GetAssignStates();
        }


        public IEnumerable<PaymentSummary> GetCollectionsByFilter(string SelectedPaymentMethod, string SelectedType, string SelectedPaymentPreference, string DateFrom, string DateTo, string UserId, string selectedCustomer)
        {
            return _paymentRepository.GetCollectionsByFilter(SelectedPaymentMethod, SelectedType, SelectedPaymentPreference, DateFrom, DateTo, UserId, selectedCustomer);
        }

        public IEnumerable<PaymentHistory> GetPaymentHistory(string _selectedCustomer, string _selectedPaymentMethod, string _selectedUser, string InstallmentNumber, string DateFrom, string DateTo, string province, string cities, string DateIFrom, string DateITo, string _selectedSupervisor)
        {
            return _paymentRepository.GetPaymentHistory(_selectedCustomer, _selectedPaymentMethod, _selectedUser, InstallmentNumber, DateFrom, DateTo, province, cities, DateIFrom, DateITo, _selectedSupervisor);
        }

        public AssignPaymentPreference GetAssignPaymentPreferenceById(int AssignPaymentPreferenceId)
        {
            return _paymentRepository.GetAssignPaymentPreferenceById(AssignPaymentPreferenceId);
        }

        public IEnumerable<AssignPaymentPreference> GetAssignPaymentPreferences()
        {
            return _paymentRepository.GetAssignPaymentPreferences();
        }

        public IEnumerable<AssignPaymentPreferenceSummary> GetAssignPaymentPreferenceSummaryByCuota(int installmentId)
        {
            return _paymentRepository.GetAssignPaymentPreferenceSummaryByCuota(installmentId);
        }

        public IEnumerable<AssignPaymentPreferenceSummary> GetAssignPaymentPreferenceSummary(string _selectedPaymentPreference, string _selectedUser, string _selectedState, string InstallmentNumber, string DateFrom, string DateTo, string customer)
        {
            return _paymentRepository.GetAssignPaymentPreferenceSummary(_selectedPaymentPreference, _selectedUser, _selectedState, InstallmentNumber, DateFrom, DateTo, customer);
        }

        public IEnumerable<AssignPaymentPreferenceSummary> GetAssignPaymentPreferenceSummaryByAssignPaymentMethod(string _selectedPaymentPreference, string _selectedUser, string isProcessed, string isAdvised)
        {
            return _paymentRepository.GetAssignPaymentPreferenceSummaryByAssignPaymentMethod(_selectedPaymentPreference, _selectedUser, isProcessed, isAdvised);
        }

        public IEnumerable<AssignPaymentPreferenceSummary> GetAssignPaymentPreferenceSummaryChannelWarned(string _selectedPaymentMethod, string _selectedUser, string _SelectedCustomer, string DateFrom, string DateTo, string isWarned)
        {
            return _paymentRepository.GetAssignPaymentPreferenceSummaryChannelWarned(_selectedPaymentMethod, _selectedUser, _SelectedCustomer, DateFrom, DateTo, isWarned);
        }

        public IEnumerable<AssignPaymentPreferenceWithVoucher> GetAssignPaymentPreferenceSummaryPaymentVoucher(string _selectedPaymentMethod, string _selectedUser, string _SelectedCustomer, string DateFrom, string DateTo, string DatePaymentFrom, string DatePaymentTo, string isPending)
        {
            return _paymentRepository.GetAssignPaymentPreferenceSummaryPaymentVoucher(_selectedPaymentMethod, _selectedUser, _SelectedCustomer, DateFrom, DateTo, DatePaymentFrom, DatePaymentTo, isPending);
        }

        public IEnumerable<PaymentVoucherDetail> GetPaymentVoucherDetail(int id)
        {
            return _paymentRepository.GetPaymentVoucherDetail(id);
        }

        public PaymentVoucher GetPaymentVoucher(int id)
        {
            return _paymentRepository.GetPaymentVoucher(id);
        }

        public IEnumerable<PaymentVoucherInstallment> GetPaymentVoucherInstallment(int id)
        {
            return _paymentRepository.GetPaymentVoucherInstallment(id);
        }

        public IEnumerable<PaymentVoucherSummary> GetPaymentVoucherSummary(string _selectedPaymentMethod, string _selectedUser, string _SelectedCustomer, string DateFrom, string DateTo, string isRegister)
        {
            return _paymentRepository.GetPaymentVoucherSummary(_selectedPaymentMethod, _selectedUser, _SelectedCustomer, DateFrom, DateTo, isRegister);
        }

        public void AddPaymentVoucher(PaymentVoucher paymentVoucher, List<PaymentVoucherInstallment> cuotas)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                   int voucher = _paymentRepository.AddPaymentVoucher(paymentVoucher);

                    foreach(PaymentVoucherInstallment cuota in cuotas)
                    {
                        cuota.PaymentVoucherId = voucher;
                        _paymentRepository.AddPaymentVoucherInstallment(cuota);
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

        public void AddAssignPaymentPreference(AssignPaymentPreference AssignPaymentPreference)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    int promissory = _paymentRepository.AddAssignPaymentPreference(AssignPaymentPreference);
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

        public void UpdateAssignPaymentPreference(AssignPaymentPreference AssignPaymentPreference)
        {

          _paymentRepository.UpdateAssignPaymentPreference(AssignPaymentPreference);
        }

        public void DeleteAssignPaymentPreference(int AssignPaymentPreferenceId)
        {
            _paymentRepository.DeleteAssignPaymentPreference(AssignPaymentPreferenceId);
        }

        public void Insert(Payment payment, int user)
        {
           using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
               try
                {
                    if (Int32.Parse(payment.PaymentMethod) != 3)
                        payment.MPSellerAccountId = null;
                    var idPago = _paymentRepository.AddPayment(payment, user);

                    ////Insertar en forma pago

                    //UserCashier userCashier = _userRepository.GetUserCashierByUserId(user);

                    //CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);


                    //PaymentPaymentMethod pm = new PaymentPaymentMethod();

                    //pm.PaymentId = idPago;
                    //pm.Saving = userCashier.CashierId.ToString();
                    //pm.Cycle = cashCycle.CashCycleId.ToString();
                    //pm.PaymentMethodId = int.Parse(payment.PaymentMethod);
                    //pm.Amount = payment.Amount;
                    //_paymentRepository.AddPaymentPaymentMethod(pm);

                    Receip oReceip = _receipService.GetReceipByNumber(payment.ReceipNumber);
                    if (oReceip != null)
                    _receipService.UpdateReceipStatus(oReceip.ReceipId, 15,null);
                    Contract contrato = _contractRepository.GetContractById(payment.ContractNumber);
                    int? estado = contrato.Status;
                    IEnumerable<Installment> cuotas = _contractRepository.GetInstallmentsByContractId(payment.ContractNumber);
                    double resto = payment.Amount;
                    if (payment.type == "1")
                    {
                        _contractRepository.UpdateContractStatus(contrato.ContractId, 19, contrato.StatusDetail.Value, contrato.PaperStatus.Value);
                        
                        
                        DateTime now = DateTime.Now;
                        if(estado.HasValue && estado.Value != 19)
                        _contractRepository.AddStatusContractHistory(contrato.ContractId, user, estado.Value, 19, "Cambio Automático por Pago de Cuota", now);
                        
                        
                        _contractRepository.ContractStatusControlRegular(payment.ContractNumber, payment.PaymentDate.Value);

                        IEnumerable<InstallmentInsterestsSummary> intereses = _contractRepository.GetInstallmentInsterestsByContractId(payment.ContractNumber);

                        foreach (InstallmentInsterestsSummary interes in intereses)
                        {
                            interes.PaymentId = idPago;
                            _contractRepository.UpdateInstallmentInsterests(interes);
                            resto = resto - interes.Amount;
                        }

                        foreach (Installment cuota in cuotas)
                        {
                            if (cuota.Status != "8" && resto > 0)
                            {
                                IEnumerable<PaymentAllocation> pagos = _contractRepository.GetPaymentAllocationByInstallmentId(cuota.InstallmentId);
                                if (pagos.Count() == 0)
                                {
                                    PaymentAllocation pago = new PaymentAllocation();
                                    pago.InstallmentId = cuota.InstallmentId;
                                    pago.PaymentId = idPago;
                                    if (cuota.OriginalAmount > resto)
                                    {
                                        pago.amount = resto;
                                        cuota.Status = "17";
                                    }
                                    else
                                    {
                                        pago.amount = cuota.OriginalAmount;
                                        cuota.Status = "8";
                                    }

                                    resto = resto - pago.amount;
                                    _contractRepository.AddPaymentAllocation(pago);
                                    cuota.PaymentDate = DateTime.Now;
                                    _contractRepository.UpdateInstallment(cuota);
                                }

                                else
                                {
                                    double total = 0;
                                    foreach (PaymentAllocation pago in pagos)
                                    {
                                        total = total + pago.amount;
                                    }
                                    PaymentAllocation nuevopago = new PaymentAllocation();
                                    nuevopago.InstallmentId = cuota.InstallmentId;
                                    nuevopago.PaymentId = idPago;
                                    if (cuota.OriginalAmount > (resto + total))
                                    {
                                        nuevopago.amount = resto;
                                        cuota.Status = "17";
                                    }
                                    else
                                    {
                                        nuevopago.amount = (cuota.OriginalAmount - total);
                                        cuota.Status = "8";
                                    }

                                    resto = resto - nuevopago.amount;
                                    _contractRepository.AddPaymentAllocation(nuevopago);
                                    cuota.PaymentDate = DateTime.Now;
                                    _contractRepository.UpdateInstallment(cuota);
                                }

                            }
                            //aca ingresar el registro del pago en la cuenta de mercadopago correspondiente;
                        }



                        Customer customer = _customerService.GetCustomer(contrato.CustomerId);
                        Payment pagoHecho = GetPayment(idPago);

                        String subject = EmailTemplates.GetPaymentSubject();
                        String body = EmailTemplates.BuildPaymentBody(customer, pagoHecho);
                        String to = customer.Email;

                        _emailService.SendEmail(to, subject, body, true, null);

                    }
                    else if (payment.type == "2")
                    {
                        _contractRepository.AddPaymentAdvance(1, payment.ContractNumber, resto, idPago);
                    }
                    else if (payment.type == "5")
                    {
                        _contractRepository.AddSuscription(contrato.CustomerId, payment.ContractNumber, resto, idPago);
                    }
                    else if (payment.type == "6")
                    {
                        _contractRepository.AddBonification(contrato.CustomerId, payment.ContractNumber, resto, idPago);
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

        public void InsertFromVoucher(PaymentVoucher voucher, IEnumerable<PaymentVoucherInstallment> voucherCuotas, int user, string contract)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    foreach (PaymentVoucherInstallment voucherCuota in voucherCuotas)
                    {
                        voucher.IsConfirmed = true;
                        voucher.ConfirmationDate = DateTime.Now;
                        voucher.ConfirmationUserId = user;
                        _paymentRepository.UpdatePaymentVoucher(voucher);

                        Payment payment = new Payment();
                        payment.PaymentDate = voucher.VoucherDate;
                        payment.PaymentMethod = voucher.PaymentMethodId.ToString();
                        payment.ReceipNumber = voucher.Code;
                        payment.Amount = voucherCuota.Amount;
                        payment.Observations = voucher.Commentary;
                        payment.type = "1";



                        var idPago = _paymentRepository.AddPayment(payment, user);


                        //inserta relacion

                        PaymentVoucherPayment voucherinsPayment = new PaymentVoucherPayment();
                        voucherinsPayment.PaymentId = idPago;
                        voucherinsPayment.PaymentVoucherInstallmentId = voucherCuota.PaymentVoucherInstallmentId;
                        _paymentRepository.AddPaymentVoucherPayment(voucherinsPayment);

                        //Insertar en forma pago

                        UserCashier userCashier = _userRepository.GetUserCashierByUserId(voucher.UserId);

                        CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);


                        PaymentPaymentMethod pm = new PaymentPaymentMethod();

                        pm.PaymentId = idPago;
                        pm.Saving = userCashier.CashierId.ToString();
                        pm.Cycle = cashCycle.CashCycleId.ToString();
                        pm.PaymentMethodId = voucher.PaymentMethodId;
                        pm.Amount = voucherCuota.Amount;
                        _paymentRepository.AddPaymentPaymentMethod(pm);



                        Contract contrato = _contractRepository.GetContractByNumber(contract).First();
                        int? estado = contrato.Status;
                        IEnumerable<Installment> cuotas = _contractRepository.GetInstallmentsByContractId(payment.ContractNumber);
                        double resto = payment.Amount;

                        _contractRepository.UpdateContractStatus(contrato.ContractId, 19, contrato.StatusDetail.Value, contrato.PaperStatus.Value);


                        DateTime now = DateTime.Now;
                        if (estado.HasValue && estado.Value != 19)
                            _contractRepository.AddStatusContractHistory(contrato.ContractId, user, estado.Value, 19, "Cambio Automático por Pago de Cuota", now);


                        _contractRepository.ContractStatusControlRegular(payment.ContractNumber, payment.PaymentDate.Value);

                        IEnumerable<InstallmentInsterestsSummary> intereses = _contractRepository.GetInstallmentInsterestsByContractId(payment.ContractNumber);

                        foreach (InstallmentInsterestsSummary interes in intereses)
                        {
                            interes.PaymentId = idPago;
                            _contractRepository.UpdateInstallmentInsterests(interes);
                            resto = resto - interes.Amount;
                        }

                        foreach (Installment cuota in cuotas)
                        {
                            if (cuota.Status != "8" && resto > 0)
                            {
                                IEnumerable<PaymentAllocation> pagos = _contractRepository.GetPaymentAllocationByInstallmentId(cuota.InstallmentId);
                                if (pagos.Count() == 0)
                                {
                                    PaymentAllocation pago = new PaymentAllocation();
                                    pago.InstallmentId = cuota.InstallmentId;
                                    pago.PaymentId = idPago;
                                    if (cuota.OriginalAmount > resto)
                                    {
                                        pago.amount = resto;
                                        cuota.Status = "17";
                                    }
                                    else
                                    {
                                        pago.amount = cuota.OriginalAmount;
                                        cuota.Status = "8";
                                    }

                                    resto = resto - pago.amount;
                                    _contractRepository.AddPaymentAllocation(pago);
                                    cuota.PaymentDate = DateTime.Now;
                                    _contractRepository.UpdateInstallment(cuota);
                                }

                                else
                                {
                                    double total = 0;
                                    foreach (PaymentAllocation pago in pagos)
                                    {
                                        total = total + pago.amount;
                                    }
                                    PaymentAllocation nuevopago = new PaymentAllocation();
                                    nuevopago.InstallmentId = cuota.InstallmentId;
                                    nuevopago.PaymentId = idPago;
                                    if (cuota.OriginalAmount > (resto + total))
                                    {
                                        nuevopago.amount = resto;
                                        cuota.Status = "17";
                                    }
                                    else
                                    {
                                        nuevopago.amount = (cuota.OriginalAmount - total);
                                        cuota.Status = "8";
                                    }

                                    resto = resto - nuevopago.amount;
                                    _contractRepository.AddPaymentAllocation(nuevopago);
                                    cuota.PaymentDate = DateTime.Now;
                                    _contractRepository.UpdateInstallment(cuota);
                                }

                            }
                            
                        }                       
                    }

                    foreach (PaymentVoucherInstallment voucherCuota in voucherCuotas)
                    {
                        Contract contrat = _contractRepository.GetContractByNumber(contract).First();
                        Customer customer = _customerService.GetCustomer(contrat.CustomerId);
                        Payment pagoHecho = new Payment();
                        pagoHecho.Amount = voucherCuota.Amount;
                        pagoHecho.PaymentDate = voucher.VoucherDate;

                        String subject = EmailTemplates.GetPaymentSubject();
                        String body = EmailTemplates.BuildPaymentBody(customer, pagoHecho);
                        String to = customer.Email;

                        _emailService.SendEmail(to, subject, body, true, null);
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

        public void InsertFromSurrender(InstallmentSurrender surrender, int user, string contract)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {


                        Payment payment = new Payment();
                        payment.PaymentDate = DateTime.Now;
                        payment.PaymentMethod = "6";
                        payment.ReceipNumber = "Cobrador";
                        payment.Amount = surrender.Amount;
                        payment.type = "1";



                        var idPago = _paymentRepository.AddPayment(payment, user);


                        //inserta relacion

                        //PaymentVoucherPayment voucherinsPayment = new PaymentVoucherPayment();
                        //voucherinsPayment.PaymentId = idPago;
                        //voucherinsPayment.PaymentVoucherInstallmentId = voucherCuota.PaymentVoucherInstallmentId;
                        //_paymentRepository.AddPaymentVoucherPayment(voucherinsPayment);

                        ////Insertar en forma pago

                        //UserCashier userCashier = _userRepository.GetUserCashierByUserId(voucher.UserId);

                        //CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);


                        //PaymentPaymentMethod pm = new PaymentPaymentMethod();

                        //pm.PaymentId = idPago;
                        //pm.Saving = userCashier.CashierId.ToString();
                        //pm.Cycle = cashCycle.CashCycleId.ToString();
                        //pm.PaymentMethodId = 6;
                        //pm.Amount = surrender.Amount;
                        //_paymentRepository.AddPaymentPaymentMethod(pm);



                        Contract contrato = _contractRepository.GetContractByNumber(contract).First();
                        int? estado = contrato.Status;
                        IEnumerable<Installment> cuotas = _contractRepository.GetInstallmentsByContractId(payment.ContractNumber);
                        double resto = payment.Amount;

                        _contractRepository.UpdateContractStatus(contrato.ContractId, 19, contrato.StatusDetail.Value, contrato.PaperStatus.Value);


                        DateTime now = DateTime.Now;
                        if (estado.HasValue && estado.Value != 19)
                            _contractRepository.AddStatusContractHistory(contrato.ContractId, user, estado.Value, 19, "Cambio Automático por Pago de Cuota", now);


                        _contractRepository.ContractStatusControlRegular(payment.ContractNumber, payment.PaymentDate.Value);

                        IEnumerable<InstallmentInsterestsSummary> intereses = _contractRepository.GetInstallmentInsterestsByContractId(payment.ContractNumber);

                        foreach (InstallmentInsterestsSummary interes in intereses)
                        {
                            interes.PaymentId = idPago;
                            _contractRepository.UpdateInstallmentInsterests(interes);
                            resto = resto - interes.Amount;
                        }

                        foreach (Installment cuota in cuotas)
                        {
                            if (cuota.Status != "8" && resto > 0)
                            {
                                IEnumerable<PaymentAllocation> pagos = _contractRepository.GetPaymentAllocationByInstallmentId(cuota.InstallmentId);
                                if (pagos.Count() == 0)
                                {
                                    PaymentAllocation pago = new PaymentAllocation();
                                    pago.InstallmentId = cuota.InstallmentId;
                                    pago.PaymentId = idPago;
                                    if (cuota.OriginalAmount > resto)
                                    {
                                        pago.amount = resto;
                                        cuota.Status = "17";
                                    }
                                    else
                                    {
                                        pago.amount = cuota.OriginalAmount;
                                        cuota.Status = "8";
                                    }

                                    resto = resto - pago.amount;
                                    _contractRepository.AddPaymentAllocation(pago);
                                    cuota.PaymentDate = DateTime.Now;
                                    _contractRepository.UpdateInstallment(cuota);
                                }

                                else
                                {
                                    double total = 0;
                                    foreach (PaymentAllocation pago in pagos)
                                    {
                                        total = total + pago.amount;
                                    }
                                    PaymentAllocation nuevopago = new PaymentAllocation();
                                    nuevopago.InstallmentId = cuota.InstallmentId;
                                    nuevopago.PaymentId = idPago;
                                    if (cuota.OriginalAmount > (resto + total))
                                    {
                                        nuevopago.amount = resto;
                                        cuota.Status = "17";
                                    }
                                    else
                                    {
                                        nuevopago.amount = (cuota.OriginalAmount - total);
                                        cuota.Status = "8";
                                    }

                                    resto = resto - nuevopago.amount;
                                    _contractRepository.AddPaymentAllocation(nuevopago);
                                    cuota.PaymentDate = DateTime.Now;
                                    _contractRepository.UpdateInstallment(cuota);
                                }

                            }

                        }

                        Contract contrat = _contractRepository.GetContractByNumber(contract).First();
                        Customer customer = _customerService.GetCustomer(contrat.CustomerId);
                        Payment pagoHecho = new Payment();
                        pagoHecho.Amount = surrender.Amount;
                        pagoHecho.PaymentDate = DateTime.Now;

                        String subject = EmailTemplates.GetPaymentSubject();
                        String body = EmailTemplates.BuildPaymentBody(customer, pagoHecho);
                        String to = customer.Email;

                        _emailService.SendEmail(to, subject, body, true, null);

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

        public void InsertVoucher(HttpPostedFileBase file, string code, DateTime fecha)
        {
            try
            {
                var path = SaveAttach(file, code, fecha);
            }
            catch (Exception ex)
            {
            }
        }

        public string SaveAttach(HttpPostedFileBase file, string code, DateTime fecha)
        {
            DateTime fechaActual = fecha;


            var path = ConfigurationManager.AppSettings["VoucherPath"];
            var filePath = System.AppDomain.CurrentDomain.BaseDirectory + path;
            filePath = System.IO.Path.Combine(filePath.ToString(), fechaActual.Year.ToString());
            path = System.IO.Path.Combine(path.ToString(), fechaActual.Year.ToString());

            filePath = System.IO.Path.Combine(filePath.ToString(), fechaActual.Month.ToString());
            path = System.IO.Path.Combine(path.ToString(), fechaActual.Month.ToString());

            filePath = System.IO.Path.Combine(filePath.ToString(), code);
            path = System.IO.Path.Combine(path.ToString(), code);


            bool isExists = System.IO.Directory.Exists(filePath);

            if (!isExists)
                System.IO.Directory.CreateDirectory(filePath);

            var pathFinal = string.Format("{0}\\{1}", filePath, file.FileName);


            _pdfGeneratorHelper.SaveFileContents(pathFinal, file.InputStream);

            return path;
        }
    }
};
