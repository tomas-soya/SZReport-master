using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SZReport.Models
{
    /// <summary>
    /// 上证指数
    /// </summary>
    public class SHA
    {
        public int ID { get; set; }

        [DisplayName("日期")]
        public DateTime Date { get; set; }

        [DisplayName("股票代码")]
        public string Code { get; set; }

        [DisplayName("股票名称")]
        public string Name { get; set; }

        [DisplayName("收盘价")]
        public double Closing { get; set; }

        [DisplayName("开盘价")]
        public double Opening { get; set; }

        [DisplayName("最高价")]
        public double Highest { get; set; }

        [DisplayName("最低价")]
        public double Lowest { get; set; }

        [DisplayName("前收盘")]
        public double FrontOpening { get; set; }

        [DisplayName("涨跌额")]
        public double UpDown { get; set; }

        [DisplayName("涨跌幅")]
        public double UpDownWidth { get; set; }

        [DisplayName("成交量")]
        public double Volume { get; set; }
        [DisplayName("成交额")]
        
        public double Turnover { get; set; }

        [DisplayName("参考高值")]
        public double ReferUp { get; set; }
        [DisplayName("参考低值")]
        public double ReferDown { get; set; }

        public double MA5 { get; set; }

        public double MA10 { get; set; }

        public double MA20 { get; set; }

        public double MA30 { get; set; }

        public double MA60 { get; set; }

        public double MA90 { get; set; }

        public double MA120 { get; set; }

        public double MA200 { get; set; }

        public double MA250 { get; set; }
    }
}
