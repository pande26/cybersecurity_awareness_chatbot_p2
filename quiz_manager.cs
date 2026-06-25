using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace cybersecurity_awareness_chatbot_p2
{//start of namespace

    public class quiz_manager
    {//start of class

        // Quiz data storage
        private ArrayList questions;
        private int current_question_index;
        private int score;
        private bool quiz_active;
        private string username;

        // Constructor - initializes quiz with questions
        public quiz_manager(string user)
        {//start of constructor

            username = user;
            questions = new ArrayList();
            current_question_index = 0;
            score = 0;
            quiz_active = false;
            load_questions();

        }//end of constructor

        // Load all quiz questions
        private void load_questions()
        {//start of method

            // Question 1 - Phishing
            questions.Add(new quiz_question
            {
                question_text = "What should you do if you receive an email asking for your password?",
                options = new string[] { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                correct_answer = 2,
                explanation = "Reporting phishing emails helps prevent scams and protects others from falling victim."
            });

            // Question 2 - Password Safety
            questions.Add(new quiz_question
            {
                question_text = "True or False: Using 'password123' as your password is safe.",
                options = new string[] { "A) True", "B) False" },
                correct_answer = 1,
                explanation = "'password123' is a weak password that is easily guessable. Always use strong, unique passwords."
            });

            // Question 3 - Two-Factor Authentication
            questions.Add(new quiz_question
            {
                question_text = "What does 2FA stand for?",
                options = new string[] { "A) Two-Factor Authentication", "B) Two-File Access", "C) Triple-Factor Authentication", "D) Two-Factor Authorization" },
                correct_answer = 0,
                explanation = "Two-Factor Authentication adds an extra layer of security by requiring a second verification step."
            });

            // Question 4 - Password Safety
            questions.Add(new quiz_question
            {
                question_text = "True or False: You should use the same password for all your accounts.",
                options = new string[] { "A) True", "B) False" },
                correct_answer = 1,
                explanation = "Using the same password across multiple accounts is dangerous. If one account is hacked, all become vulnerable."
            });

            // Question 5 - Phishing
            questions.Add(new quiz_question
            {
                question_text = "What is phishing?",
                options = new string[] { "A) A type of fishing sport", "B) A scam where attackers pretend to be trusted sources", "C) A type of computer virus", "D) A security software" },
                correct_answer = 1,
                explanation = "Phishing is a scam where attackers pretend to be trusted sources to steal personal information."
            });

            // Question 6 - Safe Browsing
            questions.Add(new quiz_question
            {
                question_text = "True or False: HTTPS websites are always completely safe to enter personal information.",
                options = new string[] { "A) True", "B) False" },
                correct_answer = 1,
                explanation = "HTTPS encrypts data but does not guarantee the website is legitimate. Always verify the website before entering personal info."
            });

            // Question 7 - Password Safety
            questions.Add(new quiz_question
            {
                question_text = "What makes a strong password?",
                options = new string[] { "A) Your birthday", "B) A mix of uppercase, lowercase, numbers, and symbols", "C) Your pet's name", "D) A common word" },
                correct_answer = 1,
                explanation = "Strong passwords use a mix of uppercase, lowercase, numbers, and symbols for maximum security."
            });

            // Question 8 - Password Safety
            questions.Add(new quiz_question
            {
                question_text = "True or False: You should never share your password with anyone.",
                options = new string[] { "A) True", "B) False" },
                correct_answer = 0,
                explanation = "Never share your password with anyone. Legitimate companies will never ask for your password."
            });

            // Question 9 - Social Engineering
            questions.Add(new quiz_question
            {
                question_text = "What is social engineering in cybersecurity?",
                options = new string[] { "A) Building social networks", "B) Manipulating people to reveal confidential information", "C) Designing social media platforms", "D) A type of computer programming" },
                correct_answer = 1,
                explanation = "Social engineering is a tactic used by attackers to manipulate people into revealing confidential information."
            });

            // Question 10 - Safe Browsing
            questions.Add(new quiz_question
            {
                question_text = "True or False: You should download software only from trusted sources.",
                options = new string[] { "A) True", "B) False" },
                correct_answer = 0,
                explanation = "Always download software from trusted sources to avoid malware and other security threats."
            });

            // Question 11 - Account Security
            questions.Add(new quiz_question
            {
                question_text = "What should you do if you suspect your account has been hacked?",
                options = new string[] { "A) Ignore it", "B) Change your password immediately", "C) Share the news on social media", "D) Wait for it to fix itself" },
                correct_answer = 1,
                explanation = "If you suspect your account is hacked, immediately change your password and enable two-factor authentication."
            });

            // Question 12 - Safe Browsing
            questions.Add(new quiz_question
            {
                question_text = "True or False: Public Wi-Fi is always safe to use for online banking.",
                options = new string[] { "A) True", "B) False" },
                correct_answer = 1,
                explanation = "Public Wi-Fi is not secure for online banking. Use a VPN or your mobile data for sensitive transactions."
            });

        }//end of method

        // Check if quiz is currently active
        public bool is_quiz_active()
        {//start of method

            return quiz_active;

        }//end of method

        // Start the quiz
        public string start_quiz(ListView chat_list)
        {//start of method

            if (questions.Count == 0)
            {
                return "Sorry, no questions are available for the quiz.";
            }

            quiz_active = true;
            current_question_index = 0;
            score = 0;

            return get_current_question();

        }//end of method

        // Process user's answer
        public string process_answer(string answer, ListView chat_list)
        {//start of method

            if (!quiz_active)
            {
                return "The quiz is not active. Type 'Start quiz' to begin.";
            }

            string clean_answer = answer.ToUpper().Trim();

            // Convert answer letter to index
            int selected_index = -1;
            quiz_question current_question = (quiz_question)questions[current_question_index];

            if (clean_answer == "A" || clean_answer == "1")
                selected_index = 0;
            else if (clean_answer == "B" || clean_answer == "2")
                selected_index = 1;
            else if (clean_answer == "C" || clean_answer == "3")
                selected_index = 2;
            else if (clean_answer == "D" || clean_answer == "4")
                selected_index = 3;

            // Validate answer
            if (selected_index == -1 || selected_index >= current_question.options.Length)
            {
                return "Please enter a valid answer (A, B, C, or D).";
            }

            // Check if answer is correct
            bool is_correct = (selected_index == current_question.correct_answer);
            string result_message = "";

            if (is_correct)
            {
                score++;
                result_message = "Correct! " + current_question.explanation;
            }
            else
            {
                string correct_option = current_question.options[current_question.correct_answer];
                result_message = "Incorrect. The correct answer was " + correct_option + ".\n" + current_question.explanation;
            }

            // Move to next question
            current_question_index++;

            // Check if quiz is complete
            if (current_question_index >= questions.Count)
            {
                quiz_active = false;
                string final_message = get_final_score_message();
                return result_message + "\n\n" + final_message;
            }

            // Return result and next question
            return result_message + "\n\n" + get_current_question();

        }//end of method

        // Get current question text
        private string get_current_question()
        {//start of method

            if (current_question_index >= questions.Count)
                return "";

            quiz_question q = (quiz_question)questions[current_question_index];
            string question_text = "Question " + (current_question_index + 1) + " of " + questions.Count + ":\n";
            question_text += q.question_text + "\n\n";

            for (int i = 0; i < q.options.Length; i++)
            {
                question_text += q.options[i] + "\n";
            }

            question_text += "\nType your answer (A, B, C, or D):";
            return question_text;

        }//end of method

        // Get final score message
        private string get_final_score_message()
        {//start of method

            int total_questions = questions.Count;
            double percentage = (double)score / total_questions * 100;

            // Determine feedback based on score
            string feedback = "";
            if (percentage >= 90)
                feedback = "Excellent! You're a cybersecurity pro!";
            else if (percentage >= 70)
                feedback = "Great job! You have good cybersecurity knowledge!";
            else if (percentage >= 50)
                feedback = "Good effort! Keep learning to improve your cybersecurity awareness.";
            else
                feedback = "Keep learning! Cybersecurity is important for everyone. Try again to improve your score.";

            return "Quiz Complete!\n" +
                   "Your Score: " + score + " out of " + total_questions + " (" + percentage.ToString("F0") + "%)\n" +
                   feedback;

        }//end of method

        // Get current question index
        public int get_current_question_index()
        {//start of method

            return current_question_index;

        }//end of method

        // Get total number of questions
        public int get_total_questions()
        {//start of method

            return questions.Count;

        }//end of method

    }//end of class

    // Class to represent a quiz question
    public class quiz_question
    {//start of class

        public string question_text { get; set; }
        public string[] options { get; set; }
        public int correct_answer { get; set; }
        public string explanation { get; set; }

    }//end of class

}//end of namespace