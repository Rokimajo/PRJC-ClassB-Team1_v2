using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CaveroApp.Areas.Identity.Data;


public class CaveroAppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public List<string>? RecurringDays { get; set; } = new List<string>();
}

