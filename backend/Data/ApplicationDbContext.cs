using backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Board> Boards { get; set; }
    public DbSet<TaskList> TaskLists { get; set; }
    public DbSet<Card> Cards { get; set; }

}