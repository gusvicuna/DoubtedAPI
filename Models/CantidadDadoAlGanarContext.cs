using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtedAPI.Models
{
    public class CantidadDadoAlGanarContext : DbContext
    {
        public DbSet<CantidadDadoAlGanar> cantidadDadoAlGanar { get; set; }

        public CantidadDadoAlGanarContext(DbContextOptions<CantidadDadoAlGanarContext> options)
             : base(options)
        {

        }
    }
}
