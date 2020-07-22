using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieNetConsole
{
    public class TVSeries: Media
    {
        
        
        public virtual int? EndYear { get; set; }
        public virtual int? SeasonNumber { get; set; }
    }
}
