using System;
using System.Collections.Generic;
using System.IO;
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
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public StudentController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_db.Students.OrderBy(a => a.Name).ToList());
        }
       
        [HttpPost]
        public IActionResult Post([FromForm] Student student)
        {
            var guid = Guid.NewGuid();
            var filepath = Path.Combine("wwwroot", guid + ".jpg");
            if (student.Image != null)
            {
                var fileStrem = new FileStream(filepath, FileMode.Create);
                student.Image.CopyTo(fileStrem);
            }
            student.ImageUrl = filepath.Remove(0, 7);
            _db.Students.Add(student);
            _db.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] Student student)
        {
            var studentresult = _db.Students.Find(id);
            if (studentresult == null)
            {
                return NotFound("No record Found about this id");
            }
            else
            {
                var guid = Guid.NewGuid();
                var filePath = Path.Combine("wwwroot", guid + ".jpg");
                if (student.Image!=null)
                {
                    var fileStrem = new FileStream(filePath, FileMode.Create);
                    student.Image.CopyTo(fileStrem);
                    
                }
                studentresult.Name = student.Name;
                studentresult.ImageUrl = filePath.Remove(0, 7);

                _db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);

            }
        }
    }
}
