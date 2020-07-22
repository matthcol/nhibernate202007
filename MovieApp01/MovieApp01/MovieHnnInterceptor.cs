using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp01
{
    class MovieHnnInterceptor : EmptyInterceptor
    {
        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            
            var res =  base.OnLoad(entity, id, state, propertyNames, types);
            Console.WriteLine("!!!!! Intercept Load");
            // TODO : to improve
            /*if (entity.GetType() == typeof(Movie))
            {
                var movie = (Movie)entity;
                movie.Title = "###" + movie.Title;
            }*/
            return res;
        }
    }
}
