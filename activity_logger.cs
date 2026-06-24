using System;
using System.Collections;

namespace cybersecurity_awareness_chatbot_p2
{
    public class activity_logger
    {
        private ArrayList activity_log;
        private int max_log_entries;

        public activity_logger()
        {
            activity_log = new ArrayList();
            max_log_entries = 50; // Store up to 50 entries
        }

        // Method to add a log entry
        public void add_log(string action)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string log_entry = timestamp + " - " + action;

            activity_log.Add(log_entry);

            // Keep only the last 50 entries
            if (activity_log.Count > max_log_entries)
            {
                activity_log.RemoveAt(0);
            }
        }

        // Method to get the last 10 entries
        public string get_recent_activity()
        {
            if (activity_log.Count == 0)
            {
                return "No activities have been logged yet.";
            }

            string result = "Here is a summary of recent actions:\n\n";
            int count = 0;
            int start_index = Math.Max(0, activity_log.Count - 10);

            for (int i = start_index; i < activity_log.Count; i++)
            {
                count++;
                result += count + ". " + activity_log[i].ToString() + "\n";
            }

            return result;
        }

        // Method to get all entries
        public string get_all_activity()
        {
            if (activity_log.Count == 0)
            {
                return "No activities have been logged yet.";
            }

            string result = "Complete Activity Log:\n\n";
            int count = 0;

            foreach (string entry in activity_log)
            {
                count++;
                result += count + ". " + entry + "\n";
            }

            return result;
        }

        // Method to get log count
        public int get_log_count()
        {
            return activity_log.Count;
        }

        // Method to clear log
        public void clear_log()
        {
            activity_log.Clear();
            add_log("Activity log was cleared");
        }
    }
}