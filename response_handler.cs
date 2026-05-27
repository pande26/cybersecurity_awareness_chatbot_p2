using System;
using System.Collections;

namespace cybersecurity_awareness_chatbot_p2
{
    public class response_handler
    {
        private ArrayList reply;
        private ArrayList ignore;
        private Random indexer;

        public response_handler(ArrayList reply_list, ArrayList ignore_list)
        {
            reply = reply_list;
            ignore = ignore_list;
            indexer = new Random();
        }

        public string process_question(string questions)
        {
            string[] words = questions.Split(' ');
            bool found = false;
            string message = "";

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
                // Remove duplicates
                ArrayList unique_answers = new ArrayList();
                foreach (string answer in answers_found)
                {
                    if (!unique_answers.Contains(answer))
                        unique_answers.Add(answer);
                }

                foreach (string per_answer in unique_answers)
                {
                    // Extract response without the topic prefix
                    int space_index = per_answer.IndexOf(' ');
                    if (space_index > 0)
                        message += per_answer.Substring(space_index + 1) + "\n";
                    else
                        message += per_answer + "\n";
                }

                return message.TrimEnd('\n');
            }

            return "";
        }
    }
}