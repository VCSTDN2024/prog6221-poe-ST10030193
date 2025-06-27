using MiniProjectGUI.Logic;
using System.Collections.Generic;
using System.Windows;


namespace MiniProjectGUI
{
    /// <summary>
    /// Interaction logic for ChatHistory.xaml
    /// </summary>
    public partial class ChatHistory : Window
    {
        public ChatHistory(List<ChatMessage> history)
        {
            InitializeComponent();

            //Displays Messsage between the Bot and user with the time stamp
            foreach (var msg in history)
            {
                HistoryListBox.Items.Add($"[{msg.Timestamp:t}] {msg.Sender}: {msg.Message}");
            }
        }
    }
}
