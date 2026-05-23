using System;
using System.Collections.Generic;
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
        public MainWindow()
        {//start of constructor
            InitializeComponent();

            /*creating an instance for the class greet_user
            with constructor*/
            new greet_user();

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

            //collect user's input
            string collected_name = user_name.Text.ToString();

            //checking if the name is entered of not
            if(collected_name != "")
            {//start of if statement

                //displaying the message if the user has entered a name
                MessageBox.Show("Hey " + collected_name);

            }//end of if statement
            else
            {//start of else statement

                //error message
                MessageBox.Show("Please enter your name...");

            }//end of else statement

        }//end of submit_username() method

    }//end of class

}//end of namespace 
