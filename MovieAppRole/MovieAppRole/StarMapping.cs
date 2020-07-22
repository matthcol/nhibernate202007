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
            Table("Stars");
        }
    }
}
