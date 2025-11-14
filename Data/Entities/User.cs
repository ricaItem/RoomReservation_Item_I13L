using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomReservation_Item_I13L.Data.Entities;

[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("PasswordHash")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Column("FullName")]
    public string FullName { get; set; } = string.Empty;

    public string? Role { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}