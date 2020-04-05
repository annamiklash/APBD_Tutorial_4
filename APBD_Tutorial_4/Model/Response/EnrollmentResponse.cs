namespace APBD_Tutorial_4.Model
{
    public class EnrollmentResponse
    {
        public string Semester { get; set; }
        public string LastName { get; set; }

        public EnrollmentResponse(string semester, string lastName)
        {
            Semester = semester;
            LastName = lastName;
        }
    }
    
    
}