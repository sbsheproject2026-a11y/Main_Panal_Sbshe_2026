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

        public async Task<List<CourseGetDTOs>> GetAll(
            int referenceId,
            int pageNo,
            int pageSize,
            string? search)
        {
            try
            {
                var data = new List<CourseGetDTOs>();

                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("GetCourse", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@ReferenceId", referenceId));

                command.Parameters.Add(new SqlParameter("@PageNo", pageNo));

                command.Parameters.Add(new SqlParameter("@PageSize", pageSize));

                command.Parameters.Add(new SqlParameter("@Search", search ?? (object)DBNull.Value));

                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    data.Add(new CourseGetDTOs
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        ParentId = Convert.ToInt32(reader["ParentId"]),
                        IsActive = Convert.ToInt32(reader["IsActive"]),
                        Code = reader["Code"]?.ToString(),
                        Name = reader["Name"]?.ToString(),
                        NameHindi = reader["CourseNameHindi"]?.ToString(),
                        DurationName = reader["Duration"]?.ToString(),
                        CategoryName = reader["Category"]?.ToString(),
                        DepartmentName = reader["Department"]?.ToString()
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


        public async Task<string> Create(CoursePostDTOs model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("insCourse", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Code", model.Code));
                command.Parameters.Add(new SqlParameter("@ShortName", model.ShortName));
                command.Parameters.Add(new SqlParameter("@Name", model.Name));
                command.Parameters.Add(new SqlParameter("@NameHindi", model.NameHindi));
                command.Parameters.Add(new SqlParameter("@Duration", model.Duration));
                command.Parameters.Add(new SqlParameter("@DurationTypeId", model.DurationTypeId));
                command.Parameters.Add(new SqlParameter("@ParentId", model.ParentId ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@IsActive", model.IsActive));
                command.Parameters.Add(new SqlParameter("@CategoryId", model.CategoryId));
                command.Parameters.Add(new SqlParameter("@DepartmentId", model.DepartmentId));
                command.Parameters.Add(new SqlParameter("@TotalClass", model.TotalClass));
                command.Parameters.Add(new SqlParameter("@ClassNo", model.ClassNo));

                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                return result?.ToString() ?? "Course created successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<CourseGetDTOs?> GetById(int id)
        {
            try
            {
                CourseGetDTOs? data = null;

                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(
                    "getEditCourse",
                    connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(
                    new SqlParameter("@Id", id));

                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    data = new CourseGetDTOs
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        IsActive = Convert.ToInt32(reader["IsActive"]),
                        Duration = Convert.ToInt32(reader["Duration"]),
                        DurationTypeId =
                            Convert.ToInt32(reader["DurationTypeId"]),
                        CategoryId =
                            Convert.ToInt32(reader["CategoryId"]),
                        DepartmentId =
                            Convert.ToInt32(reader["DepartmentId"]),
                        TotalClass =
                            Convert.ToInt32(reader["TotalClass"]),
                        ClassNo =
                            Convert.ToInt32(reader["ClassNo"]),
                        ParentId =
                            reader["ParentId"] == DBNull.Value
                                ? null
                                : Convert.ToInt32(reader["ParentId"]),
                        Code = reader["Code"]?.ToString(),
                        Name = reader["Name"]?.ToString(),
                        NameHindi =
                            reader["CourseNameHindi"]?.ToString()
                    };
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Course by id.",
                    ex);
            }
        }


        public async Task<string> Update(CoursePostDTOs model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("UpdateCourse", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Id", model.Id));

                command.Parameters.Add(new SqlParameter("@Code", model.Code));

                command.Parameters.Add(new SqlParameter("@ShortName", model.ShortName));

                command.Parameters.Add(new SqlParameter("@Name", model.Name));

                command.Parameters.Add(new SqlParameter("@NameHindi", model.NameHindi));

                command.Parameters.Add(new SqlParameter("@Duration", model.Duration));

                command.Parameters.Add(new SqlParameter("@DurationTypeId", model.DurationTypeId));

                command.Parameters.Add(new SqlParameter("@ParentId", model.ParentId ?? (object)DBNull.Value));

                command.Parameters.Add(new SqlParameter("@IsActive", model.IsActive));

                command.Parameters.Add(new SqlParameter("@CategoryId", model.CategoryId));

                command.Parameters.Add(new SqlParameter("@DepartmentId", model.DepartmentId));

                command.Parameters.Add(new SqlParameter("@TotalClass", model.TotalClass));

                command.Parameters.Add(new SqlParameter("@ClassNo", model.ClassNo));

                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                return result?.ToString()
                    ?? "Course updated successfully";
            }
            catch (Exception ex)
            {
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
                    new SqlParameter("@Code", 0));

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

        //ProductAmount  Working Start//
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

        // --------------------------------------- Course End ---------------------------------------
    }
}
