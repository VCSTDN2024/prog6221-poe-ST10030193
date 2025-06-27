using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MiniProjectGUI.Logic;
using System.IO;
using System.Text.Json;

namespace MiniProjectGUI
{
    /// <summary>
    /// Interaction logic for RemindersWindow.xaml
    /// </summary>
    public partial class RemindersWindow : Window
    {
        private List<Reminders> reminders = new List<Reminders>();

        public RemindersWindow()
        {
            InitializeComponent();
            ReminderDatePicker.SelectedDate = DateTime.Now;
            LoadRemindersFromFile();
        }

        private void AddReminder_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
                ReminderDatePicker.SelectedDate == null ||
                PriorityComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all the fields.");
                return;
            }

            var reminder = new Reminders
            {
                Description = DescriptionTextBox.Text.Trim(),
                Info = InfoTextBox.Text.Trim(),
                Date = ReminderDatePicker.SelectedDate.Value,
                Priority = ((ComboBoxItem)PriorityComboBox.SelectedItem).Content.ToString()
            };

            reminders.Add(reminder);
            ReminderListBox.Items.Add(reminder);

            SaveRemindersToFile();  // Save immediately after adding

            // Clear inputs
            DescriptionTextBox.Text = "";
            InfoTextBox.Text = "";
            ReminderDatePicker.SelectedDate = DateTime.Now;
            PriorityComboBox.SelectedIndex = -1;
        }

        private void RemoveReminder_Click(object sender, RoutedEventArgs e)
        {
            if (ReminderListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a reminder to remove.");
                return;
            }

            var selectedReminder = (Reminders)ReminderListBox.SelectedItem;

            reminders.Remove(selectedReminder);
            ReminderListBox.Items.Remove(selectedReminder);

            SaveRemindersToFile();  // Save immediately after removal
        }

        private void SaveRemindersToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(reminders);
                File.WriteAllText("reminders.json", json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving reminders: {ex.Message}");
            }
        }

        private void LoadRemindersFromFile()
        {
            try
            {
                if (File.Exists("reminders.json"))
                {
                    var json = File.ReadAllText("reminders.json");
                    reminders = JsonSerializer.Deserialize<List<Reminders>>(json);

                    ReminderListBox.Items.Clear();
                    foreach (var reminder in reminders)
                    {
                        ReminderListBox.Items.Add(reminder);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reminders: {ex.Message}");
            }
        }
    }
}
