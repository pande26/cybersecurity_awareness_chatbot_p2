using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cybersecurity_awareness_chatbot_p2
{
    public partial class MainWindow : Window
    {
        // Creating an instance of ArrayList
        ArrayList reply = new ArrayList();
        ArrayList ignore = new ArrayList();

        // Declaring all class instances
        private response_finder finder;
        private response_handler handler;
        private topic_detector detector;
        private message_displayer displayer;
        private sentiment_detector sentimentDetector;
        private task_manager taskManager;
        private quiz_manager quizManager;

        // Variables to store the last detected topic for follow-up questions
        private string last_topic = "";
        private string username = "";

        public MainWindow()
        {
            InitializeComponent();

            new greet_user();
            new respond(reply, ignore) { };

            // Initializing all the class instances
            finder = new response_finder(reply);
            handler = new response_handler(reply, ignore);
            detector = new topic_detector();
            displayer = new message_displayer();
            sentimentDetector = new sentiment_detector(reply, finder);
        }

        private void start_valerie(object sender, RoutedEventArgs e)
        {
            logo_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
        }

        private void submit_username(object sender, RoutedEventArgs e)
        {
            string name = user_name.Text.ToString().Trim();
            bool found = check_name(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter your name before continuing...");
                user_name.Focus();
                return;
            }

            // Store username
            username = name;
            displayer.set_username(name);

            // Initialize task manager
            initialize_task_manager();

            // Initialize quiz manager
            initialize_quiz_manager();

            string filename = "user_name.txt";

            if (!File.Exists(filename))
            {
                File.AppendAllText(filename, "auto_create\n");
            }

            if (!found)
            {
                File.AppendAllText(filename, name + "\n");
                MessageBox.Show("Welcome " + name);
                username_grid.Visibility = Visibility.Hidden;
                chats_grid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Welcome back " + name);
                username_grid.Visibility = Visibility.Hidden;
                chats_grid.Visibility = Visibility.Visible;
            }
        }

        private Boolean check_name(string name)
        {
            string find_name = "user_name.txt";
            bool name_found = false;

            if (File.Exists(find_name))
            {
                string[] names = File.ReadAllLines(find_name);
                foreach (string search_name in names)
                {
                    if (search_name.ToLower() == name.ToLower())
                    {
                        name_found = true;
                        break;
                    }
                }
            }
            return name_found;
        }

        private bool is_follow_up_question(string question)
        {
            string lower = question.ToLower();

            string[] follow_up_phrases = {
                "another tip", "another one", "more tips", "tell me more",
                "explain more", "elaborate", "continue", "more information",
                "what else", "anything else", "next tip", "another"
            };

            foreach (string phrase in follow_up_phrases)
            {
                if (lower.Contains(phrase))
                    return true;
            }
            return false;
        }

        private void send(object sender, RoutedEventArgs e)
        {
            string questions = question.Text.ToString().Trim();

            if (string.IsNullOrEmpty(questions))
            {
                error_method();
                question.Text = "";
                return;
            }

            // Checking for sentiment in the user's question
            string sentimentResult = sentimentDetector.process_sentiment(questions);

            if (!string.IsNullOrEmpty(sentimentResult))
            {
                display_user_message(questions);
                display_bot_message(sentimentResult);
                question.Text = "";
                return;
            }

            // Checking for follow-up question and if a topic was previously detected
            if (is_follow_up_question(questions) && !string.IsNullOrEmpty(last_topic))
            {
                string follow_up_response = "";

                if (last_topic == "password")
                {
                    follow_up_response = finder.get_response_for_topic("password");
                }
                else if (last_topic == "scam")
                {
                    follow_up_response = finder.get_response_for_topic("scam");
                }
                else if (last_topic == "privacy")
                {
                    follow_up_response = finder.get_response_for_topic("privacy");
                }
                else if (last_topic == "phishing")
                {
                    follow_up_response = finder.get_response_for_topic("phishing");
                }

                if (!string.IsNullOrEmpty(follow_up_response))
                {
                    display_user_message(questions);
                    display_bot_message(follow_up_response);
                    question.Text = "";
                    return;
                }
            }

            // Split the question into words and search for matches in the reply list
            string[] words = questions.Split(' ');
            bool found = false;
            string message = "";
            Random indexer = new Random();
            ArrayList per_word = new ArrayList();
            ArrayList answers_found = new ArrayList();

            foreach (string word in words)
            {
                if (!ignore.Contains(word.ToLower()))
                {
                    per_word.Clear();

                    foreach (string answer in reply)
                    {
                        if (answer.ToLower().Contains(word.ToLower()))
                        {
                            found = true;
                            per_word.Add(answer);
                        }
                    }

                    if (found && per_word.Count > 0)
                    {
                        int indexing = indexer.Next(0, per_word.Count);
                        answers_found.Add(per_word[indexing]);
                        found = false;
                    }
                }
            }

            if (answers_found.Count > 0)
            {
                foreach (string per_answer in answers_found)
                {
                    int space_index = per_answer.IndexOf(' ');
                    if (space_index > 0)
                        message += per_answer.Substring(space_index + 1) + "\n";
                    else
                        message += per_answer + "\n";

                    string lower_answer = per_answer.ToLower();
                    if (lower_answer.StartsWith("password"))
                        last_topic = "password";
                    else if (lower_answer.StartsWith("scam"))
                        last_topic = "scam";
                    else if (lower_answer.StartsWith("privacy"))
                        last_topic = "privacy";
                    else if (lower_answer.StartsWith("phishing"))
                        last_topic = "phishing";
                }

                display_user_message(questions);
                display_bot_message(message.TrimEnd('\n'));
            }
            else
            {
                string[] fallback_messages = {
                    "I'm sorry, I don't understand that. Could you rephrase your question?",
                    "I didn't quite get that. Try asking about passwords, scams, or privacy!",
                    "Hmm, I'm not sure how to respond to that. Can you ask something else?"
                };
                Random random = new Random();
                string fallback_message = fallback_messages[random.Next(fallback_messages.Length)];

                display_user_message(questions);
                display_bot_message(fallback_message);
            }

            question.Text = "";
        }

        private void display_user_message(string message)
        {
            string display_name = string.IsNullOrEmpty(username) ? "You" : username;

            chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = display_name + ": ", Foreground = Brushes.DarkGreen, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Black }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
        }

        private void display_bot_message(string message)
        {
            chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Black }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
        }

        private void error_method()
        {
            chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = "Please enter a question!!", Foreground = Brushes.Red }
                }
            });
        }

        // ========== TASK MANAGER METHODS ==========

        private void initialize_task_manager()
        {
            taskManager = new task_manager();
            int pendingCount = taskManager.get_pending_task_count();
            if (pendingCount > 0)
            {
                displayer.show_bot_message(chats, "You have " + pendingCount + " pending tasks. Say 'Show my tasks' to view them.");
            }
        }

        private void open_task_grid()
        {
            logo_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Hidden;
            chats_grid.Visibility = Visibility.Hidden;
            task_grid.Visibility = Visibility.Visible;

            task_chats.Items.Clear();
            show_task_bot_message("Welcome to Task Manager");
            show_task_bot_message("You can say:");
            show_task_bot_message("- Add task to [task name] - Create a new task");
            show_task_bot_message("- Show my tasks - View all tasks");
            show_task_bot_message("- Complete task [task name] - Mark task as done");
            show_task_bot_message("- Delete task [task name] - Remove a task");

            update_task_stats();
        }

        private void close_task_grid(object sender, RoutedEventArgs e)
        {
            task_grid.Visibility = Visibility.Hidden;
            chats_grid.Visibility = Visibility.Visible;
        }

        private void close_view_task_grid(object sender, RoutedEventArgs e)
        {
            viewTask_grid.Visibility = Visibility.Hidden;
            task_grid.Visibility = Visibility.Visible;
        }

        private void send_task_command(object sender, RoutedEventArgs e)
        {
            string userInput = task_question.Text.ToString().Trim();

            if (string.IsNullOrEmpty(userInput))
            {
                show_task_error_message("Please enter a command.");
                task_question.Text = "";
                return;
            }

            show_task_user_message(userInput);
            string result = taskManager.process_task_command(userInput, task_chats);

            if (result != null)
            {
                show_task_bot_message(result);
            }
            else
            {
                show_task_bot_message("I didn't understand that. Please try one of the suggested commands.");
            }

            task_question.Text = "";
            update_task_stats();
        }

        private void view_all_tasks(object sender, RoutedEventArgs e)
        {
            task_grid.Visibility = Visibility.Hidden;
            viewTask_grid.Visibility = Visibility.Visible;
            view_tasks.Items.Clear();
            taskManager.load_tasks_for_view(view_tasks);
        }

        private void manage_task(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (view_tasks.SelectedItem == null)
            {
                MessageBox.Show("Please select a task first.");
                return;
            }

            string selectedTask = view_tasks.SelectedItem.ToString();
            string result = taskManager.process_task_click(selectedTask);
            MessageBox.Show(result, "Task Update", MessageBoxButton.OK, MessageBoxImage.Information);

            view_tasks.Items.Clear();
            taskManager.load_tasks_for_view(view_tasks);
            update_task_stats();
        }

        private void back_to_chats(object sender, RoutedEventArgs e)
        {
            task_grid.Visibility = Visibility.Visible;
            viewTask_grid.Visibility = Visibility.Hidden;
            update_task_stats();
        }

        private void open_task_manager(object sender, RoutedEventArgs e)
        {
            open_task_grid();
        }

        // Helper methods for task grid
        private void show_task_user_message(string message)
        {
            task_chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "You: ", Foreground = Brushes.DarkGreen, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Black }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            task_chats.ScrollIntoView(task_chats.Items[task_chats.Items.Count - 1]);
        }

        private void show_task_bot_message(string message)
        {
            task_chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Black }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            task_chats.ScrollIntoView(task_chats.Items[task_chats.Items.Count - 1]);
        }

        private void show_task_error_message(string message)
        {
            task_chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Red }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            task_chats.ScrollIntoView(task_chats.Items[task_chats.Items.Count - 1]);
        }

        private void update_task_stats()
        {
            if (taskManager != null)
            {
                int pendingCount = taskManager.get_pending_task_count();
                task_stats.Text = "Tasks: " + pendingCount + " pending";
            }
        }

        // ========== QUIZ MANAGER METHODS ==========

        private void initialize_quiz_manager()
        {
            quizManager = new quiz_manager(username);
        }

        private void open_quiz_grid()
        {
            logo_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Hidden;
            chats_grid.Visibility = Visibility.Hidden;
            task_grid.Visibility = Visibility.Hidden;
            viewTask_grid.Visibility = Visibility.Hidden;
            quiz_grid.Visibility = Visibility.Visible;

            quiz_chats.Items.Clear();
            show_quiz_bot_message("Welcome to the Cybersecurity Quiz!");
            show_quiz_bot_message("Type 'Start quiz' to begin testing your cybersecurity knowledge.");
            show_quiz_bot_message("You will answer " + quizManager.get_total_questions() + " questions.");
            show_quiz_bot_message("Good luck!");
        }

        private void close_quiz_grid(object sender, RoutedEventArgs e)
        {
            quiz_grid.Visibility = Visibility.Hidden;
            chats_grid.Visibility = Visibility.Visible;
        }

        private void send_quiz_answer(object sender, RoutedEventArgs e)
        {
            string userInput = quiz_question.Text.ToString().Trim();

            if (string.IsNullOrEmpty(userInput))
            {
                show_quiz_error_message("Please enter an answer.");
                quiz_question.Text = "";
                return;
            }

            show_quiz_user_message(userInput);

            // Check if quiz is active
            if (!quizManager.is_quiz_active())
            {
                if (userInput.ToLower().Contains("start quiz") || userInput.ToLower().Contains("start"))
                {
                    string startMessage = quizManager.start_quiz(quiz_chats);
                    show_quiz_bot_message(startMessage);
                    update_quiz_score();
                }
                else
                {
                    show_quiz_bot_message("The quiz is not active. Type 'Start quiz' to begin.");
                }
            }
            else
            {
                // Process answer
                string result = quizManager.process_answer(userInput, quiz_chats);
                show_quiz_bot_message(result);
                update_quiz_score();

                // Check if quiz is complete
                if (!quizManager.is_quiz_active())
                {
                    show_quiz_bot_message("Quiz complete! You can start another quiz by typing 'Start quiz'.");
                }
            }

            quiz_question.Text = "";
        }

        private void end_quiz(object sender, RoutedEventArgs e)
        {
            if (quizManager.is_quiz_active())
            {
                quizManager = new quiz_manager(username);
                show_quiz_bot_message("Quiz ended. Type 'Start quiz' to begin a new one.");
                update_quiz_score();
            }
            else
            {
                show_quiz_bot_message("No quiz is currently active. Type 'Start quiz' to begin.");
            }
        }

        private void open_quiz(object sender, RoutedEventArgs e)
        {
            // Initialize quiz manager if not already done
            if (quizManager == null)
            {
                initialize_quiz_manager();
            }
            open_quiz_grid();
        }

        // Helper methods for quiz grid
        private void show_quiz_user_message(string message)
        {
            quiz_chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "You: ", Foreground = Brushes.DarkGreen, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Black }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            quiz_chats.ScrollIntoView(quiz_chats.Items[quiz_chats.Items.Count - 1]);
        }

        private void show_quiz_bot_message(string message)
        {
            quiz_chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Black }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            quiz_chats.ScrollIntoView(quiz_chats.Items[quiz_chats.Items.Count - 1]);
        }

        private void show_quiz_error_message(string message)
        {
            quiz_chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Red }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            quiz_chats.ScrollIntoView(quiz_chats.Items[quiz_chats.Items.Count - 1]);
        }

        private void update_quiz_score()
        {
            if (quizManager != null)
            {
                int current = quizManager.get_current_question_index();
                int total = quizManager.get_total_questions();
                quiz_score.Text = "Score: " + current + "/" + total;
                quiz_progress.Text = "Question " + (current + 1) + " of " + total;
            }
        }
    }
}