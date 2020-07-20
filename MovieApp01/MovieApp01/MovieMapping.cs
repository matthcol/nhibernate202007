using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp01
{
    class MovieMapping: ClassMap<Movie>
    {
        MovieMapping()
        {
            Id(x => x.Id);
            Map(x => x.Title);
            Map(x => x.Year);
        }
    }
}
