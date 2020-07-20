using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp01
{
    class Program
    {
        static void Main(string[] args)
        {
            AddMovies();
            ReadMovies();
        }
        private static void AddMovies()
        {
            Movie mGT = new Movie { Title = "Gran Torino", Year = 2008 };
        }

        private static void ReadMovies()
        {

        }

        private ISessionFactory Configure()
        {
           return Fluently.Configure()
            .Database(
               MsSqlConfiguration.MsSql2012
                    .ConnectionString("Server = Desktop - RRAODUE\\MSSQL17; Database = dbmovies03; User Id = umovie; Password = password; "))
            .Mappings(
               m=>m.FluentMappings.AddFromAssemblyOf<Movie>()
               )
            .BuildSessionFactory();
        }
    }

}
