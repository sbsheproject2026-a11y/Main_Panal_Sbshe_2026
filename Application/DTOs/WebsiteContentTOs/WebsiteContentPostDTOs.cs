using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class WebsiteContentPostDTOs
    {
        public int? Id { get; set; }
 
        public int? ProductTypeId { get; set; }
        public int? SrNo { get; set; }
        public DateTime? Date { get; set; }

      
       


        public int? IsActive { get; set; }
        public int? IsHighlight { get; set; }
        public int? IsDelete { get; set; }
   

        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Descrption { get; set; }

        public string? SEOTitle { get; set; }

        public List<ProductCategoryDocumentDTOs>? ProductCategoryDocuments { get; set; }
    }

    public class ProductCategoryDocumentDTOs
    {

        public int? Id { get; set; }
       
        public int? SrNo { get; set; }

        public string? Title { get; set; }
        public string? AltTag { get; set; }
        public IFormFile? File { get; set; }
        public string? Path { get; set; }

        public int? IsActive { get; set; }


    }



}
