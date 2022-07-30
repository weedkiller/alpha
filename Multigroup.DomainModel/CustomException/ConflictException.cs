using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.CustomException
{
    public class ConflictException : BusinessException
    {
        public ConflictException(string message)
            : base(message)
        {}
    }
}
