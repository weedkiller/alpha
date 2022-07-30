using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Shared
{
    public class MappingVm : BaseVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        public string CodeArgos { get; set; }
        
        [Required(ErrorMessage = "El campo es requerido")]
        public string CodeExternal { get; set; }
        
        [Required(ErrorMessage = "El campo es requerido")]
        public SingleSelectVm ddlMapperTypes { get; set; }

        public static MappingVm ToViewModel(Mapping entity)
        {
            return new MappingVm { 
                Id = entity.Id,
                CodeArgos = entity.CodeArgos,
                CodeExternal = entity.CodeExternal
            };
        }

        public Mapping ToModelObject()
        {
            return new Mapping { 
                Id = Id,
                CodeArgos = CodeArgos,
                CodeExternal = CodeExternal,
                MapperTypeId = GetNullableValue(ddlMapperTypes).Value
            };
        }
    }
}