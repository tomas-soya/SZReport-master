using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SZReport.Models;
using SZReport.PublicCode;


namespace SZReport.Controllers
{
    public class SHAController : Controller
    {
        public IActionResult Index()
        {
            var data = DBHelper._db.SHAs.OrderByDescending(tb => tb.Date).Take(100).ToList();
            return View(data);
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
                tb.MA5,
                tb.MA10,
                tb.MA20,
                tb.MA30,
                tb.MA60,
                tb.MA90,
                tb.MA120,
                tb.MA200,
                tb.MA250
            }).OrderBy(tb => tb.Date).ToList();

            string CHName = CodeHelper.CodeDic[Code];

            ChartData result = new ChartData();
            result.xAxis = data.Select(tb => tb.Date.ToString("yyyy-MM-dd")).ToArray();
            result.legend = new string[] { CHName, "赶紧跑路", "为国护盘", "最高", "最低","MA5", "MA10", "MA20", "MA30", "MA60", "MA90", "MA120", "MA200", "MA250"};
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
                name = "MA5",
                type = "line",
                data = data.Select(tb => (object)tb.MA5).ToArray()
            });
            result.series.Add(new series
            {
                name = "MA10",
                type = "line",
                data = data.Select(tb => (object)tb.MA10).ToArray()
            });
            result.series.Add(new series
            {
                name = "MA20",
                type = "line",
                data = data.Select(tb => (object)tb.MA20).ToArray()
            });
            result.series.Add(new series
            {
                name = "MA30",
                type = "line",
                data = data.Select(tb => (object)tb.MA30).ToArray()
            });
            result.series.Add(new series
            {
                name = "MA60",
                type = "line",
                data = data.Select(tb => (object)tb.MA60).ToArray()
            });
            result.series.Add(new series
            {
                name = "MA90",
                type = "line",
                data = data.Select(tb => (object)tb.MA90).ToArray()
            });
            result.series.Add(new series
            {
                name = "MA120",
                type = "line",
                data = data.Select(tb => (object)tb.MA120).ToArray()
            });

            result.series.Add(new series
            {
                name = "MA200",
                type = "line",
                data = data.Select(tb => (object)tb.MA200).ToArray()
            });
            result.series.Add(new series
            {
                name = "MA250",
                type = "line",
                data = data.Select(tb => (object)tb.MA250).ToArray()
            });

            if (Code=="000001")
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
                //if (data.Count > 0)
                //    ViewBag.yStart = (int)(data.Min(tb => tb.ReferDown) * 100 / Math.Max(data.Max(tb => tb.Closing), data.Max(tb => tb.ReferUp)));
            }
                if (data.Count > 0)
                    ViewBag.yStart = (int)(data.Min(tb => tb.Lowest) * 100 / data.Max(tb => tb.Highest));

            return View(result);
        }

        public IActionResult AnalysisALL(DateTime? date)
        {
            List<string> Message = new List<string>();
            foreach(var a in CodeHelper.CodeDic.Keys)
            {
                Message.Add(DataHelper.Analysis(date, a));
            }

            ViewBag.Message = Message;
            return View();
        }
        public IActionResult Analysis(DateTime? date, string Code = "000001")
        {

            ViewBag.Message = DataHelper.Analysis(date, Code);
            return View();
        }

        public IActionResult MonthData(int start,int end,int startyear,int endyear)
        {
            string Code = "000001";
            if (start < 1)
                start = 1;
            if (end > 12)
                end = 12;
            var data = DBHelper._db.SHAs
                .Where(tb => tb.Code == CodeHelper.CodeToDBCode(Code))
                .Where(tb => tb.Date.Month >= start && tb.Date.Month <= end && tb.Date.Year >= startyear && tb.Date.Year <= endyear).Select(tb => new
            {
                tb.Date,
                tb.Closing,
                tb.ReferDown,
                tb.ReferUp
            }).OrderBy(tb => tb.Date).ToList();

            ChartData result = new ChartData();
            result.xAxis = data.Select(tb => tb.Date.ToString("yyyy-MM-dd")).ToArray();
            result.legend = new string[] { "上证指数", "准备跑路", "为国护盘" };
            result.series.Add(new series
            {
                name = "上证指数",
                type = "line",
                data = data.Select(tb => (object)tb.Closing).ToArray()
            });
            result.series.Add(new series
            {
                name = "准备跑路",
                type = "line",
                data = data.Select(tb => (object)tb.ReferUp).ToArray()
            });
            result.series.Add(new series
            {
                name = "为国护盘",
                type = "line",
                data = data.Select(tb => (object)tb.ReferDown).ToArray()
            });
            if (data.Count > 0)
                ViewBag.yStart = (int)(data.Min(tb => tb.ReferDown) * 100 / Math.Max(data.Max(tb => tb.Closing), data.Max(tb => tb.ReferUp)));
            List<pieces> pieces = new List<pieces>();
            var piedata = data.GroupBy(tb => tb.Date.Year).OrderBy(tb => tb.Key).ToList();
            int total = 0;
            for (int i = 0; i < piedata.Count - 1; i++)
            {
                total += piedata[i].ToList().Count;
                pieces.Add(new Models.pieces { gt = total-1, lt = total  });
            }
            ViewBag.pieces = pieces;
            return View(result);
        }

        public IActionResult MonthGroup()
        {
            return View();
        }

        public IActionResult CodeGroup()
        {
            return View();
        }


    }
}