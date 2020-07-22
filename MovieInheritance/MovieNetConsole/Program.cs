using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using FluentNHibernate.Cfg;
using NHibernate;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Linq;

namespace MovieNetConsole
{
    class Program
    {
        private static ISessionFactory _sessionFactory;

        static void Main(string[] args)
        {
            using (var sessionFactory = CreateSessionFactory()) {
                _sessionFactory = sessionFactory;
                AddMovies();
                ReadMovies();
            }
        }

        private static void AddMovies()
        {
            Movie mGT = new Movie { Title = "Gran Torino", Year = 2008, Duration = 116 , Type = MediaType.MOVIE};
            Star sClint = new Star { FullName = "Clint Eastwood", Birthdate = new DateTime(1930, 5, 31) };
           // mGT.Director = sClint;

            TVSeries friends = new TVSeries { Title = "Friends", Year = 1994, EndYear = 2004, SeasonNumber = 10, Type=MediaType.TVSERIES};

            using (ISession session = _sessionFactory.OpenSession())
            {
                ITransaction tx = session.BeginTransaction();
                session.Save(sClint);
                session.Save(mGT);
                session.Save(friends);
                tx.Commit();
            }

        }

        private static void ReadMovies()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var movies = session.Query<Media>().ToList(); 
                // query sur Media : union
                // ou sur Movie ou sur TVSeries : 1 seule table sans join
                foreach (var m in  movies)
                {
                    Console.WriteLine("Read: {0} ({1}/{2})", m.Title, m.Type, m.GetType());
                    if (m.GetType() == typeof(Movie))
                    {
                        var mov = (Movie)m;
                        Console.WriteLine("\t - {0} mn", mov.Duration);
                    }
                }
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
              .Database(
                MsSqlConfiguration.MsSql2012
                   // .ConnectionString("Server=Desktop-RRAODUE\\MSSQL17;Database=dbmovies01;User Id=umovie;Password=password;")
                      .ConnectionString("Server=Desktop-RRAODUE\\MSSQL17;Database=dbmovies03;User Id=umovie;Password=password;")
                      .ShowSql()
              )
              .Mappings(m =>
                m.FluentMappings
                    .Add<MediaMap>()
                    .Add<MovieMap>()
                    .Add<TVSeriesMap>()
                    .Add<StarMap>()
              )
              .ExposeConfiguration(cfg =>
              {
                  new SchemaExport(cfg)
                    .Create(true, true);
              })
              .BuildSessionFactory();
        }
    }
}
