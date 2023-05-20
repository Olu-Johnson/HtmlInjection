using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting.Server;
using System.Reflection;
using Shared;
using htmlinjection.Models;

namespace htmlinjection.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var model = new UserModel();
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public ActionResult Submit(UserModel model)
    {
        PropertyInfo[] properties = typeof(UserModel).GetProperties();
        foreach (PropertyInfo property in properties)
        {
            if (property.PropertyType == typeof(string))
            {
                string value = (string)property.GetValue(model);
                string encodedValue = System.Web.HttpUtility.HtmlEncode(value);
                property.SetValue(model, encodedValue);
            }
            else if(property.PropertyType == typeof(int))
            {
                int value = (int)property.GetValue(model);
                int encodedValue = int.Parse(System.Web.HttpUtility.HtmlEncode(value));
                property.SetValue(model, encodedValue);
            }
        }

        return View("Index", model);
    }


}

