using Multigroup.DomainModel.Menu;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Multigroup.Repositories.Menu
{
    public class MenuMapper
    {
        public static IRowMapper<MenuComponent> Mapper
        {
            get
            {
                var mapper = MapBuilder<MenuComponent>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class MenuComponentPageMapper
    {
        public static IRowMapper<MenuComponentPage> Mapper
        {
            get
            {
                var mapper = MapBuilder<MenuComponentPage>.MapAllProperties()
                    .DoNotMap(x => x.MenuComponent)
                    .Build();
                return mapper;
            }
        }
    }
}
