SELECT dept_name, max(salary) max_salary
FROM instructor
group by dept_name