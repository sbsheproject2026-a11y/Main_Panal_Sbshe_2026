using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs 
{
    public class CourseGetDTOs
    {
        public int? Id { get; set; }
        public string? Code { get; set; }

        public string? ShortName { get; set; }
        public string? Name { get; set; }
        public string? NameHindi { get; set; }

        public string? DurationName { get; set; }
        public string? CategoryName { get; set; }
        public string? DepartmentName { get; set; }
        public int? Duration { get; set; }
        public int? DurationTypeId { get; set; }
        public int? ParentId { get; set; }
        public int? IsActive { get; set; }
        public int? CategoryId { get; set; }
        public int? DepartmentId { get; set; }
        public int? TotalClass { get; set; }
        public int? ClassNo { get; set; }

    }
}
