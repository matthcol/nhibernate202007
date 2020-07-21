﻿using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
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
            // AddMovies();
            ReadMovies();
            RequestMovies();
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
                // association director (bidirectionnelle)
                mGT.Director = clint;
                mImp.Director = clint;
                var moviesClint = new List<Movie> { mGT, mImp };
                clint.DirectedMovies = moviesClint;
                // association play (actors)
                var actorsImpitoyable = new List<Star> { clint,  morgan};
                mImp.Actors = actorsImpitoyable;

                tx.Commit();  // valider toutes les maj
                Console.WriteLine("Movie inserted with id {0:D}", mGT.Id);
            }
        }

        private static void ReadMovies()
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                Movie m = session.Get<Movie>(2);
                Console.WriteLine("Read: {0}", m.Title);
                Star s = session.Get<Star>(1);  // Clint Eastwood
                Console.WriteLine("Read: {0}", s.FullName);

                var movies = session.Query<Movie>(); //.ToList();
                foreach (var mov in movies)
                {
                    Console.WriteLine(" * Read: {0}", mov.Title);
                    if (mov.Director != null)
                    {
                        //Console.WriteLine("\t - {0}", mov.Director.FullName);
                    }
                }

                //IList<Movie> movies2 = session.CreateQuery("from Movie m where m.Year = 2008").List<Movie>(); // HQL
                // var query = session.CreateQuery("from Movie m where m.Director.id = :id");
                //query.SetInt32(1, s.Id);
                //query.SetInt32("id", s.Id);
                //query.SetParameter("id", s.Id);
                //query.SetParameter(1, s.Id);
                //IList<Movie> movies2 = query.List<Movie>(); // HQL
                //foreach (var mov in movies2)
                foreach (var mov in s.DirectedMovies)
                {
                    Console.WriteLine(" + Read: {0}", mov.Title);
                    Console.WriteLine("\t -> Director: {0}", mov.Director);//.FullName);

                }
            }
        }

        private static void RequestMovies()
        {
            using (ISession session = sessionFactory.OpenSession())
            {

                
            }

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
               m=>m.FluentMappings
                .Add<MovieMapping>()
                .Add<StarMapping>()
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
