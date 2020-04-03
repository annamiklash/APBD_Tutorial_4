namespace APBD_Tutorial_4.Model
{
    public class Response
    {
        public string Semester { get; set; }
        public string LastName { get; set; }

        public Response(string semester, string lastName)
        {
            Semester = semester;
            LastName = lastName;
        }
    }
    
    
}