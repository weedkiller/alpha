using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Models.Shared;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Collection
{

    public class CollectorAdvancementVm : BaseVm
    {
        public int CollectorId { get; set; }
        [Required(ErrorMessage = "El campo Fecha requerido")]
        public string Date { get; set; }
        public string Hour { get; set; }
        public string Commentary { get; set; }
        [Required(ErrorMessage = "El campo Monto requerido")]
        public double Amount { get; set; }
        public IEnumerable<CollectorSurrender> rendiciones { get; set; }
        public IEnumerable<CollectorAdvancement> avances { get; set; }

        public IEnumerable<CollectorSurrenderPM> PaymentMethods { get; set; }
        public SingleSelectVm ddlPaymentMethods { get; set; }
        public SingleSelectVm ddlPaymentMethodsPM { get; set; }
        public double AmountPayment { get; set; }
        public double AmountByPaymentMethod { get; set; }
        public CollectorAdvancement ToModelObject()
        {
            return new CollectorAdvancement
            {
                CollectorId = CollectorId,
                Date = Util.ParseDateTime(Date).Value,
                Balance = Amount,
                Amount = Amount,
                Commentary = Commentary,
                Hour = Hour
            };
        }
    }


    public class CollectorStateTableVm
    {
        public IEnumerable<CollectorStateListVm> CollectorStateList { get; set; }
        public static CollectorStateTableVm ToViewModel(IEnumerable<CollectorState> entities)
        {
            return new CollectorStateTableVm
            {
                CollectorStateList = CollectorStateListVm.ToViewModelList(entities),
            };
        }
    }

    public class CollectorStateListVm
    {
        public int CollectorId { get; set; }
        public string Collector { get; set; }
        public double PositiveBalance { get; set; }
        public double NegativeBalance { get; set; }
        public double Balance { get; set; }
        
        public static IEnumerable<CollectorStateListVm> ToViewModelList(IEnumerable<CollectorState> entities)
        {
            return entities.Select(x => new CollectorStateListVm
            {
                CollectorId = x.CollectorId,
                PositiveBalance = x.PositiveBalance,
                NegativeBalance = x.NegativeBalance,
                Collector = x.Collector,
                Balance = x.Balance,
            });
        }
    }
}