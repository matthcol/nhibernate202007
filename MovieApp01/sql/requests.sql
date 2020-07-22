use dbmovies02;

select count(*) from Movies;
select count(*) from Stars;
select count(*) from Play;

select * from Movies where year = 1992;
select * from Movies where year <> 1992;
select * from Movies where year < 1992; -- <=, > ,>=
select * from Movies where year between 1990 and 1994;

select * from Movies where title like '%star%';
select * from Movies where title like '%empire%';
select * from Movies where 	title like '%empire%' or title like '%jedi%';
select * from Movies where 	title like '%star%' and title like '%jedi%';
select * from Movies where 	(title like '%empire%' or title like '%jedi%') and genres not like '%Short%';
select * from Movies where 	title like '%empire%' or (title like '%jedi%' and genres not like '%Short%');

select * from Movies where 	title like '%star%' and year in (1977,1980);

select * from Stars where birthdate is null;
select * from Stars where birthdate is not null;

select s.*, m.title, p.role 
from Stars s join Play p on s.Id = p.id_actor
	join Movies m on p.id_movie = m.id
where name like '%Spielberg';

select * from Movies where title = 'the man who knew too much';
select * from Movies 
where year > 1950 
	and id_director in (
	select id from stars where name like '%Hitchcock%')
order by year desc, title asc;

select name, year(birthdate) as year from stars
where birthdate is not null
	and name like 'John%'
	and exists(select * from play where id = id_actor)
order by year;

select m.duration /60 as heure, m.duration % 60 as min, m.* 
from movies m where m.year = 1992;


select name, count(*) as nb_film
from movies m join  stars s on m.id_director = s.id
where m.year between 1990 and  1999
group by s.id, s.name
order by nb_film desc;
