using Dit.EbayClone.Backend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Dit.EbayClone.Backend.Domain;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Auction> Auctions { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<AuctionImage> AuctionImages { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<AuctionHistory> AuctionHistory { get; set; }
    
    public DbSet<Notification> Notifications { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(p => p.Role)
            .HasConversion<string>();
        
        modelBuilder.Entity<User>()
            .HasIndex(p => p.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(p => p.Location)
            .WithMany()
            .HasForeignKey(p => p.LocationId)
            .IsRequired(false);

        modelBuilder.Entity<User>()
            .HasMany<RefreshToken>()
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);
        
        modelBuilder.Entity<Auction>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Auctions)
            .HasForeignKey(p => p.CategoryId);
        
        modelBuilder.Entity<Auction>()
            .HasMany(p => p.Images)
            .WithOne(p => p.Auction)
            .HasForeignKey(p => p.AuctionId);
        
        modelBuilder.Entity<Bid>()
            .HasOne(p => p.Auction)
            .WithMany()
            .HasForeignKey(p => p.AuctionId);
        
        modelBuilder.Entity<Category>()
            .HasOne(p => p.ParentCategory)
            .WithMany(p => p.SubCategories)
            .HasForeignKey(f => f.ParentCategoryId);
        
        modelBuilder.Entity<Category>()
            .HasIndex(p => p.Name)
            .IsUnique();
        
        modelBuilder.Entity<Notification>()
            .HasOne(p => p.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(p => p.UserId);
        
        modelBuilder.Entity<Bid>() 
            .HasOne(p => p.Auction)
            .WithMany(b => b.Bids)
            .HasForeignKey(p => p.AuctionId)
            .OnDelete(DeleteBehavior.NoAction);
        
        base.OnModelCreating(modelBuilder);
    }  
}