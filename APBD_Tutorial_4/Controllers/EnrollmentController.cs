using System;
using System.Collections.Generic;
using APBD_Tutorial_4.Model;
using APBD_Tutorial_4.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Tutorial_4.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IStudentsDb _studentsDb;

        public EnrollmentController(IStudentsDb studentsDb)
        {
            _studentsDb = studentsDb;
        }

        [HttpPost]
        public IActionResult EnrollNewStudent(EnrollmentRequest enrollmentRequest)
        {
  
            List<Error> errorsList = ValidationHelper.ValidateRequest(enrollmentRequest);
            if (!errorsList.Count.Equals(0))
            {
                return BadRequest(errorsList);
            }
            
            string studies = _studentsDb.FindStudies(enrollmentRequest);
            if (studies==null)
            {
                return NotFound("Studies " + enrollmentRequest.Studies + " does not exist");
            }
            
            bool enrollmentExists = _studentsDb.EnrollmentExistsOnFirstSemester(enrollmentRequest);
            if (!enrollmentExists)
            {
                return BadRequest("Enrollment for " + enrollmentRequest.Studies + " does not exist for 1 semester");
            }
            
            bool indexExists = _studentsDb.IndexExists(enrollmentRequest);
            if (indexExists)
            {
                return BadRequest("Student with index " + enrollmentRequest.IndexNumber + " already exists");
            }
            try
            {
                EnrollmentResponse response = _studentsDb.EnrollNewStudent(enrollmentRequest);
                return StatusCode(201, response);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("promotion")]
        public IActionResult PromoteStudents(PromotionRequest promotionRequest)
        {
           bool exists = _studentsDb.EnrollmentExistsWithSemesterAndStudies(promotionRequest);
           if (exists == false)
           {
               return NotFound("Enrollment for semester "+ promotionRequest.Semester + " and studies " + promotionRequest.Studies + " does not exist");
           }
           IEnumerable<PromotionResponse> promotionResponses = _studentsDb.PromoteStudents(promotionRequest);
           return StatusCode(201, promotionResponses);
        }
    }
}