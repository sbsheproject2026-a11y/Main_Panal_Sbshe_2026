using Application.DTOs;
using Domain;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Data;
using System.Data.SqlClient;




namespace Application.Repository
{
    public class DropDownRepository : IDropDownRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public DropDownRepository(ApplicationDbContext context)
        {
            _context = context;
            _connectionString = _context.Database.GetConnectionString()
                ?? throw new InvalidOperationException("Database connection string not found.");
        }

        //---------------------------------------Frenchise Start------------------------------

        public async Task<List<DropDownDTOs>> GetMasterType()
        {
            try
            {
                var data = await _context.MasterType.Where(x => x.IsDelete == 0 && x.IsActive == 1)
                    .Select(x => new DropDownDTOs
                    {
                        Id = x.Id,
                        Name = x.Name


                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching MasterType data.", ex);
            }
        }
       
        public async Task<List<EntityUserResult>> getCourseCategory(int Id)
        {
            return await _context.EntityUserResult
                .FromSqlRaw("EXEC dbo.getCourseCategory @Id = {0}", Id)
                .ToListAsync();
        }
        public async Task<List<EntityUserResult>> getCourseStudent(int Id,int courseTypeId)
        {
            return await _context.EntityUserResult
                .FromSqlRaw("EXEC dbo.getCourseStudent @Id = {0},@courseTypeId = {1}", Id, courseTypeId)
                .ToListAsync();
        }
        public async Task<List<EntityUserResult>> getCourseOnlineStudent(int Id,int courseTypeId)
        {
            return await _context.EntityUserResult
                .FromSqlRaw("EXEC dbo.getCourseStudentonline @Id = {0},@courseTypeId = {1}", Id, courseTypeId)
                .ToListAsync();
        }
        public async Task<List<CommissionAmount>> SetFranchiseCourseAmount(int Id,int FId)
        {
            return await _context.CommissionAmount
                .FromSqlRaw("EXEC dbo.SetFranchiseCourseAmount @Id = {0},@FranchiseId={1}", Id, FId)
                .ToListAsync();
        }
        public async Task<List<CommissionAmount>> SetCourseCommission(int Id)
        {
            return await _context.CommissionAmount
                .FromSqlRaw("EXEC dbo.SetCourseCommission @Id = {0}", Id)
                .ToListAsync();
        }
        public async Task<List<EntityUserResult>> getEntityUser(int Id)
        {
            return await _context.EntityUserResult
                .FromSqlRaw("EXEC dbo.getEntityUser @Id = {0}", Id)
                .ToListAsync();
        }
        public async Task<List<EntityUserResult>> getFranchise(int Id)
        {
            return await _context.EntityUserResult
                .FromSqlRaw("EXEC dbo.getFranchiseDropDown @Id = {0}", Id)
                .ToListAsync();
        }
        public async Task<List<EntityUserResult>> getEmployeedropList(int Id)
        {
            return await _context.EntityUserResult
                .FromSqlRaw("EXEC dbo.getEmployeedropList @Id = {0}", Id)
                .ToListAsync();
        }
        public async Task<List<EntityUserResult>> getEmployeeFranchiselist(int Id)
        {
            return await _context.EntityUserResult
                .FromSqlRaw("EXEC dbo.getEmployeeFranchiselist @Id = {0}", Id)
                .ToListAsync();
        }
        public async Task<List<EntityWalletResult>> getwalletbalance(int Id)
        {
            return await _context.EntityWalletResult
                .FromSqlRaw(
                    "EXEC dbo.walletbalance @Id = {0}, @franchiseId = {1}",
                    112,Id
                )
                .ToListAsync();
        }
        

        public async Task<List<DropDownDTOs>> GetMasterSession()
        {
            try
            {
                var data = await _context.MasterSession.Where(x => x.IsActive == 1 && x.Status == 1 && x.MasterSessionId== 32 && x.ParentId==null)
                    .OrderByDescending(x => x.Id)
                    .Select(x => new DropDownDTOs
                    {
                        Id = x.Id,
                        Name = x.Name


                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching MasterType data.", ex);
            }
        }
        public async Task<List<DropDownDTOs>> GetOnlineSession()
        {
            try
            {
                var data = await _context.MasterSession.Where(x => x.IsActive == 1 && x.Status == 1 && x.MasterSessionId== 141 && x.ParentId == null)
                    .OrderByDescending(x => x.Id)
                    .Select(x => new DropDownDTOs
                    {
                        Id = x.Id,
                        Name = x.Name


                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching MasterType data.", ex);
            }
        }

        public async Task<List<DropDownDTOs>> GetMasterDetailType(int Id)
        {
            try
            {
                var data = await _context.MasterTypeDetail.Where(x => x.IsDelete == 0 && x.IsActive == 1 && x.MasterTypeId == Id)

                    .Select(x => new DropDownDTOs
                    {
                        Id = x.Id,
                        Name = x.Name


                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching MasterType data.", ex);
            }
        }
        public async Task<DropDownDTOs> GetMasterDetail(int Id)
        {
            try
            {
                var data = await _context.MasterTypeDetail
                    .Where(x => x.IsDelete == 0 && x.IsActive == 1 && x.Id == Id)
                    .Select(x => new DropDownDTOs
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching MasterType data.", ex);
            }
        }

        public async Task<List<DropDownDTOs>> GetMasterDetailTypeParent()
        {
            try
            {
                var data = await _context.MasterTypeDetail.Where(x => x.IsDelete == 0 && x.IsActive == 1 )

                    .Select(x => new DropDownDTOs
                    {
                        Id = x.Id,
                        Name = x.Name


                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching MasterType data.", ex);
            }
        }
       
        
        public async Task<List<DropDownDTOs>> GetParentCourse()
        {
            try
            {
                var data = await _context.Product.Where(x => x.IsDelete == 0 && x.IsActive == 1 && x.ParentId == null)

                    .Select(x => new DropDownDTOs
                    {
                        Id = x.Id,
                        Name = x.Name


                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching MasterType data.", ex);
            }
        }
        public async Task<List<DropDownDTOs>> GetCourseClass()
        {
            try
            {
                var data = await _context.Product.Where(x => x.IsDelete == 0 && x.IsActive == 1 && x.ParentId != null)

                    .Select(x => new DropDownDTOs
                    {
                        Id = x.Id,
                        Name = x.Name


                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching MasterType data.", ex);
            }
        }

        public async Task<List<DropDownDTOs>> GetState()
        {
            try
            {
                var data = await _context.Location.Where(x => x.TypeId == 2 && x.IsActive == 1)

                    .Select(x => new DropDownDTOs
                    {
                        Id = x.Id,
                        Name = x.Name


                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching State data.", ex);
            }
        }
        public async Task<List<DropDownDTOs>> GetDistrict(int Id)
        {
            try
            {
                var data = await _context.Location.Where(x => x.TypeId == 3 && x.ParentId == Id)

                    .Select(x => new DropDownDTOs
                    {
                        Id = x.Id,
                        Name = x.Name


                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching District data.", ex);
            }
        }
        public async Task<List<DropDownDTOs>> GetCity(int Id)
        {
            try
            {
                var data = await _context.Location.Where(x => x.TypeId == 4 && x.ParentId == Id)

                    .Select(x => new DropDownDTOs
                    {
                        Id = x.Id,
                        Name = x.Name


                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching City data.", ex);
            }
        }


        public async Task<DashboardStatsDTO> GetDashboard(int id)
        {
            try
            {
                DashboardStatsDTO data = null;

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("getDashboard", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", id));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            data = new DashboardStatsDTO
                            {
                                TotalStudent = Convert.ToInt32(reader["TotalStudent"]),
                                TotalFranchise = Convert.ToInt32(reader["TotalFranchise"]),
                                TotalAccFranchise = Convert.ToInt32(reader["TotalAccFranchise"]),
                                TotalEmployee = Convert.ToInt32(reader["TotalEmployee"]),
                                TotalCourse = Convert.ToInt32(reader["TotalCourse"]),
                                   
                            };

                            
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Employee by id.",  ex);
            }
        }
        public async Task<DashboardStatsDTO> GetDashboardEmployee(int id)
        {
            try
            {
                DashboardStatsDTO data = null;

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("getDashboardEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", id));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            data = new DashboardStatsDTO
                            {
                                TotalStudent = Convert.ToInt32(reader["TotalStudent"]),
                                TotalFranchise = Convert.ToInt32(reader["TotalFranchise"]),
                                TotalAccFranchise = Convert.ToInt32(reader["TotalAccFranchise"]),
                                TotalEmployee = Convert.ToInt32(reader["TotalEmployee"]),
                                TotalCourse = Convert.ToInt32(reader["TotalCourse"]),
                                   
                            };

                            
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error while fetching Employee by id.",  ex);
            }
        }

        public async Task<List<EntityUserResult>> GetStudyCentreCourse(int Id)
        {
            return await _context.EntityUserResult
                .FromSqlRaw("EXEC dbo.GetStudyCentreCourse @Id = {0}", Id)
                .ToListAsync();
        }
        public async Task<List<EntityUserResult>> GetStudyCentreDepartment(int Id)
        {
            return await _context.EntityUserResult
                .FromSqlRaw("EXEC dbo.GetStudyCentreDepartment @Id = {0}", Id)
                .ToListAsync();
        }

        public async Task<List<EntityUserResult>> getCourseStudyCentreStudent(int Id, int courseTypeId)
        {
            return await _context.EntityUserResult
                .FromSqlRaw("EXEC dbo.getCourseStudyCentreStudent @Id = {0},@courseTypeId = {1}", Id, courseTypeId)
                .ToListAsync();
        }
        public async Task<List<EntityUserResult>> GetStudyCentreState(int Id)
        {
            return await _context.EntityUserResult
                .FromSqlRaw("EXEC dbo.GetStudyCentreState @Id = {0}", Id)
                .ToListAsync();
        }

        public async Task<List<ProductAmountDtos>> getCourseAmount(int Id)
        {
            try
            {
                var data = await _context.ProductAmount.Where(x =>x.ProductId == Id && x.IsActive==1)

                    .Select(x => new ProductAmountDtos
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Amount = x.Amount,
                        AmountTypeId = x.AmountTypeId


                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching City data.", ex);
            }
        }



    }
}
