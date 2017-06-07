SELECT dept_name, min(salary) min_salary
FROM instructor
where salary in (select max(salary) from instructor group by dept_name)