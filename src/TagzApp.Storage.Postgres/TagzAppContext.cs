using Microsoft.EntityFrameworkCore;

namespace TagzApp.Storage.Postgres;

public class TagzAppContext : DbContext
{
	private readonly IConfiguration _Configuration;
	private bool _InMemory;

	public TagzAppContext() { }

	public TagzAppContext(IConfiguration configuration)
	{
		_Configuration = configuration;
	}

	public TagzAppContext(DbContextOptions<TagzAppContext> options) : base(options)
	{
	}

	public TagzAppContext(DbContextOptions<TagzAppContext> options, IConfiguration configuration) : base(options)
	{
		_Configuration = configuration;
	}

	public DbSet<PgContent> Content { get; set; }

	public DbSet<PgModerationAction> ModerationActions { get; set; }

	public DbSet<PgBlockedUser> BlockedUsers { get; set; }

	//public DbSet<Settings> Settings => Set<Settings>();

	public DbSet<Tag> TagsWatched { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{

		modelBuilder.Entity<PgContent>().HasAlternateKey(c => new { c.Provider, c.ProviderId });
		modelBuilder.Entity<PgContent>().HasOne(c => c.ModerationAction).WithOne(m => m.Content).HasForeignKey<PgModerationAction>(m => m.ContentId);

		modelBuilder.Entity<PgModerationAction>().HasAlternateKey(c => new { c.Provider, c.ProviderId });

		modelBuilder.Entity<Tag>().Property(t => t.Text)
			.HasMaxLength(50)
			.IsRequired();

		base.OnModelCreating(modelBuilder);

	}

}
