using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace cybersecurity_awareness_chatbot_p2
{//start of namespace
    public class message_displayer
    {//start of class

        private string username = "";

        public void set_username(string name)
        {//start of method
            username = name;

        }//end of method

        //method to display user messages in the chat interface
        public void show_user_message(ListView chats, string message)
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

        //method to display bot messages in the chat interface
        public void show_bot_message(ListView chats, string message)
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
        
        //method to display error messages in the chat interface
        public void show_error_message(ListView chats, string message)
        {//start of method

            chats.Items.Add(new TextBlock
            {
                Inlines = {
                    new Run { Text = "Valerie: ", Foreground = Brushes.DarkBlue, FontWeight = FontWeights.Bold },
                    new Run { Text = message, Foreground = Brushes.Red }
                },
                Margin = new Thickness(5, 2, 5, 2)
            });
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);

        }//end of method

    }//end of class

}//end of namespace