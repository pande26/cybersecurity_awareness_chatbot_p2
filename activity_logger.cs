using System;
using System.Collections;

namespace cybersecurity_awareness_chatbot_p2
{//start of namespace
    public class activity_logger
    {//start of class
        private ArrayList activity_log;
        private int max_log_entries;

        public activity_logger()
        {//start of constructor
            activity_log = new ArrayList();
            max_log_entries = 50; // Store up to 50 entries

        }//end of constructor

        // Method to add a log entry
        public void add_log(string action)
        {//start of add_log method
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string log_entry = timestamp + " - " + action;

            activity_log.Add(log_entry);

            // Keep only the last 50 entries
            if (activity_log.Count > max_log_entries)
            {//start of if statement
                activity_log.RemoveAt(0);

            }//end of if statement

        }//end of add_log method

        // Method to get the last 10 entries
        public string get_recent_activity()
        {//start of get_recent_activity method

            if (activity_log.Count == 0)
            {// start of if statement
                return "No activities have been logged yet.";

            }//end of if statement

            string result = "Here is a summary of recent actions:\n\n";
            int count = 0;
            int start_index = Math.Max(0, activity_log.Count - 10);

            for (int i = start_index; i < activity_log.Count; i++)
            {//start of for loop
                count++;
                result += count + ". " + activity_log[i].ToString() + "\n";

            }//end of for loop

            return result;
        }

        // Method to get all entries
        public string get_all_activity()
        {//start of get_all_activity method

            if (activity_log.Count == 0)
            {//start of if statement
                return "No activities have been logged yet.";

            }//end of if statement

            string result = "Complete Activity Log:\n\n";
            int count = 0;

            foreach (string entry in activity_log)
            {//start of foreach loop
                count++;
                result += count + ". " + entry + "\n";

            }// end of foreach loop

            return result;

        }//end of get_all_activity method

        // Method to get log count
        public int get_log_count()
        {//start of get_log_count method
            return activity_log.Count;

        }//end of get_log_count method

        // Method to clear log
        public void clear_log()
        {//start of clear_log method
            activity_log.Clear();
            add_log("Activity log was cleared");

        }//end of clear_log method

    }//end of class

}//end of namespace