using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DoubtedAPI.Models {
    public class FriendshipContext : DbContext {
        public FriendshipContext(DbContextOptions<FriendshipContext> options)
            : base(options) {

        }

        public DbSet<Friendship> Friendships { get; set; }
    }
}
