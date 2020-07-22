using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieNetConsole
{
    public class Star
    {
        public virtual int Id { get; set; }
        public virtual string FullName { get; set; }

        public virtual DateTime Birthdate { get; set; }
    }
}
