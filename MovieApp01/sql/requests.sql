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
