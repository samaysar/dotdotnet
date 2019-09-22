using System.Web.Http;
using Owin;

namespace StreamingServer
{
    public class Bootstrapper
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}