# cybersecurity_awareness_chatbot_p2 - Valerie

Project Overview
Valerie is a cybersecurity awareness chatbot developed for South African citizens as part of a national cybersecurity education campaign. This Part 2 version transforms the original console application into a modern WPF GUI application with enhanced features including keyword recognition, random responses, conversation flow, memory recall, and sentiment detection.

Features
Part 2 Features:
🎨 Modern GUI - Clean WPF interface with light blue color scheme
🎵 Voice Greeting - Plays a recorded WAV audio greeting when the application starts
👤 User Memory - Remembers returning users via text file storage
🔍 Keyword Recognition - Detects "password", "scam", "privacy", "phishing" topics
🎲 Random Responses - Multiple responses per topic with random selection
💬 Conversation Flow - Handles follow-up questions like "another tip", "tell me more"
😊 Sentiment Detection - Responds to emotions (worried, frustrated, confused, happy, sad, angry)
⚠️ Error Handling - Graceful handling of empty inputs and unrecognized questions
📁 Code Optimization - Separate classes for each responsibility

Technical Requirements Met
✅ WPF GUI Application
✅ Voice greeting using System.Media
✅ Logo image display
✅ User name collection with file memory
✅ Keyword recognition (password, scam, privacy)
✅ Random responses (3+ per topic)
✅ Conversation flow (follow-up questions)
✅ Memory and recall (user name storage)
✅ Sentiment detection (worried, frustrated, confused)
✅ Error handling for empty inputs
✅ Colored console/GUI output
✅ Object-oriented design with multiple classes
✅ GitHub version control (6+ commits)
✅ GitHub Actions CI workflow
✅ Two releases (v1.0, v2.0)

Project Structure
cybersecurity_awareness_chatbot_p2/
├── MainWindow.xaml              # GUI layout with color styling
├── MainWindow.xaml.cs           # Main logic and event handlers
├── greet_user.cs                # Voice greeting playback
├── respond.cs                   # All responses and ignore words
├── response_finder.cs           # Finds responses for specific topics
├── response_handler.cs          # Original response processing logic
├── topic_detector.cs            # Detects cybersecurity topics
├── sentiment_detector.cs        # Detects user sentiment
├── message_displayer.cs         # Formats and displays chat messages
├── voice_recording.wav          # Voice greeting audio file
├── logo.jpg                     # Logo image for GUI
└── README.md                    # This file

Class Descriptions
MainWindow.xaml.cs
Main application logic and event handlers
Handles UI navigation between grids (logo, username, chat)
Manages the send button and response processing
Coordinates all other classes

greet_user.cs
Plays a WAV audio greeting when the application launches
Uses System.Media.SoundPlayer class
Auto-detects the correct file path

respond.cs
Stores all response data (greeting, purpose, password, scam, privacy, phishing, sentiment)
Stores the ignore words list (stop words like "a", "an", "the")
Contains 8 password responses, 5 scam responses, 5 privacy responses

response_finder.cs
Finds random responses for specific cybersecurity topics
Searches the reply ArrayList for responses starting with a topic
Returns a randomly selected response

response_handler.cs
Original response processing algorithm
Splits user input into words
Searches for matching responses in the reply list

topic_detector.cs
Detects cybersecurity topics from user input
Recognizes keywords for password, scam, privacy, phishing, greeting, purpose, firewall, VPN
Returns the topic name as a string

sentiment_detector.cs
Detects user emotions from input text
Recognizes worried, frustrated, confused, happy, sad, angry
Provides empathetic responses with cybersecurity tips

message_displayer.cs
Formats and displays chat messages with colors
Shows user messages in dark green
Shows bot messages in dark blue
Shows error messages in red

Key Methods
sentiment_detector.cs:
detect_sentiment() - Detects user emotion from input
get_sentiment_response() - Returns empathetic response for detected emotion
get_random_tip() - Returns random cybersecurity tip
process_sentiment() - Handles complete sentiment response with tip

response_finder.cs:
get_response_for_topic() - Returns random response for specific topi

topic_detector.cs:
detect_topic() - Identifies cybersecurity topic from user input

message_displayer.cs:
show_user_message() - Displays user message in chat
show_bot_message() - Displays bot message in chat
show_error_message() - Displays error message in chat

Prerequisites
Windows Operating System
.NET Framework 4.7.2 or later
Visual Studio 2019/2022

Example Questions to Ask
General Questions:
"How are you?"
"What's your purpose?"
"What can I ask you about?"

Password Safety:
"Tell me about passwords"
"How do I create a strong password?"
"Password safety tips"

Scam Detection:
"What is a scam?"
"How to avoid online scams?"
"Tell me about fraud"

Privacy Protection:
"How can I protect my privacy?"
"Tell me about privacy"
"Privacy protection tips"

Phishing Awareness:
"What is phishing?"
"How to spot a phishing email?"

Follow-up Questions:
"Another tip"
"Tell me more"
"Explain more"

Sentiment Expressions:
"I'm worried about online scams"
"This is frustrating"
"I'm confused about cybersecurity"
"I'm happy with that tip"
"That makes me sad"
"I'm angry about scammers"

Video Presentation
An unlisted YouTube video presentation is available demonstrating:
Full application functionality
Code structure explanation
Logic and flow demonstration
Voice greeting playback
Keyword recognition
Random responses
Conversation flow
Sentiment detection

YouTube Video link: https://youtu.be/XlbPtzr7_-U


Author
Pandelani
Course: DISD0601 Y2
Part: 2 of 3
Institution: ROSEBANK INTERNATIONAL


