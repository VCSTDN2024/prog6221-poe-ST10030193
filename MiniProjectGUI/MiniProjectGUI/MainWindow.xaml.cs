using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MiniProjectGUI.Logic;

namespace MiniProjectGUI
{
    public partial class MainWindow : Window
    {

        private UserQuery chatbot;

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
                ChatListBox.Items.Add($"You: {userMessage}");

                string botReply = chatbot.GetBotResponse(userMessage); 
                ChatListBox.Items.Add($"Bot: {botReply}");

                UserInputTextBox.Clear();
            }
        }
    }
}

