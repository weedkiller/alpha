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
    public class AdjudicationControl : IJob
    {
        private ContractRepository _contractRepository;

        public AdjudicationControl()
        {
            _contractRepository = new ContractRepository();
        }

        public void Execute(IJobExecutionContext context)
        {
            _contractRepository.AdjudicationControl();
        }
    }
}
