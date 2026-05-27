using System;
using System.Collections;

namespace cybersecurity_awareness_chatbot_p2
{//start of namespace
    public class response_handler
    {//start of class

        private ArrayList reply;
        private ArrayList ignore;
        private Random indexer;

        public response_handler(ArrayList replyList, ArrayList ignoreList)
        {//start of constructor

            reply = replyList;
            ignore = ignoreList;
            indexer = new Random();

        }//end of constructor

        public string process_question(string questions)
        {//start of process_question() method

            string[] words = questions.Split(' ');
            bool found = false;
            string message = string.Empty;

            ArrayList per_word = new ArrayList();
            ArrayList answers_found = new ArrayList();

            foreach (string word in words)
            {//start of outer foreach

                if (!ignore.Contains(word.ToLower()))
                {//start of if statement

                    per_word.Clear();

                    foreach (string answer in reply)
                    {//start of inner foreach

                        if (answer.ToLower().Contains(word.ToLower()))
                        {//start of if statement

                            found = true;
                            per_word.Add(answer);

                        }//end of if statement

                    }//end of inner foreach

                    if (found && per_word.Count > 0)
                    {//start of if statement

                        int indexing = indexer.Next(0, per_word.Count);
                        answers_found.Add(per_word[indexing]);
                        found = false;

                    }//end of if statement

                }//end of if statement

            }//end of outer foreach

            if (answers_found.Count > 0)
            {//start of if statement

                // Remove duplicates
                ArrayList uniqueAnswers = new ArrayList();
                foreach (string answer in answers_found)
                {//start of foreach

                    if (!uniqueAnswers.Contains(answer))
                        uniqueAnswers.Add(answer);

                }//end of foreach

                foreach (string per_answer in uniqueAnswers)
                {//start of foreach

                    // Extract response without the topic prefix
                    int spaceIndex = per_answer.IndexOf(' ');
                    if (spaceIndex > 0)
                        message += per_answer.Substring(spaceIndex + 1) + "\n";
                    else
                        message += per_answer + "\n";

                }//end of foreach

                return message.TrimEnd('\n');
            }//end of if statement

            return "";

        }//end of process_question() method

    }//end of class

}//end of namespace