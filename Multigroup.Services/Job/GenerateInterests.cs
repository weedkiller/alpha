using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using Multigroup.Services.Shared;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Services.Job
{
    public class GenerateInterests : IJob
    {
        private ContractRepository _contractRepository;
        private readonly int daysBeforeStart = 2; 

        public GenerateInterests()
        {
            _contractRepository = new ContractRepository();
        }

        public void Execute(IJobExecutionContext context)
        {

            IEnumerable<InstallmentReminderSummary> installments = _contractRepository.GetInstallmentsBeforeStart(daysBeforeStart);

            foreach (InstallmentReminderSummary installment in installments)
            {
                _contractRepository.GenerateInterests(installment.ContractId, installment.InstallmentId);
            }
        }
    }
}
