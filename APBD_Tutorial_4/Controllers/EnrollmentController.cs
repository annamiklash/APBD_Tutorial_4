using System.Collections.Generic;
using APBD_Tutorial_4.Model;
using APBD_Tutorial_4.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Tutorial_4.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private IStudentsDb _studentsDb;

        public EnrollmentController(IStudentsDb studentsDb)
        {
            _studentsDb = studentsDb;
        }

        [HttpPost]
        public IActionResult EnrollNewStudent(Request request)
        {
            // 1 validation
            List<Error> errorsList = ValidationHelper.ValidateRequest(request);
            if (!errorsList.Count.Equals(0))
            {
                return BadRequest(errorsList);
            }

            //2. Check if studies exists -> 404
            bool exists = _studentsDb.StudiesExist(request);
            if (!exists)
            {
                return NotFound();
            }

            //3. Check if enrollment exists
            bool enrollmentExists = _studentsDb.EnrollmentExists(request);
            if (!enrollmentExists)
            {
                return BadRequest();
            }
            
            //4. check if index exists

            bool indexExists = _studentsDb.IndexExists(request);
            if (indexExists)
            {
                return BadRequest();
            }
            
            var response =_studentsDb.EnrollNewStudent(request);
            return Ok(response);
        }
    }
}