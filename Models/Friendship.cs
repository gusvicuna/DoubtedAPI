using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtedAPI.Models {
    public class Friendship {
        public long Id { get; set; }
        public bool Accepted { get; set; }

        public long InvitatorId { get; set; }
        public User InvitatorUser { get; set; }
        public long InvitatedId { get; set; }
        public User InvitatedUser { get; set; }
    }
}
