using System;
using System.Collections.Generic;
using System.Text;

namespace UseADO_DB
{
    public class Student
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int YearResult { get; set; }
        public int SectionId { get; set; }
        public bool Active { get; set; }
    }
}
