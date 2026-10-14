using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Data;
using System.Text.RegularExpressions;




namespace Application.Repository
{
    public class WebsiteContentRepository : IWebsiteContentRepository
    {
        private readonly ApplicationDbContext _context;

        public WebsiteContentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //---------------------------------------Frenchise Start------------------------------


        public async Task<List<FrenchiseGetDTOs>> GetAll(int id)
        {
            try
            {
                var data = new List<FrenchiseGetDTOs>();

                var connectionString = _context.Database.GetConnectionString();

                await using (var connection = new SqlConnection(connectionString))
                await using (var command = new SqlCommand("GetListFrenchise", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@Id", id));

                    await connection.OpenAsync();

                    await using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            data.Add(new FrenchiseGetDTOs
                            {
                                Id = Convert.ToInt32(reader["Id"]),

                                Code = reader["Code"]?.ToString(),
                                Name = reader["Name"]?.ToString(),
                                FatherName = reader["FatherName"]?.ToString(),
                                Address = reader["Address"]?.ToString(),
                                Pincode = reader["PinCode"]?.ToString(),
                                MobileNo = reader["MobileNo"]?.ToString(),
                                Email = reader["Email"]?.ToString(),

                                CityName = reader["CityName"]?.ToString(),
                                DistrictName = reader["DistrictName"]?.ToString(),
                                StateName = reader["StateName"]?.ToString()
                            });
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Frenchise data.", ex);
            }
        }


        public async Task<List<WebsiteContentGetDTOs>> GetAllData()
        {
            try
            {

                var data = await (from pa in _context.ProductCategory
                                  join p in _context.ProductCategoryDetail on pa.Id equals p.ProductCategoryId into productGroup
                                  from p in productGroup.DefaultIfEmpty()

                                  where pa.IsDelete == 1 && pa.ProductTypeId != null

                                  orderby pa.Id descending
                                  select new WebsiteContentGetDTOs
                                  {
                                      Id = pa.Id,
                                      Title = pa.Name,
                                      ProductTypeId = pa.ProductTypeId,
                                      IsHighlight = pa.IsHighlight,
                                      ShortDescription = p.ShortDescription,
                                      Descrption = p.Descrption,
                                      SEOTitle = p.SEOTitle,
                                      IsActive = pa.IsActive,
                                      ProductCategoryDocuments = _context.ProductCategoryDocument.Where(x => x.ProductCategoryId == pa.Id && x.IsActive == 1)
                                        .Select(x => new ProductCategoryDocumentDTOs
                                           {
                            Id = x.Id,
 
                            SrNo = x.SrNo,
                            Title = x.Title,
                            Path = x.Path,
                            AltTag = x.AltTag,
                            IsActive = x.IsActive
                        })
                        .ToList()


                                  })

                                  .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching MasterTypeDetail data.", ex);
            }
        }


        public async Task<WebsiteContentGetDTOs?> GetbyId(int Id)
        {
            try
            {

                var data = await (from pa in _context.ProductCategory
                                  join p in _context.ProductCategoryDetail on pa.Id equals p.ProductCategoryId into productGroup
                                  from p in productGroup.DefaultIfEmpty()

                                  where pa.IsDelete == 1 && pa.ProductTypeId != null && pa.Id==Id

                                  orderby pa.Id descending
                                  select new WebsiteContentGetDTOs
                                  {
                                      Id = pa.Id,
                                      Title = pa.Name,
                                      ProductTypeId = pa.ProductTypeId,
                                      IsHighlight = pa.IsHighlight,
                                      ShortDescription = p.ShortDescription,
                                      Descrption = p.Descrption,
                                      SEOTitle = p.SEOTitle,
                                      IsActive = pa.IsActive,
                                      ProductCategoryDocuments = _context.ProductCategoryDocument.Where(x => x.ProductCategoryId == pa.Id && x.IsActive == 1)
                                        .Select(x => new ProductCategoryDocumentDTOs
                                        {
                                            Id = x.Id,

                                            SrNo = x.SrNo,
                                            Title = x.Title,
                                            Path = x.Path,
                                            AltTag = x.AltTag,
                                            IsActive = x.IsActive
                                        })
                        .ToList()


                                  })

                                  .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching MasterTypeDetail data.", ex);
            }
        }

        public async Task<string> Create(WebsiteContentPostDTOs model)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Create Product Category
                var productcategory = new ProductCategory
                {
                    SrNo = model.SrNo,
                    Name = model.Title,
                    ProductTypeId = model.ProductTypeId,
                    Date = DateTime.Now,
                    IsActive = model.IsActive,
                    IsDelete = 0
                };

                await _context.ProductCategory.AddAsync(productcategory);
                await _context.SaveChangesAsync();


                // 2. Create Product Category Detail
                var productcategorydetail = new ProductCategoryDetail
                {
                    ProductCategoryId = productcategory.Id,
                    Title = model.Title,
                    ShortDescription = model.ShortDescription,
                    Descrption = model.Descrption,
                    SEOTitle = model.SEOTitle,
                    IsActive = model.IsActive
                };

                await _context.ProductCategoryDetail.AddAsync(
                    productcategorydetail
                );


                // 3. Create Product Category Documents
                if (model.ProductCategoryDocuments != null && model.ProductCategoryDocuments.Any())
                {
                    foreach (var item in model.ProductCategoryDocuments)
                    {
                        var productcategorydocument = new ProductCategoryDocument
                        {
                            ProductCategoryId = productcategory.Id,
                            SrNo = item.SrNo,
                            Title = item.Title,
                            AltTag = item.AltTag,
                            Path = item.Path,
                            Date = DateTime.Now,
                            IsActive = item.IsActive
                        };

                        await _context.ProductCategoryDocument.AddAsync(productcategorydocument);
                    }
                }


                // Save Detail + Documents
                await _context.SaveChangesAsync();


                // Commit everything
                await transaction.CommitAsync();

                return "created successfully";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<string> Update(WebsiteContentPostDTOs model)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // ==========================================
                // 1. Product Category
                // ==========================================

                var productcategory = await _context.ProductCategory
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (productcategory != null)
                {
                    productcategory.SrNo = model.SrNo;
                    productcategory.Name = model.Title;
                    productcategory.ProductTypeId = model.ProductTypeId;
                    productcategory.IsActive = model.IsActive;

                    _context.ProductCategory.Update(productcategory);
                }


                // ==========================================
                // 2. Product Category Detail
                // ==========================================

                var productcategorydetail =
                    await _context.ProductCategoryDetail
                        .FirstOrDefaultAsync(x =>
                            x.ProductCategoryId == model.Id);

                if (productcategorydetail != null)
                {
                    productcategorydetail.Title = model.Title;
                    productcategorydetail.ShortDescription =
                        model.ShortDescription;
                    productcategorydetail.Descrption =
                        model.Descrption;
                    productcategorydetail.SEOTitle =
                        model.SEOTitle;
                    productcategorydetail.IsActive =
                        model.IsActive;

                    _context.ProductCategoryDetail.Update(
                        productcategorydetail
                    );
                }


                // ==========================================
                // 3. Remove Old Documents
                // ==========================================

                var oldDocuments =
                    await _context.ProductCategoryDocument
                        .Where(x => x.ProductCategoryId == model.Id)
                        .ToListAsync();

                if (oldDocuments.Any())
                {
                    _context.ProductCategoryDocument.RemoveRange(
                        oldDocuments
                    );
                }


                // ==========================================
                // 4. Add New Documents
                // ==========================================

                if (model.ProductCategoryDocuments != null &&
                    model.ProductCategoryDocuments.Any())
                {
                    foreach (var item in model.ProductCategoryDocuments)
                    {
                        var productcategorydocument =
                            new ProductCategoryDocument
                            {
                                ProductCategoryId = model.Id,
                                Title = item.Title,
                                AltTag = item.AltTag,
                                Path = item.Path,
                                Date = DateTime.Now,
                                IsActive = item.IsActive
                            };

                        await _context.ProductCategoryDocument.AddAsync(
                            productcategorydocument
                        );
                    }
                }


                // ==========================================
                // 5. Save All Changes
                // ==========================================

                await _context.SaveChangesAsync();


                // ==========================================
                // 6. Commit Transaction
                // ==========================================

                await transaction.CommitAsync();


                return "updated successfully";
            }
            catch (Exception ex)
            {
                // Rollback if anything fails
                await transaction.RollbackAsync();

                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<string> Delete(int Id)
        {
            try
            {
                var data = await _context.ProductCategory
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return "MasterTypeDetail not found";
                }

                data.IsDelete = 1;


                await _context.SaveChangesAsync();

                return "delete successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while delete MasterTypeDetail", ex);
            }
        }


       
        public async Task<List<MasterTypeDetailGetDTOs>> GetWebsiteMaster(int id)
        {
            try
            {

                var data = await (from pa in _context.MasterTypeDetail
                                 

                                  where pa.IsDelete == 0 && pa.MasterTypeId==id

                                  orderby pa.Id descending
                                  select new MasterTypeDetailGetDTOs
                                  {
                                      Id = pa.Id,
                                      SrNo = pa.SrNo,
                                      Name = pa.Name,
                                      Code = pa.Code,
                                   
                                      FilePath = pa.FilePath,
                                      IsActive = pa.IsActive

                                  })

                                  .ToListAsync();

                // Slug compute karo (client side, kyunki EF mein custom function translate nahi hota)
                foreach (var item in data)
                {
                    item.Slug = GenerateSlug(item.Name);
                }

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching MasterTypeDetail data.", ex);
            }
        }

