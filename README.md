# 🛡️ Cyber Awareness Chatbot – WPF Application

> A smart desktop assistant that educates users on common cybersecurity threats and offers prevention advice through interactive conversation.

---

## 📌 Project Overview

This application is a **C# WPF-based chatbot** designed to improve cyber awareness through real-time interaction. It mimics human-like conversation using a combination of predefined responses, sentiment detection, and topic recognition. The chatbot educates users about various cyber threats such as phishing, ransomware, and password attacks, while offering actionable safety tips.

---

## 💡 Key Features

### 🤖 Chatbot Conversation Engine
- Understands and responds to cybersecurity-related questions.
- Recognizes topics like phishing, malware, password attacks, DoS, and more.

### 📚 Topic Synonyms & Keyword Matching
- Handles a variety of phrases using synonym mapping.
- Makes the chatbot flexible to different user input styles.

### 💬 Sentiment Analysis
- Detects emotional tone (positive, neutral, negative) and responds accordingly.
- Encourages empathy in replies, creating a more human-like experience.

### 🧠 User Profile Memory
- Remembers interests and favourite topics shared by the user.
- Brings up preferences in future interactions to make the chat feel personal.

### 💾 Chat History Logging
- All messages (user and bot) are saved with timestamps in a local JSON file.
- Chat history can be viewed using a dedicated window.

### ⏰ Reminders Feature
- Allows the user to set cybersecurity-related reminders.
- Accessed through specific keywords like `"remind"` or `"reminder"`.

### 🎨 WPF Graphical User Interface
- Clean and intuitive design built using WPF and XAML.
- Layout includes chat display (`ListBox`), input area (`TextBox`), and action buttons.

### 🔊 Sound Greeting
- Optionally plays a welcome greeting sound on startup.

### 🧠 Cyber Awareness Quiz (Optional Bonus)
- A built-in quiz tests the user's knowledge on cybersecurity.
- Promotes reinforcement learning and interaction.

---

## 🛠️ Technologies Used

- **C#** – Core programming language.
- **WPF (Windows Presentation Foundation)** – GUI framework.
- **.NET** – Target platform.
- **JSON** – For storing chat logs and reminders.
- **System.Media.SoundPlayer** – For playing welcome audio.

---

## 🧑‍💻 How to Run the Application

### 1. 🛠 Prerequisites
- Visual Studio 2022 (or newer)
- .NET Desktop Development workload installed

### 2. 🧾 Setup Instructions
1. Clone or download the project repository.
2. Open the solution file (`.sln`) in Visual Studio.
3. Build the project to restore dependencies.
4. Run the application (`F5`).

----
## 🧪 How to Use the App

1. Start the app; the chatbot greets you.
2. Type in a cybersecurity-related question (e.g., `"What is phishing?"`).
3. Use `ask` to view supported topics.
4. Type `tip` followed by a topic (e.g., `"tip phishing"`) for prevention advice.
5. Type `remind` or `reminder` to set a reminder.
6. View chat history or reminders using the respective buttons.
7. Type `exit` to end the session.

