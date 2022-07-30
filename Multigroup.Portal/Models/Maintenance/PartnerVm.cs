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

namespace Multigroup.Portal.Models.Maintenance
{
    public class PartnerVm : BaseVm
    {
        public int PartnerId { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Name { get; set; }
        public bool Active { get; set; }
        public double Percentage { get; set; }


        public Partner ToModelObject()
        {
            return new Partner
            {
                PartnerId = PartnerId,
                Name = Name,
                Active = Active,
                Percentage = Percentage,
            };
        }

        public static PartnerVm FromDomainModel(Partner entity)
        {
            return new PartnerVm
            {
                PartnerId = entity.PartnerId,
                Name = entity.Name,
                Active = entity.Active,
                Percentage = entity.Percentage,
            };
        }

    }

    public class PercentageDefinitionVm : BaseVm
    {
        public int PercentageDefinitionId { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Name { get; set; }
        public SingleSelectVm ddlPartners { get; set; }
        public IEnumerable<PercentageDefinitionPartner> Partners { get; set; }
        public double PercentagePartner { get; set; }


        public PercentageDefinition ToModelObject()
        {
            return new PercentageDefinition
            {
                PercentageDefinitionId = PercentageDefinitionId,
                Name = Name,
                Partners = Partners,
            };
        }

        public static PercentageDefinitionVm FromDomainModel(PercentageDefinition entity)
        {
            return new PercentageDefinitionVm
            {
                PercentageDefinitionId = entity.PercentageDefinitionId,
                Name = entity.Name,
                Partners = entity.Partners,
            };
        }

    }

    public class BalancePartnerFilterVm
    {
        public IEnumerable<string> SelectedPartner { get; set; }
        public IEnumerable<SelectListItem> ListPartner { get; set; }
        public bool IsAssigned { get; set; }
    }

    public class BalancePartnerTableVm
    {
        public IEnumerable<BalancePartnerListVm> BalancePartnerList { get; set; }

        public static BalancePartnerTableVm ToViewModel(IEnumerable<BalancePartner> entities)
        {
            return new BalancePartnerTableVm
            {
                BalancePartnerList = BalancePartnerListVm.ToViewModelList(entities),
            };
        }
    }

    public class BalancePartnerListVm : BaseVm
    {
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string Date { get; set; }


        public static IEnumerable<BalancePartnerListVm> ToViewModelList(IEnumerable<BalancePartner> entities)
        {
            return entities.Select(x => new BalancePartnerListVm
            {
                TransactionType = x.TransactionType,
                Date = x.Date.ToShortDateString(),
                Description = x.Description,
                Amount = x.Amount,
            });
        }

    }
}