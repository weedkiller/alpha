using Multigroup.Common.Logging;
using Multigroup.Portal.Filters;
using Multigroup.Portal.Providers;
using Multigroup.Portal.Models.ContractModel;
using Multigroup.Portal.Models.Shared;
using Multigroup.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multigroup.Portal.Models.Collection;
using Multigroup.Portal.Helpers;
using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Utilities;

namespace Multigroup.Portal.Controllers
{
    public class CollectionsController : Controller
    {
        private PaymentService _paymentService;
        private ReceipService _receipService;
        private ContractService _contractService;
        private UserService _userService;
        private MPSellerAccountService _mpSellerAcount;
        private ProfileProvider _profileProvider;
        private CustomerService _customerService;
        private PaymentMethodsService _paymentMethodsService;
        private List<HttpPostedFileBase> _filesLoad;
        private VisitService _visitService;

        public CollectionsController()
        {
            _contractService = new ContractService();
            _paymentService = new PaymentService();
            _userService = new UserService();
            _receipService = new ReceipService();
            _mpSellerAcount = new MPSellerAccountService();
            _profileProvider = new ProfileProvider();
            _customerService = new CustomerService();
            _paymentMethodsService = new PaymentMethodsService();
            _visitService = new VisitService();
        }
        public ActionResult Index()
        {
            var model = new CollectionsFiltersVm();
            var collections = new CollectionHelper();

            model.ListPaymentMethod = new SelectList(_paymentService.GetPaymentMethods(), "PaymentMethodId", "Name");
            model.ListType = new SelectList(_paymentService.GetPaymentTypes(), "PaymentTypeId", "Name");
            model.ddlPaymentPreference = collections.GetPaymentPreferenceSingleSelectVm();
            model.ListUsers = new SelectList(_userService.GetUsersCollection().Where(x => x.UserType == 7), "UserId", "Names");
            var customer = _contractService.GetCustomer();
            var items = customer.Select(x => new Item { Id = x.CustomerId, Name = x.Surname + " " + x.Name });
            model.ListCustomers = new SelectList(items, "Id", "Name");
            return PartialView("~/Views/Collections/Collection/Collections.cshtml", model);
        }

        public ActionResult GetCollectionsHistory(List<string> SelectedPaymentMethod, List<string> SelectedType, List<string> SelectedPaymentPreference, string DateFrom, string DateTo, List<string> UserId, List<string> SelectedCustomer)
        {
            if (DateFrom == "")
                DateFrom = "01/01/1900";
            if (DateTo == "")
                DateTo = "01/01/2100";
            string selectedPaymentMethod = (SelectedPaymentMethod[0] != "" && SelectedPaymentMethod.Count() > 0) ? String.Join(",", SelectedPaymentMethod) : "0";
            string selectedType = (SelectedType[0] != "" && SelectedType.Count() > 0) ? String.Join(",", SelectedType) : "0";
            string selectedPaymentPreference = (SelectedPaymentPreference[0] != "" && SelectedPaymentPreference.Count() > 0) ? String.Join(",", SelectedPaymentPreference) : "0";
            string userId = (UserId[0] != "" && UserId.Count() > 0) ? String.Join(",", UserId) : "0";
            string _selectedCustomer = (SelectedCustomer[0] != "" && SelectedCustomer.Count() > 0) ? String.Join(",", SelectedCustomer) : "0";
            IEnumerable<PaymentSummary> pagos = _paymentService.GetCollectionsByFilter(selectedPaymentMethod, selectedType, selectedPaymentPreference, DateFrom, DateTo, userId, _selectedCustomer);
            double total = 0;
            foreach(PaymentSummary pago in pagos)
            {
                total += pago.Amount;
            }
            ViewBag.Total = total;
            var model = CollectionsTableVm.ToViewModel(pagos);
            return PartialView("~/Views/Collections/Collection/CollectionsTable.cshtml", model);
        }

