using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieNetConsole
{
    public class MovieMap:SubclassMap<Movie>
    {
        MovieMap()
        { 
            Map(x => x.Duration)
                .Nullable();
            /* References(x => x.Director)
                 .Column("id_director")
                 .Nullable();*/
            // DiscriminatorValue(MediaType.MOVIE.ToString());
            // KeyColumn("media_id");
        }
    }

}
