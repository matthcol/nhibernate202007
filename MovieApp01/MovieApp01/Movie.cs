﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp01
{
    public class Movie
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual int Year { get; set; }
        public virtual int? Duration { get; set; }
        public virtual string Genres { get; set; }
        
        // associations :
        public virtual Star Director { get; set; }
        public virtual IEnumerable<Star> Actors { get; set; } // quid du role ?

        public override string ToString()
        {
            return Title;
        }
    }
}
