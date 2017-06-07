insert into takes (id,course_id,sec_id, semester,year)
select id,"CS-001",1,"Fall",2009
from student
where dept_name = "Comp.Sci.";