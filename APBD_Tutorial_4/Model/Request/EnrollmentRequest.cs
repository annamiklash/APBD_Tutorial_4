using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace APBD_Tutorial_4.Model
{
    public class EnrollmentRequest
    {
        [Required] public string IndexNumber { get; set; }

        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        [Required] public string BirthDate { get; set; }

        [Required] public string Studies { get; set; }

        public EnrollmentRequest(string indexNumber, string firstName, string lastName, string birthDate, string studies)
        {
            IndexNumber = indexNumber;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Studies = studies;
        }
        
        public EnrollmentRequest()
        {
   
        }
        

      
    }
}