#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace UserDashboard.Models;

public class Rol
{
    [Key]
    public int RoleId { get; set; }
    public string RolName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdateAt { get; set; } = DateTime.Now;
}