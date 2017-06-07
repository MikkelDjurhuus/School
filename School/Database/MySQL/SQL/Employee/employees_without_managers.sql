select employeeName
from employee
full join manages using(employeeName)
where (employeeName = null or empoyeeName = '')