        // GET: Collections/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Collections/Create
        public ActionResult Create()
        {
            var model = new CollectionVm();
            var collections = new CollectionHelper();
            var user = _profileProvider.CurrentUserId;
            var rol = _userService.GetUser(user).Roles;
            ViewBag.Rol = rol.FirstOrDefault().RoleId;
            model.ListPaymentMethod = new SelectList(_paymentService.GetPaymentMethods(), "PaymentMethodId", "Name");
            model.ListType = new SelectList(_paymentService.GetPaymentTypes(), "PaymentTypeId", "Name");
            model.ddlMPSellerAccount = collections.GetMPSellerAccountSingleSelectVm();
            return PartialView("~/Views/Collections/Collection/CollectionCreate.cshtml", model);
        }

        // POST: Collections/Create
        [HttpPost]
        public ActionResult Create(CollectionVm model)
        {
            try
            {
                var payment = model.ToModelObject();
                Contract contract = _contractService.GetContract(payment.ContractNumber);
                if (contract.Status == 2)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El Contrato se encuentra en estado de Scoring");
                }
                else
                {

                    IEnumerable<InstallmentInsterestsSummary> intereses = _contractService.GetInstallmentInsterestsByContractId(contract.ContractId);
                    double total = 0;
                    foreach(InstallmentInsterestsSummary interes in intereses)
                    {
                        total += interes.Amount;
                    }
                    if (payment.Amount <= total)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El Monto a pagar no alcanza a abonar el total de los intereses generados por lo que no se puede abonar la cuota");
                    }


                    if (payment.PaymentMethod == "1" && payment.PaymentMethod == "2" && payment.PaymentMethod == "6")
                    {
                        Payment pago = new Payment();
                        string[] letras = payment.ReceipNumber.Split('-');
                        if (letras.Count() > 1)
                            pago = _paymentService.GetPaymentByReceipNumber(letras[1], payment.PaymentMethod);
                        else
                            pago = _paymentService.GetPaymentByReceipNumber(letras[0], payment.PaymentMethod);


                        if (pago != null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El Número de comprobante ya ha sido cargado con anterioridad en este medio de pago");
                        }

                    }
                    else
                    {
                        Payment pago = _paymentService.GetPaymentByReceipNumber(payment.ReceipNumber, payment.PaymentMethod);

                        if (pago != null )
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El Número de comprobante ya ha sido cargado con anterioridad en este medio de pago");
                        }
                    }



                    Receip oReceip = _receipService.GetReceipByNumber(payment.ReceipNumber);
                    if ((oReceip == null && payment.PaymentMethod != "3" && payment.PaymentMethod != "4" && payment.PaymentMethod != "8" && payment.PaymentMethod != "10") && !payment.type.Equals("6"))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El Número de comprobante no existe");
                    }

