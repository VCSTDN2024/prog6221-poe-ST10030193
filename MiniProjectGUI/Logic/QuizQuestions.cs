using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjectGUI.Logic
{
    internal class QuizQuestions
    {
        public string Text { get; }
        public string[] Options { get; }
        public int CorrectIndex { get; }

        public QuizQuestions(string text, string[] options, int correctIndex)
        {
            Text = text;
            Options = options;
            CorrectIndex = correctIndex;
        }
    }
}
