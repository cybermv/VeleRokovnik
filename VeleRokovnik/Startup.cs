using System;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VeleRokovnik.Startup))]
namespace VeleRokovnik
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}