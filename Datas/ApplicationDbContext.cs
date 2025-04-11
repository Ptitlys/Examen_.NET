using Examen.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Examen.Models;

public class ApplicationDbContext : IdentityDbContext<User, Role, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public DbSet<Intervention> Interventions { get; set; }
    public DbSet<Technician> Technicians { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<ServiceType> ServiceTypes { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
