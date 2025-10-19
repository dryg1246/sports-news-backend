using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SportsNewsAPI;

public class SportsNewsContext :  IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
   public SportsNewsContext(DbContextOptions<SportsNewsContext> options) : base(options)
   {
   }
   
   public DbSet<News> TodayNews { get; set; }
   public DbSet<News> FootballNews { get; set; }
   public DbSet<News> RecentNews { get; set; }
   public DbSet<News> BasketballNews { get; set; }
   public DbSet<News> BadmintonNews { get; set; }
   public DbSet<News> HockeyNews { get; set; }
   public DbSet<News> CyclingNews { get; set; }
   public DbSet<Article> Articles { get; set; }
   public DbSet<Sports> Sports { get; set; }
   public DbSet<Source> Sources { get; set; }
   public DbSet<Team> Teams { get; set; }
   public DbSet<MatchEvent> MatchEvents { get; set; }
   public DbSet<League> Leagues { get; set; }
   public DbSet<Match> Match { get; set; }
   public DbSet<User> User { get; set; }
   
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<News>()
         .HasMany(n => n.Articles)
         .WithOne(a => a.News)
         .HasForeignKey(a => a.NewsId)
         .OnDelete(DeleteBehavior.Cascade);

      // Source → Articles
      modelBuilder.Entity<Source>()
         .HasMany(s => s.Articles)
         .WithOne(a => a.Source)
         .HasForeignKey(a => a.SourceId)
         .OnDelete(DeleteBehavior.SetNull);
   }
   
}