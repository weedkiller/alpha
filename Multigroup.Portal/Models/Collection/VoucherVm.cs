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
    public class VoucherVm : BaseVm
    {
        public int AssignPaymentPreferenceId { get; set; }
        [Required(ErrorMessage = "Debe Ingresar Monto")]
        public float Amount { get; set; }
        [Required(ErrorMessage = "Debe ingresar Número de Comprobante")]
        public string Voucher { get; set; }
        [Required(ErrorMessage = "Debe Ingresar Fecha de Pago")]
        public string PaymentDate { get; set; }
        public string ContractNumber { get; set; }
        public string Name { get; set; }
        public string Commentary { get; set; }
        [Required(ErrorMessage = "Debe seleccionar Medio de Pago")]
        public SingleSelectVm ddlPaymentMethod { get; set; }
        public IEnumerable<AssignPaymentPreferenceWithVoucher> installments { get; set; }

        public PaymentVoucher ToModelObject()
        {
            return new PaymentVoucher
            {
                VoucherDate = Util.ParseDateTime(PaymentDate),
                Amount = Amount,
                Code = Voucher,
                PaymentMethodId = GetNullableValue(ddlPaymentMethod).Value,
            };
        }

        public static VoucherVm FromDomainModel(PaymentVoucher entity)
        {
            return new VoucherVm
            {
                PaymentDate = Util.GetFrenchFormatDate(entity.VoucherDate),
                Amount = entity.Amount,
                Voucher = entity.Code,
            };
        }
    }
}