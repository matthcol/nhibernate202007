using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp01
{
    public class Star
    {
        public virtual int Id { get; set; }
        public virtual string FullName { get; set; }  // obligatoire
        public virtual DateTime? Birthdate { get; set; } // pas obligatoire

        public virtual IEnumerable<Movie> DirectedMovies { get; set; }
        public virtual IEnumerable<Movie> PlayedMovies { get; set; }

        public override string ToString() => FullName;
        
    }
}
