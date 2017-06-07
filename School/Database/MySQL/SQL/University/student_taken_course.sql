SELECT distinct student.name
FROM takes
join student using(ID)
where dept_name = "Comp. Sci."