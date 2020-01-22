using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SZReport.Models;

namespace SZReport.PublicCode
{
    public static class DataUpdater
    {
        private const string demourl = "http://quotes.money.163.com/service/chddata.html?code=0000001&start=19901219&end=20150911&fields=TCLOSE;HIGH;LOW;TOPEN;LCLOSE;CHG;PCHG;VOTURNOVER;VATURNOVER";

        private const string url = "http://quotes.money.163.com/service/chddata.html?code={0}&start={1}&end={2}&fields=TCLOSE;HIGH;LOW;TOPEN;LCLOSE;CHG;PCHG;VOTURNOVER;VATURNOVER";


        public static void UpdateData(string Code)
        {
            string start = "19700101";
            //var datetime = new DateTime(2019, 10, 1);
            //var datas = DBHelper._db.SHAs.Where(tb => tb.Date >= datetime).ToList();
            //DBHelper._db.SHAs.RemoveRange(datas);
            //DBHelper._db.SaveChanges();
            //return;
            try
            {
                start = DBHelper._db.SHAs.Where(tb => tb.Code == CodeHelper.CodeToDBCode(Code)).Max(tb => tb.Date).AddDays(1).ToString("yyyyMMdd");
            }
            catch(Exception e)
            {

            }


            string end = DateTime.Now.AddDays(1).ToString("yyyyMMdd");

            if (start == end)
                return;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(url, CodeHelper.CodeToWebCode(Code), start, end));
            WebResponse response = request.GetResponse();
            
            Stream resStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(resStream, Encoding.GetEncoding("GB2312"));

            var result = reader.ReadToEnd();
            result = result.Replace("None", "-1");
            var rows = result.Split("\r\n");

            for (int i = 1; i < rows.Length; i++)
            {
                if (rows[i] == "")
                    continue;
                var columns = rows[i].Split(",");

                SHA sha = new SHA();
                sha.Date = DateTime.Parse(columns[0]);
                sha.Code = columns[1];
                sha.Name = columns[2];
                sha.Closing = double.Parse(columns[3]);
                sha.Highest = double.Parse(columns[4]);
                sha.Lowest = double.Parse(columns[5]);
                sha.Opening = double.Parse(columns[6]);
                sha.FrontOpening = double.Parse(columns[7]);
                sha.UpDown = double.Parse(columns[8]);
                sha.UpDownWidth = double.Parse(columns[9]);
                sha.Volume = double.Parse(columns[10]);
                sha.Turnover = double.Parse(columns[11]);
                sha.MA5 = 0;
                sha.MA10 = 0;
                sha.MA20 = 0;
                sha.MA30 = 0;
                sha.MA60 = 0;
                sha.MA90 = 0;
                sha.MA120 = 0;
                sha.MA200 = 0;
                sha.MA250 = 0;
                

                if (Code == "000001")
                {
                    var daydiff = (sha.Date - new DateTime(1993, 2, 15)).Days;
                    var todaylow = (daydiff * 23.2091 + 21114.3729) / 100;
                    var todayhigh = (daydiff * 22.2049 + 153682) / 100;
                    sha.ReferUp = todayhigh;
                    sha.ReferDown = todaylow;
                }

                DBHelper._db.Add(sha);
            }

            DBHelper._db.SaveChanges();
            UpdateMA(Code);
            Console.WriteLine("");
        }

        public static void UpdateRefer()
        {

        }

        public static void UpdateMA(string Code)
        {
            var alldata = DBHelper._db.SHAs.Where(tb => tb.Code == CodeHelper.CodeToDBCode(Code)).OrderByDescending(tb => tb.Date).ToList();
            var needcal = alldata.Where(tb => tb.MA5 == 0).ToList();
            foreach(var a in needcal)
            {
                if (a.MA5 == -1 || a.MA5 > 0)
                    continue;
                var current = alldata.Where(tb => tb.Date <= a.Date).OrderByDescending(tb=>tb.Date).ToList();
                if (current.Count < 250)
                {
                    a.MA5 = -1;
                    a.MA10 = -1;
                    a.MA20 = -1;
                    a.MA30 = -1;
                    a.MA60 = -1;
                    a.MA90 = -1;
                    a.MA120 = -1;
                    a.MA200 = -1;
                    a.MA250 = -1;
                }
                else
                {
                    a.MA5 = current.Take(5).Sum(tb => tb.Closing) / 5;
                    a.MA10 = current.Take(10).Sum(tb => tb.Closing) / 10;
                    a.MA20 = current.Take(20).Sum(tb => tb.Closing) / 20;
                    a.MA30 = current.Take(30).Sum(tb => tb.Closing) / 30;
                    a.MA60 = current.Take(60).Sum(tb => tb.Closing) / 60;
                    a.MA90 = current.Take(90).Sum(tb => tb.Closing) / 90;
                    a.MA120 = current.Take(120).Sum(tb => tb.Closing) / 120;
                    a.MA200 = current.Take(200).Sum(tb => tb.Closing) / 200;
                    a.MA250 = current.Take(250).Sum(tb => tb.Closing) / 250;
                }
                DBHelper._db.Update(a);
            }
            DBHelper._db.SaveChanges();
        }
    }

}
