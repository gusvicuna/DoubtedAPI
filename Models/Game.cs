using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtedAPI.Models
{
    public class Game
    {
        public long Id { get; set; }
        [Required]
        public string GameName { get; set; }
        public int MaxPlayers { get; set; }
        public int Round { get; set; }
        public int PlayerTurn { get; set; }
        public bool GameStarted { get; set; }
        public bool Obligando { get; set; }

        public long Player1Id { get; set; }
        public long Player2Id { get; set; }
        public long Player3Id { get; set; }
        public long Player4Id { get; set; }
        public long Player5Id { get; set; }
        public Player Player1 { get; set; }
       /* public Player Player2 { get; set; }
        public Player Player3 { get; set; }
        public Player Player4 { get; set; }
        public Player Player5 { get; set; }*/
    }
}
