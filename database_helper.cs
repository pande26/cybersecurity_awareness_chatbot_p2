using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace cybersecurity_awareness_chatbot_p2
{
    public class database_helper
    {
        // Connection string using SQL LocalDB
        private string connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=cybersecurity_tasks;Integrated Security=True;";

        // Method to test the connection
        public void test_connection()
        {
            SqlConnection connect = new SqlConnection(connection);
            try
            {
                connect.Open();
                MessageBox.Show("Connected to database successfully!");
                connect.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show("Connection error: " + error.Message);
            }
        }

        // Method to drop existing database if it exists (to fix the error)
        public void drop_database_if_exists()
        {
            try
            {
                string masterConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";

                using (SqlConnection conn = new SqlConnection(masterConnection))
                {
                    conn.Open();

                    string dropDb = "IF EXISTS (SELECT name FROM sys.databases WHERE name = 'cybersecurity_tasks') " +
                                    "BEGIN " +
                                    "ALTER DATABASE cybersecurity_tasks SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " +
                                    "DROP DATABASE cybersecurity_tasks " +
                                    "END";

                    SqlCommand cmd = new SqlCommand(dropDb, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Drop database error: " + error.Message);
            }
        }

        // Method to create database and table if they don't exist
        public void ensure_database_setup()
        {
            try
            {
                // First drop existing database to avoid the file exists error
                drop_database_if_exists();

                // First connect to master to create database
                string masterConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";

                using (SqlConnection conn = new SqlConnection(masterConnection))
                {
                    conn.Open();

                    // Create database
                    string createDb = "CREATE DATABASE cybersecurity_tasks";
                    SqlCommand cmd = new SqlCommand(createDb, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                // Now create table in the database
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();

                    string createTable = "CREATE TABLE tasks (" +
                                        "task_id INT PRIMARY KEY IDENTITY(1,1), " +
                                        "task_name VARCHAR(100) NOT NULL, " +
                                        "task_description VARCHAR(200), " +
                                        "task_due_date VARCHAR(20), " +
                                        "task_status VARCHAR(20))";

                    SqlCommand cmd = new SqlCommand(createTable, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Setup error: " + error.Message);
            }
        }

        // Method to insert a task
        public void insert_task(string name, string description, string due_date, string status)
        {
            using (SqlConnection connects = new SqlConnection(connection))
            {
                try
                {
                    connects.Open();

                    string query = "INSERT INTO tasks (task_name, task_description, task_due_date, task_status) " +
                                   "VALUES (@name, @description, @due_date, @status)";

                    SqlCommand run_query = new SqlCommand(query, connects);
                    run_query.Parameters.AddWithValue("@name", name);
                    run_query.Parameters.AddWithValue("@description", description);
                    run_query.Parameters.AddWithValue("@due_date", due_date);
                    run_query.Parameters.AddWithValue("@status", status);

                    run_query.ExecuteNonQuery();
                    connects.Close();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Insert error: " + error.Message);
                }
            }
        }

        // Method to load tasks
        public void load_tasks(ListView view_task)
        {
            SqlConnection connects = new SqlConnection(connection);

            try
            {
                connects.Open();

                string query = "SELECT * FROM tasks ORDER BY task_id DESC";
                SqlCommand run_query = new SqlCommand(query, connects);

                SqlDataReader data_collect = run_query.ExecuteReader();

                bool data_found = false;

                while (data_collect.Read())
                {
                    data_found = true;

                    string task_id = data_collect["task_id"].ToString();
                    string task_name = data_collect["task_name"].ToString();
                    string task_description = data_collect["task_description"].ToString();
                    string task_due_date = data_collect["task_due_date"].ToString();
                    string task_status = data_collect["task_status"].ToString();

                    string displayText = task_id + ". " + task_name;
                    if (!string.IsNullOrEmpty(task_description))
                    {
                        displayText += " - " + task_description;
                    }
                    if (!string.IsNullOrEmpty(task_due_date))
                    {
                        displayText += " (Due: " + task_due_date + ")";
                    }
                    displayText += " [" + task_status + "]";

                    view_task.Items.Add(displayText);
                }

                if (!data_found)
                {
                    view_task.Items.Add("No tasks found.");
                }

                data_collect.Close();
                connects.Close();
            }
            catch (Exception error)
            {
                view_task.Items.Add("Error loading tasks: " + error.Message);
            }
        }

        // Method to delete a task
        public void delete_task(int task_id)
        {
            using (SqlConnection connects = new SqlConnection(connection))
            {
                try
                {
                    connects.Open();

                    string query = "DELETE FROM tasks WHERE task_id = @task_id";
                    SqlCommand run_query = new SqlCommand(query, connects);
                    run_query.Parameters.AddWithValue("@task_id", task_id);

                    run_query.ExecuteNonQuery();
                    connects.Close();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Delete error: " + error.Message);
                }
            }
        }

        // Method to mark task as completed
        public void complete_task(int task_id)
        {
            using (SqlConnection connects = new SqlConnection(connection))
            {
                try
                {
                    connects.Open();

                    string query = "UPDATE tasks SET task_status = 'Completed' WHERE task_id = @task_id";
                    SqlCommand run_query = new SqlCommand(query, connects);
                    run_query.Parameters.AddWithValue("@task_id", task_id);

                    run_query.ExecuteNonQuery();
                    connects.Close();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Update error: " + error.Message);
                }
            }
        }

        // Method to get task by ID
        public SqlDataReader get_task_by_id(int task_id)
        {
            SqlConnection connects = new SqlConnection(connection);

            try
            {
                connects.Open();

                string query = "SELECT * FROM tasks WHERE task_id = @task_id";
                SqlCommand run_query = new SqlCommand(query, connects);
                run_query.Parameters.AddWithValue("@task_id", task_id);

                return run_query.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (Exception)
            {
                connects.Close();
                return null;
            }
        }

        // Method to count pending tasks
        public int count_tasks()
        {
            using (SqlConnection connects = new SqlConnection(connection))
            {
                try
                {
                    connects.Open();

                    string query = "SELECT COUNT(*) FROM tasks WHERE task_status = 'Pending' OR task_status IS NULL";
                    SqlCommand run_query = new SqlCommand(query, connects);

                    int count = (int)run_query.ExecuteScalar();
                    connects.Close();
                    return count;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
    }
}