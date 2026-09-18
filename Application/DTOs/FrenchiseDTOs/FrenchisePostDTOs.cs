using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs 
{
    public class FrenchisePostDTOs
    {
        public int? Id { get; set; }
        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? ReferenceId { get; set; }
        public int? EntityTypeId { get; set; }
        public string? ShortName { get; set; }

        public string? FatherName { get; set; }
        public int? LocationId { get; set; }
        public string? Address { get; set; }
        public string? Pincode { get; set; }
        public int? ContactTypeId { get; set; }
        public int? AddressTypeId { get; set; }
        public string? MobileNo { get; set; }
        public string? WhatsAppNo { get; set; }
        public string? Email { get; set; }
        public int? DesignationId { get; set; }
        public int? RoleId { get; set; }
        public int? FatherEntityTypeId { get; set; }
        public int? WhatsAppContactTypeId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        public string? AadhaarCardimage { get; set; }
        public IFormFile? AadhaarCard1 { get; set; }
        public string? PanCardimage { get; set; }
        public IFormFile? PanCard1 { get; set; }
        public string? PassportPhoto { get; set; }
        public IFormFile? PassportPhoto1 { get; set; }
        public string? AuthorizedSignature { get; set; }
        public IFormFile? AuthorizedSignature1 { get; set; }
        public string? InstituteCertificate { get; set; }
        public IFormFile? InstituteCertificate1 { get; set; }
        public string? BuildingPhoto1 { get; set; }
        public IFormFile? BuildingPhoto11 { get; set; }
        public string? BuildingPhoto2 { get; set; }
        public IFormFile? BuildingPhoto22 { get; set; }
        public int? InstituteTypeId { get; set; }
        public int? NumberofClassrooms { get; set; }
        public int? ComputerLabAvailable { get; set; }
        public int? NumberofComputers { get; set; }
        public string? Affiliation { get; set; }
        public string? Website { get; set; }
        public string? TotalArea { get; set; }
        public string? OtherFacilities { get; set; }
        public string? AadhaarCardNumber { get; set; }
        public string? PANCardNumber { get; set; }
        public string? Remark { get; set; }
        public string? Experience { get; set; }


        public List<FranchiseDepartmentDTOs>? DepartmentIds { get; set; }


    }
    public class FranchiseDepartmentDTOs
    {
        public int? DepartmentId { get; set; }
     
    }
    public class FranchiseUpdatePasswordDTOs
    {
        public int? Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }

    }

    public class AccRegistrationPostDTOs
    {
        public int? Id { get; set; }
        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? ReferenceId { get; set; }
        public int? EntityTypeId { get; set; }
        public string? ShortName { get; set; }

        public string? FatherName { get; set; }
        public int? LocationId { get; set; }
        public string? Address { get; set; }
        public string? Pincode { get; set; }
        public int? ContactTypeId { get; set; }
        public int? AddressTypeId { get; set; }
        public string? MobileNo { get; set; }
        public string? WhatsAppNo { get; set; }
        public string? Email { get; set; }
        public int? DesignationId { get; set; }
        public int? RoleId { get; set; }
        public int? FatherEntityTypeId { get; set; }
        public int? WhatsAppContactTypeId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        public string? InstituteName { get; set; }
        public string? Experience { get; set; }
        public string? Occupation { get; set; }
        public string? ExpectedAdmissions { get; set; }
        public string? Remark { get; set; }
        public DateTime? Dob { get; set; }
        public string? AadhaarCardimage { get; set; }
        public IFormFile? AadhaarCard1 { get; set; }
        public string? PanCardimage { get; set; }
        public IFormFile? PanCard1 { get; set; }
        public string? PassportPhoto { get; set; }
        public IFormFile? PassportPhoto1 { get; set; }
        public string? AuthorizedSignature { get; set; }
        public IFormFile? AuthorizedSignature1 { get; set; }
        public string? InstituteCertificate { get; set; }
        public IFormFile? InstituteCertificate1 { get; set; }
        public string? BuildingPhoto1 { get; set; }
        public IFormFile? BuildingPhoto11 { get; set; }
        public string? BuildingPhoto2 { get; set; }
        public IFormFile? BuildingPhoto22 { get; set; }





    }


}
