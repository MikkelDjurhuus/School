delete from takes
where sec_id in (select sec_id
from section
where course_id in (select course_id
from course
where title like '%database%'))
