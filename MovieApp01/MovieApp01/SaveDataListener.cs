using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp01
{
    class SaveDataListener : ISaveOrUpdateEventListener
    {
        public void OnSaveOrUpdate(SaveOrUpdateEvent @event)
        {
            var data = @event.Entity;
            Console.WriteLine("@@@ Data saved or updated : {0} <{1}>", data.GetType(), data);
        }
    }
}
