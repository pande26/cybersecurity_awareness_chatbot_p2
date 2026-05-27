using System;
using System.Collections;

namespace cybersecurity_awareness_chatbot_p2
{
    public class response_finder
    {
        private ArrayList reply;
        private Random random;

        public response_finder(ArrayList reply_list)
        {
            reply = reply_list;
            random = new Random();
        }

        public string get_response_for_topic(string topic)
        {
            ArrayList matching_responses = new ArrayList();

            foreach (string response in reply)
            {
                string lower_response = response.ToLower();
                if (lower_response.StartsWith(topic + " "))
                {
                    int space_index = response.IndexOf(' ');
                    if (space_index > 0)
                    {
                        matching_responses.Add(response.Substring(space_index + 1));
                    }
                    else
                    {
                        matching_responses.Add(response);
                    }
                }
            }

            if (matching_responses.Count == 0)
                return "";

            int index = random.Next(0, matching_responses.Count);
            return matching_responses[index].ToString();
        }
    }
}