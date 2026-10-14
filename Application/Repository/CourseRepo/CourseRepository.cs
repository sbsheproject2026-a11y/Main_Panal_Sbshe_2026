using Application.DTOs;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Data;

namespace Application.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;

            _connectionString = _context.Database.GetConnectionString()
                ?? throw new InvalidOperationException(
                    "Database connection string not found.");
        }

        // --------------------------------------- Course Start ---------------------------------------



        public async Task<List<CourseGetDTOs>> GetAll()
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
                                  join mtdcat in _context.MasterTypeDetail on p1.CategoryId equals mtdcat.Id into mtdcat1
                                  from mtdcat in mtdcat1.DefaultIfEmpty()
                                  where p.IsDelete == 0 && pa.ProductTypeId == null

                                  orderby p.Id descending

                                  select new CourseGetDTOs
                                  {
                                      Id = p.Id,
                                      Code = p.Code,
                                      Name = p.Name,
                                      NameHindi = p.CourseNameHindi,
                                      DepartmentName = mtddep.Name,
                                      CategoryName = mtdcat.Name,
                                      ParentId = p.ParentId,
                                      IsHighlight = pa.IsHighlight,
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

        public async Task<string> Create(CoursePostDTOs model)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Create Product Category
                var productcategory = new ProductCategory
                {
                    Code = model.Code,
                    ShortName = model.ShortName,
                    Name = model.Name,
                    ProductTypeId = null,
                    Date = DateTime.Now,
                    IsActive = model.IsActive,
                    IsHighlight = model.IsHighlight,
                    IsDelete = 0
                };

                await _context.ProductCategory.AddAsync(productcategory);
                await _context.SaveChangesAsync();
                // 2. Create Product Category
                var product = new Product
                {
                    ProductCategoryId = productcategory.Id,
                    Code = model.Code,
                    ShortName = model.ShortName,
                    Name = model.Name,
                    CourseNameHindi = model.NameHindi,
                    Duration = model.Duration,
                    DurationTypeId = model.DurationTypeId,
                    ParentId = model.ParentId,
                    Eligbilty = model.Eligbilty,
                    Date = DateTime.Now,
                    IsActive = model.IsActive,
                    IsDelete = 0
                };

                await _context.Product.AddAsync(product);
                await _context.SaveChangesAsync();

                // 3. Create Product Category
                var productpersonaldetail = new ProductPersonalDetail
                {
                    ProductId = product.Id,
                    CategoryId = model.CategoryId,
                    DepartmentId = model.DepartmentId,
                    StudyModeId = model.StudyModeId,
                    TotalClass = model.TotalClass,
                    ClassNo = 1,

                };

                await _context.ProductPersonalDetail.AddAsync(productpersonaldetail);
                await _context.SaveChangesAsync();
                // 3. Create Product Category
                var productlink = new ProductLink
                {
                    ProductId = product.Id,
                    ProductCategoryId = productcategory.Id,

                };

                await _context.ProductLink.AddAsync(productlink);
                await _context.SaveChangesAsync();


                // 4. Create Product Category Detail
                var productcategorydetail = new ProductCategoryDetail
                {
                    ProductCategoryId = productcategory.Id,
                    Title = model.Name,
                    ShortDescription = model.ShortDescription,
                    Descrption = model.Descrption,
                    SEOTitle = null,
                    IsActive = model.IsActive
                };

                await _context.ProductCategoryDetail.AddAsync(
                    productcategorydetail
                );


                // 5. Create Product Category Documents
                if (model.ProductCategoryDocuments != null && model.ProductCategoryDocuments.Any())
                {
                    foreach (var item in model.ProductCategoryDocuments)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Name) && item.DocumentTypeId > 0)
                        {

                            var productcategorydocument = new ProductCategoryDocument
                            {
                                ProductCategoryId = productcategory.Id,
                                DocumentTypeId = item.DocumentTypeId,
                                DocExtensionId = item.DocExtensionId,
                                Title = item.Name,
                                AltTag = item.AltTag,
                                Path = item.Fileupload,
                                Date = DateTime.Now,
                                IsActive = 1,
                            };

                            await _context.ProductCategoryDocument.AddAsync(productcategorydocument);
                        }
                    }
                }

                // 6. Create Product Category Documents

                if (model.ProductAmounts != null && model.ProductAmounts.Any())
                {
                    foreach (var item in model.ProductAmounts)
                    {
                        if (item.Amount > 0 && item.AmountTypeId > 0)
                        {
                            var productamount = new ProductAmount
                            {
                                ProductId = product.Id,
                                Name = item.Name,
                                Amount = item.Amount,
                                AmountTypeId = item.AmountTypeId,
                                Date = DateTime.Now,
                                IsActive = 1
                            };

                            await _context.ProductAmount.AddAsync(productamount);
                        }


                        if (item.AmountTypeId == 114)
                        {
                            var commission = new Commission
                            {
                                ProductId = product.Id,
                                DepartmentId = model.DepartmentId,
                                Amount = item.Amount,
                                Date = DateTime.Now,
                                IsActive = 1
                            };

                            await _context.Commission.AddAsync(commission);

                        }
                    }



                }

                if (model.TotalClass.HasValue && model.TotalClass.Value > 1)
                {
                    // 1. Master se parent duration details lao
                    var durationRecord = _context.MasterTypeDetail.FirstOrDefault(x => x.Id == model.Duration);

                    int parentDurationMonths = 0;

                    if (durationRecord != null)
                    {
                        string name = durationRecord.Name.ToLower();

                        string numberPart = new string(name.Where(char.IsDigit).ToArray());
                        int numericValue = 0;
                        int.TryParse(numberPart, out numericValue);

                        if (name.Contains("year"))
                        {
                            parentDurationMonths = numericValue * 12;
                        }
                        else
                        {
                            parentDurationMonths = numericValue;
                        }
                    }

                    // 2. Calculate per class duration
                    int perClassDurationMonths = 0;
                    int DurationMonthsId = 0;

                    if (parentDurationMonths > 0)
                    {
                        perClassDurationMonths = parentDurationMonths / model.TotalClass.Value;

                        var durationRecord1 = _context.MasterTypeDetail
                            .FirstOrDefault(x => x.Name == perClassDurationMonths.ToString() && x.MasterTypeId == 11);

                        if (durationRecord1 != null)
                        {
                            DurationMonthsId = durationRecord1.Id;
                        }
                        else
                        {
                            DurationMonthsId = 0;
                        }
                    }

                    // ✅ Sirf EK entry banegi, aur ClassNo = TotalClass.Value hogi
                    int i = model.TotalClass.Value;   // e.g. 2 aaya to i = 2

                    Product classProduct = new Product()
                    {
                        ProductCategoryId = productcategory.Id,
                        Code = $"{model.Code}-C{i}",
                        Name = $"{model.Name} - Class {i}",
                        ShortName = $"{model.ShortName}-C{i}",
                        ParentId = product.Id,
                        Date = DateTime.Now,
                        Duration = DurationMonthsId,
                        DurationTypeId = model.DurationTypeId,
                        Eligbilty = model.Eligbilty,
                        IsActive = model.IsActive,
                        IsDelete = 0,
                    };
                    _context.Product.Add(classProduct);
                    _context.SaveChanges();

                    ProductPersonalDetail classPersonal = new ProductPersonalDetail()
                    {
                        ProductId = classProduct.Id,
                        CategoryId = model.CategoryId,
                        DepartmentId = model.DepartmentId,
                        TotalClass = model.TotalClass,
                        ClassNo = i,
                    };
                    _context.ProductPersonalDetail.Add(classPersonal);
                    _context.SaveChanges();
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


        public async Task<CourseGetDTOs?> GetById(int id)
        {
            try
            {

                var data = await (from pa in _context.ProductCategory
                                  join p in _context.Product on pa.Id equals p.ProductCategoryId into product
                                  from p in product.DefaultIfEmpty()
                                  join ppd in _context.ProductPersonalDetail on p.Id equals ppd.ProductId into productppd
                                  from p1 in productppd.DefaultIfEmpty()
                                  join ppl in _context.ProductLink on p.Id equals ppl.ProductId into productppl
                                  from p4 in productppl.DefaultIfEmpty()
                                  join pcd in _context.ProductCategoryDetail on p.Id equals pcd.ProductCategoryId into productGroup
                                  from p3 in productGroup.DefaultIfEmpty()

                                  where p.Id == id

                                  orderby pa.Id descending
                                  select new CourseGetDTOs
                                  {
                                      Id = p.Id,
                                      Code = p.Code ?? "",
                                      Name = p.Name,
                                      Eligbilty = p.Eligbilty ?? "",
                                      Duration = p.Duration,
                                      DurationTypeId = p.DurationTypeId,
                                      NameHindi = p.CourseNameHindi,
                                      IsHighlight = pa.IsHighlight,
                                      ParentId = p.ParentId,
                                      DepartmentId = p1.DepartmentId,
                                      CategoryId = p1.CategoryId,
                                      TotalClass = p1.TotalClass,
                                      ClassNo = p1.ClassNo,
                                      StudyModeId = p1.StudyModeId,
                                      ShortDescription = p3.ShortDescription,
                                      Descrption = p3.Descrption,
                                      IsActive = pa.IsActive,
                                      //              ProductCategoryDocuments = _context.ProductCategoryDocument.Where(x => x.ProductCategoryId == pa.Id && x.IsActive == 1)
                                      //                .Select(x => new ProductMaterialDtos
                                      //                {
                                      //                    Id = x.Id,

                                      //                    SrNo = x.SrNo,
                                      //                    Name = x.Title,
                                      //                    Fileupload = x.Path,
                                      //                    AltTag = x.AltTag,
                                      //                    //IsActive = x.IsActive
                                      //                })
                                      //.ToList(),
                                      //              ProductAmounts = _context.ProductAmount.Where(x => x.ProductId == p.Id && x.IsActive == 1)
                                      //                .Select(x => new ProductAmountDtos
                                      //                {
                                      //                    Id = x.Id,


                                      //                    Name = x.Name,
                                      //                    Amount = x.Amount,
                                      //                    AmountTypeId = x.AmountTypeId,
                                      //                    //IsActive = x.IsActive
                                      //                })
                                      //.ToList()


                                  })

                                  .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching  data.", ex);
            }
        }


        public async Task<string> Update(CoursePostDTOs model)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var product = await _context.Product.FirstOrDefaultAsync(x => x.Id == model.Id);
                var productcategory = await _context.ProductCategory.FirstOrDefaultAsync(x => x.Id == product.ProductCategoryId);

                if (product == null)
                {
                    return "Course not found";
                }



                if (productcategory != null)
                {
                    productcategory.Code = model.Code;
                    productcategory.ShortName = model.ShortName;
                    productcategory.Name = model.Name;
                    productcategory.IsActive = model.IsActive;
                    productcategory.IsHighlight = model.IsHighlight;
                }

                if (product != null)
                {
                    product.Code = model.Code;
                    product.ShortName = model.ShortName;
                    product.Name = model.Name;
                    product.CourseNameHindi = model.NameHindi;
                    product.Duration = model.Duration;
                    product.DurationTypeId = model.DurationTypeId;
                    product.ParentId = model.ParentId;
                    product.Eligbilty = model.Eligbilty;
                    product.IsActive = model.IsActive;
                }
                var productpersonaldetail = await _context.ProductPersonalDetail.FirstOrDefaultAsync(x => x.ProductId == product.Id);

                if (productpersonaldetail != null)
                {
                    productpersonaldetail.CategoryId = model.CategoryId;
                    productpersonaldetail.DepartmentId = model.DepartmentId;
                    productpersonaldetail.TotalClass = model.TotalClass;
                    productpersonaldetail.ClassNo = model.ClassNo;
                    productpersonaldetail.StudyModeId = model.StudyModeId;
                }

                var productcategorydetail = await _context.ProductCategoryDetail.FirstOrDefaultAsync(x => x.ProductCategoryId == product.ProductCategoryId);

                if (productcategorydetail != null)
                {
                    productcategorydetail.Title = model.Name;
                    productcategorydetail.ShortDescription = model.ShortDescription;
                    productcategorydetail.Descrption = model.Descrption;
                    productcategorydetail.IsActive = model.IsActive;
                }

                //var oldAmounts = await _context.ProductAmount.Where(x => x.ProductId == product.Id).ToListAsync();

                //if (oldAmounts.Any())
                //{
                //    _context.ProductAmount.RemoveRange(oldAmounts);
                //}

                //if (model.ProductAmounts != null && model.ProductAmounts.Any())
                //{
                //    foreach (var item in model.ProductAmounts)
                //    {
                //        if (item.Amount != null || item.Amount != 0)
                //        {

                //            var productamount = new ProductAmount
                //            {
                //                ProductId = product.Id,
                //                Name = item.Name,
                //                Amount = item.Amount,
                //                AmountTypeId = item.AmountTypeId,
                //                Date = DateTime.Now,
                //                IsActive = 1
                //            };

                //            await _context.ProductAmount.AddAsync(
                //                productamount
                //            );
                //        }
                //    }
                //}

                //var oldDocuments = await _context.ProductCategoryDocument.Where(x => x.ProductCategoryId == product.ProductCategoryId).ToListAsync();

                //if (oldDocuments.Any())
                //{
                //    _context.ProductCategoryDocument.RemoveRange(
                //        oldDocuments
                //    );
                //}

                //if (model.ProductCategoryDocuments != null && model.ProductCategoryDocuments.Any())
                //{
                //    foreach (
                //        var item in model.ProductCategoryDocuments)
                //    {
                //        if (item.Name != null || item.Name != "")
                //        {

                //            var productcategorydocument =
                //            new ProductCategoryDocument
                //            {
                //                ProductCategoryId = product.ProductCategoryId,
                //                DocumentTypeId = item.DocumentTypeId,
                //                Title = item.Name,
                //                AltTag = item.AltTag,
                //                Path = item.Fileupload,
                //                Date = DateTime.Now,
                //                IsActive = 1
                //            };

                //            await _context.ProductCategoryDocument.AddAsync(productcategorydocument);
                //        }
                //    }
                //}

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return "updated successfully";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return "Something went wrong: " + ex.Message;
            }
        }

        public async Task<string> Delete(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(
                    "GetCourseDelete",
                    connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(
                    new SqlParameter("@Id", id));

                command.Parameters.Add(
                    new SqlParameter("@Code", 1));

                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                return result?.ToString()
                    ?? "Course deleted successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }

        //Course  Working end//


        //ProductAmount  Working Start//
        public async Task<string> CreateCourseAmount(List<ProductAmountDtos> model)
        {
            try
            {
                if (model == null || model.Count == 0)
                {
                    return "At least one Amount is required";
                }

                foreach (var item in model)
                {
                    if (!item.Amount.HasValue)
                    {
                        return "Amount is required";
                    }

                    var data = new ProductAmount
                    {
                        ProductId = item.ProductId,
                        Name = item.Name,
                        Amount = item.Amount,
                        AmountTypeId = item.AmountTypeId,
                        IsActive = item.IsActive ?? 1,
                        Date = DateTime.UtcNow
                    };

                    await _context.ProductAmount.AddAsync(data);
                }

                await _context.SaveChangesAsync();

                return "Course amounts created successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<List<GetProductAmountDtos>> GetProductAmount(int id)
        {
            try
            {
                var data = await (from pa in _context.ProductAmount
                                  join p in _context.MasterTypeDetail on pa.AmountTypeId equals p.Id into productGroup
                                  from p in productGroup.DefaultIfEmpty()

                                  where pa.IsActive == 1 && pa.ProductId == id

                                  orderby pa.Id descending

                                  select new GetProductAmountDtos
                                  {
                                      Id = pa.Id,
                                      ProductId = pa.ProductId,
                                      AmountTypeId = pa.AmountTypeId,
                                      Name = pa.Name,
                                      Amount = pa.Amount,
                                      AmountType = p.Name,
                                      IsActive = pa.IsActive
                                  }
                ).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Product Amount data.", ex);
            }
        }

        public async Task<GetProductAmountDtos> GetProductAmountById(int id)
        {
            try
            {
                var data = await _context.ProductAmount.Where(x => x.Id == id)
                    .Select(x => new GetProductAmountDtos
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ProductId = x.ProductId,
                        Amount = x.Amount,
                        AmountTypeId = x.AmountTypeId,

                    })
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Amount by id.", ex);
            }
        }

        public async Task<string> UpdateProductAmount(ProductAmountDtos model)
        {
            try
            {
                var data = await _context.ProductAmount
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (data == null)
                {
                    return "Lecture not found";
                }

                data.Name = model.Name;

                data.Amount = model.Amount;
                data.AmountTypeId = model.AmountTypeId;
                // data.IsActive = model.IsActive;



                await _context.SaveChangesAsync();

                return "ProductAmount updated successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating ProductAmount", ex);
            }
        }

        public async Task<string> DeleteProductAmount(int Id)
        {
            try
            {
                var data = await _context.ProductAmount
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return "Product Amount not found";
                }

                data.IsActive = 0;


                await _context.SaveChangesAsync();

                return "Product Amount delete successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while delete Product Amount", ex);
            }
        }

        //ProductAmount  Working End//


        //Course  Material  Working Start//
        public async Task<string> CreateCourseMaterial(ProductMaterialDtos model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("insCourseMaterial", connection);

                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.Add(new SqlParameter("@Name", model.Name));
                command.Parameters.Add(new SqlParameter("@ProductId", model.ProductId));
                command.Parameters.Add(new SqlParameter("@AltTag", model.AltTag));
                command.Parameters.Add(new SqlParameter("@DocumentTypeId", model.DocumentTypeId));
                command.Parameters.Add(new SqlParameter("@SrNo", model.SrNo));
                command.Parameters.Add(new SqlParameter("@Path", model.Fileupload));
                command.Parameters.Add(new SqlParameter("@DocExtensionId", model.DocExtensionId));


                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                return result?.ToString() ?? "Course created successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<List<ProductMaterialGetDtos>> getCourseMaterial(int id)
        {
            try
            {
                var data = new List<ProductMaterialGetDtos>();

                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("getCourseMaterial", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Id", id));



                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    data.Add(new ProductMaterialGetDtos
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        SrNo = Convert.ToInt32(reader["SrNo"]),
                        DocumentTypeId = Convert.ToInt32(reader["DocumentTypeId"]),

                        Name = reader["Title"]?.ToString(),
                        AltTag = reader["AltTag"]?.ToString(),
                        Fileupload = reader["Path"]?.ToString(),
                    });
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Course data.",
                    ex);
            }
        }


        public async Task<string> DeleteCourseMaterial(int Id)
        {
            try
            {
                var data = await _context.ProductCategoryDocument
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return "data not found";
                }

                data.IsActive = 0;


                await _context.SaveChangesAsync();

                return "delete successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while delete Product ", ex);
            }
        }

        public async Task<string> SetFranchiseAmount(CommissionDtos item)
        {
            try
            {
                // 1️⃣ Null check
                if (item == null)
                    return "Data is required";

                // 2️⃣ Sirf FranchiseId wale valid (FranchiseId, Amount, DepartmentId, ProductId zaroori)
                if (!item.FranchiseId.HasValue || item.FranchiseId.Value <= 0 ||
                    !item.Amount.HasValue ||
                    !item.DepartmentId.HasValue ||
                    !item.ProductId.HasValue)
                {
                    return "Amount, DepartmentId, ProductId, FranchiseId are required";
                }

                // 3️⃣ Matching purana record dhundho (3 fields)
                var existing = await _context.FranchiseCommission
                    .FirstOrDefaultAsync(x =>
                        x.FranchiseId == item.FranchiseId.Value &&
                        x.DepartmentId == item.DepartmentId.Value &&
                        x.ProductId == item.ProductId.Value);

                // 4️⃣ Mil gaya → REMOVE
                if (existing != null)
                    _context.FranchiseCommission.Remove(existing);

                // 5️⃣ Naya record INSERT
                var newRecord = new FranchiseCommission
                {
                    ProductId = item.ProductId,
                    DepartmentId = item.DepartmentId,
                    Amount = item.Amount,
                    FranchiseId = item.FranchiseId,
                    IsActive = item.IsActive ?? 1,
                    Date = DateTime.UtcNow
                };

                await _context.FranchiseCommission.AddAsync(newRecord);

                // 6️⃣ Save
                await _context.SaveChangesAsync();

                return existing != null ? "Deleted & Inserted" : "Inserted";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }
        public async Task<string> SetCourseCommission(CommissionDtos item)
        {
            try
            {
                // 1️⃣ Null check
                if (item == null)
                    return "Data is required";

                // 2️⃣ Sirf FranchiseId wale valid (FranchiseId, Amount, DepartmentId, ProductId zaroori)
                if (!item.Amount.HasValue ||
                    !item.DepartmentId.HasValue ||
                    !item.ProductId.HasValue)
                {
                    return "Amount, DepartmentId, ProductId are required";
                }

                // 3️⃣ Matching purana record dhundho (3 fields)
                var existing = await _context.Commission
                    .FirstOrDefaultAsync(x => x.DepartmentId == item.DepartmentId.Value && x.ProductId == item.ProductId.Value);

                // 4️⃣ Mil gaya → REMOVE
                if (existing != null)
                    _context.Commission.Remove(existing);

                // 5️⃣ Naya record INSERT
                var newRecord = new Commission
                {
                    ProductId = item.ProductId,
                    DepartmentId = item.DepartmentId,
                    Amount = item.Amount,
                    IsActive = item.IsActive ?? 1,
                    Date = DateTime.UtcNow
                };

                await _context.Commission.AddAsync(newRecord);

                // 6️⃣ Save
                await _context.SaveChangesAsync();

                return existing != null ? "Deleted & Inserted" : "Inserted";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }

        //Course  Material  Working End//

        // --------------------------------------- Course End ---------------------------------------
    }
}
