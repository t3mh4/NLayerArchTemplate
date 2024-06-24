using Microsoft.AspNetCore.Mvc;
using NLayerArchTemplate.Core.Enums;
using NLayerArchTemplate.WebUI.Configuration.Filters;

namespace NLayerArchTemplate.WebUI.Configuration.BuilderServices
{
    public static class AuthorizationFilterOptions
    {
        public static void AddAuthorizationFilter(this MvcOptions options, IConfiguration configuration)
        {
            var authorizationType = configuration.GetValue<string>("Features:AuthorizationType");
            if (authorizationType == nameof(AuthorizationType.Default))
            {
                options.Filters.Add(typeof(DefaultAuthorizationFilter));
            }
            else if (authorizationType == nameof(AuthorizationType.DefaultWithSql))
            {
                options.Filters.Add(typeof(OnlineAuthorizationFilter));
            }
        }
    }
}
