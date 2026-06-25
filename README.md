# Cybersecurity Awareness Chatbot - Valerie (Part 3/POE)

Project Overview
This is a Cybersecurity Awareness Chatbot developed for South African citizens as part of a cybersecurity education campaign. The chatbot, named "Valerie", provides interactive education about online safety practices including password safety, scam detection, privacy protection, and phishing awareness. This Part 3/POE version includes a modern WPF GUI application with advanced features including Task Management with SQL LocalDB, Cybersecurity Quiz, NLP Simulation, and Activity Logging.

Features
Part 3 Features:
Feature	Description
🎨 Modern GUI	Clean WPF interface with gradient backgrounds and professional styling
🎵 Voice Greeting	Plays WAV audio greeting on application startup
👤 User Memory	Remembers returning users via text file storage
🔍 Keyword Recognition	Detects "password", "scam", "privacy", "phishing" topics
🎲 Random Responses	Multiple responses per topic with random selection
💬 Conversation Flow	Handles follow-up questions like "another tip", "tell me more"
😊 Sentiment Detection	Responds to emotions (worried, frustrated, confused, happy, sad, angry)
📋 Task Manager	Add, view, complete, and delete cybersecurity tasks with SQL LocalDB
🔔 Task Reminders	Set reminders for tasks with date calculation
❓ Cybersecurity Quiz	12-question quiz with immediate feedback and scoring
🧠 NLP Simulation	Recognizes different phrasing for task and quiz commands
📝 Activity Log	Tracks all user actions with timestamps
⚠️ Error Handling	Graceful handling of empty inputs and unrecognized questions
📁 Code Optimization	Separate classes for each responsibility

Technical Requirements Met
✅ WPF GUI Application
✅ Voice greeting using System.Media
✅ Logo image display in GUI
✅ User name collection with file memory
✅ Keyword recognition (password, scam, privacy, phishing)
✅ Random responses (3+ per topic)
✅ Conversation flow (follow-up questions)
✅ Memory and recall (user name storage)
✅ Sentiment detection (worried, frustrated, confused, happy, sad, angry)
✅ Task Manager with SQL LocalDB integration
✅ Task reminders with date calculation
✅ Cybersecurity Quiz (12 questions with scoring)
✅ NLP Simulation (keyword-based command recognition)
✅ Activity Log with timestamps
✅ Error handling for empty inputs
✅ Colored GUI output
✅ Object-oriented design with multiple classes
✅ GitHub version control (6+ commits)
✅ GitHub Actions CI workflow
✅ Three releases (v1.0, v2.0, v3.0)

Project Structure
cybersecurity_awareness_chatbot_p2/
├── MainWindow.xaml              # GUI layout with all grids
├── MainWindow.xaml.cs           # Main logic and event handlers
├── greet_user.cs                # Voice greeting playback
├── respond.cs                   # All responses and ignore words
├── response_finder.cs           # Finds responses for specific topics
├── response_handler.cs          # Original response processing logic
├── topic_detector.cs            # Detects cybersecurity topics
├── sentiment_detector.cs        # Detects user sentiment
├── message_displayer.cs         # Formats and displays chat messages
├── database_helper.cs           # SQL LocalDB operations for tasks
├── task_manager.cs              # Task management logic
├── quiz_manager.cs              # Quiz management with 12 questions
├── nlp_processor.cs             # Natural language processing simulation
├── activity_logger.cs           # Activity logging with timestamps
├── voice_recording.wav          # Voice greeting audio file
├── logo.jpg                     # Logo image for GUI
└── README.md                    # This file

Class Descriptions
Class	Purpose
MainWindow.xaml.cs	Main application logic, event handlers, UI navigation
greet_user.cs	Plays WAV audio greeting using SoundPlayer
respond.cs	Stores all response data and ignore words list
response_finder.cs	Finds random responses for specific topics
response_handler.cs	Original response processing algorithm
topic_detector.cs	Detects cybersecurity topics from user input
sentiment_detector.cs	Detects user emotions and provides empathetic responses
message_displayer.cs	Formats and displays chat messages with colors
database_helper.cs	Handles all SQL LocalDB operations (insert, load, delete, complete)
task_manager.cs	Manages task creation, viewing, completion, and deletion
quiz_manager.cs	Manages 12 cybersecurity quiz questions with scoring
nlp_processor.cs	Simulates NLP using keyword detection for commands
activity_logger.cs	Logs all user actions with timestamps

Key Methods
database_helper.cs:
test_connection() - Tests database connection
ensure_database_setup() - Creates database and table if not exist
insert_task() - Inserts a new task into database
load_tasks() - Loads all tasks from database
delete_task() - Deletes a task by ID
complete_task() - Marks a task as completed
count_tasks() - Counts pending tasks

task_manager.cs:
process_task_command() - Processes task-related commands
handle_add_task() - Handles adding a new task
process_reminder_response() - Processes reminder responses
process_task_click() - Processes double-click on task
get_pending_task_count() - Returns count of pending tasks

quiz_manager.cs:
start_quiz() - Starts the quiz
process_answer() - Processes user's answer
get_current_question() - Returns current question
get_final_score_message() - Returns final score with feedback
get_total_questions() - Returns total number of questions

nlp_processor.cs:
process_nlp() - Processes natural language input
contains_keyword() - Checks if input contains keywords
ExtractTaskName() - Extracts task name from various phrasings

activity_logger.cs:
add_log() - Adds a log entry with timestamp
get_recent_activity() - Returns last 10 log entries
get_all_activity() - Returns all log entries

Prerequisites
Windows Operating System
.NET Framework 4.7.2 or later
Visual Studio 2019/2022
SQL LocalDB (comes with Visual Studio)

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

Task Manager Commands
Command	Description
"Add task to [task name]"	Creates a new task
"Show my tasks"	Displays all tasks
"Complete task [task name]"	Marks task as complete
"Delete task [task name]"	Deletes a task
"Remind me to [task] in X days"	Adds task with reminder

Activity Log
The bot logs all significant actions with timestamps:
Action	Logged When
Application started	User logs in
New user registered	First time user
Returning user	Existing user
User asked question	Every user question
Sentiment detected	When sentiment is found
NLP command processed	When NLP handles a command
Task added	When user adds a task
Task completed	When user completes a task
Task deleted	When user deletes a task
Quiz started	When user starts quiz
Quiz completed	When quiz is finished with score

Releases
Release	Tag	Description
Part 1	v1.0	Console version
Part 2	v2.0	WPF GUI version
Part 3	v3.0	Complete version with Task Manager, Quiz, NLP, and Activity Log

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
Task Manager with database
Cybersecurity Quiz
NLP Simulation
Activity Log

YouTube Video link: https://youtu.be/qzE6slRxDqQ

Author
Pandelani
Course: DISD0601 Y2
Part: 3 of 3
Institution: ROSEBANK INTERNATIONAL


