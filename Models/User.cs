using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtedAPI.Models {
    public class User {
        public long Id { get; set; }
        [Required]
        public string Username { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int DudasCorrectas { get; set; }
        public int DudasIncorrectas { get; set; }
        public int CalzadasCorrectas { get; set; }
        public int CalzadasIncorrectas { get; set; }
        //public List<int> DadosAlGanar { get; set; }
    }
}
