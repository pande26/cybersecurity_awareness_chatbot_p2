using System;
using System.Collections;

namespace cybersecurity_awareness_chatbot_p2
{//start of namespace
    public class response_handler
    {//start of class

        private ArrayList reply;
        private ArrayList ignore;
        private Random indexer;

        public response_handler(ArrayList reply_list, ArrayList ignore_list)
        {//start of constructor
            reply = reply_list;
            ignore = ignore_list;
            indexer = new Random();

        }//end of constructor

        public string process_question(string questions)
        {//start of method

            // Split the question into words and search for matches in the reply list
            string[] words = questions.Split(' ');
            bool found = false;
            string message = "";

            ArrayList per_word = new ArrayList();
            ArrayList answers_found = new ArrayList();

            foreach (string word in words)
            {//start of foreach
                if (!ignore.Contains(word.ToLower()))
                {//start of if statement
                    per_word.Clear();

                    foreach (string answer in reply)
                    {//start of inner foreach
                        if (answer.ToLower().Contains(word.ToLower()))
                        {//start of inner if statement
                            found = true;
                            per_word.Add(answer);
                        }//end of inner if statement

                    }//end of inner foreach

                    if (found && per_word.Count > 0)
                    {//start of if statement
                        int indexing = indexer.Next(0, per_word.Count);
                        answers_found.Add(per_word[indexing]);
                        found = false;

                    }//end of if statement

                }//end of if statement

            }//end of foreach

            if (answers_found.Count > 0)
            {//start of if statement

                // Remove duplicates
                ArrayList unique_answers = new ArrayList();
                foreach (string answer in answers_found)
                {//start of foreach
                    if (!unique_answers.Contains(answer))
                        unique_answers.Add(answer);
                }//end of foreach

                foreach (string per_answer in unique_answers)
                {//start of foreach
                    // Extract response without the topic prefix
                    int space_index = per_answer.IndexOf(' ');
                    if (space_index > 0)
                        message += per_answer.Substring(space_index + 1) + "\n";
                    else
                        message += per_answer + "\n";
                }//end of foreach

                return message.TrimEnd('\n');
            }//end of if statement

            return "";

        }//end of method

    }//end of class

}//end of namespace