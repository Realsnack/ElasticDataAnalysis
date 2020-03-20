using Microsoft.Extensions.Configuration;

namespace BackEnd.Models
{
    public class EndpointVersion : ElasticDTO.EndpointVersion
    {
        public EndpointVersion(IConfiguration configuration)
        {
            AppName = configuration.GetValue<string>("AppTitle");
            AppVersion = configuration.GetValue<string>("AppVersion");
        }
    }
}