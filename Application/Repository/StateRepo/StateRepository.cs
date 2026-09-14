using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Domain;
using Microsoft.EntityFrameworkCore;
using Model;




namespace Application.Repository 
{
    public class StateRepository : IStateRepository
    {
        private readonly ApplicationDbContext _context;

        public StateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //---------------------------------------State Start------------------------------

        public async Task<List<LocationGetDTOs>> GetAll(int typeid)
        {
            try
            {
                var data = await _context.Location.Where(x => x.IsActive==1 && x.TypeId==typeid).OrderByDescending(x => x.Id)
                    .Select(x => new LocationGetDTOs
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        TypeId=x.TypeId,
                         
                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                 
                throw new Exception("Error while fetching State data.", ex);
            }
        }

        public async Task<string> Create(LocationPostDTOs model)
        {
            try
            {
                var data = new Location
                {
                    Code = model.Code,
                    Name = model.Name,
                    Date = DateTime.Now,
                    ParentId = model.ParentId ?? null,
                    IsActive = model.IsActive,
                    TypeId=model.TypeId,
                     
                };

                await _context.Location.AddAsync(data);
                await _context.SaveChangesAsync();

                return "Location created successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }

        public async Task<LocationGetDTOs> GetById(int id)
        {
            try
            {
                var data = await _context.Location
                    .Where(x => x.Id == id)
                    .Select(x => new LocationGetDTOs
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
                throw new Exception("Error while fetching Location by id.", ex);
            }
        }

        public async Task<string> Update(LocationPostDTOs model)
        {
            try
            {
                var data = await _context.Location
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (data == null)
                {
                    return "Location not found";
                }

                data.Name = model.Name;
                data.Code = model.Code;
                data.ParentId = model.ParentId ?? null;
                data.IsActive = model.IsActive;
                data.TypeId = model.TypeId;
                 

                await _context.SaveChangesAsync();

                return "Location updated successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating Location", ex);
            }
        }
       
        
        public async Task<string> Delete(int Id)
        {
            try
            {
                var data = await _context.Location
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return "MasterType not found";
                }

                data.IsActive = 0;
                

                await _context.SaveChangesAsync();

                return "Location delete successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating Location", ex);
            }
        }

        //---------------------------------------State End------------------------------


 
      

    }
}
