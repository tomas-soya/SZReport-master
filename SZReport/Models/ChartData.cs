using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SZReport.Models
{
    public class ChartData
    {
        public string[] legend { get; set; }

        public string[] xAxis { get; set; }

        public List<series> series { get; set; }

        public ChartData()
        {
            series = new List<series>();
        }



    }

    public class series
    {
        public string name { get; set; }

        public string type { get; set; }

        public object[] data { get; set; }
    }

    public class pieces
    {
        public int gt { get; set; }

        public int lt { get; set; }
    }
}
