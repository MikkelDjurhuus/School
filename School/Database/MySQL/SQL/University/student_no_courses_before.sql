SELECT distinct id, name
FROM takes
join student using(ID)
where (select min(year)) > 2008
group by ID, name;