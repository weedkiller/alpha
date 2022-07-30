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

namespace Multigroup.Portal.Models.Collection
{
    public class CollectorAssigmmentVm : BaseVm
    {
        public int AssignPaymentPreferenceId { get; set; }
        [Required(ErrorMessage = "El campo  Fecha de Visita es requerido")]
        public string VisitDate { get; set; }
        [Required(ErrorMessage = "El campo Cobrador es requerido")]
        public SingleSelectVm ddlCollector{ get; set; }
        public IEnumerable<CollectorAssigmmentSummary> Asignaciones { get; set; }
    }

    public class CollectorSurrenderVm : BaseVm
    {
        [Required(ErrorMessage = "El campo  Fecha de Visita es requerido")]
        public string SurrenderDate { get; set; }
        [Required(ErrorMessage = "El campo Monto es requerido")]
        public double Amount { get; set; }
        public double AmountReceipt { get; set; }
        public IEnumerable<AssignPaymentPreferenceCollector> Asignaciones { get; set; }
        public IEnumerable<CollectorSurrenderPM> PaymentMethods { get; set; }
        public IEnumerable<CollectorSurrenderReceipt> Recibos { get; set; }
        public SingleSelectVm ddlPaymentMethods { get; set; }
        public SingleSelectVm ddlPaymentMethodsPM { get; set; }
        public double AmountPayment { get; set; }
        public double AmountByPaymentMethod { get; set; }
        public double AmountByInstallment { get; set; }
        public double ReceipNumber { get; set; }
        public double ReceipAmount { get; set; }
        public string Commentary { get; set; }
    }
}