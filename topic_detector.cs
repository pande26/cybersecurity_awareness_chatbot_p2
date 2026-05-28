namespace cybersecurity_awareness_chatbot_p2
{//start of namespace
    public class topic_detector
    {//start of class
        public string detect_topic(string question)
        {//start of method

            //converting the question to lowercase for easier keyword matching
            string lower = question.ToLower();

            // Password related keywords
            if (lower.Contains("password") || lower.Contains("passphrase") ||
                lower.Contains("login") || lower.Contains("credentials"))
                return "password";

            // Scam related keywords
            if (lower.Contains("scam") || lower.Contains("scammer") ||
                lower.Contains("fraud") || lower.Contains("fake"))
                return "scam";

            // Privacy related keywords
            if (lower.Contains("privacy") || lower.Contains("private") ||
                lower.Contains("personal information") || lower.Contains("data protection"))
                return "privacy";

            // Phishing related keywords
            if (lower.Contains("phishing") || lower.Contains("phish"))
                return "phishing";

            // Greeting related keywords
            if (lower.Contains("how are you") || lower.Contains("hello") ||
                lower.Contains("hi") || lower.Contains("hey"))
                return "greeting";

            // Purpose related keywords
            if (lower.Contains("purpose") || lower.Contains("what do you do") ||
                lower.Contains("what is your purpose"))
                return "purpose";

            // Firewall related keywords
            if (lower.Contains("firewall"))
                return "firewall";

            // VPN related keywords
            if (lower.Contains("vpn"))
                return "vpn";

            // Hacked account related keywords
            if (lower.Contains("hacked") || lower.Contains("compromised"))
                return "hacked account";

            // Fraud related keywords
            if (lower.Contains("fraud"))
                return "fraud";

            return "unknown";
        }//end of method

    }//end of class

}//end of namespace