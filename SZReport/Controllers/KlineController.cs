using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SZReport.Models;
using SZReport.PublicCode;

namespace SZReport.Controllers
{
    public class KlineController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult History(string Code = "000001")
        {
            ViewBag.Code = Code;
            return View();
        }

        public IActionResult History_Data(int startMonth = -1000, string Code = "000001")
        {
            var startDay = DateTime.Now.AddDays(30 * startMonth);
            var data = DBHelper._db.SHAs.Where(tb => tb.Code == CodeHelper.CodeToDBCode(Code)).Where(tb => tb.Date >= startDay).Select(tb => new
            {
                tb.Date,
                tb.Closing,
                tb.ReferDown,
                tb.ReferUp,
                tb.Highest,
                tb.Lowest,
                Turnover = (int)(tb.Turnover / 100000000)
            }).OrderBy(tb => tb.Date).ToList();

            string CHName = CodeHelper.CodeDic[Code];

            ChartData result = new ChartData();
            result.xAxis = data.Select(tb => tb.Date.ToString("yyyy-MM-dd")).ToArray();
            result.legend = new string[] { CHName, "赶紧跑路", "为国护盘", "最高", "最低", "成交" };
            result.series.Add(new series
            {
                name = CHName,
                type = "line",
                data = data.Select(tb => (object)tb.Closing).ToArray()
            });

            result.series.Add(new series
            {
                name = "最高",
                type = "line",
                data = data.Select(tb => (object)tb.Highest).ToArray()
            });
            result.series.Add(new series
            {
                name = "最低",
                type = "line",
                data = data.Select(tb => (object)tb.Lowest).ToArray()
            });
            result.series.Add(new series
            {
                name = "成交",
                type = "line",
                data = data.Select(tb => (object)tb.Turnover).ToArray()
            });

            if (Code == "000001")
            {
                result.series.Add(new series
                {
                    name = "赶紧跑路",
                    type = "line",
                    data = data.Select(tb => (object)tb.ReferUp).ToArray()
                });
                result.series.Add(new series
                {
                    name = "为国护盘",
                    type = "line",
                    data = data.Select(tb => (object)tb.ReferDown).ToArray()
                });
            }
            if (data.Count > 0)
                ViewBag.yStart = (int)(data.Min(tb => tb.Lowest) * 100 / data.Max(tb => tb.Highest));

            return View(result);
        }
    }
}