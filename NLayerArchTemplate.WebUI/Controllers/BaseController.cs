using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace NLayerArchTemplate.WebUI.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            
        }

        public T GetManager<T>() where T : class
        {
            return HttpContext.RequestServices.GetRequiredService<T>();
        }
    }
}
