using System;
using Microsoft.Win32.SafeHandles;

namespace APBD_Tutorial_4.Model
{
    public class Student
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string HashedPassword { get; set; }
        public AccessToken AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }

        public Student(string indexNumber, string firstName, string lastName, string birthDate, string hashedPassword)
        {
            IndexNumber = indexNumber;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            HashedPassword = hashedPassword;
        }

        public Student()
        {
        }

        public Student(string indexNumber, string firstName, string lastName, string birthDate)
        {
            IndexNumber = indexNumber;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        public override string ToString()
        {
            return $"IndexNumber : {IndexNumber}, FirstName {FirstName}, LastName {LastName}, BirthDate {BirthDate}, ";
        }
    }
}