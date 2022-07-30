using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Enums;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Multigroup.Portal.Models.Shared;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.ContractModel
{
    public class ContractStatusVm : BaseVm
    {
        public string Customer { get; set; }
        public string ContractNumber { get; set; }
        public string Agency { get; set; }
        public string GoodRequested { get; set; }
        public int ContractTerm { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int TotalInstallments { get; set; }
        public int PaidInstallments { get; set; }
        public int InstallmentsToExpire { get; set; }
        public int PartialInstallments { get; set; }
        public int OwedInstallments { get; set; }
        public double Advance { get; set; }
        public double Bonification { get; set; }
        public double Monto { get; set; }

        public static ContractStatusVm FromDomainModel(ContractResume entity)
        {
            return new ContractStatusVm
            {
                Customer = entity.Customer,
                ContractNumber = entity.ContractNumber,
                Agency = entity.Agency,
                GoodRequested = entity.GoodRequested,
                 ContractTerm = entity.ContractTerm,
                 StartDate = Util.GetFrenchFormatDate(entity.StartDate),
                 EndDate = Util.GetFrenchFormatDate(entity.EndDate),
                 TotalInstallments = entity.TotalInstallments,
                 PaidInstallments = entity.PaidInstallments,
                 InstallmentsToExpire = entity.InstallmentsToExpire,
                 PartialInstallments = entity.PartialInstallments,
                 OwedInstallments = entity.OwedInstallments,
                 Advance = entity.Advance,
                 Bonification = entity.Bonification,
                Monto = entity.Monto
            };
        }
    }
}