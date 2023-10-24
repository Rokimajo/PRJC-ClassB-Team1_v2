using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CaveroApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the CaveroAppUser class
public class CaveroAppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public List<DateTime>? RecurringDays { get; set; }
}

