using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_SupEvent.Startup))]
namespace _SupEvent
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
