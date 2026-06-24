-- Drop the database if it exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'cybersecurity_tasks')
BEGIN
    ALTER DATABASE cybersecurity_tasks SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE cybersecurity_tasks;
END
GO

-- Create the database
CREATE DATABASE cybersecurity_tasks;
GO

-- Use the database
USE cybersecurity_tasks;
GO

-- Create the tasks table
CREATE TABLE tasks (
    task_id INT PRIMARY KEY IDENTITY(1,1),
    task_name VARCHAR(100) NOT NULL,
    task_description VARCHAR(200),
    task_due_date VARCHAR(20),
    task_status VARCHAR(20)
);
GO

-- Verify table creation
SELECT * FROM tasks;
GO