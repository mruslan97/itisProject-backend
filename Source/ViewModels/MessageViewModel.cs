using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.ViewModels
{
    public class MessageViewModel
    {
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string Text { get; set; }
    }
}
