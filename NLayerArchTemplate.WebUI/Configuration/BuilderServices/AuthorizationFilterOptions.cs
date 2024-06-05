using Microsoft.AspNetCore.Mvc;
using NLayerArchTemplate.Core.Enums;
using NLayerArchTemplate.WebUI.Configuration.Filters;

namespace NLayerArchTemplate.WebUI.Configuration.BuilderServices
{
    public static class AuthorizationFilterOptions
    {
        public static void AddAuthorizationFilter(this MvcOptions options, IConfiguration configuration)
        {
            if (configuration.GetValue<string>("Features:AuthorizationType") == AuthorizationType.Default.ToString())
            {
                options.Filters.Add(typeof(DefaultAuthorizationFilter));
            }
            else if (configuration.GetValue<string>("Features:AuthorizationType") == AuthorizationType.DefaultWithSql.ToString())
            {
                options.Filters.Add(typeof(OnlineAuthorizationFilter));
            }
        }
    }
}
