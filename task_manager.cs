using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace cybersecurity_awareness_chatbot_p2
{
    public class task_manager
    {
        private database_helper dbHelper;

        // Variables to hold task details
        private string task_name = "";
        private string task_description = "";
        private string task_due_date = "";
        private string task_status = "Pending";
        private bool waiting_for_reminder = false;
        private string pending_task_name = "";

        public task_manager()
        {
            dbHelper = new database_helper();
            dbHelper.ensure_database_setup();
        }

        // Main method to process task commands
        public string process_task_command(string user_input, ListView chat_list)
        {
            // Check if we're waiting for a reminder response
            if (waiting_for_reminder)
            {
                return process_reminder_response(user_input, chat_list);
            }

            string lowerInput = user_input.ToLower().Trim();

            // Check for "add task" command
            if (lowerInput.StartsWith("add task") || lowerInput.StartsWith("create task") ||
                lowerInput.StartsWith("new task") || lowerInput.Contains("add a task"))
            {
                return handle_add_task(user_input, chat_list);
            }

            // Check for "show tasks" command
            if (lowerInput.Contains("show tasks") || lowerInput.Contains("view tasks") ||
                lowerInput.Contains("list tasks") || lowerInput.Contains("my tasks"))
            {
                return handle_show_tasks(chat_list);
            }

            // Check for "complete task" command
            if (lowerInput.Contains("complete task") || lowerInput.Contains("finish task") ||
                lowerInput.Contains("mark done") || lowerInput.Contains("mark complete"))
            {
                return handle_complete_task(user_input, chat_list);
            }

            // Check for "delete task" command
            if (lowerInput.Contains("delete task") || lowerInput.Contains("remove task"))
            {
                return handle_delete_task(user_input, chat_list);
            }

            // Check for "remind me" command
            if (lowerInput.Contains("remind me") || lowerInput.Contains("set reminder"))
            {
                return handle_add_reminder(user_input, chat_list);
            }

            return null;
        }

        // Handle adding a task
        private string handle_add_task(string user_input, ListView chat_list)
        {
            // Extract task name
            task_name = user_input;

            string[] prefixes = { "add task", "create task", "new task", "add a task" };
            foreach (string prefix in prefixes)
            {
                if (user_input.ToLower().StartsWith(prefix))
                {
                    task_name = user_input.Substring(prefix.Length).Trim();
                    break;
                }
            }

            if (string.IsNullOrEmpty(task_name))
            {
                return "Please specify a task name. Example: 'Add task to enable two-factor authentication'";
            }

            waiting_for_reminder = true;
            pending_task_name = task_name;
            task_description = "No description provided";

            return "Task '" + task_name + "' added. Would you like a reminder? Type 'Yes, remind me in X days' or 'No'.";
        }

        // Process reminder response
        private string process_reminder_response(string user_input, ListView chat_list)
        {
            string lowerInput = user_input.ToLower().Trim();

            if (lowerInput.StartsWith("no"))
            {
                dbHelper.insert_task(pending_task_name, task_description, "No reminder", "Pending");
                waiting_for_reminder = false;
                string name = pending_task_name;
                pending_task_name = "";
                return "Task '" + name + "' saved. You can view it by saying 'Show my tasks'.";
            }

            if (lowerInput.Contains("remind me in") || lowerInput.Contains("remind in") ||
                lowerInput.StartsWith("yes"))
            {
                string days_number = Regex.Replace(user_input, @"[^0-9]", "");

                if (string.IsNullOrEmpty(days_number))
                {
                    return "I couldn't understand how many days. Please say 'Yes, remind me in 5 days' or 'No'.";
                }

                int days = int.Parse(days_number);
                DateTime reminder_date = DateTime.Now.AddDays(days);
                string format_date = reminder_date.ToString("MMMM dd yyyy");

                task_due_date = format_date;
                task_status = "Pending";

                dbHelper.insert_task(pending_task_name, task_description, task_due_date, task_status);

                string name = pending_task_name;
                waiting_for_reminder = false;
                pending_task_name = "";

                return "I'll remind you in " + days + " days, on " + format_date +
                       ". Task '" + name + "' has been saved.";
            }

            return "I didn't understand that. Please say 'Yes, remind me in X days' or 'No'.";
        }

        // Handle showing tasks
        private string handle_show_tasks(ListView chat_list)
        {
            dbHelper.load_tasks(chat_list);

            int count = dbHelper.count_tasks();
            if (count > 0)
            {
                return "You have " + count + " pending task(s). To complete a task, say 'Complete task [task name]'.";
            }
            else
            {
                return "You don't have any pending tasks. Say 'Add task to...' to create one.";
            }
        }

        // Handle completing a task
        private string handle_complete_task(string user_input, ListView chat_list)
        {
            string task_search = user_input;
            string[] prefixes = { "complete task", "finish task", "mark done", "mark complete" };

            foreach (string prefix in prefixes)
            {
                if (user_input.ToLower().Contains(prefix))
                {
                    task_search = user_input.Substring(user_input.ToLower().IndexOf(prefix) + prefix.Length).Trim();
                    break;
                }
            }

            if (string.IsNullOrEmpty(task_search))
            {
                return "Please specify which task to complete. Example: 'Complete task enable 2FA'";
            }

            return "To complete a task, please use the 'View Tasks' button and double-click on the task.";
        }

        // Handle deleting a task
        private string handle_delete_task(string user_input, ListView chat_list)
        {
            string task_search = user_input;
            string[] prefixes = { "delete task", "remove task" };

            foreach (string prefix in prefixes)
            {
                if (user_input.ToLower().Contains(prefix))
                {
                    task_search = user_input.Substring(user_input.ToLower().IndexOf(prefix) + prefix.Length).Trim();
                    break;
                }
            }

            if (string.IsNullOrEmpty(task_search))
            {
                return "Please specify which task to delete. Example: 'Delete task enable 2FA'";
            }

            return "To delete a task, please use the 'View Tasks' button and double-click on the task.";
        }

        // Handle adding a reminder
        private string handle_add_reminder(string user_input, ListView chat_list)
        {
            return "Please specify a task and reminder date. Example: 'Remind me to update password in 7 days'";
        }

        // Method to get pending task count
        public int get_pending_task_count()
        {
            return dbHelper.count_tasks();
        }

        // Method to load tasks for the view grid
        public void load_tasks_for_view(ListView view_tasks)
        {
            view_tasks.Items.Clear();
            dbHelper.load_tasks(view_tasks);
        }

        // Method to get task ID from display text
        public int get_task_id_from_display(string display_text)
        {
            try
            {
                string[] parts = display_text.Split('.');
                if (parts.Length > 0)
                {
                    return int.Parse(parts[0].Trim());
                }
            }
            catch (Exception)
            {
                return -1;
            }
            return -1;
        }

        // Method to process double-click on task
        public string process_task_click(string selected_item)
        {
            if (string.IsNullOrEmpty(selected_item) || selected_item.StartsWith("No tasks found"))
            {
                return "No task selected.";
            }

            int task_id = get_task_id_from_display(selected_item);
            if (task_id == -1)
            {
                return "Could not identify the task.";
            }

            using (var reader = dbHelper.get_task_by_id(task_id))
            {
                if (reader != null && reader.Read())
                {
                    string status = reader["task_status"].ToString();
                    reader.Close();

                    if (status == "Completed")
                    {
                        dbHelper.delete_task(task_id);
                        return "Task already completed. It has been removed from your list.";
                    }
                    else
                    {
                        dbHelper.complete_task(task_id);
                        return "Task marked as completed.";
                    }
                }
                else
                {
                    if (reader != null) reader.Close();
                    return "Task not found.";
                }
            }
        }
    }
}