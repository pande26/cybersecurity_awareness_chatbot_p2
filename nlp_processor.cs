using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace cybersecurity_awareness_chatbot_p2
{//start of namespace
    public class nlp_processor
    {//start of class
        private ArrayList reply;
        private ArrayList ignore;
        private string username;
        private task_manager taskManager;
        private quiz_manager quizManager;

        public nlp_processor(ArrayList reply_list, ArrayList ignore_list, string user, task_manager task_mgr, quiz_manager quiz_mgr)
        {//start of constructor
            reply = reply_list;
            ignore = ignore_list;
            username = user;
            taskManager = task_mgr;
            quizManager = quiz_mgr;

        }//end of constructor

        public string process_nlp(string user_input, ListView chat_list)
        {//start of process_nlp method
            string lower_input = user_input.ToLower().Trim();

            // Check for activity log command FIRST - return null so MainWindow handles it
            if (contains_keyword(lower_input, new string[] { "show activity log", "what have you done", "summary", "recent actions", "activity log" }))
            {//start of if statement
                return null;

            }//end of if statement

            // Check for task-related commands
            if (contains_keyword(lower_input, new string[] { "add task", "create task", "new task", "add a task", "task to" }))
            {//start of if statement
                return handle_add_task(user_input, chat_list);

            }//end of if statement

            // Check for reminder-related commands
            if (contains_keyword(lower_input, new string[] { "remind me", "set reminder", "remind in", "remind to" }))
            {//start of if statement
                return handle_add_reminder(user_input, chat_list);

            }//end of if statement

            // Check for show tasks command
            if (contains_keyword(lower_input, new string[] { "show tasks", "view tasks", "list tasks", "my tasks", "what tasks" }))
            {//start of if statement
                return handle_show_tasks(chat_list);

            }//end of if statement

            // Check for complete task command
            if (contains_keyword(lower_input, new string[] { "complete task", "finish task", "mark done", "mark complete" }))
            {//start of if statement
                return handle_complete_task(user_input, chat_list);

            }//end of if statement

            // Check for delete task command
            if (contains_keyword(lower_input, new string[] { "delete task", "remove task" }))
            {//start of if statement
                return handle_delete_task(user_input, chat_list);

            }//end of if statement

            // Check for quiz command
            if (contains_keyword(lower_input, new string[] { "start quiz", "begin quiz", "quiz", "take quiz" }))
            {//start of if statement
                return handle_start_quiz(chat_list);

            }//end of if statement

            return null;

        }//end of process_nlp method

        private bool contains_keyword(string input, string[] keywords)
        {//start of contains_keyword method

            foreach (string keyword in keywords)
            {//start of foreach loop
                if (input.Contains(keyword))
                    return true;
            
            }//end of foreach loop

            return false;

        }//end of contains_keyword method

        private string handle_add_task(string user_input, ListView chat_list)
        {//start of handle_add_task method
            string task_name = ExtractTaskName(user_input);

            if (string.IsNullOrEmpty(task_name))
            {//start of if statement
                return "Please specify a task name. Example: 'Add a task to enable two-factor authentication'";

            }//end of if statement

            string result = taskManager.process_task_command("add task " + task_name, chat_list);
            return result;
        }//end of handle_add_task method

        private string handle_add_reminder(string user_input, ListView chat_list)
        {//start of handle_add_reminder method
            string task_name = ExtractTaskName(user_input);

            if (string.IsNullOrEmpty(task_name))
            {//start of if statement
                return "Please specify what you want to be reminded about.";
            
            }//end of if statement

            string result = taskManager.process_task_command("add task " + task_name, chat_list);
            return result;
        }//end of handle_add_reminder method

        private string handle_show_tasks(ListView chat_list)
        {//start of handle_show_tasks method
            string result = taskManager.process_task_command("show tasks", chat_list);
            return result;

        }//end of handle_show_tasks method

        private string handle_complete_task(string user_input, ListView chat_list)
        {//start of handle_complete_task method
            string task_name = ExtractTaskName(user_input);

            if (string.IsNullOrEmpty(task_name))
            {//start of if statement
                return "Please specify which task to complete. Example: 'Complete task enable 2FA'";

            }//end of if statement       

            string result = taskManager.process_task_command("complete task " + task_name, chat_list);
            return result;

        }//end of handle_complete_task method

        private string handle_delete_task(string user_input, ListView chat_list)
        {//start of handle_delete_task method
            string task_name = ExtractTaskName(user_input);

            if (string.IsNullOrEmpty(task_name))
            {//start of if statement
                return "Please specify which task to delete. Example: 'Delete task enable 2FA'";

            }//end of if statement

            string result = taskManager.process_task_command("delete task " + task_name, chat_list);
            return result;

        }//end of handle_delete_task method

        private string handle_start_quiz(ListView chat_list)
        {//start of handle_start_quiz method
            if (quizManager == null)
            {//start of if statement
                return "Quiz manager is not initialized. Please restart the application.";

            }//end of if statement

            string result = quizManager.start_quiz(chat_list);
            return result;

        }//end of handle_start_quiz method

        private string ExtractTaskName(string input)
        {//start of ExtractTaskName method
            string lower_input = input.ToLower();

            // Pattern 1: "add task to X", "create task X", "new task X"
            string[] patterns = { "add task to", "add task", "create task", "new task", "task to" };
            foreach (string pattern in patterns)
            {//start of foreach loop

                if (lower_input.Contains(pattern))
                {//start of if statement
                    int start_index = lower_input.IndexOf(pattern) + pattern.Length;
                    if (start_index < input.Length)
                    {//start of if statement
                        string extracted = input.Substring(start_index).Trim();
                        string[] stop_words = { "to ", "for ", "a ", "an " };
                        foreach (string sw in stop_words)
                        {//start of foreach loop
                            if (extracted.ToLower().StartsWith(sw))
                            {//start of if statement
                                extracted = extracted.Substring(sw.Length).Trim();
                            }//end of if statement
                        }//end of foreach loop
                        return extracted;

                    }//end of if statement

                }//end of if statement

            }//end of foreach loop

            // Pattern 2: "remind me to X", "remind me X", "set reminder for X"
            string[] reminder_patterns = { "remind me to", "remind me", "set reminder for", "set reminder to" };
            foreach (string pattern in reminder_patterns)
            {//start of foreach loop
                if (lower_input.Contains(pattern))
                {//start of if statement
                    int start_index = lower_input.IndexOf(pattern) + pattern.Length;
                    if (start_index < input.Length)
                    {//start of if statement
                        string extracted = input.Substring(start_index).Trim();
                        string[] date_words = { "tomorrow", "today", "in", "days", "day", "next week", "next" };
                        foreach (string dw in date_words)
                        {//start of foreach loop
                            if (extracted.ToLower().Contains(dw))
                            {//start of if statement
                                int pos = extracted.ToLower().IndexOf(dw);
                                extracted = extracted.Substring(0, pos).Trim();
                            }//end of if statement
                        }//end of foreach loop

                        if (!string.IsNullOrEmpty(extracted))
                        {//start of if statement
                            return extracted;

                        }//end of if statement

                    }//end of if statement

                }//end of if statement

            }//end of foreach loop

            // Pattern 3: "complete task X", "finish task X"
            string[] complete_patterns = { "complete task", "finish task", "mark done", "mark complete" };
            foreach (string pattern in complete_patterns)
            {//start of foreach loop

                if (lower_input.Contains(pattern))
                {//start of if statement

                    int start_index = lower_input.IndexOf(pattern) + pattern.Length;
                    if (start_index < input.Length)
                    {//start of if statement
                        return input.Substring(start_index).Trim();

                    }//end of if statement

                }//end of if statement

            }//end of foreach loop

            // Pattern 4: "delete task X", "remove task X"
            string[] delete_patterns = { "delete task", "remove task" };
            foreach (string pattern in delete_patterns)
            {//start of foreach loop

                if (lower_input.Contains(pattern))
                {//start of if statement

                    int start_index = lower_input.IndexOf(pattern) + pattern.Length;
                    if (start_index < input.Length)
                    {//start of if statement
                        return input.Substring(start_index).Trim();

                    }//end of if statement

                }//end of if statement

            }//end of foreach loop

            // If no pattern matches, try to get the last part of the sentence
            string[] words = input.Split(' ');
            if (words.Length > 3)
            {//start of if statement
                return string.Join(" ", words, 3, words.Length - 3).Trim();

            }//end of if statement

            return input;

        }//end of ExtractTaskName method

    }//end of class

}//end of namespace