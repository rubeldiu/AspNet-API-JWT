using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApi.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        [Required]
        public int Qty { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public DateTime ReservationTime { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }


    }
}
