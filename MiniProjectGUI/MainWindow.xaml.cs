using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using System.IO;
using MiniProjectGUI.Logic;

namespace MiniProjectGUI
{
    public partial class MainWindow : Window
    {

        private UserQuery chatbot;
        private List<ChatMessage> chatHistory = new List<ChatMessage>();

        public MainWindow()
        {
            InitializeComponent();
            chatbot = new UserQuery("User", "");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = UserInputTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(userMessage))
            {
                // Display user message
                ChatListBox.Items.Add($"You: {userMessage}");

                // Get bot response
                string botReply = chatbot.GetBotResponse(userMessage);
                ChatListBox.Items.Add($"Bot: {botReply}");

                // Create ChatMessage objects
                var userMsg = new ChatMessage
                {
                    Sender = "User",
                    Message = userMessage,
                    Timestamp = DateTime.Now
                };

                var botMsg = new ChatMessage
                {
                    Sender = "Bot",
                    Message = botReply,
                    Timestamp = DateTime.Now
                };

                // Add to history list
                chatHistory.Add(userMsg);
                chatHistory.Add(botMsg);

                // Save the chat to json file
                SaveChatHistory();

                // Clear input
                UserInputTextBox.Clear();
            }
        }
        private void SaveChatHistory()
        {
            var json = JsonSerializer.Serialize(chatHistory, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("chat_history.json", json);
        }

        private void ViewHistory_Click(object sender, RoutedEventArgs e)
        {
            var historyWindow = new ChatHistory(chatHistory);
            historyWindow.ShowDialog();
        }
    }
}

