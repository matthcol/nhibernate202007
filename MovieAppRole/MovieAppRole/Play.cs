using MovieApp01;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppRole
{
    public class Play
    {
        public virtual int Id { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual Star Actor { get; set; }
        public virtual string Role { get; set; }
    }
}
