using System;

namespace Rokolabs.AutomationTestingTask.Entities
{
    public class Account
    {
	    public int Id { get; set; }
	    public string Login { get; set; }
	    public string Password { get; set; }
	    public Guid? SessionUserId { get; set; }
    }
}