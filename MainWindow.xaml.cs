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
{//start of namespace

    public partial class MainWindow : Window
    {//start of class

        // Creating an instance of ArrayList to store chatbot responses
        ArrayList reply = new ArrayList();
        // Creating an instance of ArrayList to store words to ignore
        ArrayList ignore = new ArrayList();

        // Declaring all class instances for various features
        private response_finder finder;           // Finds responses for specific topics
        private response_handler handler;         // Handles response processing logic
        private topic_detector detector;          // Detects cybersecurity topics
        private message_displayer displayer;      // Displays formatted messages
        private sentiment_detector sentimentDetector; // Detects user sentiment
        private task_manager taskManager;         // Manages tasks
        private quiz_manager quizManager;         // Manages quiz functionality
        private nlp_processor nlpProcessor;       // Processes natural language commands
        private activity_logger logger;           // Logs user activities

        // Variables to store the last detected topic for follow-up questions
        private string last_topic = "";
        private string username = "";

        public MainWindow()
        {//start of constructor

            InitializeComponent();

            // Play voice greeting when application starts
            new greet_user();
            // Load responses and ignore words from respond class
            new respond(reply, ignore) { };

            // Initializing all the class instances
            finder = new response_finder(reply);
            handler = new response_handler(reply, ignore);
            detector = new topic_detector();
            displayer = new message_displayer();
            sentimentDetector = new sentiment_detector(reply, finder);

        }//end of constructor

        // Event handler for the Start Valerie button - shows username grid
        private void start_valerie(object sender, RoutedEventArgs e)
        {//start of method

            logo_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;

        }//end of method

        // Event handler for username submission - validates and stores username
        private void submit_username(object sender, RoutedEventArgs e)
        {//start of method

            string name = user_name.Text.ToString().Trim();
            bool found = check_name(name);

            // Validate that name is not empty
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter your name before continuing...");
                user_name.Focus();
                return;
            }

            // Store username
            username = name;
            displayer.set_username(name);

            // Initialize all feature managers
            initialize_task_manager();
            initialize_quiz_manager();
            initialize_nlp_processor();
            initialize_activity_logger();

            string filename = "user_name.txt";

            // Create file if it doesn't exist
            if (!File.Exists(filename))
            {
                File.AppendAllText(filename, "auto_create\n");
            }

            // Check if user is new or returning
            if (!found)
            {
                // New user - save name and show welcome
                File.AppendAllText(filename, name + "\n");
                MessageBox.Show("Welcome " + name);
                username_grid.Visibility = Visibility.Hidden;
                chats_grid.Visibility = Visibility.Visible;

                // Log welcome
                logger.add_log("New user registered: " + name);
            }
            else
            {
                // Returning user - show welcome back
                MessageBox.Show("Welcome back " + name);
                username_grid.Visibility = Visibility.Hidden;
                chats_grid.Visibility = Visibility.Visible;

                // Log returning user
                logger.add_log("Returning user: " + name);
            }

        }//end of method

        // Method to check if user name already exists in the file
        private Boolean check_name(string name)
        {//start of method

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

        }//end of method

        // Method to detect if user is asking a follow-up question
        private bool is_follow_up_question(string question)
        {//start of method

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

        }//end of method

        // Main send method - processes user input and generates response
        private void send(object sender, RoutedEventArgs e)
        {//start of method

            string questions = question.Text.ToString().Trim();

            // Check for empty input
            if (string.IsNullOrEmpty(questions))
            {
                error_method();
                question.Text = "";
                return;
            }

            // LOG: User interaction
            logger.add_log("User asked: " + questions);

            // CHECK FOR SENTIMENT - First priority
            string sentimentResult = sentimentDetector.process_sentiment(questions);

            if (!string.IsNullOrEmpty(sentimentResult))
            {
                display_user_message(questions);
                display_bot_message(sentimentResult);
                logger.add_log("Sentiment detected and responded");
                question.Text = "";
                return;
            }

            // CHECK FOR NLP COMMANDS - Second priority
            if (nlpProcessor == null)
            {
                initialize_nlp_processor();
            }

            string nlpResult = nlpProcessor.process_nlp(questions, chats);

            if (!string.IsNullOrEmpty(nlpResult))
            {
                display_user_message(questions);
                display_bot_message(nlpResult);
                logger.add_log("NLP command processed: " + questions);
                question.Text = "";
                return;
            }

            // CHECK FOR ACTIVITY LOG COMMAND
            if (questions.ToLower().Contains("show activity log") ||
                questions.ToLower().Contains("what have you done") ||
                questions.ToLower().Contains("summary") ||
                questions.ToLower().Contains("recent actions"))
            {
                string log_result = logger.get_recent_activity();
                display_user_message(questions);
                display_bot_message(log_result);
                logger.add_log("User viewed activity log");
                question.Text = "";
                return;
            }

            // CHECK FOR FOLLOW-UP QUESTIONS
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
                    logger.add_log("Follow-up response given for topic: " + last_topic);
                    question.Text = "";
                    return;
                }
            }

            // REGULAR RESPONSE PROCESSING - Split question into words and search for matches
            string[] words = questions.Split(' ');
            bool found = false;
            string message = "";
            Random indexer = new Random();
            ArrayList per_word = new ArrayList();
            ArrayList answers_found = new ArrayList();

            foreach (string word in words)
            {
                // Skip ignored words
                if (!ignore.Contains(word.ToLower()))
                {
                    per_word.Clear();

                    // Search for matching answers in reply list
                    foreach (string answer in reply)
                    {
                        if (answer.ToLower().Contains(word.ToLower()))
                        {
                            found = true;
                            per_word.Add(answer);
                        }
                    }

                    // If matches found, pick one randomly
                    if (found && per_word.Count > 0)
                    {
                        int indexing = indexer.Next(0, per_word.Count);
                        answers_found.Add(per_word[indexing]);
                        found = false;
                    }
                }
            }

            // Display found answers
            if (answers_found.Count > 0)
            {
                foreach (string per_answer in answers_found)
                {
                    // Extract response without the topic prefix
                    int space_index = per_answer.IndexOf(' ');
                    if (space_index > 0)
                        message += per_answer.Substring(space_index + 1) + "\n";
                    else
                        message += per_answer + "\n";

                    // Store the topic for follow-ups
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
                logger.add_log("Regular response given for topic: " + last_topic);
            }
            else
            {
                // Fallback responses when nothing matches
                string[] fallback_messages = {
                    "I'm sorry, I don't understand that. Could you rephrase your question?",
                    "I didn't quite get that. Try asking about passwords, scams, or privacy!",
                    "Hmm, I'm not sure how to respond to that. Can you ask something else?"
                };
                Random random = new Random();
                string fallback_message = fallback_messages[random.Next(fallback_messages.Length)];

                display_user_message(questions);
                display_bot_message(fallback_message);
                logger.add_log("Fallback response given for unrecognized input");
            }

            question.Text = "";

        }//end of method

        // Helper method to display user message in chat
        private void display_user_message(string message)
        {//start of method

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

        }//end of method

        // Helper method to display bot message in chat
        private void display_bot_message(string message)
        {//start of method

            chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Black }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);

        }//end of method

        // Error method for empty input
        private void error_method()
        {//start of method

            chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = "Please enter a question!!", Foreground = Brushes.Red }
                }
            });

        }//end of method

        // ========== TASK MANAGER METHODS ==========

        // Initialize the task manager
        private void initialize_task_manager()
        {//start of method

            taskManager = new task_manager();
            int pendingCount = taskManager.get_pending_task_count();
            if (pendingCount > 0)
            {
                displayer.show_bot_message(chats, "You have " + pendingCount + " pending tasks. Say 'Show my tasks' to view them.");
            }

        }//end of method

        // Open the task manager grid
        private void open_task_grid()
        {//start of method

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

        }//end of method

        // Close the task manager grid
        private void close_task_grid(object sender, RoutedEventArgs e)
        {//start of method

            task_grid.Visibility = Visibility.Hidden;
            chats_grid.Visibility = Visibility.Visible;

        }//end of method

        // Close the view tasks grid
        private void close_view_task_grid(object sender, RoutedEventArgs e)
        {//start of method

            viewTask_grid.Visibility = Visibility.Hidden;
            task_grid.Visibility = Visibility.Visible;

        }//end of method

        // Send task command from task manager
        private void send_task_command(object sender, RoutedEventArgs e)
        {//start of method

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

                // Log task action
                if (userInput.ToLower().Contains("add task") || userInput.ToLower().Contains("create task"))
                {
                    logger.add_log("Task added: " + userInput);
                }
                else if (userInput.ToLower().Contains("complete task") || userInput.ToLower().Contains("mark done"))
                {
                    logger.add_log("Task completed: " + userInput);
                }
                else if (userInput.ToLower().Contains("delete task") || userInput.ToLower().Contains("remove task"))
                {
                    logger.add_log("Task deleted: " + userInput);
                }
                else if (userInput.ToLower().Contains("show tasks") || userInput.ToLower().Contains("view tasks"))
                {
                    logger.add_log("User viewed tasks");
                }
            }
            else
            {
                show_task_bot_message("I didn't understand that. Please try one of the suggested commands.");
            }

            task_question.Text = "";
            update_task_stats();

        }//end of method

        // View all tasks - opens the view tasks grid
        private void view_all_tasks(object sender, RoutedEventArgs e)
        {//start of method

            task_grid.Visibility = Visibility.Hidden;
            viewTask_grid.Visibility = Visibility.Visible;
            view_tasks.Items.Clear();
            taskManager.load_tasks_for_view(view_tasks);

        }//end of method

        // Handle double-click on task to manage it
        private void manage_task(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {//start of method

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

        }//end of method

        // Go back to task manager from view tasks
        private void back_to_chats(object sender, RoutedEventArgs e)
        {//start of method

            task_grid.Visibility = Visibility.Visible;
            viewTask_grid.Visibility = Visibility.Hidden;
            update_task_stats();

        }//end of method

        // Open task manager from main chat
        private void open_task_manager(object sender, RoutedEventArgs e)
        {//start of method

            open_task_grid();

        }//end of method

        // Helper methods for task grid display
        private void show_task_user_message(string message)
        {//start of method

            task_chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "You: ", Foreground = Brushes.DarkGreen, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Black }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            task_chats.ScrollIntoView(task_chats.Items[task_chats.Items.Count - 1]);

        }//end of method

        private void show_task_bot_message(string message)
        {//start of method

            task_chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Black }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            task_chats.ScrollIntoView(task_chats.Items[task_chats.Items.Count - 1]);

        }//end of method

        private void show_task_error_message(string message)
        {//start of method

            task_chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Red }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            task_chats.ScrollIntoView(task_chats.Items[task_chats.Items.Count - 1]);

        }//end of method

        // Update task statistics display
        private void update_task_stats()
        {//start of method

            if (taskManager != null)
            {
                int pendingCount = taskManager.get_pending_task_count();
                task_stats.Text = "Tasks: " + pendingCount + " pending";
            }

        }//end of method

        // ========== QUIZ MANAGER METHODS ==========

        // Initialize the quiz manager
        private void initialize_quiz_manager()
        {//start of method

            quizManager = new quiz_manager(username);

        }//end of method

        // Open the quiz grid
        private void open_quiz_grid()
        {//start of method

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

        }//end of method

        // Close the quiz grid
        private void close_quiz_grid(object sender, RoutedEventArgs e)
        {//start of method

            quiz_grid.Visibility = Visibility.Hidden;
            chats_grid.Visibility = Visibility.Visible;

        }//end of method

        // Send quiz answer
        private void send_quiz_answer(object sender, RoutedEventArgs e)
        {//start of method

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

                    // Log quiz start
                    logger.add_log("Quiz started by user");
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
                    int score = quizManager.get_current_question_index();
                    int total = quizManager.get_total_questions();
                    show_quiz_bot_message("Quiz complete! You can start another quiz by typing 'Start quiz'.");

                    // Log quiz completion with score
                    logger.add_log("Quiz completed. Score: " + score + "/" + total);
                }
            }

            quiz_question.Text = "";

        }//end of method

        // End the current quiz
        private void end_quiz(object sender, RoutedEventArgs e)
        {//start of method

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

        }//end of method

        // Open quiz from main chat
        private void open_quiz(object sender, RoutedEventArgs e)
        {//start of method

            // Initialize quiz manager if not already done
            if (quizManager == null)
            {
                initialize_quiz_manager();
            }
            open_quiz_grid();

        }//end of method

        // Helper methods for quiz grid
        private void show_quiz_user_message(string message)
        {//start of method

            quiz_chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "You: ", Foreground = Brushes.DarkGreen, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Black }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            quiz_chats.ScrollIntoView(quiz_chats.Items[quiz_chats.Items.Count - 1]);

        }//end of method

        private void show_quiz_bot_message(string message)
        {//start of method

            quiz_chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Black }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            quiz_chats.ScrollIntoView(quiz_chats.Items[quiz_chats.Items.Count - 1]);

        }//end of method

        private void show_quiz_error_message(string message)
        {//start of method

            quiz_chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Red }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            quiz_chats.ScrollIntoView(quiz_chats.Items[quiz_chats.Items.Count - 1]);

        }//end of method

        // Update quiz score display
        private void update_quiz_score()
        {//start of method

            if (quizManager != null)
            {
                int current = quizManager.get_current_question_index();
                int total = quizManager.get_total_questions();
                quiz_score.Text = "Score: " + current + "/" + total;
                quiz_progress.Text = "Question " + (current + 1) + " of " + total;
            }

        }//end of method

        // ========== NLP PROCESSOR ==========

        // Initialize the NLP processor
        private void initialize_nlp_processor()
        {//start of method

            // Initializing the NLP processor with required dependencies
            nlpProcessor = new nlp_processor(reply, ignore, username, taskManager, quizManager);

        }//end of method

        // ========== ACTIVITY LOGGER ==========

        // Initialize the activity logger
        private void initialize_activity_logger()
        {//start of method

            // Initializing the activity logger
            logger = new activity_logger();
            logger.add_log("Application started by user: " + username);

        }//end of method

        // Close the application
        private void close_application(object sender, RoutedEventArgs e)
        {//start of method

            Application.Current.Shutdown();

        }//end of method

    }//end of class

}//end of namespace