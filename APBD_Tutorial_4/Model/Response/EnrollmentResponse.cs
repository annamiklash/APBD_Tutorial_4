namespace APBD_Tutorial_4.Model
{
    public class EnrollmentResponse
    {
        public int Semester { get; set; }
        public string LastName { get; set; }

        public EnrollmentResponse(int semester, string lastName)
        {
            Semester = semester;
            LastName = lastName;
        }
    }
    
    
}