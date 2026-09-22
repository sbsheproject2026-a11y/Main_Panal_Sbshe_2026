using Application.DTOs.MasterTypeDTOs;
using Domain;
using Microsoft.EntityFrameworkCore;
using Model;




namespace Application.Repository 
{
    public class MasterTypeRepository : IMasterTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public MasterTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MasterTypeGetDTOs>> GetAll()
        {
            try
            {
                var data = await _context.MasterType
      .Where(x => x.IsDelete == 0 && x.IsActive == 1)
      .OrderByDescending(x => x.Id)
    

                   .Select(x => new MasterTypeGetDTOs
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                       IsActive = x.IsActive
                        
                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                 
                throw new Exception("Error while fetching MasterType data.", ex);
            }
        }

        public async Task<string> Create(MasterTypePostDTOs model)
        {
            try
            {
                var data = new MasterType
                {
                    Code = model.Code,
                    Name = model.Name,
                    Date = DateTime.Now,
                    ParentId = model.ParentId ?? null,
                    IsActive = model.IsActive,
                   
                    IsDelete = 0
                };

                await _context.MasterType.AddAsync(data);
                await _context.SaveChangesAsync();

                return "MasterType created successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }

        public async Task<MasterTypeGetDTOs> GetById(int id)
        {
            try
            {
                var data = await _context.MasterType
                    .Where(x => x.Id == id)
                    .Select(x => new MasterTypeGetDTOs
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        IsActive = x.IsActive
                    })
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching MasterType by id.", ex);
            }
        }

        public async Task<string> Update(MasterTypePostDTOs model)
        {
            try
            {
                var data = await _context.MasterType
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (data == null)
                {
                    return "MasterType not found";
                }

                data.Name = model.Name;
                data.Code = model.Code;
                data.ParentId = model.ParentId ?? 0;
                data.IsActive = model.IsActive;

                await _context.SaveChangesAsync();

                return "MasterType updated successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating MasterType", ex);
            }
        }
        public async Task<string> Delete(int Id)
        {
            try
            {
                var data = await _context.MasterType
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return "MasterType not found";
                }

                data.IsDelete = 1;
                

                await _context.SaveChangesAsync();

                return "MasterType delete successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating MasterType", ex);
            }
        }




    }
}
