using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProjectGUI.Logic
{
    internal class BotResponse
    {
        public string Response { get; set; }
        public List<string> PreventionTips { get; set; }

        public BotResponse(string response, List<string> preventionTips = null)
        {
            Response = response;
            PreventionTips = preventionTips ?? new List<string>();
        }
    }
}
