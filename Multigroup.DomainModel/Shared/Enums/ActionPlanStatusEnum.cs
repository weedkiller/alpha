using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared.Enums
{
    public enum ActionPlanStatusEnum
    {
        [Description("Cerrada")]
        ECLS,

        [Description("Pendiente de ejecucion")]
        EEXECP,

        [Description("Ejecutada")]
        EEXEC,

        [Description("Abierta")]
        EOPN,

        [Description("Creada")]
        CRT
    }
}
