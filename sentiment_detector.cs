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

            if (lower.Contains("worried") || lower.Contains("anxious") || lower.Contains("scared") || lower.Contains("nervous"))
                return "worried";

            if (lower.Contains("frustrated") || lower.Contains("annoyed") || lower.Contains("confused"))
                return "frustrated";

            if (lower.Contains("happy") || lower.Contains("great") || lower.Contains("good") || lower.Contains("awesome"))
                return "happy";

            if (lower.Contains("sad") || lower.Contains("upset") || lower.Contains("terrible") || lower.Contains("bad"))
                return "sad";

            if (lower.Contains("angry") || lower.Contains("mad"))
                return "angry";

            return "neutral";

        }//end of method

        //method to get response for detected sentiment
        public string get_sentiment_response(string sentiment)
        {//start of method

            foreach (string answer in reply)
            {//start of foreach

                string lowerAnswer = answer.ToLower();
                if (lowerAnswer.StartsWith(sentiment + " "))
                {//start of if statement

                    int spaceIndex = answer.IndexOf(' ');
                    if (spaceIndex > 0)
                    {//start of inner if statement
                        return answer.Substring(spaceIndex + 1);
                    }//end of inner if statement
                    return answer;
                }//end of if statement

            }//end of foreach
            return "";

        }//end of method

        //method to get a random cybersecurity tip
        public string get_random_tip()
        {//start of method

            string[] topics = { "password", "scam", "privacy", "phishing" };
            string randomTopic = topics[random.Next(topics.Length)];

            string tip = finder.get_response_for_topic(randomTopic);

            if (string.IsNullOrEmpty(tip))
            {//if no tip is found for the random topic, return a default tip
                tip = "Always be cautious online and never share personal information with strangers.";
            }//end of if statement

            return tip;

        }//end of method

        // Combined method: handles sentiment and returns appropriate response with tip
        public string process_sentiment(string question)
        {//start of method
            string sentiment = detect_sentiment(question);

            // If the sentiment is not neutral, get a response and possibly add a tip
            if (sentiment != "neutral")
            {//start of if statement
                string sentimentResponse = get_sentiment_response(sentiment);

                if (!string.IsNullOrEmpty(sentimentResponse))
                {//start of if statement

                    // If worried, frustrated, or confused, add a tip
                    if (sentiment == "worried" || sentiment == "frustrated" || sentiment == "confused")
                    {//start of if statement
                        return sentimentResponse + "\n\nHere's a tip to help you stay safe online:\n" + get_random_tip();
                    }//end of if statement
                    return sentimentResponse;

                }//end of if statement

            }//end of if statement

            return "";

        }//end of method

    }//end of class

}//end of namespace