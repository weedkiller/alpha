using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Multigroup.Portal.Models.Shared
{
    public class MappingTableVm
    {
        public IEnumerable<MappingItemVm> MappingTests { get; set; }
        public IEnumerable<MappingItemVm> MappingComponents { get; set; }
    }

    public class MappingItemVm
    {
        public int Id { get; set; }
        public string SourceName { get; set; }
        public string CodeArgos { get; set; }
        public string CodeExternal { get; set; }

        public static IEnumerable<MappingItemVm> ToViewModelList(IEnumerable<Mapping> entities)
        {
            return entities.Select(x => new MappingItemVm { 
                Id = x.Id,
                CodeArgos = x.CodeArgos,
                CodeExternal = x.CodeExternal,
                SourceName = x.MapperType.Description
            });
        }
    }
}