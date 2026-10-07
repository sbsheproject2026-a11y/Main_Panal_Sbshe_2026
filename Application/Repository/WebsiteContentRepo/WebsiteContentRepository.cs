using Application.DTOs;

using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Data;




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

    }
}
