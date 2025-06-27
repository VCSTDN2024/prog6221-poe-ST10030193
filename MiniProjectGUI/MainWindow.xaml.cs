using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using System.IO;
using MiniProjectGUI.Logic;
using System.Media;
using System.Windows.Controls;
using System.Windows.Media;

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
                string keyword = userMessage.ToLower();

                if (keyword.Contains("activity log") || keyword.Contains("what have you done for me") || keyword.Contains("activity") || keyword.Contains("history"))
                {
                    // Open the ChatHistory form/window
                    var historyWindow = new ChatHistory(chatHistory);
                    historyWindow.Owner = this;
                    historyWindow.ShowDialog();
                }
                else if (keyword.Contains("reminder") || keyword.Contains("remind"))
                {
                    OpenRemindersWindow();
                }
                else
                {
                    // Display user message - styled ListBoxItem (right aligned, light blue)
                    var userItem = new ListBoxItem
                    {
                        Content = $"You: {userMessage}",
                        Background = Brushes.Pink,
                        HorizontalContentAlignment = HorizontalAlignment.Right,
                        Padding = new Thickness(8),
                        Margin = new Thickness(5),
                        Foreground = Brushes.Black
                    };
                    ChatListBox.Items.Add(userItem);

                    // Get bot response
                    string botReply = chatbot.GetBotResponse(userMessage);

                    // Display bot response - styled ListBoxItem (left aligned, light gray)
                    var botItem = new ListBoxItem
                    {
                        Content = $"Bot: {botReply}",
                        Background = Brushes.LightGray,
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        Padding = new Thickness(8),
                        Margin = new Thickness(5),
                        Foreground = Brushes.Black
                    };
                    ChatListBox.Items.Add(botItem);

                    // Scroll to the latest message
                    ChatListBox.ScrollIntoView(botItem);

                    // Create ChatMessage objects for history
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

                    // Save chat history to JSON
                    SaveChatHistory();
                }

                // Clear input in all cases
                UserInputTextBox.Clear();
            }
        }


        private RemindersWindow remindersWindow = null;

        private void OpenRemindersWindow()
        {
            if (remindersWindow == null || !remindersWindow.IsVisible)
            {
                remindersWindow = new RemindersWindow();
                remindersWindow.Show();
            }
            else
            {
                remindersWindow.Activate();  // Bring to front if already open
            }
        }
        private void SaveChatHistory()
        {
            var json = JsonSerializer.Serialize(chatHistory, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("chat_history.json", json);
        }
        private void OpenQuizButton_Click(object sender, RoutedEventArgs e)
        {
            var quizWindow = new Quiz();
            quizWindow.Owner = this;
            quizWindow.ShowDialog();
        }

       

    }
}

