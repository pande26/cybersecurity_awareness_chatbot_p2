using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace cybersecurity_awareness_chatbot_p2
{
    public class nlp_processor
    {
        private ArrayList reply;
        private ArrayList ignore;
        private string username;
        private task_manager taskManager;
        private quiz_manager quizManager;

        public nlp_processor(ArrayList reply_list, ArrayList ignore_list, string user, task_manager task_mgr, quiz_manager quiz_mgr)
        {
            reply = reply_list;
            ignore = ignore_list;
            username = user;
            taskManager = task_mgr;
            quizManager = quiz_mgr;
        }

        public string process_nlp(string user_input, ListView chat_list)
        {
            string lower_input = user_input.ToLower().Trim();

            // Check for task-related commands
            if (contains_keyword(lower_input, new string[] { "add task", "create task", "new task", "add a task", "task to" }))
            {
                return handle_add_task(user_input, chat_list);
            }

            // Check for reminder-related commands
            if (contains_keyword(lower_input, new string[] { "remind me", "set reminder", "remind in", "remind to" }))
            {
                return handle_add_reminder(user_input, chat_list);
            }

            // Check for show tasks command
            if (contains_keyword(lower_input, new string[] { "show tasks", "view tasks", "list tasks", "my tasks", "what tasks" }))
            {
                return handle_show_tasks(chat_list);
            }

            // Check for complete task command
            if (contains_keyword(lower_input, new string[] { "complete task", "finish task", "mark done", "mark complete" }))
            {
                return handle_complete_task(user_input, chat_list);
            }

            // Check for delete task command
            if (contains_keyword(lower_input, new string[] { "delete task", "remove task" }))
            {
                return handle_delete_task(user_input, chat_list);
            }

            // Check for quiz command
            if (contains_keyword(lower_input, new string[] { "start quiz", "begin quiz", "quiz", "take quiz" }))
            {
                return handle_start_quiz(chat_list);
            }

            // Check for activity log command
            if (contains_keyword(lower_input, new string[] { "show activity log", "what have you done", "summary", "recent actions" }))
            {
                return "Activity log feature coming soon.";
            }

            return null;
        }

        private bool contains_keyword(string input, string[] keywords)
        {
            foreach (string keyword in keywords)
            {
                if (input.Contains(keyword))
                    return true;
            }
            return false;
        }

        private string handle_add_task(string user_input, ListView chat_list)
        {
            // Extract task name using Regex
            string task_name = ExtractTaskName(user_input);

            if (string.IsNullOrEmpty(task_name))
            {
                return "Please specify a task name. Example: 'Add a task to enable two-factor authentication'";
            }

            // Check if task is already in progress
            string result = taskManager.process_task_command("add task " + task_name, chat_list);
            return result;
        }

        private string handle_add_reminder(string user_input, ListView chat_list)
        {
            // Extract task name and days
            string task_name = ExtractTaskName(user_input);

            if (string.IsNullOrEmpty(task_name))
            {
                return "Please specify what you want to be reminded about.";
            }

            // Extract number of days
            string days_number = Regex.Replace(user_input, @"[^0-9]", "");
            int days = 0;
            if (!string.IsNullOrEmpty(days_number))
            {
                days = int.Parse(days_number);
            }
            else if (user_input.ToLower().Contains("tomorrow"))
            {
                days = 1;
            }
            else if (user_input.ToLower().Contains("next week"))
            {
                days = 7;
            }

            if (days > 0)
            {
                string result = taskManager.process_task_command("add task " + task_name, chat_list);
                // The task_manager will ask for reminder confirmation
                return result;
            }
            else
            {
                string result = taskManager.process_task_command("add task " + task_name, chat_list);
                return result;
            }
        }

        private string handle_show_tasks(ListView chat_list)
        {
            string result = taskManager.process_task_command("show tasks", chat_list);
            return result;
        }

        private string handle_complete_task(string user_input, ListView chat_list)
        {
            string task_name = ExtractTaskName(user_input);

            if (string.IsNullOrEmpty(task_name))
            {
                return "Please specify which task to complete. Example: 'Complete task enable 2FA'";
            }

            string result = taskManager.process_task_command("complete task " + task_name, chat_list);
            return result;
        }

        private string handle_delete_task(string user_input, ListView chat_list)
        {
            string task_name = ExtractTaskName(user_input);

            if (string.IsNullOrEmpty(task_name))
            {
                return "Please specify which task to delete. Example: 'Delete task enable 2FA'";
            }

            string result = taskManager.process_task_command("delete task " + task_name, chat_list);
            return result;
        }

        private string handle_start_quiz(ListView chat_list)
        {
            if (quizManager == null)
            {
                return "Quiz manager is not initialized. Please restart the application.";
            }

            string result = quizManager.start_quiz(chat_list);
            return result;
        }

        private string ExtractTaskName(string input)
        {
            // Try to extract task name using various patterns
            string lower_input = input.ToLower();

            // Pattern 1: "add task to X", "create task X", "new task X"
            string[] patterns = { "add task to", "add task", "create task", "new task", "task to" };
            foreach (string pattern in patterns)
            {
                if (lower_input.Contains(pattern))
                {
                    int start_index = lower_input.IndexOf(pattern) + pattern.Length;
                    if (start_index < input.Length)
                    {
                        string extracted = input.Substring(start_index).Trim();
                        // Remove common stop words at the start
                        string[] stop_words = { "to ", "for ", "a ", "an " };
                        foreach (string sw in stop_words)
                        {
                            if (extracted.ToLower().StartsWith(sw))
                            {
                                extracted = extracted.Substring(sw.Length).Trim();
                            }
                        }
                        return extracted;
                    }
                }
            }

            // Pattern 2: "remind me to X", "remind me X", "set reminder for X"
            string[] reminder_patterns = { "remind me to", "remind me", "set reminder for", "set reminder to" };
            foreach (string pattern in reminder_patterns)
            {
                if (lower_input.Contains(pattern))
                {
                    int start_index = lower_input.IndexOf(pattern) + pattern.Length;
                    if (start_index < input.Length)
                    {
                        string extracted = input.Substring(start_index).Trim();
                        // Remove date/time words
                        string[] date_words = { "tomorrow", "today", "in", "days", "day", "next week", "next" };
                        foreach (string dw in date_words)
                        {
                            if (extracted.ToLower().Contains(dw))
                            {
                                int pos = extracted.ToLower().IndexOf(dw);
                                extracted = extracted.Substring(0, pos).Trim();
                            }
                        }
                        if (!string.IsNullOrEmpty(extracted))
                        {
                            return extracted;
                        }
                    }
                }
            }

            // Pattern 3: "complete task X", "finish task X"
            string[] complete_patterns = { "complete task", "finish task", "mark done", "mark complete" };
            foreach (string pattern in complete_patterns)
            {
                if (lower_input.Contains(pattern))
                {
                    int start_index = lower_input.IndexOf(pattern) + pattern.Length;
                    if (start_index < input.Length)
                    {
                        return input.Substring(start_index).Trim();
                    }
                }
            }

            // Pattern 4: "delete task X", "remove task X"
            string[] delete_patterns = { "delete task", "remove task" };
            foreach (string pattern in delete_patterns)
            {
                if (lower_input.Contains(pattern))
                {
                    int start_index = lower_input.IndexOf(pattern) + pattern.Length;
                    if (start_index < input.Length)
                    {
                        return input.Substring(start_index).Trim();
                    }
                }
            }

            // If no pattern matches, try to get the last part of the sentence
            string[] words = input.Split(' ');
            if (words.Length > 3)
            {
                // Return everything after the first 3 words
                return string.Join(" ", words, 3, words.Length - 3).Trim();
            }

            return input;
        }
    }
}