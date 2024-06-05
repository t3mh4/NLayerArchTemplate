using Microsoft.AspNetCore.Mvc;
using NLayerArchTemplate.Core.Extensions;
using System.Net;

namespace NLayerArchTemplate.WebUI.Configuration.ActionResults;

public class JsonActionResult : JsonResult
{
    private readonly int _statusCode;

    public JsonActionResult(object json) : base(json)
    {
        _statusCode = HttpStatusCode.OK.ToInt32();
    }

    public JsonActionResult(object json, int statusCode) : base(json)
    {
        _statusCode = statusCode;
    }

    public override Task ExecuteResultAsync(ActionContext context)
    {
        context.HttpContext.Response.StatusCode = _statusCode;
        return base.ExecuteResultAsync(context);
    }
}
