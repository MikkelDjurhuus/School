DELETE FROM takes
WHERE id in (SELECT ID FROM student WHERE name = 'Chavez')
