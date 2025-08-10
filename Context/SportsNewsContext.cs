using Microsoft.EntityFrameworkCore;

namespace SportsNewsAPI;

public class SportsNewsContext : DbContext
{
   public SportsNewsContext(DbContextOptions<SportsNewsContext> options) : base(options)
   {
   }
   
   public DbSet<TodayNews> TodayNews { get; set; }
   public DbSet<Sports> Sports { get; set; }
   public DbSet<Team> Teams { get; set; }
   public DbSet<MatchEvent> MatchEvents { get; set; }
   public DbSet<League> Leagues { get; set; }
   public DbSet<Match> Match { get; set; }
   
}