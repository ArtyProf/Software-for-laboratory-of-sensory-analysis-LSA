using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(LSA.Areas.Identity.IdentityHostingStartup))]
namespace LSA.Areas.Identity
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