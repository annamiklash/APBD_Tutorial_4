using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using APBD_Tutorial_4.Factory;
using APBD_Tutorial_4.Handlers;
using APBD_Tutorial_4.Model;
using APBD_Tutorial_4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace APBD_Tutorial_4.Controllers
{
    [ApiController]
    [Route("/api/students")]
    public class StudentsController : ControllerBase 
    {
        private readonly IStudentsDb _studentsDb;
        private IConfiguration configuration;

        public StudentsController(IStudentsDb studentsDb, IConfiguration configuration)
        {
            _studentsDb = studentsDb;
            this.configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult GetStudents()
        {
            List<Student> studentList = (List<Student>) _studentsDb.GetStudents();
            if (studentList == null)
            {
                return NotFound();
            }
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


        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            var accessToken = TokensGenerator.GenerateInitialAccessToken(request, configuration, _studentsDb);
            var refreshToken = TokensGenerator.GenerateRefreshToken(request, _studentsDb,accessToken);

            if (accessToken == null || refreshToken == null)
            {
                return Unauthorized("Incorrect request");
            }
            
            return Ok(new AuthRequest(accessToken, refreshToken, request.Username));
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken(AuthRequest request)
        {
            var response = RefreshTokenHandler.RefreshToken(request, _studentsDb, configuration);
            var responseErrors = response.Errors;
            if (responseErrors.Count > 0)
            {
                return BadRequest(responseErrors);
            }
            return Ok(response);
        }
    }
}