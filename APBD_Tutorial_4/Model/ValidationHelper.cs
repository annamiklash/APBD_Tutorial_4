using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace APBD_Tutorial_4.Model
{
    public static class ValidationHelper
    {
        public static List<Error> ValidateRequest(Request request)
        {
            List<Error> errorList = new List<Error>();

            if (!IsIndexNumberValid(request.IndexNumber))
            {
                errorList.Add(new Error(request.IndexNumber, "Wrong index format"));
            }

            if (!IsNameValid(request.FirstName))
            {
                errorList.Add(new Error(request.FirstName, "Wrong First Name format"));
            }

            if (!IsNameValid(request.LastName))
            {
                errorList.Add(new Error(request.LastName, "Wrong Last Name format"));
            }

            if (!IsDateValid(request.BirthDate))
            {
                errorList.Add(new Error(request.BirthDate.ToString(CultureInfo.InvariantCulture), "Wrong Last Name format"));
            }

            return errorList;
        }

        private static bool IsIndexNumberValid(string indexNumber)
        {
            return Regex.IsMatch(indexNumber, "^s[0-9]+$", RegexOptions.IgnoreCase);
        }

        private static bool IsNameValid(string name)
        {
            return Regex.IsMatch(name, "^[A-Z][-a-zA-Z]+$");
        }

        private static bool IsDateValid(DateTime birthDate)
        {
            var birthDateDate = birthDate.Date.ToString(CultureInfo.CurrentCulture);
            return Regex.IsMatch(birthDateDate, @"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$");
        }
    }
}