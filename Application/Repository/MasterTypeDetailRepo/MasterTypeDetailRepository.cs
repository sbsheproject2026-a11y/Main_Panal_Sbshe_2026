using Application.DTOs.MasterTypeDTOs;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;




namespace Application.Repository
{
    public class MasterTypeDetailRepository : IMasterTypeDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public MasterTypeDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MasterTypeDetailGetDTOs>> GetAll()
        {
            try
            {

                var data = await (from pa in _context.MasterTypeDetail
                                  join p in _context.MasterType on pa.MasterTypeId equals p.Id into productGroup
                                  from p in productGroup.DefaultIfEmpty()

                                  where pa.IsDelete == 0

                                  orderby pa.Id descending
                                  select new MasterTypeDetailGetDTOs
                                  {
                                      Id = pa.Id,
                                      SrNo = pa.SrNo,
                                      Name = pa.Name,
                                      Code = pa.Code,
                                      MasterTypeId = pa.MasterTypeId,
                                      FilePath = pa.FilePath,
                                      IsActive = pa.IsActive

                                  })
                                  
                                  .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching MasterTypeDetail data.", ex);
            }
        }

        public async Task<string> Create(MasterTypeDetailPostDTOs model)
        {
            try
            {
                var data = new MasterTypeDetail
                {
                    Code = model.Code,
                    Name = model.Name,
                    MasterTypeId = model.MasterTypeId,
                    Date = DateTime.Now,
                    ParentId = model.ParentId ?? null,
                    SrNo = model.SrNo ?? null,
                    IsActive = model.IsActive,
                    FilePath = model.FilePath,
                    IsDelete = 0
                };

                await _context.MasterTypeDetail.AddAsync(data);
                await _context.SaveChangesAsync();

                return "MasterTypeDetail created successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }

        public async Task<MasterTypeDetailGetDTOs> GetById(int id)
        {
            try
            {
                var data = await _context.MasterTypeDetail
                    .Where(x => x.Id == id)
                    .Select(x => new MasterTypeDetailGetDTOs
                    {
                        Id = x.Id,
                        SrNo = x.SrNo,
                        Name = x.Name,
                        Code = x.Code,
                        FilePath = x.FilePath,
                        IsActive = x.IsActive,
                        MasterTypeId = x.MasterTypeId
                    })
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching MasterTypeDetail by id.", ex);
            }
        }

        public async Task<string> Update(MasterTypeDetailPostDTOs model)
        {
            try
            {
                var data = await _context.MasterTypeDetail
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (data == null)
                {
                    return "MasterTypeDetail not found";
                }

                data.Name = model.Name;
                data.Code = model.Code;
                data.ParentId = model.ParentId ?? null;
                data.SrNo = model.SrNo;
                data.IsActive = model.IsActive;
                data.FilePath = model.FilePath;

                data.MasterTypeId = model.MasterTypeId;

                await _context.SaveChangesAsync();

                return "MasterTypeDetail updated successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating MasterTypeDetail", ex);
            }
        }
        public async Task<string> Delete(int Id)
        {
            try
            {
                var data = await _context.MasterTypeDetail
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return "MasterTypeDetail not found";
                }

                data.IsDelete = 1;


                await _context.SaveChangesAsync();

                return "MasterTypeDetail delete successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while delete MasterTypeDetail", ex);
            }
        }



        //----------------------Master Type Drop Down----------------------------------------


    }
}
