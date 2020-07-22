using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppRole
{
    public class PlayMap : ClassMap<Play>
    {
        PlayMap()
        {
            Id(x => x.Id);
            References(x => x.Movie).Column("id_movie");
            References(x => x.Actor).Column("id_actor");
            Map(x => x.Role).Length(150);
        }

    }
}
