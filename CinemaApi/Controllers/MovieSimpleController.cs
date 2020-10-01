using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CinemaApi.Data;
using CinemaApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieSimpleController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public MovieSimpleController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/<MovieController>
        [HttpGet]
        public  IActionResult Get()
        {
            // return _db.Movies.OrderBy(a => a.Name).ToList();
            return Ok(_db.Movies);
        }

        // GET api/<MovieController>/5
        [HttpGet("{id}")]
        public Movie Get(int id)
        {
            var movie = _db.Movies.Find(id) ;
            return movie;
        }

        //Attribute Routing
        [HttpGet("[action]/{id}")]
        public int Test(int id)
        {
            return id;
        }


        // POST api/<MovieController>
        [HttpPost]
        public IActionResult Post([FromForm] Movie movieObject)
        {
           var guid= Guid.NewGuid();
            var filepath = Path.Combine("wwwroot", guid + ".jpg");
            if (movieObject.Image!=null)
            {
                var fileSterm = new FileStream(filepath, FileMode.Create);
                movieObject.Image.CopyTo(fileSterm);
            }
            movieObject.ImageUrl = filepath.Remove(0,7);
            _db.Movies.Add(movieObject);
            _db.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<MovieController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] Movie movieObject)
        {
            var movie = _db.Movies.Find(id);
            if (movie==null)
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
                movie.Rating = movieObject.Rating;
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }
            
        }

        // DELETE api/<MovieController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
          var movie=  _db.Movies.Find(id);
            if (movie==null)
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
    }
}
