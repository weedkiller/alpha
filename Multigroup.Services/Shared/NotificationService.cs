using MCC.Common.Notifications;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Configuration;
using System.Threading.Tasks;
using Multigroup.Repositories.Shared;


namespace Multigroup.Services.Shared
{
    class NotificationService
    {
        private CustomerService _customerService;
        private PaymentService _paymentService;
        private EmailService _emailService;
        private GeneralService _generalService;
        private ContractRepository _contractRepository;

        public NotificationService()
        {
            _customerService = new CustomerService();
            _emailService = new EmailService();
            _paymentService = new PaymentService();
            _generalService = new GeneralService();
            _contractRepository = new ContractRepository();
        }

        public void Welcome(int customerId)
        {
            Contract contrato = _contractRepository.GetContractByCustomerId(customerId);
            Customer customer = _customerService.GetCustomer(customerId);
            String subject = "";
            String body = "";
            String to = customer.Email;

            subject = EmailTemplates.GetDebtCancelationSubject();
            body = EmailTemplates.BuildDebtCancelationBody();

            _emailService.SendEmail(to, subject, body, true, null);
        }

        public void NotifyScoringContract(Contract contract)
        {
            Customer customer = _customerService.GetCustomer(contract.CustomerId);

            String subject = EmailTemplates.GetScoringContractSubject(customer);
            var link = "http://adm.concesionariamg.com/contract/ScoringContract?id=" + _generalService.Encriptar(contract.ContractId.ToString());
            String body = EmailTemplates.BuildScoringContractBody(customer, link);
            String to = customer.Email;

            _emailService.SendEmail("link.mgconcesionaria@gmail.com", subject, body, true, null);
            _emailService.SendEmail(to, subject, body, true, null);
        }

        public void RemindPayment(InstallmentReminderSummary installment, Attachment attachment)
        {

            String subject = EmailTemplates.GetReminderSubject();
            String body = EmailTemplates.BuildReminderBody();
            String to = installment.CustomerEmail;

            _emailService.SendEmail(to, subject, body, true, attachment);
        }

        public void IntimateForPayment(InstallmentReminderSummary installment)
        {
            String subject = EmailTemplates.GetIntimationSubject();
            String body = EmailTemplates.BuildIntimationBody();
            String to = installment.CustomerEmail;

            _emailService.SendEmail(to, subject, body, true, null);
        }

        public void NotifyPayment(int customerId, int paymentId)
        {
            Customer customer = _customerService.GetCustomer(customerId);
            Payment payment = _paymentService.GetPayment(paymentId);

            String subject = EmailTemplates.GetPaymentSubject();
            String body = EmailTemplates.BuildPaymentBody(customer, payment);
            String to = customer.Email;

            _emailService.SendEmail(to, subject, body, true, null);
        }

        public void NotifyCancelation(int customerId)
        {
            Customer customer = _customerService.GetCustomer(customerId);

            String subject = EmailTemplates.GetCancelationSubject();
            String body = EmailTemplates.BuildCancelationBody();
            String to = customer.Email;

            Attachment attachment = new Attachment(EmailTemplates.GetCancelationAttachPath());
            _emailService.SendEmail(to, subject, body, true, attachment);
            _emailService.SendEmail("contrato.mgconcesionaria@gmail.com", subject, body, true, attachment);
        }

        public void NotifySignContract(string path, int customerId, int contractId, IEnumerable<ContractFile> documentos, User user)
        {
            Customer customer = _customerService.GetCustomer(customerId);

            String subject = EmailTemplates.GetSingSubject();
            String body = EmailTemplates.BuildSignBody(customer);
            String to = customer.Email;

            String toUser = user.Email;


            Attachment attachment = new Attachment(path);

            _emailService.SendEmail(to, subject, body, true, attachment);

            List<Attachment> attachments = new List<Attachment>();
            foreach (ContractFile documento in documentos)
            {
                var pathDNI = ConfigurationManager.AppSettings["ActionPlanDocumentWebPath"];
                var filePath = System.AppDomain.CurrentDomain.BaseDirectory + pathDNI; 
                filePath = System.IO.Path.Combine(filePath.ToString(), contractId.ToString());

                attachments.Add(new Attachment(string.Format("{0}\\{1}", filePath, documento.Name)));
            }

            _emailService.SendEmailSeller(toUser, subject, body, true, attachment, attachments);

            _emailService.SendEmailSeller("contrato.mgconcesionaria@gmail.com", subject, body, true, attachment, attachments);

        }

        public void  NotifyNewContract(Contract contract)
        {
            Customer customer = _customerService.GetCustomer(contract.CustomerId);

            String subject = EmailTemplates.GetNewContractSubject(customer);
            var link = "http://adm.concesionariamg.com/contract/SignContract?id=" + _generalService.Encriptar(contract.ContractId.ToString());
            String body = EmailTemplates.BuildNewContractBody(customer, link);
            String to = customer.Email;

             _emailService.SendEmail(to, subject, body, true, null);
             _emailService.SendEmail("link.mgconcesionaria@gmail.com", subject, body, true, null);

        }

        public void NotifyContract(int customerId)
        {
            Customer customer = _customerService.GetCustomer(customerId);

            String subject = EmailTemplates.GetContractSubject();
            String body = EmailTemplates.BuildContractBody(customer);
            String to = customer.Email;

            Attachment attachment = new Attachment(EmailTemplates.GetCancelationAttachPath());

            _emailService.SendEmail(to, subject, body, true, attachment);
        }

        public void NotifyAdjudication(int customerId)
        {
            Customer customer = _customerService.GetCustomer(customerId);

            String subject = EmailTemplates.GetAdjudicationSubject();
            String body = EmailTemplates.BuildAdjudicationBody();
            String to = customer.Email;

            Attachment attachment = new Attachment(EmailTemplates.GetAdjudicationAttachPath());

            _emailService.SendEmail(to, subject, body, true, attachment);
        }

        public void NotifyCancelationWeb(int contractId)
        {
            Contract contrato = _contractRepository.GetContractWebById(contractId);
            Customer customer = _customerService.GetCustomer(contrato.CustomerId);
            String subject = "";
            String body = "";
            String to = customer.Email;


                subject = EmailTemplates.GetCancelationWebSubject();
                body = EmailTemplates.BuildCancelationWebBody();

                _emailService.SendEmail(to, subject, body, true, null);
                _emailService.SendEmail("contrato.mgconcesionaria@gmail.com", subject, body, true, null);

        }

        public void NotifyScoringContract(string path, int customerId, int contractId, IEnumerable<ContractFile> documentos, User user)
        {
            Customer customer = _customerService.GetCustomer(customerId);

            String subject = EmailTemplates.GetWelcomeSubject();
            String body = EmailTemplates.BuildWelcomeBody(customer);
            String to = customer.Email;

            String toUser = user.Email;


            Attachment attachment = new Attachment(path);

            _emailService.SendEmail(to, subject, body, true, attachment);

            List<Attachment> attachments = new List<Attachment>();

            _emailService.SendEmailSeller(toUser, subject, body, true, attachment, attachments);

            _emailService.SendEmailSeller("contrato.mgconcesionaria@gmail.com", subject, body, true, attachment, attachments);

        }

        public void NotifyDebtCancelation(int customerId)
        {
            Customer customer = _customerService.GetCustomer(customerId);

            String subject = EmailTemplates.GetDebtCancelationSubject();
            String body = EmailTemplates.BuildDebtCancelationBody();
            String to = customer.Email;

            _emailService.SendEmail(to, subject, body, true, null);
        }

    }
}
