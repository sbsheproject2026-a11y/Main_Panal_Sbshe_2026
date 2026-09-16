namespace Application.DTOs
{
    public class CoursePostDTOs
    {
        public int? Id { get; set; }
        public string? Code { get; set; }

        public string? ShortName { get; set; }
        public string? Name { get; set; }
        public string? NameHindi { get; set; }
 
        public int? Duration { get; set; }
        public int? DurationTypeId { get; set; }
        public int? ParentId { get; set; }
        public int? IsActive { get; set; }
        public int? CategoryId { get; set; }
        public int? DepartmentId { get; set; }
        public int? TotalClass { get; set; }
        public int? ClassNo { get; set; }

     

    }
    public class ProductAmountDtos
    {
        public int Id { get; set; }
        public string? Name { get; set; }
         
        public int? ProductId { get; set; }
        public int? Amount { get; set; }
        public int? AmountTypeId { get; set; }
        public int? IsActive { get; set; }
    }
    public class GetProductAmountDtos
    {
        public int Id { get; set; }
        public string? Name { get; set; }
         
        public int? ProductId { get; set; }
        public decimal? Amount { get; set; }
        public int? AmountTypeId { get; set; }
        public string? AmountType { get; set; }
        public int? IsActive { get; set; }
    }
}
