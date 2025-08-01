using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateManagement.Data.Entity.Notification
{
    public class Notification
{
    
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string Type { get; set; } = "info"; // info, warning, alert

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Target audience: "all", "renters", "landlords"
    public string Audience { get; set; } = "all";

    public ICollection<ApplicationUserNotification> UserNotifications { get; set; }
    }
}