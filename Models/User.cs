using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtedAPI.Models {
    public class User {
        public long Id { get; set; }
        public string Username { get; set; }
    }
}
