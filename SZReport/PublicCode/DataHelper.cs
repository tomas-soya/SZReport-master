using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SZReport.PublicCode
{
    public static class DataHelper
    {
        public static string Analysis(DateTime? date, string Code)
        {
            var thisdate = date == null ? DateTime.Now : date.Value;

            string CHName = CodeHelper.CodeDic[Code];

            int[] ago = { 1, 3, 6, 12, 36, 60 };
            string[] agomessage = { "近1月百分比：", "近3月百分比：", "近6月百分比：", "近1年百分比：", "近3年百分比：", "近5年百分比：" };
            List<DateTime> dateTimes = new List<DateTime>();



            for (int i = 0; i < ago.Length; i++)
            {
                dateTimes.Add(thisdate.AddDays(30 * ago[i] * -1));
            }

            var data = DBHelper._db.SHAs.Where(tb => tb.Code == CodeHelper.CodeToDBCode(Code)).Select(tb => new
            {
                tb.Date,
                tb.Closing
            }).OrderByDescending(tb => tb.Date).ToList();

            var thisdata = data[0];

            string Message = "<br><h3>" + CHName + " " + thisdata.Date.ToString("yyyy-MM-dd") + " 收盘：" + thisdata.Closing.ToString("f2") + "</h3>";

            //if (Code == "000001")
            {
                var buyline = BuyLine(thisdata.Date, Code);
                if (thisdata.Closing >= buyline && thisdata.Closing / buyline < 1.01)
                {
                    Message += "<h4  style='color:green'>注意：即将进入买入区间</h4>";
                }
                if (thisdata.Closing < buyline)
                    Message += "<h4  style='color:green'>" + "下一交易日参考买点：" + buyline.ToString("f2") + "</h4>";
                else
                    Message += "<h4  style='color:red'>" + "下一交易日参考买点：" + buyline.ToString("f2") + "</h4>";

            }

            for (int i = 0; i < dateTimes.Count; i++)
            {
                var currentData = data.Where(tb => tb.Date >= dateTimes[i]).ToList();
                var rank = ((double)currentData.Where(tb => tb.Closing < thisdata.Closing).ToList().Count * 100) / currentData.Count;
                if (rank < 30)
                {
                    Message += "<h5 style='color:green'>" + agomessage[i] + rank.ToString("f2") + "%</h5>";
                }
                else if (rank < 70)
                {
                    Message += "<h5 style='color:orange'>" + agomessage[i] + rank.ToString("f2") + "%</h5>";
                }
                else
                {
                    Message += "<h5 style='color:red'>" + agomessage[i] + rank.ToString("f2") + "%</h5>";
                }
            }

            return Message;
        }


        public static double BuyLine(DateTime date,string Code)
        {
            int[] line = { 20, 250 };
            List<double> buyLines = new List<double>();
            List<string> result = new List<string>();
            var data = DBHelper._db.SHAs.Where(tb => tb.Code == CodeHelper.CodeToDBCode(Code)).Select(tb => new
            {
                tb.Date,
                tb.Closing
            }).ToList();
            for (int i = 0; i < line.Length; i++)
            {
                var before = data.Where(tb => tb.Date <= date).OrderByDescending(tb => tb.Date).Take(line[i]).ToList();
                var avg = before.Sum(tb => tb.Closing) / before.Count;

                buyLines.Add(avg);
            }

            return buyLines.Min();
        }

    }
}
