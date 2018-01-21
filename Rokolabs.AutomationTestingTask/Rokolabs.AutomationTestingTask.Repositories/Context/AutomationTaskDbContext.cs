using System.Data.Entity;
using Rokolabs.AutomationTestingTask.Entities;

namespace Rokolabs.AutomationTestingTask.Repositories.Context
{
	public class AutomationTaskDbContext : DbContext
	{
		public DbSet<Account> Accounts { get; set; }
		public DbSet<Event> Events { get; set; }
	}
}
