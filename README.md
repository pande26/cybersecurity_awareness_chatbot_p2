# cybersecurity_awareness_chatbot_p2 - Valerie

Project Overview
Valerie is a cybersecurity awareness chatbot developed for South African citizens as part of a national cybersecurity education campaign. This Part 2 version transforms the original console application into a modern WPF GUI application with enhanced features including keyword recognition, random responses, conversation flow, memory recall, and sentiment detection.

Features
Part 2 Features:
Feature	Description
🎨 Modern GUI	Clean WPF interface with light blue color scheme
🎵 Voice Greeting	Plays WAV audio greeting on application startup
👤 User Memory	Remembers returning users via text file storage
🔍 Keyword Recognition	Detects "password", "scam", "privacy", "phishing" topics
🎲 Random Responses	Multiple responses per topic with random selection
💬 Conversation Flow	Handles follow-up questions like "another tip", "tell me more"
😊 Sentiment Detection	Responds to emotions (worried, frustrated, confused, happy, sad, angry)
⚠️ Error Handling	Graceful handling of empty inputs and unrecognized questions
📁 Code Optimization	Separate classes for each responsibility

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
├── message_displayer.cs         # Formatted chat messages
├── voice_recording.wav          # Voice greeting audio file
├── logo.jpg                     # Logo image for GUI
└── README.md                    # This file

Class Descriptions
Class	                Purpose
MainWindow.xaml.cs	   Main application logic, event handlers, UI navigation
greet_user.cs	         Plays WAV audio greeting using SoundPlayer
respond.cs	           Stores all response data and ignore words list
response_finder.cs	   Finds random responses for specific topics (password, scam, privacy)
response_handler.cs	   Original response processing algorithm
topic_detector.cs	     Detects cybersecurity topics from user input
message_displayer.cs   Formats and displays chat messages with colors

