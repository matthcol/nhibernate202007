using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Event;
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
            //UpdateData();
            // UpdateData2();
            // DetachData();
            //ReadMovies();
            //RequestMoviesCriteria();
            RequestMoviesHQL();
            //RequestMoviesLinQ();
        }
        private static void DetachData()
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                var mGranTorino = session.Get<Movie>(1); // to work offline
                session.Evict(mGranTorino);
                var mGreatEscape = session.Get<Movie>(2); // to work online
                mGreatEscape.Duration = 172;  // MAJ d'1 objet du cache hibernate
                mGranTorino.Title = "GRAN TORINO";  // MAJ de l'objet hors cache
                session.Flush();
            }
        }
        private static void UpdateData2()
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                var tx = session.BeginTransaction();
                var mImpitoyable = session.Get<Movie>(3);
                mImpitoyable.Duration = 130;
                tx.Commit();

                tx = session.BeginTransaction();
                displayStars(mImpitoyable.Actors, "\t - a");
                var actorsImpitoyable = new List<Star>();
                actorsImpitoyable.AddRange(mImpitoyable.Actors);
                var gene = new Star { FullName = "Gene Hackman" };
                session.Save(gene);
                actorsImpitoyable.Add(gene);
                gene.PlayedMovies = new List<Movie> { mImpitoyable };
                mImpitoyable.Actors = actorsImpitoyable;
                tx.Commit();
            }
        }
        private static void UpdateData()
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                var tx = session.BeginTransaction();
                var mGE = session.Get<Movie>(2);
                var john = new Star { FullName = "John Sturges" };
                session.Save(john); // on rend persistent le nouveau director
                mGE.Director = john;
                tx.Commit();

                tx = session.BeginTransaction();
                var mGA = new Movie { Title = "Guet-apens", Year = 1972 };
                var sam = new Star { FullName = "Sam Peckinpah" };
                session.Save(sam);  // omit with cascade.SaveUpdate
                mGA.Director = sam; // TODO : set the reverse association
                var filmSam = new List<Movie> { mGA };
                sam.DirectedMovies = filmSam;
                session.Save(mGA); // save same before grace au cascade.SaveUpdate
                tx.Commit();
                Console.WriteLine("Guet-Apens {0} / Sam {1}", mGA.Id, sam.Id);


                /*tx = session.BeginTransaction();
                mGA.Director = null; // enleve le lien (TODO : other direction)
                session.Delete(sam);
                tx.Commit();*/

                tx = session.BeginTransaction();
                session.Delete(mGA);
                // session.Delete(sam); // opt
                tx.Commit();
                // if sam not deleted, he keeps Guet-Apens in its directed movies in memory
                displayMovies(sam.DirectedMovies, "\t - m");
            }
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
                var playedMovies = new List<Movie> { mImp };
                mImp.Actors = actorsImpitoyable;
                clint.PlayedMovies = playedMovies;
                morgan.PlayedMovies = playedMovies; // Attention au partage de liste

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
                /*displayMovies(s.DirectedMovies, "\t * mD");
                displayMovies(s.PlayedMovies, "\t ~ mA");

                var movies = session.Query<Movie>(); //.ToList();
                foreach (var mov in movies)
                {
                    Console.WriteLine(" * Read: {0}", mov.Title);
                    if (mov.Director != null)
                    {
                        Console.WriteLine("\t - d: {0}", mov.Director.FullName);

                    }
                    displayStars(mov.Actors, "\t - a");
                }*/

                //IList<Movie> movies2 = session.CreateQuery("from Movie m where m.Year = 2008").List<Movie>(); // HQL
                // var query = session.CreateQuery("from Movie m where m.Director.id = :id");
                //query.SetInt32(1, s.Id);
                //query.SetInt32("id", s.Id);
                //query.SetParameter("id", s.Id);
                //query.SetParameter(1, s.Id);
                //IList<Movie> movies2 = query.List<Movie>(); // HQL
                //foreach (var mov in movies2)
                /*foreach (var mov in s.DirectedMovies)
                {
                    Console.WriteLine(" + Read: {0}", mov.Title);
                    Console.WriteLine("\t -> Director: {0}", mov.Director);//.FullName);

                }*/
            }
        }

        private static void RequestMoviesCriteria()
        {
            using (ISession session = sessionFactory.OpenSession())
            {   // api by Criteria
                /*var movies = session.CreateCriteria<Movie>().List<Movie>();
                displayMovies(movies, "- all");*/
                //var movieExample = new Movie { Title = "the man who knew%" , Year= 1956};
                //var movies = session.CreateCriteria<Movie>()
                //.CreateAlias("Director", "d")
                //.Add(Restrictions.Gt("Year", 1950))
                //.SetFetchMode("Actors", FetchMode.Join)
                //.AddOrder(Order.Desc("Year"))
                // .CreateCriteria("Director") //, JoinType.LeftOuterJoin)
                //   .Add(Restrictions.Like("FullName", "%Hitchcock%"))
                // .Add(Restrictions.Like("d.FullName", "%Hitchcock%"))
                //.Add(Expression.Eq("Year", 1992))  // lt <, gt >, le <=, ge >=
                //.Add(Expression.Not(Expression.Eq("Year", 1992)))
                //.Add(Expression.Between("Year", 1970, 1982))
                //.Add(Restrictions.InsensitiveLike("Title", "%star%"))
                /*.Add(Restrictions.Or(
                    Restrictions.Like("Title", "Star%"),
                    Restrictions.Like("Title", "Indiana%")
                    ))*/
                /*.Add(Restrictions.InG<int>("Year", new List<int> { 1977, 1983, 1984}))*/
                //.CreateCriteria("Director")
                //    .Add(Restrictions.Like("FullName","%Lucas"))
                /*.Add(Example.Create(movieExample)
                    .ExcludeZeroes() // exclus NULL + int à 0
                    .EnableLike()
                    .IgnoreCase()
                    .ExcludeProperty("Year")
                )*/
                /*  .Add(Example.Create(movieExample)
                      .EnableLike()
                      .ExcludeZeroes()
                      .ExcludeProperty("Year"))*/

                //   .List<Movie>();
                //displayMovies(movies, "- m");
                //Console.WriteLine(movies.Count());
                /* var res = session.CreateCriteria<Movie>()
                     .Add(Restrictions.Between("Year", 1990, 1999))
                     .SetProjection(
                         Projections.GroupProperty("Year"),
                         Projections.RowCount()
                         )
                         .List<object[]>();*/
                /*.SetProjection(Projections.ProjectionList()
                    .Add(Projections.RowCount())
                    .Add(Projections.Min("Year")))
                *.List<object[]>();*/
                /*foreach (var e in res)
                {
                    Console.WriteLine("year={0};  nb={1}", e[0], e[1]);
                    //Console.WriteLine("nb={0};  min year={1}", e[0], e[1]);
                    //Console.WriteLine(e.GetType());
                }*/

                /*var res2 = session.CreateCriteria<Star>()
                     .Add(Restrictions.IsNotEmpty("PlayedMovies"))
                     .Add(Restrictions.IsNotNull("Birthdate"))
                     .Add(Restrictions.Like("FullName", "John%"))
                     .SetProjection(
                         Projections.Property("FullName"),
                         Projections.Alias(
                             Projections.SqlFunction("year", NHibernateUtil.Int32, Projections.Property("Birthdate")),
                             "year"))
                         .AddOrder(Order.Asc("year"))
                     //.List<int>();
                     .SetFirstResult(100)  // Skip
                     .SetMaxResults(50)     // taille page
                     .List<object[]>();
                 //displayStars(stars, "* s");
                 foreach (var e in res2)
                 {
                     Console.WriteLine("name={0}; year={1}", e[0], e[1]);
                 }
*/
            } 
        }

        private static void RequestMoviesHQL()
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                // requete en HQL
                /*var res3 = session.CreateQuery("from Movie m where m.Year = 1992")
                                .List<Movie>();
                displayMovies(res3, "# HQL");*/
                /*var res3 = session.CreateQuery("select m.Duration /60 as heure, mod(m.Duration,60) as min, m from Movie m where m.Year = 1992")
                                .List<Object[]>();
                foreach(var row in res3)
                {
                    Movie m = (Movie) row[2];
                    Console.WriteLine("# HQL : {0}H{1} {2}", row[0], row[1], m.Title);
                }*/

                /*                "select s.FullName, extract(year from s.Birthdate) from Star s where s.Birthdate is not null and s.FullName like :name")
                                                .SetParameter("name", "John%")
                */


                /*var res3 = session.CreateQuery(
                                "select m from Movie m where m.Director.FullName like :name")
                                .SetParameter("name","%Hitchcock")
                                .List<Movie>();
                displayMovies(res3, "~HQL");*/
                /*var res3 = session.CreateQuery(
                     @"select d, count(m)  
                    from Movie m join m.Director d
                    where m.Year between :year1 and  :year2 
                    group d, d.FullName, d.Birthdate
                    having count(m) > 1
                    order by count(m) desc"
                    )
                    .SetParameter("year1", 1990)
                    .SetParameter("year2", 1999)
                    .List<Object[]>();
                foreach (var row in res3)
                {
                    var director = (Star)row[0];
                    Console.WriteLine("# HQL : {0}, {1}", director.FullName, row[1]);
                }*/
                /*var res3 = session.CreateQuery(
                     @"select s, count(m)
                    from Star s left join s.DirectedMovies m
                    where s.FullName like 'John%'
                            and s.Birthdate is not NULL
                    group by s, s.FullName, s.Birthdate 
                    order by count(m) desc"
                    )
                    .List<Object[]>();
                foreach (var row in res3)
                {
                    var director = (Star)row[0];
                    Console.WriteLine("# HQL : {0}, {1}", director.FullName, row[1]);
                }*/

                /*var res4 = session.CreateQuery(
                     @"select d, m
                       from Movie m join m.Director d join m.Actors a
                        where  d.Id = a.Id"
                    )
                    .List<Object[]>();
                foreach (var row in res4)
                {
                    var director = (Star) row[0];
                    var movie = (Movie) row[1];
                    Console.WriteLine("* HQL : {0} -> {1} ({2})",
                        director.FullName, movie.Title, movie.Year);
                }*/

                // size, elements, indices, minIndex, maxIndex, minElement, maxElement
                /*var res4 = session.CreateQuery(
                    @"from Movie m
                    where m.Actors.size >= 10"
                    )
                    .List<Movie>();*/
                // displayMovies(res4, "# m");
                var res4 = session.CreateQuery(
                      @"select m
                     from Movie m 
                     where  exists elements(m.Actors)
                        and Year=1992"
                      )
                      .List<Movie>();
                displayMovies(res4, "### m");

            }
        }

        private static void RequestMoviesLinQ()
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                /*var res4 = session.Query<Movie>()
                            .Where(m => m.Year == 1992)
                            .Where(m => m.Title.StartsWith("U"))  // n movies
                            // Following : load EAGER associate objects
                            .Fetch(m => m.Director)   // n+1 : quasi tjrs gagnant
                            //.ThenFetch(d => d.DirectedMovies)
                            .FetchMany(m => m.Actors)  // n*nA (nA : nb acteurs moyen)
                            .ToList();
                foreach (var m in res4) {
                    Console.WriteLine("- {0} {1} {2}", m.Title, m.Director.FullName, m.Actors.Count());
                    foreach (var a in m.Actors)
                    {
                        Console.WriteLine("\t * a: {0}", a.FullName);
                    }
                } */

                /*int page = 10;
                int numPage = 0;
                var q = session.QueryOver<Star>()
                            .Where(s => s.Birthdate.Value.Year < 1930)
                            .JoinQueryOver(s => s.PlayedMovies)
                                .Where(m => m.Year.IsBetween(1950).And(1960));
                var res5 = q.Skip(numPage * page)
                            .Take(page)
                            .List();
                displayStars(res5, "#p0 s");
                numPage++;
                res5 = q.Skip(numPage * page)
                            .Take(page)
                            .List();
                displayStars(res5, "#p1 s");*/


                /*IEnumerable<Star> stars = session.Query<Star>()
                        .Where(s => s.FullName.Like("Steven%"))
                        .ToFuture();
                IFutureValue<int> res = session.Query<Star>()
                        .Where(s => s.FullName.Like("John%"))
                        .ToFutureValue(q => q.Count());
                foreach (var star in stars)
                {
                    Console.WriteLine("- {0} (nb John: {1})", star.FullName, res.Value);
                }
*/
                // cf : BatchQuery



                /*var movies3 = session.Query<Movie>()
                    .Where(m => m.Year == 1992)
                    .ToList();
                displayMovies(movies3, "@ m");
*/
                /*var res5 = session.Query<Movie>()
                    .Where(m => m.Year == 1992)
                    .Select(m=>new Object[]{m.Title, m.Year /10)
                    .ToList();*/
                //displayMovies(movies3, "@ m");
            }

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
                     .ConnectionString("Server=Desktop-RRAODUE\\MSSQL17;Database=dbmovies01;User Id=umovie;Password=password;")
                     .ShowSql()
                     )
             .Mappings(
                m => m.FluentMappings
                 .Add<MovieMapping>()
                 .Add<StarMapping>()
                //.AddFromAssemblyOf<Movie>()
                )
            /*.ExposeConfiguration(cfg =>
              {
                  new SchemaExport(cfg)
                    .Create(false, true);
              })*/
            .ExposeConfiguration(
                 c =>
                 {
                     c.AppendListeners(ListenerType.Save, new ISaveOrUpdateEventListener[]{
                     new SaveDataListener()
                     });
                     c.SetInterceptor(new MovieHnnInterceptor());
                 })
            .BuildSessionFactory();
        }
    }

}
