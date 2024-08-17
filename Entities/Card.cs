using System;
using System.Collections.Generic;


namespace FlashTimes.Models;

public class Card{

        [Key]
        public int CardID { get; set; }

        public string Question { get; set; }
        public string Answer { get; set; }
        public int UserID { get; set; }
        
        public User user{ get; set; }

        
}

