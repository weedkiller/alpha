using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using Multigroup.Services.Shared;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Services.Job
{
    public class InstallmentReminder : IJob
    {
        private ContractService _contractService;
        private EmailService _emailService;
        private MPSellerAccountService _mpSellerAccountService;
        private MPService _mpService;

        private readonly int daysBeforeStart = 1; //1
        private readonly int daysBeforeDue = 5; //5
        private readonly int daysAfterDue = 5; //5

        private NotificationService _notificationService;

        public InstallmentReminder()
        {
            _contractService = new ContractService();
            _emailService = new EmailService();
            _notificationService = new NotificationService();
            _mpSellerAccountService = new MPSellerAccountService();
            _mpService = new MPService();
        }

        public void Execute(IJobExecutionContext context)
        {
            //System.Diagnostics.Debug.WriteLine("Starting SendReminderBeforeInitialDate");
            //SendReminderBeforeInitialDate();

            System.Diagnostics.Debug.WriteLine("Starting SendReminderBeforeDueDate");
            SendReminderBeforeDueDate();

            System.Diagnostics.Debug.WriteLine("Starting SendIntimationAfterDueDate");
            SendIntimationAfterDueDate();
            
            //_notificationService.Welcome(3);
            //_notificationService.NotifyAdjudication(3);
            //_notificationService.NotifyCancelation(3);
            //_notificationService.NotifyDebtCancelation(3);
            //_notificationService.NotifyPayment(3, 28);
            
        }


        private void SendReminderBeforeInitialDate()
        {
            IEnumerable<InstallmentReminderSummary> installments = _contractService.GetInstallmentsBeforeStart(daysBeforeStart);

            Attachment attachment;

            foreach (InstallmentReminderSummary installment in installments)
            {
                attachment = null;

                if (installment.PaymentPreference == 3)
                {

                    MPSellerAccount mpSellerAccount = _mpSellerAccountService.GetNextMPSellerAccount(installment.DueDate);
                    //attachment = _mpService.CreateMPPayment(installment, mpSellerAccount);
                    
                    _mpSellerAccountService.AddMPPaymentInstallment(mpSellerAccount.MPSellerAccountId, installment.ContractId, installment.DueDate
                        , installment.OriginalAmount);

                    MemoryStream ms = new MemoryStream( ASCIIEncoding.ASCII.GetBytes("Cuenta MP: " + mpSellerAccount.MPAccountId + " -- Valor cuota: " + installment.OriginalAmount));

                    attachment = new Attachment(ms, new ContentType("text/plain"));
                    attachment.Name = "archivo.txt";
                }

                _notificationService.RemindPayment(installment, attachment);               
            }
        }

        private void SendReminderBeforeDueDate()
        {
            IEnumerable<InstallmentReminderSummary> installments = _contractService.GetInstallmentsBeforeDue(daysBeforeDue);

            foreach (InstallmentReminderSummary installment in installments)
            {
                _notificationService.RemindPayment(installment, null);
            }

        }

        private void SendIntimationAfterDueDate()
        {
            IEnumerable<InstallmentReminderSummary> installments = _contractService.GetInstallmentsAfterDue(daysAfterDue);

            foreach (InstallmentReminderSummary installment in installments)
            {
                _notificationService.IntimateForPayment(installment);
            }
        }
    }
}
