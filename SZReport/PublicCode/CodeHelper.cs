using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SZReport.PublicCode
{
    public static class CodeHelper
    {
        public static Dictionary<string, string> CodeDic = InitDic();
        private static Dictionary<string, string> InitDic()
        {
            Dictionary<string, string> result = new Dictionary<string, string>
            {
                { "000001", "上证指数" },
                { "000905", "中证500" },
                { "399363", "计算机指" },
                { "399975", "证券公司" },
                { "399959", "军工指数" },
                { "399990", "煤炭等权" },
                { "399417", "新能源车" },
                { "000121", "医药主题" },
                { "399006", "创业板指" },
                { "399812", "养老产业" },
                { "000074", "消费等权" },
                { "399300", "沪深300" },
                { "000122", "农业主题" },
                { "000016", "上证50" },
                { "399986", "中证银行" },
                { "000006", "地产指数" },
                { "399997", "中证白酒" }
            };
            return result;
        }

        public static string CodeToDBCode(string Code)
        {
            return "'" + Code;
        }

        public static string DBCodeToCode(string Code)
        {
            return Code.Replace("'", "");
        }

        public static string CodeToWebCode(string Code)
        {
            if (Code.StartsWith("0"))
                return "0" + Code;
            else
                return "1" + Code;
            return Code;
        }
    }
}
