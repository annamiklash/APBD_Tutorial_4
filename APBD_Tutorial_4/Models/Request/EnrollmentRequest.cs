using System.ComponentModel.DataAnnotations;

namespace APBD_Tutorial_4.Model
{
    public class EnrollmentRequest
    {
        [Required] public string IndexNumber { get; set; }

        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        [Required] public string BirthDate { get; set; }

        [Required] public string Password { get; set; }

        [Required] public string Studies { get; set; }

        public EnrollmentRequest(string indexNumber, string firstName, string lastName, string birthDate, string studies, string password)
        {
            IndexNumber = indexNumber;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Studies = studies;
            Password = password;
        }
        
        public EnrollmentRequest()
        {
   
        }
    }
}