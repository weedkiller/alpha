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
    public class ContractStatus : IJob
    {
        private ContractRepository _contractRepository;

        public ContractStatus()
        {
            _contractRepository = new ContractRepository();
        }

        public void Execute(IJobExecutionContext context)
        {
            _contractRepository.ExpiredVisits();
            _contractRepository.ContractStatusControl();
            _contractRepository.ExpiredInstallment();
            _contractRepository.Control();
        }
    }
}
