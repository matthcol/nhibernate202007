using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp01
{
    public class MovieListener : ISaveOrUpdateEventListener
    {
        public void OnSaveOrUpdate(SaveOrUpdateEvent @event)
        {
            var res = @event.Entity;
            if (res.GetType() == typeof(Movie))
            {
                var m = (Movie)res;
                Console.WriteLine("%%%% Save or update:", m.Title);
            }
        }
    }

}
