using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CinemaApi.Data;
using CinemaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public MoviesController(ApplicationDbContext db)
        {
            _db = db;

        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public IActionResult Post([FromForm] Movie movieObject)
        {
            var guid = Guid.NewGuid();
            var filepath = Path.Combine("wwwroot", guid + ".jpg");
            if (movieObject.Image != null)
            {
                var fileSterm = new FileStream(filepath, FileMode.Create);
                movieObject.Image.CopyTo(fileSterm);
            }
            movieObject.ImageUrl = filepath.Remove(0, 7);
            _db.Movies.Add(movieObject);
            _db.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<MovieController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Put(int id, [FromForm] Movie movieObject)
        {
            var movie = _db.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("No Record found about this Id ");
            }
            else
            {
                var guid = Guid.NewGuid();
                var filepath = Path.Combine("wwwroot", guid + ".jpg");
                if (movieObject.Image != null)
                {
                    var fileSterm = new FileStream(filepath, FileMode.Create);
                    movieObject.Image.CopyTo(fileSterm);
                    movie.ImageUrl = filepath.Remove(0, 7);
                }
                movie.Name = movieObject.Name;
                movie.Language = movieObject.Language;
                movie.Description = movieObject.Description;
                movie.Duration = movieObject.Duration;
                movie.Genre = movieObject.Genre;
                movie.PlayingDate = movieObject.PlayingDate;
                movie.PlayingTime = movieObject.PlayingTime;
                movie.Rating = movieObject.Rating;
                movie.TailorUrl = movieObject.TailorUrl;
                movie.TicketPrice = movieObject.TicketPrice;
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }

        }

        // DELETE api/<MovieController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var movie = _db.Movies.Find(id);
            if (movie == null)
            {
                return NotFound("No record found");
            }
            else
            {
                _db.Movies.Remove(movie);
                _db.SaveChanges();
                return Ok("Record Deleted Successfully");
            }

        }

        // GET: api/<MovieController>
        //https://localhost:44397/api/movies/allmovies?pageNumber=1&pageSize=3
        //Sorting and Pagging

        [HttpGet("[action]")]
       
        public IActionResult AllMovies(string sort,int? pageNumber, int? pageSize)
        {
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;
             var movies= from movie in _db.Movies
                    select new
                    {
                        Id = movie.Id,
                        Name = movie.Name,
                        Duration = movie.Duration,
                        Language = movie.Language,
                        Rating = movie.Rating,
                        ImageUrl = movie.ImageUrl
                    };

            switch (sort)
            {
                case "desc":
                    return Ok(movies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).OrderByDescending(m => m.Rating));
                case "asc":
                    return Ok(movies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).OrderBy(m => m.Rating));
                default:
                    return Ok(movies.Skip((currentPageNumber - 1)* currentPageSize).Take(currentPageSize));
            }
           
        }
        [HttpGet("[action]")] //action token
        //https://localhost:44397/api/movies/findmovies?moviename=ni
        //Searching
        public IActionResult FindMovies(string movieName)
        {
            var movies = from movie in _db.Movies
                         where movie.Name.StartsWith(movieName)
                         select new
                         {
                             Id = movie.Id,
                             Name = movie.Name,
                             ImageUrl = movie.ImageUrl
                         };
            return Ok(movies);
        }

        [HttpGet("[action]/{id}")]

        public IActionResult MovieDetail(int id)
        {
          var movie=  _db.Movies.Find(id);
            if (movie==null)
            {
                return NotFound();
            }
            else
            {
                return Ok(movie);
            }
            
        }
    }
}
