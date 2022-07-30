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
    public class MercadoPagoVm : BaseVm
    {
        public int MPSellerAccountId { get; set; }
        public string MPAccountId { get; set; }
        public int UserName { get; set; }
        public int UserId { get; set; }
        public string Pass { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public string Token { get; set; }


        public MPSellerAccount ToModelObject()
        {
            return new MPSellerAccount
            {
                MPSellerAccountId = MPSellerAccountId,
                MPAccountId = MPAccountId,
                UserId = UserId,
                Pass = Pass,
                MinAmount = MinAmount,
                MaxAmount = MaxAmount,
                Token = Token,
            };
        }

        public static MercadoPagoVm FromDomainModel(MPSellerAccount entity)
        {
            return new MercadoPagoVm
            {
                MPSellerAccountId = entity.MPSellerAccountId,
                MPAccountId = entity.MPAccountId,
                UserId = entity.UserId,
                Pass = entity.Pass,
                MinAmount = entity.MinAmount,
                MaxAmount = entity.MaxAmount,
                Token = entity.Token,
            };
        }
    }
}