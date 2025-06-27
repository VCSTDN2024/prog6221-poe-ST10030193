using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MiniProjectGUI.Logic;

namespace MiniProjectGUI
{
    /// <summary>
    /// Interaction logic for Quiz.xaml
    /// </summary>
    public partial class Quiz : Window
    {
        private List<QuizQuestions> questions = new List<QuizQuestions>();
        private List<QuizQuestions> currentQuizQuestions = new List<QuizQuestions>();

        public Quiz()
        {
            InitializeComponent();
            LoadQuestions();
            GenerateQuizUI();
        }

        // Randomly select a subset of questions
        private List<QuizQuestions> GetRandomQuestions(int count)
        {
            var random = new Random();
            return questions.OrderBy(q => random.Next()).Take(count).ToList();
        }

        private void GenerateQuizUI()
        {
            QuizPanel.Children.Clear(); // Clear existing UI
            currentQuizQuestions = GetRandomQuestions(10); // Pick 10 random questions

            for (int i = 0; i < currentQuizQuestions.Count; i++)
            {
                var q = currentQuizQuestions[i];
                var questionStack = new StackPanel { Margin = new Thickness(0, 10, 0, 10) };

                var questionText = new TextBlock
                {
                    Text = $"{i + 1}. {q.Text}",
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.Wrap
                };
                questionStack.Children.Add(questionText);

                var group = new StackPanel();
                for (int j = 0; j < q.Options.Length; j++)
                {
                    var rb = new RadioButton
                    {
                        Content = q.Options[j],
                        GroupName = $"Q{i}",
                        Tag = j
                    };
                    group.Children.Add(rb);
                }

                questionStack.Children.Add(group);
                QuizPanel.Children.Add(questionStack);
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            int score = 0;

            for (int i = 0; i < currentQuizQuestions.Count; i++)
            {
                var questionPanel = (StackPanel)QuizPanel.Children[i];
                var answersStack = (StackPanel)questionPanel.Children[1];

                bool answeredCorrectly = false;

                foreach (RadioButton rb in answersStack.Children)
                {
                    if (rb.IsChecked == true)
                    {
                        if ((int)rb.Tag == currentQuizQuestions[i].CorrectIndex)
                        {
                            score++;
                            answeredCorrectly = true;
                        }
                        break;
                    }
                }

                if (!answeredCorrectly)
                {
                    int correctIndex = currentQuizQuestions[i].CorrectIndex;
                    string correctAnswer = ((RadioButton)answersStack.Children[correctIndex]).Content.ToString();

                    // Remove previous correct answers (if retaking)
                    for (int j = answersStack.Children.Count - 1; j >= 0; j--)
                    {
                        if (answersStack.Children[j] is TextBlock tb && tb.Text.StartsWith("Correct answer:"))
                        {
                            answersStack.Children.RemoveAt(j);
                        }
                    }

                    var correctAnswerTextBlock = new TextBlock
                    {
                        Text = $"Correct answer: {correctAnswer}",
                        Foreground = Brushes.Red,
                        Margin = new Thickness(0, 5, 0, 0)
                    };

                    answersStack.Children.Add(correctAnswerTextBlock);
                }
            }

            ResultText.Text = $"You scored {score} out of {currentQuizQuestions.Count}";

            if (score >= 5)
            {
                ResultText.Text += "\nGreat job! You're a cybersecurity pro!!!";
            }
            else
            {
                ResultText.Text += "\nKeep learning to stay safe online.";
            }
        }

        private void LoadQuestions()
        {
            questions.Add(new QuizQuestions("What does 'phishing' mean?",
                new[] { "A type of firewall", "A social engineering attack", "An antivirus software", "A password manager" }, 1));
            questions.Add(new QuizQuestions("Which of the following is a strong password?",
                new[] { "123456", "password", "Qw!7$zL9@", "abcdefg" }, 2));
            questions.Add(new QuizQuestions("What should you do if you receive a suspicious email?",
                new[] { "Reply asking for more info", "Click the link to check", "Ignore or report it", "Forward it to colleagues" }, 2));
            questions.Add(new QuizQuestions("What is multi-factor authentication (MFA)?",
                new[] { "Using one password only", "Biometric login only", "Two or more methods to verify identity", "Logging in via VPN" }, 2));
            questions.Add(new QuizQuestions("A VPN is used to:",
                new[] { "Make the internet faster", "Bypass cyber laws", "Securely connect to a network", "Hack into systems" }, 2));
            questions.Add(new QuizQuestions("Which file extension is most suspicious?",
                new[] { ".jpg", ".exe", ".docx", ".pdf" }, 1));
            questions.Add(new QuizQuestions("What is a firewall?",
                new[] { "An actual wall", "A virus", "A network security device", "A search engine" }, 2));
            questions.Add(new QuizQuestions("Social engineering attacks target:",
                new[] { "Network cables", "Human behavior", "Software bugs", "Encryption keys" }, 1));
            questions.Add(new QuizQuestions("Which action helps protect your personal info online?",
                new[] { "Using public Wi-Fi", "Sharing login details", "Regular software updates", "Reusing passwords" }, 2));
            questions.Add(new QuizQuestions("Which of these is NOT a cybersecurity threat?",
                new[] { "Ransomware", "Phishing", "Firewall", "Spyware" }, 2));
            questions.Add(new QuizQuestions("What is ransomware?",
        new[] { "A backup tool", "Malware that locks files for payment", "A firewall upgrade", "A browser plugin" }, 1));
            questions.Add(new QuizQuestions("Which is the safest way to manage passwords?",
                new[] { "Use the same one everywhere", "Write them on paper", "Use a password manager", "Use your birthdate" }, 2));
            questions.Add(new QuizQuestions("What does HTTPS signify?",
                new[] { "High-Tech Protocol", "Secured communication", "Hyper Transfer", "Hacked Transmission" }, 1));
            questions.Add(new QuizQuestions("What is a common sign of a phishing site?",
                new[] { "No ads", "Correct URL", "HTTPS only", "Misspelled domain name" }, 3));
            questions.Add(new QuizQuestions("What is the role of antivirus software?",
                new[] { "Create viruses", "Manage documents", "Detect and remove malware", "Speed up browsing" }, 2));
            questions.Add(new QuizQuestions("What is the best way to verify a link in an email?",
                new[] { "Click it", "Hover over it", "Ignore it", "Open it on phone" }, 1));
            questions.Add(new QuizQuestions("Which of the following is a type of malware?",
                new[] { "Trojan", "Firewall", "Patch", "Protocol" }, 0));
            questions.Add(new QuizQuestions("Why should software be regularly updated?",
                new[] { "For new color schemes", "To prevent bugs", "For better sound", "For security patches" }, 3));
            questions.Add(new QuizQuestions("A secure password should:",
                new[] { "Be short and easy", "Use common words", "Include symbols and numbers", "Use only lowercase" }, 2));
            questions.Add(new QuizQuestions("Two-factor authentication requires:",
                new[] { "Only a fingerprint", "Password and another method", "One password", "Just a username" }, 1));

            questions.Add(new QuizQuestions("Which of these is a phishing attempt?",
                new[] { "An email from IT asking to verify your password", "An email from a friend", "A calendar invite", "A browser update" }, 0));
            questions.Add(new QuizQuestions("Spyware is designed to:",
                new[] { "Speed up your PC", "Protect your files", "Secretly gather data", "Install games" }, 2));
            questions.Add(new QuizQuestions("A DDoS attack attempts to:",
                new[] { "Fix network issues", "Speed up servers", "Overwhelm systems with traffic", "Hack into Wi-Fi" }, 2));
            questions.Add(new QuizQuestions("What is the main function of encryption?",
                new[] { "Hide passwords", "Secure data by making it unreadable", "Delete old files", "Speed up downloads" }, 1));
            questions.Add(new QuizQuestions("Which of these passwords is most secure?",
                new[] { "sunshine123", "John1985", "M!k3@L0u#9Z", "abc123" }, 2));
            questions.Add(new QuizQuestions("Cookies in browsers are used to:",
                new[] { "Track and store session info", "Cook food", "Secure passwords", "Detect viruses" }, 0));
            questions.Add(new QuizQuestions("What is shoulder surfing?",
                new[] { "Typing passwords too fast", "Looking over someone’s shoulder to steal information", "Losing your phone", "Sharing your password" }, 1));
            questions.Add(new QuizQuestions("Public Wi-Fi is often risky because:",
                new[] { "It’s slow", "It lacks encryption", "It has ads", "It’s free" }, 1));
            questions.Add(new QuizQuestions("Which of the following is a secure practice?",
                new[] { "Using default passwords", "Opening unknown attachments", "Enabling MFA", "Clicking popups" }, 2));
            questions.Add(new QuizQuestions("What is the purpose of a digital certificate?",
                new[] { "Scan devices", "Authenticate a website’s identity", "Hide files", "Hack software" }, 1));

            questions.Add(new QuizQuestions("A botnet is:",
                new[] { "A software update", "A group of infected devices", "A firewall tool", "A type of Wi-Fi" }, 1));
            questions.Add(new QuizQuestions("Which one is a secure website address?",
                new[] { "http://bank.com", "https://secure.bank.com", "ftp://files.net", "http://login.com" }, 1));
            questions.Add(new QuizQuestions("An example of social engineering is:",
                new[] { "SQL injection", "Man-in-the-middle", "Impersonation call to get credentials", "Firewall breach" }, 2));
            questions.Add(new QuizQuestions("Zero-day vulnerability means:",
                new[] { "Known and patched", "Exploited before a patch is available", "Bug introduced by user", "Old software bug" }, 1));
            questions.Add(new QuizQuestions("What is the main risk of using the same password on all accounts?",
                new[] { "It’s convenient", "Increased risk if one account is breached", "Hard to remember", "Slower login" }, 1));
            questions.Add(new QuizQuestions("The best way to back up data is:",
                new[] { "Store it on desktop", "Email it to yourself", "Use external or cloud backup", "Keep it on USB only" }, 2));
            questions.Add(new QuizQuestions("How often should you change your passwords?",
                new[] { "Never", "Once a year", "Only when compromised", "Regularly or when breached" }, 3));
            questions.Add(new QuizQuestions("Which device is safest for sensitive data?",
                new[] { "Shared office PC", "Public kiosk", "Personal secured device", "Internet cafe PC" }, 2));
            questions.Add(new QuizQuestions("What is the first step after a data breach?",
                new[] { "Ignore it", "Notify IT/security team", "Change email", "Tell social media" }, 1));
            questions.Add(new QuizQuestions("Which of these should you NOT do?",
                new[] { "Log out of sessions", "Click unknown links", "Use complex passwords", "Enable MFA" }, 1));
        }

        private void RetakeQuiz_Click(object sender, RoutedEventArgs e)
        {
            GenerateQuizUI();
            ResultText.Text = "";
        }
    }

}


