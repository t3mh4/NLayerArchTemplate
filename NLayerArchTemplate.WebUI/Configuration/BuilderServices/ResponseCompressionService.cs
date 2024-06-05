using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace NLayerArchTemplate.WebUI.BuilderServices;
public static class ResponseCompressionService
{
    public static IServiceCollection AddResponseCompressionService(this IServiceCollection service)
    {
        service.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
            //options.Providers.Add<BrotliCompressionProvider>();
        });

        service.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        //service.Configure<GzipCompressionProviderOptions>(options =>
        //{
        //    options.Level = CompressionLevel.Fastest;
        //});

        return service;
    }
}
