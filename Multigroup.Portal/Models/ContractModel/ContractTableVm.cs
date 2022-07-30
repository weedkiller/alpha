using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Enums;
using Multigroup.Portal.Models.Shared;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.ContractModel
{
    public class ContractFilterVm
    {
        public IEnumerable<string> SelectedStatus { get; set; }
        public IEnumerable<SelectListItem> ListStatus { get; set; }
        public IEnumerable<string> SelectedStatusDetail { get; set; }
        public IEnumerable<SelectListItem> ListStatusDetail { get; set; }
        public IEnumerable<string> SelectedClient { get; set; }
        public IEnumerable<SelectListItem> ListClient { get; set; }

        public IEnumerable<string> SelectedAgency { get; set; }
        public IEnumerable<SelectListItem> ListAgency { get; set; }
        public IEnumerable<string> SelectedContractType { get; set; }
        public IEnumerable<SelectListItem> ListContractType { get; set; }
        public SingleSelectVm ddlGerente { get; set; }
        public SingleSelectVm ddlSupervisor { get; set; }
    }
    public class ContractTableVm
    {
        public IEnumerable<ContractListVm> ContractList { get; set; }

        public static ContractTableVm ToViewModel(IEnumerable<ContractSummary> entities)
        {
            return new ContractTableVm
            {
                ContractList = ContractListVm.ToViewModelList(entities),
            };
        }
    }

    public class ContractListVm
    {
        public int ContractId { get; set; }
        public int Number { get; set; }
        public string RegistrationDate { get; set; }
        public string Agency { get; set; }
        public string Customer { get; set; }
        public string Zone { get; set; }
        public string PaperStatus { get; set; }
        public string Status { get; set; }
        public string Color { get; set; }
        public string ContractType { get; set; }
        public string DNI { get; set; }
        public string User { get; set; }
        public string AdmissionDate { get; set; }
        public string InitDate { get; set; }
        public double FirstInstallmentAmount { get; set; }
        public double InstallmentAmount { get; set; }
        public string ColorLarge { get; set; }

        public static IEnumerable<ContractListVm> ToViewModelList(IEnumerable<ContractSummary> entities)
        {
            return entities.Select(x => new ContractListVm
            {
                ContractId = x.ContractId,
                Number = x.Number,
                RegistrationDate = Util.GetFrenchFormatDate(x.RegistrationDate),
                Agency = x.Agency,
                Customer = x.Customer,
                Zone = x.Zone,
                PaperStatus = x.PaperStatus,
                Status = x.Status,
                Color = x.Color,
                DNI = x.DNI,
                ContractType = x.ContractType,
                User = x.User,
                AdmissionDate = Util.GetFrenchFormatDate(x.AdmissionDate),
                InitDate = Util.GetFrenchFormatDate(x.InitDate),
                FirstInstallmentAmount = x.FirstInstallmentAmount,
                InstallmentAmount = x.InstallmentAmount,
                ColorLarge = (x.Color == null) ? "" : x.Color.Replace("green", "Mes Pago").Replace("black", "Debe Mes Actual").Replace("red", "Debe Más de 2 meses").Replace("#ecca14", "Debe Hasta 2 meses"),
            });
        }
    }

    public class ContractTableCuotasVm
    {
        public IEnumerable<ContractTableCuotasListVm> ContractTableCuotasList { get; set; }

        public static ContractTableCuotasVm ToViewModel(IEnumerable<ContractCuotas> entities)
        {
            return new ContractTableCuotasVm
            {
                ContractTableCuotasList = ContractTableCuotasListVm.ToViewModelList(entities),
            };
        }
    }

    public class ContractTableCuotasListVm
    {
        public int Number { get; set; }
        public string PaymentDate { get; set; }
        public double Amount { get; set; }
        public string User { get; set; }
        public double Anio { get; set; }
        public double Mes { get; set; }

        public static IEnumerable<ContractTableCuotasListVm> ToViewModelList(IEnumerable<ContractCuotas> entities)
        {
            return entities.Select(x => new ContractTableCuotasListVm
            {
                Number = x.Number,
                Amount = x.Amount,
                PaymentDate = Util.GetFrenchFormatDate(x.PaymentDate),
                User = x.User,
                Anio = x.Anio,
                Mes = x.Mes,
            });
        }
    }
}