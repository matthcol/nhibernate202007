using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp01
{
    class Program
    {
        private static ISessionFactory sessionFactory;
        static void Main(string[] args)
        {
            sessionFactory = Configure();
            AddMovies();
            ReadMovies();
        }
        private static void AddMovies()
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                ITransaction tx = session.BeginTransaction();
                Movie mGT = new Movie { Title = "Gran Torino", Year = 2008 };
                session.Save(mGT);
                tx.Commit();
                Console.WriteLine("Movie inserted with id {0:D}", mGT.Id);
            }
        }

        private static void ReadMovies()
        {

        }

        private static ISessionFactory Configure()
        {
           return Fluently.Configure()
            .Database(
               MsSqlConfiguration.MsSql2012
                    .ConnectionString("Server=Desktop-RRAODUE\\MSSQL17;Database=dbmovies03;User Id=umovie;Password=password;")
                    .ShowSql()                    
                    )
            .Mappings(
               m=>m.FluentMappings.AddFromAssemblyOf<Movie>()
               )
           .ExposeConfiguration(cfg =>
             {
                 new SchemaExport(cfg)
                   .Create(false, true);
             })
            .BuildSessionFactory();
        }
    }

}
