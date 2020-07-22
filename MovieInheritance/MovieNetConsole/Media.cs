using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieNetConsole
{
    public abstract class Media
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual int Year { get; set; }
        public virtual string Genres { get; set; }

        public virtual MediaType Type { get; set; }

    }
}
