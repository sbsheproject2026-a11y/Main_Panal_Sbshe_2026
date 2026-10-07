using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MasterTypeDTOs
{
    public class MasterTypeDetailPostDTOs
    {
        public int? Id { get; set; }
        [Required]
        public string? Name { get; set; }

        public string? Code { get; set; }
        public IFormFile? File { get; set; }
        public string? FilePath { get; set; }
      
        public int MasterTypeId { get; set; }
        public int? SrNo { get; set; }
        public int? ParentId { get; set; }
        public int? IsActive { get; set; }
     
    }
}
