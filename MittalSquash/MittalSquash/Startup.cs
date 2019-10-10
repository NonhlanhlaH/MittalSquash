using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MittalSquash.Startup))]
namespace MittalSquash
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
