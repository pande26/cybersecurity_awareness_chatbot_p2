using System;
using System.Collections;

namespace cybersecurity_awareness_chatbot_p2
{//start of namespace
    public class sentiment_detector
    {//start of class

        private ArrayList reply;
        private response_finder finder;
        private Random random;

        public sentiment_detector(ArrayList reply_list, response_finder response_finder)
        {//start of constructor
            reply = reply_list;
            finder = response_finder;
            random = new Random();

        }//end of constructor

        //method to detect sentiment from user input
        public string detect_sentiment(string question)
        {//start of method

            string lower = question.ToLower();

            //check for confused FIRST (priority)
            if (lower.Contains("confused") || lower.Contains("don't understand") ||
                lower.Contains("not sure") || lower.Contains("doesn't make sense"))
                return "confused";

            // Check for worried
            if (lower.Contains("worried") || lower.Contains("anxious") ||
                lower.Contains("scared") || lower.Contains("nervous") || lower.Contains("concerned"))
                return "worried";

            // Check for frustrated
            if (lower.Contains("frustrated") || lower.Contains("annoyed"))
                return "frustrated";

            // Check for happy
            if (lower.Contains("happy") || lower.Contains("great") ||
                lower.Contains("good") || lower.Contains("awesome") ||
                lower.Contains("excellent") || lower.Contains("wonderful"))
                return "happy";

            // Check for sad
            if (lower.Contains("sad") || lower.Contains("upset") ||
                lower.Contains("terrible") || lower.Contains("bad") ||
                lower.Contains("depressed") || lower.Contains("lonely"))
                return "sad";

            // Check for angry
            if (lower.Contains("angry") || lower.Contains("mad") ||
                lower.Contains("furious") || lower.Contains("annoyed"))
                return "angry";

            return "neutral";
        }

        // Method to get response for detected sentiment from reply ArrayList
        public string get_sentiment_response(string sentiment)
        {
            ArrayList matching_responses = new ArrayList();

            // Find all responses that start with the sentiment
            foreach (string answer in reply)
            {
                string lowerAnswer = answer.ToLower();
                if (lowerAnswer.StartsWith(sentiment + " "))
                {
                    int spaceIndex = answer.IndexOf(' ');
                    if (spaceIndex > 0)
                    {
                        matching_responses.Add(answer.Substring(spaceIndex + 1));
                    }
                    else
                    {
                        matching_responses.Add(answer);
                    }
                }
            }

            //if no matching responses found, return a default message
            if (matching_responses.Count == 0)
            {
                switch (sentiment)
                {
                    case "confused":
                        return "that's okay, confusion is normal. i'll explain it clearly for you.";
                    case "worried":
                        return "it's okay to feel worried. i'm here to help you stay safe online.";
                    case "frustrated":
                        return "i understand you're frustrated. let's work through the issue step by step.";
                    case "happy":
                        return "that's great to hear! i'm glad things are going well.";
                    case "sad":
                        return "i'm sorry you're feeling this way. i'm here for you.";
                    case "angry":
                        return "i understand you're angry. let's try solve the issue together.";
                    default:
                        return "i'm here to help you with your cybersecurity questions.";
                }
            }

            // Return a random response from matching ones
            int index = random.Next(0, matching_responses.Count);
            return matching_responses[index].ToString();
        }

        // Method to get a random cybersecurity tip
        public string get_random_tip()
        {
            string[] topics = { "password", "scam", "privacy", "phishing" };
            string randomTopic = topics[random.Next(topics.Length)];

            string tip = finder.get_response_for_topic(randomTopic);

            if (string.IsNullOrEmpty(tip))
            {
                string[] default_tips = {
                    "Always use strong, unique passwords for each account.",
                    "Never click on links in suspicious emails.",
                    "Keep your software and antivirus programs updated.",
                    "Think before you share personal information online.",
                    "Use two-factor authentication whenever possible."
                };
                tip = default_tips[random.Next(default_tips.Length)];
            }

            return tip;
        }

        //combined method: handles sentiment and returns appropriate response with tip
        public string process_sentiment(string question)
        {
            string sentiment = detect_sentiment(question);

            if (sentiment != "neutral")
            {
                string sentimentResponse = get_sentiment_response(sentiment);

                if (!string.IsNullOrEmpty(sentimentResponse))
                {
                    // For worried, frustrated, or confused, add a helpful tip
                    if (sentiment == "worried" || sentiment == "frustrated" || sentiment == "confused")
                    {
                        return sentimentResponse + "\n\nHere's a tip to help you:\n" + get_random_tip();
                    }
                    return sentimentResponse;
                }
            }

            return "";
        }

        // Method to check if user input contains sentiment (for priority routing)
        public bool has_sentiment(string question)
        {
            return detect_sentiment(question) != "neutral";
        }
    }
}