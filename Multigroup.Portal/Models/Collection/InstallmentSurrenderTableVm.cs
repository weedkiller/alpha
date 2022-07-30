using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Multigroup.Portal.Utilities;

namespace Multigroup.Portal.Models.Collection
{

    public class InstallmentSurrenderTableVm
    {
        public IEnumerable<InstallmentSurrenderListVm> SurrenderList { get; set; }

        public static InstallmentSurrenderTableVm ToViewModel(IEnumerable<InstallmentSurrenderSummary> entities)
        {
            return new InstallmentSurrenderTableVm
            {
                SurrenderList = InstallmentSurrenderListVm.ToViewModelList(entities),
            };
        }
    }

    public class InstallmentSurrenderListVm
    {

        public int InstallmentSurrenderId { get; set; }
        public int CollectorSurrenderId { get; set; }
        public double Amount { get; set; }
        public int Cuota { get; set; }
        public int Contrato { get; set; }
        public string Cliente { get; set; }

        public static IEnumerable<InstallmentSurrenderListVm> ToViewModelList(IEnumerable<InstallmentSurrenderSummary> entities)
        {
            return entities.Select(x => new InstallmentSurrenderListVm
            {
                InstallmentSurrenderId = x.InstallmentSurrenderId,
                CollectorSurrenderId = x.CollectorSurrenderId,
                Cuota = x.Cuota,
                Contrato = x.Contrato,
                Amount = x.Amount,
                Cliente = x.Cliente,
            });
        }
    }
}