using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CaveroApp.Areas.Identity.Data;


/// <summary>
/// Represents a user in the Cavero application.
/// </summary>
public class CaveroAppUser : IdentityUser
{
    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the list of recurring days for the user.
    /// This property can be null, which means that the user does not have any recurring days.
    /// </summary>
    public List<DateTime>? RecurringDays { get; set; }
}