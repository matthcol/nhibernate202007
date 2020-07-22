using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieNetConsole
{
    public class MediaMap: ClassMap<Media>
    {
        MediaMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Sequence("seq_movie");
            Map(x => x.Title)
                .Length(250)
                .Not.Nullable();
            Map(x => x.Year)
                .Not.Nullable();
            // Discriminant :
            /*  Map(x => x.Type, "media_type");
              DiscriminateSubClassesOnColumn("media_type")
                   .AlwaysSelectWithValue()
              //.Formula("arbitrary SQL expression")
                  .ReadOnly()
                  .Not.Nullable()
                  .CustomType<string>();*/
            UseUnionSubclassForInheritanceMapping();

        }
    }
}
