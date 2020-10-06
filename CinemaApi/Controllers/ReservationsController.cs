using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaApi.Data;
using CinemaApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        //we need this to access the reservation table on the database
        private readonly ApplicationDbContext _db;
        public ReservationsController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpPost]
        public IActionResult Post([FromBody] Reservation reservation)
        {
            reservation.ReservationTime = DateTime.Now;
            _db.Reservations.Add(reservation);
            _db.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpGet]
        public IActionResult GetReservation()
        {
            var reservations = from reservation in _db.Reservations
                              join customer in _db.Users on reservation.UserId equals customer.Id
                              join movie in _db.Movies on reservation.MovieId equals movie.Id
                              select new
                              {
                                  Id = reservation.Id,
                                  ReservationTime = reservation.ReservationTime,
                                  CustomerName = customer.Name,
                                  MovieName = movie.Name
                              };
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public IActionResult GetReservationDetail(int id)
        {
            var reservationDetail = (from reservation in _db.Reservations
                                    join customer in _db.Users on reservation.Id equals customer.Id
                                    join movie in _db.Movies on reservation.Id equals movie.Id
                                    where reservation.Id == id
                                    select new
                                    {
                                        Id = reservation.Id,
                                        ReservationTime = reservation.ReservationTime,
                                        CustomerName = customer.Name,
                                        MovieName = movie.Name,
                                        Email = customer.Email,
                                        Qty = reservation.Qty,
                                        Price = reservation.Price

                                    }).FirstOrDefault();
            return Ok(reservationDetail);

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var reservation = _db.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound("Data is not found agains this Id");
            }
            else
            {
                _db.Reservations.Remove(reservation);
                _db.SaveChanges();
                return Ok("Record Deleted");
            }

        }
    }
}
