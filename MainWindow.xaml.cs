using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public MainWindow()
        {//start of constructor
            InitializeComponent();

            /*creating an instance for the class greet_user
            with constructor*/
            new greet_user();

            /*creating an instance for the respond class 
             * without an object name*/
            new respond(reply, ignore) { };

            /*creating an instance for the ResponseHandler class
            with constructor passing reply and ignore*/
            new response_handler(reply, ignore);


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

            // ADD THIS CHECK
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter your name before continuing...");
                user_name.Focus();
                return;
            }

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

                }//end of if statement

            }//end of foreach

            //returning the status of if the user is found or not
            return name_found;

        }//end of check_name

        private void send(object sender, RoutedEventArgs e)
        {//start of send method 

            //get the question from the design
            //user input
            string questions = question.Text.ToString();

            //if statement to  check if the user entered a question or not
            if (questions == "")
            {//start of if statement 

                //call the error method
                error_method();

            }//end of if statement
            else
            {//start of else statement

                //temp variables and arrays
                string[] words = questions.Split(' ');

                bool found = false;
                string message = string.Empty;

                Random indexer = new Random();

                ArrayList per_word = new ArrayList();
                ArrayList answers_found = new ArrayList();

                //alternate per word from the words array
                foreach (string word in words)
                {//start of the main foreach

                    //check if the word is allowed or not
                    if (!ignore.Contains(word.ToLower()))
                    {//start of check if

                        per_word.Clear();
                        //foreach to search for the answer of the word allowed
                        foreach (string answer in reply)
                        {//start of answer foreach 

                            //check and store
                            if (answer.Contains(word.ToLower()))
                            {//start of check answer if

                                found = true;

                                //store all answers for the word
                                per_word.Add(answer);

                            }//end of check answer if

                        }//end of answer foreach

                        //then check if found is true and store
                        //per random
                        if (found)
                        {//start of found if

                            //get the random indexer
                            int indexing = indexer.Next(0,per_word.Count );

                            //storing one answer per word now
                            answers_found.Add(per_word[indexing]);

                        }//end of found if

                    }//end of check word if

                }//end of the main foreach

                //check and show the user the answer
                if (found)
                {//start of found if true

                    //get all of answers and show to the user
                    foreach (string per_answer in answers_found)
                    {//start of show answer loop

                        //append all message
                        message += per_answer + "\n";

                    }//end of show answer loop]

                    //add the message or answers to the list view
                    chats.Items.Add(message);

                    //auto scroll
                    chats.ScrollIntoView(chats.Items[chats.Items.Count-1]);
                }//end of found if true

            }//end of else statement

        }//end of send method

        //error method
        private void error_method()
        {//start of error method

            //calling the chats which is a listview
            chats.Items.Add(
                new TextBlock
                {
                    Inlines =
                    {
                        new Run
                        {
                            Text = "Valerie : " ,
                            Foreground = Brushes.LightBlue
                        } ,
                        new Run
                        {
                            Text = "Please enter a question!!" ,
                            Foreground = Brushes.Red
                        }
                    }
                }

            );

        }//end of error method


    }//end of class

}//end of namespace 
