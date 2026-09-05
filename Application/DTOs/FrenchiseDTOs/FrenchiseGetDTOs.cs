using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs 
{
    public class FrenchiseGetDTOs
    {
        public int? Id { get; set; }
        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? ReferenceId { get; set; }
        public int? DistrictId { get; set; }
        public int? StateId { get; set; }
         
        public string? ShortName { get; set; }

        public string? FatherName { get; set; }
        public int? LocationId { get; set; }
        public string? Address { get; set; }
        public string? Pincode { get; set; }
  
        public string? MobileNo { get; set; }
        public string? WhatsAppNo { get; set; }
        public string? Email { get; set; }
  
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? CityName { get; set; }
        public string? DistrictName { get; set; }
        public string? StateName { get; set; }
   

    }
}
