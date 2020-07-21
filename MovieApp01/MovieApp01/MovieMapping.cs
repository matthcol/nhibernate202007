using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp01
{
    public class MovieMapping: ClassMap<Movie>
    {
        MovieMapping()
        {
            Id(x => x.Id).GeneratedBy.Identity();
                //.Guid();
                //.Assigned(); // provided by NHibernate
                //.Sequence("seq_movie_id");
                //.Identity(); // default pour un int
            Map(x => x.Title).Length(250).Not.Nullable();
            Map(x => x.Year).Not.Nullable();
            Map(x => x.Duration).Nullable(); // default mode
            References<Star>(x => x.Director)
                //.Fetch.Join()
                .Column("id_director")
                .Nullable();
            HasManyToMany<Star>(x => x.Actors)
                .Table("Play")
                .ParentKeyColumn("id_movie")
                .ChildKeyColumn("id_actor");  // maxime : cascade.saveUpdate
            Table("Movies");
        }
    }
}
