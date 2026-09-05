using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MasterTypeDTOs
{
    public class MasterTypeDetailGetDTOs
    {
        public int Id { get; set; }
       
        public string? Name { get; set; }

        public string? Code { get; set; }
        public DateTime? Date { get; set; }
        public int MasterTypeId { get; set; }
        public int? ParentId { get; set; }
        public int? IsActive { get; set; }
        public int? IsDelete { get; set; }
    }
}
