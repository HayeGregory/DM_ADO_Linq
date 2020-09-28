using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Consommation
{
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int YearResult { get; set; }
        public int SectionID { get; set; }
        public bool Active { get; set; }

        public Student()
        {

        }

        public Student(string firstName, string lastName, DateTime birthDate, int yearResult, int sectionID, bool active)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            YearResult = yearResult;
            SectionID = sectionID;
            Active = active;
        }
    }
}