                    if (payment.type.Equals("6") && payment.Amount > 6500)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El Monto debe ser menor o igual a $6500");
                    }

                    Customer oCustomer = _customerService.GetCustomer(contract.CustomerId);

                    if (oCustomer.Email == null || !Util.isValidEmail(oCustomer.Email))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El cliente debe tener un email válido");
                    }
                    
                    _paymentService.Insert(payment, _profileProvider.CurrentUserId);
                    return RedirectToAction("Create");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Collections/Edit/5
        public ActionResult Resume(int id)
        {
            IEnumerable<PaymentResume> resume = _paymentService.GetPaymentResume(id);
            var model = CollectionsResumeTableVm.ToViewModel(resume);
            return PartialView("~/Views/Collections/Collection/CollectionsResume.cshtml", model);
        }

        // POST: Collections/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Collections/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Collections/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult IndexPaymentVoucher()
        {
            AssignPaymentPreferenceFiltersVm model = new AssignPaymentPreferenceFiltersVm();
            var collections = new CollectionHelper();
            model.ListUser = new SelectList(_userService.GetUsersTelemarketer(), "Id", "Name");
            model.ListPaymentMethod = new SelectList(_paymentMethodsService.GetPaymentMethodss(), "PaymentMethodId", "Name");
            model.ListCustomers = new SelectList(_customerService.GetCustomersComplete(), "CustomerId", "Name");
            model.Pending = false;
            return PartialView("~/Views/Collections/Collection/PaymentVoucher.cshtml", model);
        }

        public ActionResult GetTablePaymentVoucher(List<string> Telemarketer, List<string> PaymentMethod, List<string> Customer, string DateFrom, string DatePaymentTo, string DatePaymentFrom, string DateTo, string IsPending)
        {
            string Telemarketers = (Telemarketer[0] != "" && Telemarketer.Count() > 0) ? String.Join(",", Telemarketer) : "0";
            string PaymentMethods = (PaymentMethod[0] != "" && PaymentMethod.Count() > 0) ? String.Join(",", PaymentMethod) : "0";
            string Customers = (Customer[0] != "" && Customer.Count() > 0) ? String.Join(",", Customer) : "0";
            if (DateFrom == "")
                DateFrom = "01/01/1900";
            if (DateTo == "")
                DateTo = "01/01/2100";
            if (DatePaymentFrom == "")
                DatePaymentFrom = "01/01/1900";
            if (DatePaymentTo == "")
                DatePaymentTo = "01/01/2100";
            var assign = _paymentService.GetAssignPaymentPreferenceSummaryPaymentVoucher(PaymentMethods, Telemarketers, Customers, DateFrom, DateTo, DatePaymentFrom, DatePaymentTo, IsPending);
            var model = AssignPaymentPreferenceWithVoucherTableVm.ToViewModel(assign);
            return PartialView("~/Views/Collections/Collection/PaymentVoucherTable.cshtml", model);
        }

        [LogActionFilter]
        [Description("Register Voucher")]
        public ActionResult VoucherRegister(string number)
        {
            var model = new VoucherVm();
            var collections = new CollectionHelper();


            Contract contrato = _contractService.GetContractByNumber(number).First();
            Customer cliente = _customerService.GetCustomer(contrato.CustomerId);
            model.ContractNumber = number;
            model.Name = cliente.Surname + " " + cliente.Name;
            model.ddlPaymentMethod = collections.GetPaymentMethodSingleSelectVm();
            model.installments = _paymentService.GetAssignPaymentPreferenceSummaryPaymentVoucher("0", "0", cliente.CustomerId.ToString(), "01/01/1900", "01/01/2100", "01/01/1900", "01/01/2100", "1"); ;
            double monto = 0;
            int pm = 0;
            bool pmb = false;
            foreach (AssignPaymentPreferenceWithVoucher cuotas in model.installments)
            {
                if (pm == 0)
                    pm = cuotas.PaymentMethodId;
                monto = monto + (cuotas.Amount - cuotas.PaymentAmount);

                if (pm != cuotas.PaymentMethodId)
                    pmb = true;
            }
            ViewBag.Amount = monto.ToString();
            Session["SessionTable"] = model.installments;
            Session["pm"] = pm;
            Session["pmb"] = pmb;
            model.ddlPaymentMethod.SelectedItem = pm.ToString();
            return PartialView("~/Views/Collections/Collection/VoucherRegister.cshtml", model);
        }

        public ActionResult InstallmentByAmount(string total)
        {
            IEnumerable<AssignPaymentPreferenceWithVoucher> list = (IEnumerable<AssignPaymentPreferenceWithVoucher>)Session["SessionTable"];
            List<VoucherListVm> listColor = new List<VoucherListVm>();
            VoucherTableVm model = VoucherTableVm.ToViewModel(list);
            double tot = double.Parse(total);
            foreach (VoucherListVm item in model.VoucherList)
            {
                if (tot <= 0)
                    item.Color = "black";
                else
                {
                    tot = tot - (item.Amount - item.PaymentAmount);
                    if (tot > 0 || tot == 0)
                        item.Color = "green";
                    else
                        item.Color = "#ecca14";
                }

                listColor.Add(item);
            }
            model.VoucherList = listColor;
            return PartialView("~/Views/Collections/Collection/VoucherRegisterTable.cshtml", model);
        }



        //Files


        [HttpPost]
        public ActionResult CreateVoucher(VoucherVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, message);
                }

                if (Convert.ToDateTime(model.PaymentDate) > DateTime.Now.Date)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de comprobante no puede ser mayor a la actual");
                }

                bool pmb = (bool)Session["pmb"];

                if(pmb && model.Commentary.Equals(""))
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe ingresar comentario porque una cuota no se corresponde con el medio de pago del comprobante");

                if (SaveFile(model))
                {

                    PaymentVoucher voucher = model.ToModelObject();
                    voucher.Commentary = model.Commentary;
                    voucher.IsConfirmed = false;
                    voucher.UserId = _profileProvider.CurrentUserId;

                    IEnumerable<AssignPaymentPreferenceWithVoucher> list = (IEnumerable<AssignPaymentPreferenceWithVoucher>)Session["SessionTable"];
                    List<PaymentVoucherInstallment> cuotas = new List<PaymentVoucherInstallment>();


                    double total = model.Amount;

                    foreach (AssignPaymentPreferenceWithVoucher item in list)
                    {

                        if (total > 0)
                        {
                            PaymentVoucherInstallment cuota = new PaymentVoucherInstallment();
                            total = total - (item.Amount - item.PaymentAmount);
                            if (total > 0 || total == 0)
                            {
                                cuota.AssignPaymentPreferenceId = item.AssignPaymentPreferenceId;
                                cuota.Amount = (item.Amount - item.PaymentAmount);
                                cuota.InstallmentId = item.InstallmentId;
                                cuota.IsSamePaymentMethod = (voucher.PaymentMethodId == item.PaymentMethodId) ? true : false;
                            }
                            else
                            {
                                cuota.AssignPaymentPreferenceId = item.AssignPaymentPreferenceId;
                                cuota.Amount = total + (item.Amount - item.PaymentAmount);
                                cuota.InstallmentId = item.InstallmentId;
                                cuota.IsSamePaymentMethod = (voucher.PaymentMethodId == item.PaymentMethodId) ? true : false;
                            }
                            cuotas.Add(cuota);
                        }
                    }

                    _paymentService.AddPaymentVoucher(voucher, cuotas);


                    return IndexPaymentVoucher();
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe ingresar comprobante");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        public ActionResult LoadFile()
        {
            bool isSavedSuccessfully = true;

            if (SessionProvider.UpLoadFiles == null)
            {
                _filesLoad = new List<HttpPostedFileBase>();
            }
            else
            {
                _filesLoad = SessionProvider.UpLoadFiles;
            }

            string fName = string.Empty;

            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName + DateTime.Now.ToString();
                    if (file != null && file.ContentLength > 0)
                    {
                        _filesLoad.Add(file);
                        SessionProvider.UpLoadFiles = _filesLoad;
                    }
                }
            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error al cargar el archivo" });
            }
        }

        public ActionResult RemoveFiles(string fileDelete)
        {
            bool isSavedSuccessfully = true;
            try
            {
                if (SessionProvider.UpLoadFiles != null)
                {
                    _filesLoad = SessionProvider.UpLoadFiles;
                    HttpPostedFileBase file = null;

                    foreach (HttpPostedFileBase fileName in _filesLoad)
                    {
                        if (fileName.FileName.Equals(fileDelete))
                        {
                            file = fileName;
                        }
                    }

                    if (file != null)
                    {
                        _filesLoad.Remove(file);
                    }
                    SessionProvider.UpLoadFiles = _filesLoad;
                }
            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = "ok" });
            }
            else
            {
                return Json(new { Message = "Error al eliminar el archivo" });
            }
        }

        public bool SaveFile(VoucherVm voucher)
        {

            bool archivo = false;
            string fName = string.Empty;

            try
            {
                if (SessionProvider.UpLoadFiles != null)
                {
                    foreach (HttpPostedFileBase fileName in SessionProvider.UpLoadFiles)
                    {
                        //   HttpPostedFileBase file = fileName;
                        //Save file content goes here
                        if (fileName != null && fileName.ContentLength > 0)
                        {
                            _paymentService.InsertVoucher(fileName, voucher.ContractNumber, Util.ParseDateTime(voucher.PaymentDate).Value);
                            archivo = true;
                        }
                    }

                }
                return archivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SessionProvider.UpLoadFiles = null;
            }
        }

        public ActionResult GetSavedFiles()
        {

            var model = new List<DataUploadFilesResultVm>();

            return PartialView("~/Views/Collections/Collection/VoucherAttach.cshtml", model);
        }


        // Imputación cobro de cuota

        public ActionResult IndexImputPayment()
        {
            AssignPaymentPreferenceFiltersVm model = new AssignPaymentPreferenceFiltersVm();
            var collections = new CollectionHelper();
            model.ListUser = new SelectList(_userService.GetUsersTelemarketer(), "Id", "Name");
            model.ListPaymentMethod = new SelectList(_paymentMethodsService.GetPaymentMethodss(), "PaymentMethodId", "Name");
            model.ListCustomers = new SelectList(_customerService.GetCustomersComplete(), "CustomerId", "Name");
            model.Register = true;
            return PartialView("~/Views/Collections/Collection/ImputPayment.cshtml", model);
        }

        public ActionResult GetTableImputPayment(List<string> Telemarketer, List<string> PaymentMethod, List<string> Customer, string DateFrom, string DateTo, string IsRegister)
        {
            string Telemarketers = (Telemarketer[0] != "" && Telemarketer.Count() > 0) ? String.Join(",", Telemarketer) : "0";
            string PaymentMethods = (PaymentMethod[0] != "" && PaymentMethod.Count() > 0) ? String.Join(",", PaymentMethod) : "0";
            string Customers = (Customer[0] != "" && Customer.Count() > 0) ? String.Join(",", Customer) : "0";
            if (DateFrom == "")
                DateFrom = "01/01/1900";
            if (DateTo == "")
                DateTo = "01/01/2100";
            var assign = _paymentService.GetPaymentVoucherSummary(PaymentMethods, Telemarketers, Customers, DateFrom, DateTo, IsRegister);
            var model = PaymentVoucherSummaryTableVm.ToViewModel(assign);
            return PartialView("~/Views/Collections/Collection/ImputPaymentTable.cshtml", model);
        }

        public ActionResult PaymentVoucherDetail(int id)
        {
            var model = PaymentVoucherDetailTableVm.ToViewModel(_paymentService.GetPaymentVoucherDetail(id));
            return PartialView("~/Views/Collections/Collection/PaymentVoucherDetailTable.cshtml", model);
        }

        public ActionResult ImputPaymentVoucher(int PaymentVoucherId, string contract)
        {
            try
            {
                PaymentVoucher voucher = _paymentService.GetPaymentVoucher(PaymentVoucherId);

                IEnumerable<PaymentVoucherInstallment> cuotas = _paymentService.GetPaymentVoucherInstallment(PaymentVoucherId);

                _paymentService.InsertFromVoucher(voucher, cuotas, _profileProvider.CurrentUserId, contract);

                return IndexImputPayment();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        public ActionResult IndexSurrenderImput()
        {
            AssignPaymentPreferenceFiltersVm model = new AssignPaymentPreferenceFiltersVm();
            var collections = new CollectionHelper();
            model.ListUser = new SelectList(_userService.GetUsersCollector(), "Id", "Name");
            return PartialView("~/Views/Collections/Collection/ImputSurrenderImput.cshtml", model);
        }

        public ActionResult GetTableSurrenderImput(List<string> Collector)
        {
            string Collectors = (Collector[0] != "" && Collector.Count() > 0) ? String.Join(",", Collector) : "0";

            var assign = _visitService.GetInstallmentSurrenderSummary(Collectors);
            var model = InstallmentSurrenderTableVm.ToViewModel(assign);
            return PartialView("~/Views/Collections/Collection/ImputSurrenderImputTable.cshtml", model);
        }

        public ActionResult ImputSurrenderImput(int InstallmentSurrenderId, string contract)
        {
            try
            {
                InstallmentSurrender surrender = _visitService.GetInstallmentSurrenderById(InstallmentSurrenderId);


                _paymentService.InsertFromSurrender(surrender, _profileProvider.CurrentUserId, contract);

                return IndexImputPayment();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
    }
}
