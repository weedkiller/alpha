using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Enums;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.ContractModel
{
    public class InterestTableVm
    {
        public IEnumerable<InterestListVm> InterestList { get; set; }

        public static InterestTableVm ToViewModel(IEnumerable<InstallmentInsterestsSummary> entities)
        {
            return new InterestTableVm
            {
                InterestList = InterestListVm.ToViewModelList(entities),
            };
        }
    }

    public class InterestListVm
    {
        public int InstallmentInterestsId { get; set; }
        public int ContractId { get; set; }
        public int InstallmentId { get; set; }
        public int PaymentId { get; set; }
        public double Amount { get; set; }
        public DateTime InitialDate { get; set; }
        public int Number { get; set; }

        public static IEnumerable<InterestListVm> ToViewModelList(IEnumerable<InstallmentInsterestsSummary> entities)
        {
            return entities.Select(x => new InterestListVm
            {
                InstallmentInterestsId = x.InstallmentInterestsId,
                ContractId = x.ContractId,
                InstallmentId = x.InstallmentId,
                PaymentId = x.PaymentId,
                Amount = x.Amount,
                InitialDate = x.InitialDate,
                Number = x.Number
            });
        }
    }
}