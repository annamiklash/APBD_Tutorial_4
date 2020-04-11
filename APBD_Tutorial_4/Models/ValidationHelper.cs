using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace APBD_Tutorial_4.Model
{
    public static class ValidationHelper
    {

        private const string INDEX_NUMBER_REGEX = "^s[0-9]+$";
        private const string NAME_REGEX = "^[A-Z][-a-zA-Z]+$";
        private const string DATE_REGEX = @"^\s*(3[01]|[12][0-9]|0?[1-9])\.(1[012]|0?[1-9])\.((?:19|20)\d{2})\s*$";

            public static List<Error> ValidateRequest(EnrollmentRequest enrollmentRequest)
        {
            List<Error> errorList = new List<Error>();

            if (!IsIndexNumberValid(enrollmentRequest.IndexNumber))
            {
                errorList.Add(new Error("IndexNumber",enrollmentRequest.IndexNumber, "Invalid Index Number format. Should match " + INDEX_NUMBER_REGEX));
            }

            if (!IsNameValid(enrollmentRequest.FirstName))
            {
                errorList.Add(new Error("FirstName", enrollmentRequest.FirstName, "Invalid First Name format. Should match " + NAME_REGEX));
            }

            if (!IsNameValid(enrollmentRequest.LastName))
            {
                errorList.Add(new Error("LastName",enrollmentRequest.LastName, "Invalid Last Name format. Should match " + NAME_REGEX));
            }

            if (!IsDateValid(enrollmentRequest.BirthDate))
            {
                errorList.Add(new Error("BirthDate",enrollmentRequest.BirthDate, "Invalid Date format. Should match " + DATE_REGEX));
            }

            return errorList;
        }

        private static bool IsIndexNumberValid(string indexNumber)
        {
            return Regex.IsMatch(indexNumber, INDEX_NUMBER_REGEX, RegexOptions.IgnoreCase);
        }

        private static bool IsNameValid(string name)
        {
            return Regex.IsMatch(name, NAME_REGEX);
        }

        private static bool IsDateValid(string birthDate)
        {
          return Regex.IsMatch(birthDate, DATE_REGEX);
        }
    }
}