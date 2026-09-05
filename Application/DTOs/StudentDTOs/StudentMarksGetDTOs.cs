using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs 
{
    public class StudentMarksGetDTOs
    {
        public int Id { get; set; }


        public DateTime? CreatedAt { get; set; }

        public int? StudentId { get; set; }
        public int? SubjectId { get; set; }
        public int? TheoryMarks { get; set; }
        public int? PracticalMarks { get; set; }
        public int? AssignmentMarks { get; set; }

        public int? IsActive { get; set; }


    }
}
