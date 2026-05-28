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

        //creating an instance of ArrayList
        ArrayList reply = new ArrayList();
        ArrayList ignore = new ArrayList();

        //declaring all class instances
        private response_finder finder;
        private response_handler handler;
        private topic_detector detector;
        private message_displayer displayer;
        private sentiment_detector sentimentDetector;

        //variables to store the last detected topic for follow-up questions
        private string last_topic = "";
        private string username = "";

        public MainWindow()
        {//start of constructor
            InitializeComponent();

            /*creating an instance for the class greet_user
            with constructor*/
            new greet_user();

            /*creating an instance for the respond class 
             * without an object name*/
            new respond(reply, ignore) { };

            //initializing all the class instances
            finder = new response_finder(reply);
            handler = new response_handler(reply, ignore);
            detector = new topic_detector();
            displayer = new message_displayer();
            sentimentDetector = new sentiment_detector(reply, finder);

        }//end of constructor

        private void start_valerie(object sender, RoutedEventArgs e)
        {//start of method

            //set logo grid to hidden
            logo_grid.Visibility = Visibility.Hidden;

            //set username grid to visible
            username_grid.Visibility = Visibility.Visible;

        }//end of method

        private void submit_username(object sender, RoutedEventArgs e)
        {//start of submit_username() method

            //temp variables
            string name = user_name.Text.ToString().Trim();
            bool found = check_name(name);

            //checking for empty input
            if (string.IsNullOrWhiteSpace(name))
            {//start of if statement

                MessageBox.Show("Please enter your name before continuing...");
                user_name.Focus();
                return;

            }//end of if statement

            // Store username
            username = name;
            displayer.set_username(name);

            //temp variables
            string filename = "user_name.txt";

            //check if the file exists or not, the auto create
            if (!File.Exists(filename))
            {//start of if statement

                //auto create the file using AppendAllText() function
                File.AppendAllText(filename, "auto_create\n");

            }//end of if statement


            //store the username into a text file
            if (!found)
            {//start of if statement

                //store the name in a test file
                File.AppendAllText(filename, name + "\n");
                MessageBox.Show("Welcome " + name );

                //hide the username grid and set the chats grid to visible
                username_grid.Visibility = Visibility.Hidden;
                chats_grid.Visibility = Visibility.Visible;

            }//end of if statement
            else
            {//start of else statement 

                //welcome back the user
                MessageBox.Show("Welcome back " + name );

                //set the username grid to be hidden
                username_grid.Visibility = Visibility.Hidden;
                chats_grid.Visibility = Visibility.Visible;

            }//end of else statement

        }//end of submit_username() method

        //method to check name for the user
        private Boolean check_name(string name)
        {//start of check_name

            //temporal variable for the text file part
            string find_name = "user_name.txt";
            bool name_found = false;

            if (File.Exists(find_name))
            {//start of if statement

                //one dimension array to read all the names from the text file
                string[] names = File.ReadAllLines(find_name);

                //foreach to loop through the 1D array and search for the current user_name
                foreach (string search_name in names)
                {//start of foreach

                    //if statement to check if the username is found or not
                    if (search_name.ToLower() == name.ToLower())
                    {//start of if statement

                        //name_found must be true
                        name_found = true;
                        break;

                    }//end of if statement

                }//end of foreach

            }//end of if statement

            //returning the status of if the user is found or not
            return name_found;

        }//end of check_name

        //method to check if the question is a follow-up question
        private bool is_follow_up_question(string question)
        {//start of is_follow_up_question
            string lower = question.ToLower();

            // List of follow-up phrases
            string[] follow_up_phrases = {
                "another tip", "another one", "more tips", "tell me more",
                "explain more", "elaborate", "continue", "more information",
                "what else", "anything else", "next tip", "another"
                 };

            foreach (string phrase in follow_up_phrases)
            {//start of foreach

                if (lower.Contains(phrase))
                    return true;

            }//end of foreach
            return false;

        }//end of is_follow_up_question

        //method to handle the send button click event
        private void send(object sender, RoutedEventArgs e)
        {//start of send method 

            //get the user's question
            string questions = question.Text.ToString().Trim();

            //checking for empty input
            if (string.IsNullOrEmpty(questions))
            {//start of if statement

                error_method();
                question.Text = "";
                return;

            }//end of if statement

            //checking for sentiment in the user's question
            string sentimentResult = sentimentDetector.process_sentiment(questions);

            if (!string.IsNullOrEmpty(sentimentResult))
            {
                display_user_message(questions);
                display_bot_message(sentimentResult);
                question.Text = "";
                return;
            }

            //checking for follow-up question and if a topic was previously detected
            if (is_follow_up_question(questions) && !string.IsNullOrEmpty(last_topic))
            {
                string follow_up_response = "";

                // Get another response for the same topic
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

                // If a follow-up response is found, display it
                if (!string.IsNullOrEmpty(follow_up_response))
                {
                    display_user_message(questions);
                    display_bot_message(follow_up_response);
                    question.Text = "";
                    return;
                }
            }

            //split the question into words and search for matches in the reply list
            string[] words = questions.Split(' ');
            bool found = false;
            string message = "";
            Random indexer = new Random();
            ArrayList per_word = new ArrayList();
            ArrayList answers_found = new ArrayList();

            //loop through each word in the user's question
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

            //if answers are found, display them
            if (answers_found.Count > 0)
            {
                foreach (string per_answer in answers_found)
                {
                    // Extract response without topic prefix
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
            }

            question.Text = "";

        }//end of send method

        // Helper method to display user message
        private void display_user_message(string message)
        {//start of display_user_message

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

        }//end of display_user_message

        // Helper method to display bot message
        private void display_bot_message(string message)
        {//start of display_bot_message

            chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Black }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);

        }//end of display_bot_message

        // Error method for empty input
        private void error_method()
        {//start of error_method
            chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = "Please enter a question!!", Foreground = Brushes.Red }
                }
            });

        }//end of error_method

    }//end of class

}//end of namespace