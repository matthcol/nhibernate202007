using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp01
{
    class Movie
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual int Year { get; set; }
        public virtual int? Duration { get; set; }
        public virtual string Genres { get; set; }
    }
}
