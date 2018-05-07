using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMStringUtil.BO
{
    public class StructText
    {
        public string text { get; set; }
        public List<StructText> children { get; set; }
    }
}
