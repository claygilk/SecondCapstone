-- restart autoincrement
DBCC CHECKIDENT (project, RESEED, 0)
DBCC CHECKIDENT (department, RESEED, 0)
DBCC CHECKIDENT (employee, RESEED, 0)


DELETE FROM project_employee;
DELETE FROM employee;
DELETE FROM department;
DELETE FROM project;


INSERT INTO department (name) VALUES ('Department of Testing');
INSERT INTO department (name) VALUES ('Network Test Administration');

INSERT INTO project (name, from_date, to_date) VALUES ('Project TestX', '1961-03-01', '2002-08-31');
INSERT INTO project (name, from_date, to_date) VALUES ('Forlorn TestCake', '2010-01-01', '2011-10-15');

INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date, job_title)
VALUES (1, 'Test', 'Keppard', '1953-07-15', '2001-04-01', 'Chief Head Honcho');
INSERT INTO employee (department_id, first_name, last_name, birth_date, hire_date, job_title)
VALUES (2, 'Flo', 'Testerson', '1990-12-28', '2011-08-01', 'Floss Replenisher');

INSERT INTO project_employee (project_id, employee_id) VALUES (1, 2);
INSERT INTO project_employee (project_id, employee_id) VALUES (2, 1);

