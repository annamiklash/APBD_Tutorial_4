using System.Collections.Generic;
using APBD_Tutorial_4.Model;
using APBD_Tutorial_4.Services;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APBD_Tutorial_4.Controllers
{
    [ApiController]
    [Route("/api/students")]
    public class StudentsController : ControllerBase //contains many useful helper methods that we will use
    {
        private readonly IStudentsDb _studentsDb;

        public StudentsController(IStudentsDb studentsDb)
        {
            _studentsDb = studentsDb;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            List<Student> studentList = (List<Student>) _studentsDb.GetStudents();
            return Ok(studentList);
        }


        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
             Enrollment semester =_studentsDb.GetSemesterByIndex(indexNumber);
            if (semester == null)
            {
                return NotFound();
            }

            return Ok(semester);
        }
    }
}