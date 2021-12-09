using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Cadastro.Areas.Identity.IdentityHostingStartup))]
namespace Cadastro.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}