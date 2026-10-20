using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs 
{
    public class WebsiteContentGetDTOs
    {
        public int? Id { get; set; }

        public int? ProductTypeId { get; set; }
        public int? SrNo { get; set; }

        public DateTime? Date { get; set; }
        public int? IsActive { get; set; }
        public int? IsHighlight { get; set; }
        public int? IsDelete { get; set; }


        public string? Title { get; set; }
        public string? ProductType { get; set; }
        public string? ShortDescription { get; set; }
        public string? Descrption { get; set; }

        public string? SEOTitle { get; set; }


        public List<ProductCategoryDocumentDTOs>? ProductCategoryDocuments { get; set; }

    }
}
