using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MiniProjectGUI.Logic
{
    internal class UserQuery
    {
        private UserProfile user = new UserProfile();
        private Dictionary<string, BotResponse> botResponses;
        private Dictionary<string, string> topicKeywords;
        private Random r = new Random();

        public UserQuery(string name, string initialQuestion)
        {
            InitializeResponses();
            InitializeKeywords();
        }

        public string GetBotResponse(string input)
        {
            input = input.ToLower().Trim();
            string responseText = "";

            if (string.IsNullOrWhiteSpace(input))
                return "Please type something.";

            if (input.Contains("exit"))
                return "👋 Goodbye! Stay safe online.";

            if (input.Contains("how are you"))
                return "I am good thanks for asking how are you ?.";

            if (input.Contains("ask"))
            {
                var topics = string.Join("\n- ", botResponses.Keys.OrderBy(k => k));
                return "📚 You can ask about:\n- " + topics;
            }

            if (input.Contains("tip"))
            {
                foreach (var pair in botResponses)
                {
                    if (input.Contains(pair.Key.ToLower()) && pair.Value.PreventionTips?.Any() == true)
                    {
                        var tip = pair.Value.PreventionTips[r.Next(pair.Value.PreventionTips.Count)];
                        return $"💡 Tip on {pair.Key}: {tip}";
                    }
                }
                return "I couldn't find a tip related to that topic.";
            }

            string profileUpdateMessage = TryUpdateUserProfile(input);
            if (!string.IsNullOrEmpty(profileUpdateMessage))
                return profileUpdateMessage;

            foreach (var keyword in topicKeywords.Keys)
            {
                if (input.Contains(keyword))
                {
                    string mainTopic = topicKeywords[keyword];
                    if (botResponses.ContainsKey(mainTopic))
                    {
                        var botReply = botResponses[mainTopic];
                        var tipText = "";
                        if (botReply.PreventionTips?.Any() == true)
                        {
                            tipText = "\nPrevention Tips:\n" + string.Join("\n- ", botReply.PreventionTips);
                        }
                        return $"{botReply.Response}{(string.IsNullOrWhiteSpace(tipText) ? "" : "\n" + tipText)}";
                    }
                }
            }

            return HandleSentimentOrUnknown(input);
        }

        private string HandleSentimentOrUnknown(string input)
        {
            string sentiment = AnalyzeSentiment(input);
            string fallback = "🤔 I'm not sure I understand. Try asking about a specific topic or type 'ask' to see the list.";

            if (!string.IsNullOrEmpty(user.Interest))
            {
                if (sentiment == "positive")
                    return $"Nice! Doing more with {user.Interest} must be fun.";
                if (sentiment == "negative")
                    return $"Sorry you're feeling down. Maybe {user.Interest} can lift your mood.";
            }

            if (!string.IsNullOrEmpty(user.FavouriteTopic) && r.Next(2) == 0)
                return $"Remember, your favourite topic is {user.FavouriteTopic} 😊.";

            if (sentiment == "positive")
                return "Glad to hear that! 😄";
            if (sentiment == "negative")
                return "I’m here for you. Let me know how I can help.";

            return fallback;
        }

        private string TryUpdateUserProfile(string input)
        {
            input = input.ToLower();
            string confirmation = "";

            var interestMatch = System.Text.RegularExpressions.Regex.Match(input, @"\b(i love|i like|i enjoy|interest)\b\s+(.*)");
            if (interestMatch.Success)
            {
                string interest = interestMatch.Groups[2].Value.Trim(new char[] { '.', '!', '?', ' ' });
                user.Interest = interest;
                confirmation += $"Got it! I'll remember that you're interested in {interest}.\n";
            }

            var topicMatch = System.Text.RegularExpressions.Regex.Match(input, @"\bmy (favourite|favorite) topic is\b\s+(.*)");
            if (topicMatch.Success)
            {
                string topic = topicMatch.Groups[2].Value.Trim(new char[] { '.', '!', '?', ' ' });
                user.FavouriteTopic = topic;
                confirmation += $"Thanks! I'll remember your favorite topic is {topic}.";
            }

            return confirmation.Trim();
        }

        private string AnalyzeSentiment(string message)
        {
            var positiveWords = new[] { "good", "great", "happy", "awesome", "love", "fantastic", "amazing", "excited" };
            var negativeWords = new[] { "bad", "sad", "angry", "hate", "terrible", "awful", "upset" };

            message = message.ToLower();
            int score = 0;

            foreach (var word in positiveWords)
                if (message.Contains(word)) score++;

            foreach (var word in negativeWords)
                if (message.Contains(word)) score--;

            if (score > 0) return "positive";
            if (score < 0) return "negative";
            return "neutral";
        }

        private void InitializeResponses()
        {
            botResponses = new Dictionary<string, BotResponse>
    {
        { "purpose", new BotResponse("I'm here to help you learn about cybersecurity threats and provide guidance on avoiding common traps :)") },

        { "phishing", new BotResponse(
            "A phishing attack is when a scammer tries to trick you into giving up sensitive info—like passwords, credit card numbers, or personal details—by pretending to be someone you trust.",
            new List<string>
            {
                "Don’t click on links or download attachments from sketchy emails.",
                "Check the sender’s email address carefully—it might look 'off'.",
                "Look for weird grammar or urgent scare tactics.",
                "Always type website URLs manually instead of clicking email links.",
                "Use two-factor authentication wherever possible."
            })
        },

        { "password", new BotResponse(
            "Password attacks are when cybercriminals try to guess or steal your password to break into your accounts. They may use brute force or stolen passwords from breaches.",
            new List<string>
            {
                "Use strong, unique passwords for every account.",
                "Turn on two-factor authentication (2FA).",
                "Don’t use obvious passwords like '123456' or 'password'.",
                "Use a password manager to keep track of passwords.",
                "Change passwords regularly—especially after a breach."
            })
        },

        { "suspicious links", new BotResponse(
            "Suspicious links may appear legit but lead to malicious sites. They often come via email, social media, or messages.",
            new List<string>
            {
                "Hover over links before clicking to preview the URL.",
                "Avoid links with weird characters or lots of hyphens.",
                "Use a link expander for shortened URLs.",
                "Avoid clicking links from unknown senders.",
                "Visit official sites directly instead of using email links."
            })
        },

        { "ransomware", new BotResponse(
            "Ransomware is malicious software that blocks access to your data unless a ransom is paid.",
            new List<string>
            {
                "Back up important files and keep backups offline.",
                "Avoid clicking unknown links or attachments.",
                "Keep your system updated with security patches.",
                "Use reputable antivirus software.",
                "Enable real-time protection features."
            })
        },

        { "denial of services", new BotResponse(
            "A Denial of Service attack floods a system with traffic, making it slow or unusable.",
            new List<string>
            {
                "Use firewalls and intrusion detection systems.",
                "Employ rate limiting and traffic filtering.",
                "Partner with DDoS protection services.",
                "Patch system vulnerabilities quickly."
            })
        },

        { "malware", new BotResponse(
            "Malware is software designed to harm, exploit, or control systems. It includes viruses, worms, spyware, and ransomware.",
            new List<string>
            {
                "Install and update a trusted antivirus.",
                "Don’t download software from unknown sources.",
                "Avoid suspicious links or popups.",
                "Keep your OS and software patched."
            })
        },

        { "spamming", new BotResponse(
            "Spam is unwanted digital junk mail. Some spam can hide malicious links or downloads.",
            new List<string>
            {
                "Never click on links or download attachments from unknown senders.",
                "Check for suspicious or unusual email addresses.",
                "Ignore urgent or threatening messages.",
                "Use spam filters and report suspicious emails.",
                "Go directly to the official site, not the provided link."
            })
        },

        { "spyware", new BotResponse(
            "Spyware is malicious software that secretly monitors your activities and sends data to attackers without your knowledge.",
            new List<string>
            {
                "Avoid downloading software from untrusted sources.",
                "Keep your operating system and apps updated.",
                "Use reputable anti-spyware or antivirus software.",
                "Be cautious when clicking pop-ups or suspicious links."
            })
        },

        { "man in the middle", new BotResponse(
            "A Man-in-the-Middle attack intercepts communication between two parties to steal or alter information.",
            new List<string>
            {
                "Avoid using public Wi-Fi for sensitive transactions.",
                "Use VPNs to encrypt your internet connection.",
                "Make sure websites use HTTPS.",
                "Be cautious of unexpected certificate warnings."
            })
        },

        { "sql injection", new BotResponse(
            "SQL Injection attacks exploit vulnerabilities in databases to manipulate or steal data.",
            new List<string>
            {
                "Always validate and sanitize user inputs in applications.",
                "Use prepared statements and parameterized queries.",
                "Keep software and database systems updated.",
                "Limit database permissions and access."
            })
        },

        { "zero day", new BotResponse(
            "A Zero-Day attack exploits unknown security vulnerabilities before they are patched.",
            new List<string>
            {
                "Keep software updated regularly.",
                "Use security solutions that detect unusual behaviors.",
                "Implement network segmentation and monitoring.",
                "Have an incident response plan ready."
            })
        },

        { "social engineering", new BotResponse(
            "Social Engineering tricks people into giving away confidential information or performing actions that compromise security.",
            new List<string>
            {
                "Be skeptical of unsolicited requests for sensitive info.",
                "Verify identities before sharing information.",
                "Educate yourself about common social engineering tactics.",
                "Report suspicious contacts to your IT or security team."
            })
        },

        { "botnet", new BotResponse(
            "A Botnet is a network of infected devices controlled remotely by attackers to launch attacks or send spam.",
            new List<string>
            {
                "Keep all devices updated with the latest security patches.",
                "Use strong, unique passwords for device accounts.",
                "Avoid clicking unknown or suspicious links.",
                "Install and maintain antivirus and firewall protection."
            })
        },

        { "drive by download", new BotResponse(
            "Drive-by downloads automatically install malware when visiting compromised or malicious websites.",
            new List<string>
            {
                "Keep your web browser and plugins updated.",
                "Use security extensions that block malicious scripts.",
                "Avoid visiting suspicious or untrusted websites.",
                "Use antivirus software that scans downloads."
            })
        },

        { "credential stuffing", new BotResponse(
            "Credential stuffing uses stolen username/password pairs to try to access multiple accounts.",
            new List<string>
            {
                "Use unique passwords for every account.",
                "Enable two-factor authentication (2FA).",
                "Monitor your accounts for unusual activity.",
                "Change passwords immediately if a breach occurs."
            })
        },

        { "hello", new BotResponse("Hello!! What can I help you with?") },
        { "hey", new BotResponse("Hi :)!") },
    };
        }

        private void InitializeKeywords()
        {
            topicKeywords = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "purpose", "purpose" },

        { "phishing", "phishing" },
        { "phishing attack", "phishing" },
        { "email scam", "phishing" },

        { "password", "password" },
        { "password attacks", "password" },
        { "weak password", "password" },

        { "suspicious links", "suspicious links" },
        { "malicious links", "suspicious links" },

        { "ransomware", "ransomware" },

        { "denial of service", "denial of services" },
        { "dos attack", "denial of services" },
        { "ddos", "denial of services" },

        { "malware", "malware" },

        { "spamming", "spamming" },
        { "spam", "spamming" },

        { "hello", "hello" },
        { "hi", "hello" },
        { "hey", "hey" },
        { "spyware", "spyware" },
        { "man in the middle", "man in the middle" },
        { "mitm", "man in the middle" },
        { "middle attack", "man in the middle" },

        { "sql injection", "sql injection" },
        { "sql attack", "sql injection" },

        { "zero day", "zero day" },
        { "zero-day", "zero day" },

        { "social engineering", "social engineering" },
        { "social hack", "social engineering" },
        { "phishing scam", "social engineering" },

        { "botnet", "botnet" },
        { "bot net", "botnet" },

        { "drive by download", "drive by download" },
        { "drive-by download", "drive by download" },
        { "driveby", "drive by download" },

        { "credential stuffing", "credential stuffing" },
        { "credential attack", "credential stuffing" },
        { "account stuffing", "credential stuffing" }
    };
        }
    }
}
