using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchCoreApi.Common.Performance
{
    public class TableData
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int Count { get; set; }
        public double Percent { get; set; }
        public DateTime Date { get; set; }
    }
}
