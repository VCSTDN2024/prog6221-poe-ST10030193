using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjectGUI.Logic
{
     internal class Reminders
    {
        public string Description { get; set; }
        public string Info { get; set; }
        public DateTime Date { get; set; }
        public string Priority { get; set; }

        public override string ToString()
        {
            return $"{Date.ToShortDateString()} - [{Priority}] {Description}: {Info}";
        }
    }
}
