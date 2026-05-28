using System;
using System.Collections;

namespace cybersecurity_awareness_chatbot_p2
{//start of namespace
    public class response_finder
    {//start of class

        private ArrayList reply;
        private Random random;

        public response_finder(ArrayList reply_list)
        {//start of constructor
            reply = reply_list;
            random = new Random();

        }//end of constructor

        public string get_response_for_topic(string topic)
        {//start of method

            ArrayList matching_responses = new ArrayList();

            //looking for responses that start with the topic followed by a space
            foreach (string response in reply)
            {//start of foreach
                string lower_response = response.ToLower();
                if (lower_response.StartsWith(topic + " "))
                {//start of if statement
                    int space_index = response.IndexOf(' ');
                    if (space_index > 0)
                    {//start of inner if statement
                        matching_responses.Add(response.Substring(space_index + 1));
                    }//end of inner if statement
                    else
                    {//start of else statement
                        matching_responses.Add(response);
                    }//end of else statement

                }//end of if statement

            }//end of foreach

            // If no matching responses are found, return an empty string
            if (matching_responses.Count == 0)
                return "";

            int index = random.Next(0, matching_responses.Count);
            return matching_responses[index].ToString();

        }//end of method

    }//end of class

}//end of namespace