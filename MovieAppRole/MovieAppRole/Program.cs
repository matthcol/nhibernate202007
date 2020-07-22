using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MovieAppRole;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
                Movie mGT = new Movie { Title = "Gran Torino", Year = 2008, Duration = 116 };
                Movie mGE = new Movie { Title = "La Grande Evasion", Year = 1963 };
                Movie mImp = new Movie { Title = "Impitoyable", Year = 1992 };
                session.Save(mGT);
                session.Save(mGE);
                session.Save(mImp);
                Star clint = new Star { FullName = "Clint Eastwood", Birthdate = new DateTime(1930, 5, 31) };
                Star mcQueen = new Star { FullName = "Steve McQueen", Birthdate = new DateTime(1930, 3, 24) };
                Star morgan = new Star { FullName = "Morgan Freeman", Birthdate = new DateTime(1937,6,1) };
                session.Save(clint);
                session.Save(mcQueen);
                session.Save(morgan);
                // association director
                mGT.Director = clint;
                mImp.Director = clint;
                // association play (actors)
                var playImpClint = new Play { Actor = clint, Movie = mImp, Role = "Bill Muny" };
                var playImpMorgan = new Play { Actor = morgan, Movie = mImp, Role = "Ned Logan" };

                var playTorinoClint = new Play { Actor = clint, Movie = mGT, Role = "Walt Kowalski" };

                var playEscapeSteve = new Play { Actor = mcQueen, Movie = mGE, Role = "Hilts 'The Cooler King'" };
                
                session.Save(playImpClint);
                session.Save(playImpMorgan);
                session.Save(playTorinoClint);
                session.Save(playEscapeSteve);

                tx.Commit();  // valider toutes les maj
               
            }
        }

        private static void ReadMovies()
        {
            using (ISession session = sessionFactory.OpenSession())
            {

                var play = session.Get<Play>(1);
                var movie = play.Movie;
                var actor = play.Actor;
                Console.WriteLine("{0} joue le rôle de {1} dans {2}", actor.FullName, play.Role, movie.Title);

                var res = session.CreateQuery(
                    @"select p.Actor, p.Role
                       from Play p
                       where p.Movie = :m"
                    )
                    .SetParameter("m", movie)
                    .List<Object[]>();
                foreach (var row in res)
                {
                    var actorMovie = (Star)row[0];
                    var role = row[1];
                    Console.WriteLine("\t # {0} le rôle de {1}", actorMovie.FullName, role);
                }

                foreach(var p in movie.Plays)
                {
                    Console.WriteLine("\t ** {0} le rôle de {1}", p.Actor.FullName, p.Role);
                }















                /*Movie m = session.Get<Movie>(1);
                Console.WriteLine("Read: {0}", m.Title);
                var res = session.CreateQuery(
                    @"select p.Actor, p.Role from Play p
                    where p.Movie = 3")
                    .List<Object[]>();
                foreach(var row in res)
                {
                    var star = (Star)row[0];
                    var role = row[1];
                    Console.WriteLine("\t - {0} : {1}", star.FullName, role);
                }
                
*/            }
        }

        private static void displayStars(IEnumerable<Star> stars, string bullet)
        {
            foreach (var s in stars)
            {
                Console.WriteLine("{0}: {1} ({2})", bullet, s.FullName, s.Birthdate);
            }
        }

        private static void displayMovies(IEnumerable<Movie> movies, string bullet)
        {
            foreach (var m in movies)
            {
                Console.WriteLine("{0}: {1} ({2}, {3} mn)", bullet, m.Title, m.Year, m.Duration);
            }
        }




        private static ISessionFactory Configure()
        {
           return Fluently.Configure()
            .Database(
               MsSqlConfiguration.MsSql2012
                    .ConnectionString("Server=Desktop-RRAODUE\\MSSQL17;Database=dbmovies03;User Id=umovie;Password=password;")
                    .ShowSql().FormatSql()                    
                    )
            .Mappings(
               m=>m.FluentMappings
                .Add<MovieMapping>()
                .Add<StarMapping>()
                .Add<PlayMap>()
                //.AddFromAssemblyOf<Movie>()
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
