using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SZReport.PublicCode;

namespace SZReport.Controllers
{
    public class UpdateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult UpdateALL()
        {
            try
            {
                foreach (var a in CodeHelper.CodeDic)
                {
                    DataUpdater.UpdateData(a.Key);
                }
                ViewBag.result = "success";
            }
            catch(Exception e)
            {
                ViewBag.result = e.Message;
            }
            return View();
        }
    }
}