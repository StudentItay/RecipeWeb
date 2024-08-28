using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Models;

namespace WebApplication1.Areas.Identity.Data;

public class WebApplication1Context : IdentityDbContext<WebApplication1User>
{
    public WebApplication1Context(DbContextOptions<WebApplication1Context> options)
        : base(options)
    {
    }

    public DbSet<AddComment> AddComment { get; set; }
    public DbSet<FavoriteModel> FavoriteModel { get; set; }

    public DbSet<SubscriberModel> SubscriberModel { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());


    }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<WebApplication1User>
{
    public void Configure(EntityTypeBuilder<WebApplication1User> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired(); ;

        builder.Property(x => x.LastName).HasMaxLength(100).IsRequired(); ;
    }
}

public class CommentModelConfiguration : IEntityTypeConfiguration<AddComment>
{

    public void Configure(EntityTypeBuilder<AddComment> builder)
    {
        builder.HasKey(x => x.CommentID);
      
        builder.Property(x => x.RecipeID).IsRequired();
        builder.Property(x => x.UserName).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.Rating).IsRequired();
        builder.Property(x => x.CommentText).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
    }
}

public class FavoriteModelConfiguration : IEntityTypeConfiguration<FavoriteModel>
{
    public void Configure(EntityTypeBuilder<FavoriteModel> builder)
    {
        builder.HasKey(x => x.FavoriteId);

        builder.Property(x => x.RecipeId).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.UserName).HasMaxLength(100);
        builder.Property(x => x.Date).IsRequired().HasDefaultValueSql("GETDATE()");
        builder.Property(x => x.FavoriteIcon).IsRequired();
        builder.Property(x => x.LikeIcon).IsRequired();
    }
}

public class SubscriberModelConfiguration : IEntityTypeConfiguration<SubscriberModel>
{
    public void Configure(EntityTypeBuilder<SubscriberModel> builder)
    {
        builder.HasKey(x => x.SubscriberID);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
        builder.Property(x => x.SubscriptionDate).IsRequired().HasDefaultValueSql("GETDATE()");
    }
}