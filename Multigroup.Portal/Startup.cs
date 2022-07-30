using Microsoft.Owin;
using Multigroup.Services.Job;
using Owin;

[assembly: OwinStartupAttribute(typeof(Multigroup.Portal.Startup))]
namespace Multigroup.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            JobScheduler.Start();
        }
    }
}
