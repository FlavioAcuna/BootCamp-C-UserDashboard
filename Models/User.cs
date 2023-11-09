#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserDashboard.Models;
public class User
{
    [Key]
    public int UserId { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    [MinLength(2, ErrorMessage = "El nombre de tener al menos 2 caracteres")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    [MinLength(2, ErrorMessage = "El apellido de tener al menos 2 caracteres")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    [EmailAddress]
    [EmailUnico]
    public string Email { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "La contraseña de contener al menos 8 caracteres")]
    public string Password { get; set; }
    [Required(ErrorMessage = "Este campo es requerido")]
    [DataType(DataType.Password)]
    [NotMapped]
    [Compare("Password")]
    public string PasswordConfirm { get; set; }
    public int RoleId { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdateAt { get; set; } = DateTime.Now;

}