        private static string GenerateSlug(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            var slug = name.Trim().ToLowerInvariant();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");       // special chars hatao
            slug = Regex.Replace(slug, @"\s+", "-");                // spaces ko - karo
            slug = Regex.Replace(slug, @"-+", "-");                 // multiple -- ko single -
            return slug.Trim('-');
        }


        public async Task<List<WebsiteCourseGetDTOs>> GetCourseData(int id)
        {
            try
            {
                var data = await (from p in _context.Product
                                  join pa in _context.ProductCategory on p.ProductCategoryId equals pa.Id into productGroup1
                                  from pa in productGroup1.DefaultIfEmpty()
                                  join ppd in _context.ProductPersonalDetail on p.Id equals ppd.ProductId into productGroup2
                                  from p1 in productGroup2.DefaultIfEmpty()
                                  join ppl in _context.ProductLink on p.Id equals ppl.ProductId into productGroup3
                                  from p4 in productGroup3.DefaultIfEmpty()
                                  join pcd in _context.ProductCategoryDetail on p.Id equals pcd.ProductCategoryId into productGroup4
                                  from p3 in productGroup4.DefaultIfEmpty()
                                  join mtd in _context.MasterTypeDetail on p.Duration equals mtd.Id into mtdd
                                  from mtd in mtdd.DefaultIfEmpty()
                                  join mtd1 in _context.MasterTypeDetail on p.DurationTypeId equals mtd1.Id into mtdd1
                                  from mtd1 in mtdd1.DefaultIfEmpty()
                                  join mtddep in _context.MasterTypeDetail on p1.DepartmentId equals mtddep.Id into mtddep1
                                  from mtddep in mtddep1.DefaultIfEmpty()
                                  where p.IsDelete == 0 && pa.ProductTypeId == null &&  p1.DepartmentId==id  && p.ParentId==null

                                  orderby p.Id descending



                                  select new WebsiteCourseGetDTOs
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      Code=p.Code,
                                      NameHindi = p.CourseNameHindi,
                                    
                                      Eligbilty=p.Eligbilty,

                                      StudyModeId=p1.StudyModeId,


                                      DurationName = mtd.Name + ' ' + mtd1.Name,
                                      ShortDescription = p3 != null ? p3.ShortDescription : null,
                                      Descrption = p3 != null ? p3.Descrption : null,
                                      IsActive = pa.IsActive,

                                      ProductCategoryDocuments = _context.ProductCategoryDocument
                                          .Where(x => x.ProductCategoryId == pa.Id && x.IsActive == 1)
                                          .Select(x => new ProductMaterialDtos
                                          {
                                              Id = x.Id,
                                              SrNo = x.SrNo,
                                              Name = x.Title,
                                              Fileupload = x.Path,
                                              AltTag = x.AltTag,
                                          })
                                          .ToList(),

                                      ProductAmounts = _context.ProductAmount
                                          .Where(x => x.ProductId == p.Id && x.IsActive == 1)
                                          .Select(x => new ProductAmountDtos
                                          {
                                              Id = x.Id,
                                              Name = x.Name,
                                              Amount = x.Amount,
                                              AmountTypeId = x.AmountTypeId,
                                          })
                                          .ToList()
                                  })
                                  .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching MasterTypeDetail data.", ex);
            }
        }



    }
}
