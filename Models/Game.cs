using Newtonsoft.Json;
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
        [Required]
        public string PassWord { get; set; }
        public int MaxPlayers { get; set; }
        public int Round { get; set; }
        public int PlayerTurn { get; set; }
        public bool GameStarted { get; set; }
        public bool Obligando { get; set; }
        public bool NewRound { get; set; }
        public bool CanCalzar { get; set; }

        [JsonIgnore]
        public List<Player> players { get; set; }
    }
}
