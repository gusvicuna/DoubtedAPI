using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtedAPI.Models
{
    public class Player
    {
        public long Id { get; set; }
        public bool HaObligando { get; set; }
        public bool AcceptationState { get; set; }
        //revisar este array
        public int[] dados = new int[] {1,2,3,4,5 };
        public string Name { get; set; }
        public long GameId { get; set; }
        //public Game game { get; set; }
        public long UserId { get; set; }
        //public User user { get; set; }

        public string LastPType { get; set; }
        public int LastPQuantity { get; set; }
        public int LastPPinta { get; set; }
        public int TurnNumber { get; set; }
    }
}
