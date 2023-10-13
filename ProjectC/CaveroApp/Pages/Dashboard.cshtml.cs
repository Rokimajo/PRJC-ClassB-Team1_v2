using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaveroApp.Pages;

public class Dashboard : PageModel
{
    public string Message { get; set; } = "TestMessage for Dashboard!";
    public void OnGet()
    {
        Message += $" Server time is { DateTime.Now }";
    }

    public void OnPost()
    {
        Message += $": Post has been made.";
    }

    public void OnPostText()
    {
        Message += $": TextPost has been made.";
    }
}