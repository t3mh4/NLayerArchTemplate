﻿using Microsoft.AspNetCore.Mvc;

namespace NLayerArchTemplate.WebUI.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.PageHeader = "Anasayfa";
            await Task.CompletedTask;
            return View();
        }
    }
}