using System;

namespace APBD_Tutorial_4.Model
{
    public class Student
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public Student(string indexNumber, string firstName, string lastName, DateTime birthDate)
        {
            IndexNumber = indexNumber;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        public Student()
        {
        }
    }
}