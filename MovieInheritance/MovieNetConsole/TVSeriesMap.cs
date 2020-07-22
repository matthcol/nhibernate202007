using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieNetConsole
{
    public class TVSeriesMap : SubclassMap<TVSeries>
    {
        TVSeriesMap()
        {
            Map(x => x.EndYear);
            Map(x => x.SeasonNumber);
            // DiscriminatorValue(MediaType.TVSERIES.ToString());
            KeyColumn("media_id");
        }
    }
    
}
