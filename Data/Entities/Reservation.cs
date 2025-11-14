using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomReservation_Item_I13L.Data.Entities;

[Table("Reservations")]
public class Reservation
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string RoomName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [StringLength(20)]
    public string? ContactNumber { get; set; }

    [StringLength(255)]
    public string? Email { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = "Pending";

    [Required]
    [StringLength(50)]
    public string PaymentStatus { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

