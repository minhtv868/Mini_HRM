using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.Common.Helpers
{
    public class ThumbImageSettings
    {
        public bool CreateImage { get; set; }
        public string PrefixPath { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
