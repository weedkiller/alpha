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

namespace Multigroup.Portal.Models.Administration
{
    public class CashCycleVm : BaseVm
    {
        public int CashCycleId { get; set; }
        [Required(ErrorMessage = "El campo Número de ciclo es requerido")]
        public int CycleNumber { get; set; }
        public string Date { get; set; }
        public bool Validate { get; set; }
        public bool Closed { get; set; }
        public string Time { get; set; }
        public int CashierId { get; set; }
        public SingleSelectVm ddlUserValidate { get; set; }
        public string Cashier { get; set; }
        public string lastDate { get; set; }
        public SingleSelectVm ddlPaymentMethods{ get; set; }
        public SingleSelectVm ddlPaymentMethodPM { get; set; }
        public IEnumerable<CashCyclePaymentMethod> PaymentMethods { get; set; }


        public CashCycle ToModelObject()
        {
            return new CashCycle
            {
                CashCycleId = CashCycleId,              
                UserValidateId = GetNullableValue(ddlUserValidate).Value,
                CycleNumber = CycleNumber,
                Validate = Validate,
                CashierId = CashierId,
            };
        }

        public static CashCycleVm FromDomainModel(CashCycle entity)
        {
            return new CashCycleVm
            {
                CashCycleId = entity.CashCycleId,
                Time = entity.Time,
                Closed = entity.Closed,
                Validate = entity.Validate,
                Date = (entity.Date.HasValue) ? entity.Date.Value.ToShortDateString() : " - ",
                CycleNumber = entity.CycleNumber,
                PaymentMethods = entity.PaymentMethods,             
            };
        }
    }

    public class CashCycleTransferVm : BaseVm
    {
        public SingleSelectVm ddlOriginCashier { get; set; }
        public SingleSelectVm ddlDestinyCashier { get; set; }
        public SingleSelectVm ddlOriginPaymentMethods { get; set; }
        public SingleSelectVm ddlDestinyPaymentMethods{ get; set; }
        [Required(ErrorMessage = "El campo Monto es requerido")]
        public double Amount{ get; set; }
        public double Balance { get; set; }
        public string Commentary { get; set; }
        public int Origin { get; set; }
        public int Destiny { get; set; }

        public TransferCashier ToModelObject()
        {
            return new TransferCashier
            {
                OriginCashierId = GetNullableValue(ddlOriginCashier).Value,
                DestinyCashierId = GetNullableValue(ddlDestinyCashier).Value,
                Amount = Amount,
                Commentary = Commentary,
            };
        }

    }

}