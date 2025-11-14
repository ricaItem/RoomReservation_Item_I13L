using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomReservation_Item_I13L.Data.Entities
{
    [Table("Rooms")]
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string RoomName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? RoomType { get; set; }

        public int Capacity { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
