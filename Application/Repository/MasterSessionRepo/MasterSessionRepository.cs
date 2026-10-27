using Application.DTOs;
using Application.DTOs.MasterTypeDTOs;
using Domain;
using Microsoft.EntityFrameworkCore;
using Model;




namespace Application.Repository
{
    public class MasterSessionRepository : IMasterSessionRepository
    {
        private readonly ApplicationDbContext _context;

        public MasterSessionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MasterSessionGetDTOs>> GetAll()
        {
            try
            {
                var data = await (from pa in _context.MasterSession
                                  join p in _context.MasterTypeDetail on pa.MasterSessionId equals p.Id into productGroup
                                  from p in productGroup.DefaultIfEmpty()


                                  where pa.Status == 1

                                  orderby pa.Id descending
                                  select new MasterSessionGetDTOs
                                  {
                                      Id = pa.Id,
                                      Name = pa.Name,
                                      ShortName = pa.ShortName,
                                      MasterSessionName = p.Name,
                                      StartDate = pa.StartDate,
                                      EndDate = pa.EndDate,
                                      IssueDate = pa.IssueDate,
                                      MasterSessionId = (int)pa.MasterSessionId,
                                      IsActive = pa.IsActive,
                                      Status = pa.Status,
                                      ParentId = pa.ParentId

                                  })
                  .ToListAsync();

                foreach (var item in data)
                {
                    item.StartDate1 = item.StartDate?.ToString("dd-MM-yyyy");
                    item.EndDate1 = item.EndDate?.ToString("dd-MM-yyyy");
                    item.IssueDate1 = item.IssueDate?.ToString("dd-MM-yyyy");
                }
                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching MasterTypeDetail data.", ex);
            }
        }

        public async Task<string> Create(MasterSessionPostDTOs model)
        {
            try
            {
                var data = new MasterSession
                {
                    ShortName = model.ShortName,
                    Name = model.Name,
                    MasterSessionId = model.MasterSessionId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IssueDate = model.IssueDate,
                    ParentId = model.ParentId ?? null,
                    IsActive = model.IsActive,
                    Status = 1,
                };

                await _context.MasterSession.AddAsync(data);
                await _context.SaveChangesAsync();

                return "MasterSession created successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }



        public async Task<MasterSessionGetDTOs> GetById(int id)
        {
            try
            {
                var data = await _context.MasterSession
                    .Where(x => x.Id == id)
                    .Select(x => new MasterSessionGetDTOs
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ShortName = x.ShortName,
                        IsActive = x.IsActive,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        IssueDate = x.IssueDate,
                        ParentId = (int)x.ParentId,
                        MasterSessionId = (int)x.MasterSessionId,

                    })
                    .FirstOrDefaultAsync();
                if (data != null)
                {
                    data.StartDate1 = data.StartDate?.ToString("yyyy-MM-dd");
                    data.EndDate1 = data.EndDate?.ToString("yyyy-MM-dd");
                    data.IssueDate1 = data.IssueDate?.ToString("yyyy-MM-dd");
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching MasterTypeDetail by id.", ex);
            }
        }

        public async Task<string> Update(MasterSessionPostDTOs model)
        {
            try
            {
                var data = await _context.MasterSession
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (data == null)
                {
                    return "MasterSession not found";
                }

                data.Name = model.Name;
                data.ShortName = model.ShortName;
                data.MasterSessionId = model.MasterSessionId;
                data.ParentId = model.ParentId;
                data.StartDate = model.StartDate;
                data.EndDate = model.EndDate;
                data.IssueDate = model.IssueDate;
                data.IsActive = model.IsActive;

                await _context.SaveChangesAsync();

                return "MasterSession updated successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating MasterSession", ex);
            }
        }
        public async Task<string> Delete(int Id)
        {
            try
            {
                var data = await _context.MasterSession
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return "MasterSession not found";
                }

                data.Status = 0;


                await _context.SaveChangesAsync();

                return "MasterSession delete successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while delete MasterSession", ex);
            }
        }



        //----------------------Master Type Drop Down----------------------------------------



    }
}
