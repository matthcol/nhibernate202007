using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp01
{
    public class StarMapping: ClassMap<Star>
    {
        StarMapping()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.FullName).Column("name").Not.Nullable();
            Map(x => x.Birthdate).Nullable();
            HasMany<Movie>(x => x.DirectedMovies)
                .KeyColumn("id_director") // non obligatoire si mode Inverse et seule association
               // .Cascade.SaveUpdate()
                .Inverse();  // association bidirectionnelle déjà mappé pour l'autre classe (FK)
            HasManyToMany<Movie>(x => x.PlayedMovies)
                .Table("Play")
                .ParentKeyColumn("id_actor")
                .ChildKeyColumn("id_movie")
                .Inverse();
            Table("Stars");
        }
    }
}
