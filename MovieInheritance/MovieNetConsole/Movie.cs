using NHibernate.Linq.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieNetConsole
{
    public class Movie : Media
    {
      
        public virtual int? Duration { get; set; }

        public virtual Star Director { get; set; } 

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Title);
            sb.AppendFormat("({0:D}, {1:D} mn, ", Year, Duration);
            sb.Append(Genres);
            sb.Append(")");
            return sb.ToString();
        }
    }
}
