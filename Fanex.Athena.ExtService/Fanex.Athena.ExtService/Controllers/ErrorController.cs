using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.Common.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index(int code)
        {
            var viewModel = new BaseViewModel
            {
                AccessToken = "Not token",
                BaseCurrency = string.Empty,
                Lang = string.Empty
            };
            ViewBag.Code = code;
            return View(viewModel);
        }
    }
}