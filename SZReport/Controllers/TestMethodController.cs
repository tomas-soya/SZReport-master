using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SZReport.PublicCode;

namespace SZReport.Controllers
{
    public class TestMethodController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Six()
        {
            int[] lines = { 5, 10, 20, 30, 50, 60, 90, 180, 360 };
            //均线合并买入测试
            int topline = 20;
            int bottomline = 250;

            List<string> result = new List<string>();
            var date = DateTime.Now.AddDays(30 * 36 * -1);
            var data = DBHelper._db.SHAs.Where(tb => tb.Code == CodeHelper.CodeToDBCode("000001")).Select(tb => new
            {
                tb.Date,
                tb.Closing,
                tb.Lowest,
                tb.Highest
            }).ToList();
            while (date <= DateTime.Now)
            {
                var thisdata = data.Where(tb => tb.Date.Year == date.Year && tb.Date.Month == date.Month && tb.Date.Day == date.Day).ToList();
                date = date.AddDays(1);
                if (thisdata.Count == 0)
                    continue;

                var topdata= data.Where(tb => tb.Date < date).OrderByDescending(tb => tb.Date).Take(topline).ToList();
                var top = topdata.Sum(tb => tb.Closing) / topline;

                var bottomdata = data.Where(tb => tb.Date < date).OrderByDescending(tb => tb.Date).Take(bottomline).ToList();
                var bottom = bottomdata.Sum(tb => tb.Closing) / bottomline;
                bool canbuy = true;
                if (thisdata[0].Closing >= bottom || thisdata[0].Closing >= top)
                {
                    canbuy = false;
                }

                if (canbuy)
                    result.Add(thisdata[0].Date.ToString("yyyy-MM-dd"));
            }

            

            return View(result);
        }


        public IActionResult Sell()
        {
            int[] lines = { 5, 10, 20, 30, 50, 60, 90, 180, 360 };
            //卖出测试
            int topline = 5;
            int bottomline = 250;

            List<string> result = new List<string>();
            var date = DateTime.Now.AddDays(30 * 36 * -1);
            var data = DBHelper._db.SHAs.Where(tb => tb.Code == CodeHelper.CodeToDBCode("000001")).Select(tb => new
            {
                tb.Date,
                tb.Closing,
                tb.Lowest,
                tb.Highest
            }).ToList();
            while (date <= DateTime.Now)
            {
                var thisdata = data.Where(tb => tb.Date.Year == date.Year && tb.Date.Month == date.Month && tb.Date.Day == date.Day).ToList();
                date = date.AddDays(1);
                if (thisdata.Count == 0)
                    continue;

                var topdata = data.Where(tb => tb.Date < date).OrderByDescending(tb => tb.Date).Take(topline).ToList();
                var top = topdata.Sum(tb => tb.Closing) / topline;

                //var bottomdata = data.Where(tb => tb.Date < date).OrderByDescending(tb => tb.Date).Take(bottomline).ToList();
                //var bottom = bottomdata.Sum(tb => tb.Lowest) / bottomline;
                bool canbuy = true;
                if (thisdata[0].Closing > top)
                {
                    canbuy = false;
                }

                if (canbuy)
                    result.Add(thisdata[0].Date.ToString("yyyy-MM-dd"));
            }



            return View("Six",result);
        }

        public IActionResult All()
        {
            int[] lines = { 5, 7, 10, 20, 30, 50, 60, 90, 100, 150, 180, 250, 360 };
            var mindate = new DateTime(2019, 8, 1);
            var maxdate = new DateTime(2019, 8, 30);

            List<List<string>> Result = new List<List<string>>();

            var data = DBHelper._db.SHAs.Where(tb => tb.Code == CodeHelper.CodeToDBCode("000001")).Select(tb => new
            {
                tb.Date,
                tb.Closing
            }).ToList();

            while (mindate < maxdate)
            {
                List<string> res = new List<string>();
                var thisdata = data.Where(tb => tb.Date == mindate).ToList();
                
                if (thisdata.Count==0)
                {
                    mindate = mindate.AddDays(1);
                    continue;
                }
                    
                res.Add(mindate.ToShortDateString()  + " ：" + thisdata[0].Closing);

                for (int j = 0; j <lines.Length; j ++)
                {
                    string s = "";
                    var before = data.Where(tb => tb.Date < mindate).OrderByDescending(tb => tb.Date).Take(lines[j]).ToList();
                    var avg = before.Sum(tb => tb.Closing) / before.Count;
                    if (thisdata[0].Closing < avg)
                    {
                        s = "击破 " + lines[j].ToString() + "日均线：" + avg;
                        res.Add(s);
                    }

                }
                mindate = mindate.AddDays(1);
                Result.Add(res);
            }

            return View(Result);
        }
        public IActionResult BuyLineTest()
        {


            var data = DBHelper._db.SHAs.Where(tb => tb.Code == CodeHelper.CodeToDBCode("000001")).Select(tb => new
            {
                tb.Date,
                tb.Closing
            }).ToList();

            //int[] lines = { 5, 10, 20, 30, 60, 180, 365 };

            List<DateTime> dateTimes = new List<DateTime>();
            dateTimes.Add(new DateTime(2018, 4, 11));
            //dateTimes.Add(new DateTime(2017, 4, 24));
            //dateTimes.Add(new DateTime(2018, 5, 30));
            //dateTimes.Add(new DateTime(2018, 12, 10));
            //dateTimes.Add(new DateTime(2019, 7, 22));

            List<List<string>> Result = new List<List<string>>();
            for (int i = 0; i < dateTimes.Count; i++)
            {
                List<string> res = new List<string>();

                var thisdata = data.Where(tb => tb.Date.Year == dateTimes[i].Year && tb.Date.Month == dateTimes[i].Month && tb.Date.Day == dateTimes[i].Day).ToList();
                for (int j = 5; j <= 360; j += 5)
                {
                    string s = dateTimes[i].ToShortDateString() + ", " + j.ToString() + " 日均线：";
                    var before = data.Where(tb => tb.Date < dateTimes[i]).OrderByDescending(tb => tb.Date).Take(j).ToList();
                    var avg = before.Sum(tb => tb.Closing) / before.Count;
                    if (thisdata[0].Closing <= avg)
                    {
                        s += "无效";
                    }
                    else
                    {
                        s += "有效";
                        res.Add(s);
                    }

                }
                Result.Add(res);
            }

            return View(Result);
        }

    }
}