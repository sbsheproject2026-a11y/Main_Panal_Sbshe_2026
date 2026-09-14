using Application.DTOs.MasterTypeDTOs;
using Domain;
using Microsoft.EntityFrameworkCore;
using Model;




namespace Application.Repository
{
    public class EnquiriesRepository : IEnquiriesRepository
    {
        private readonly ApplicationDbContext _context;

        public EnquiriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EnquiriesGetDTOs>> GetAll(int id)
        {
            try
            {
                var data = await _context.Enquiries.Where(x => x.TypeId == id && x.IsActive == 1).OrderByDescending(x => x.Id)
                    .Select(x => new EnquiriesGetDTOs
                    {
                        Id = x.Id,
                        Name = x.Name,
                        MobileNo = x.MobileNo,
                        Email = x.Email,
                        Course = x.Course,
                        Address = x.Address,
                        Status = x.Status,
                        Remarks = x.Remarks

                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching MasterType data.", ex);
            }
        }

        public async Task<string> Create(EnquiriesPostDTOs model)
        {
            try
            {
                var data = new Enquiries
                {

                    Name = model.Name,
                    MobileNo = model.MobileNo ?? null,
                    Email = model.Email ?? null,
                    Course = model.Course ?? null,
                    EnquiryDate=DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CourseId = model.CourseId ?? null,
                    Address = model.Address ?? null,
                    Status = model.Status ?? null,
                    Remarks = model.Remarks ?? null,
                    TypeId = model.TypeId,
                    IsActive=1 

                };

                await _context.Enquiries.AddAsync(data);
                await _context.SaveChangesAsync();

                return "Student created successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<string> Delete(int Id)
        {
            try
            {
                var data = await _context.Enquiries
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return "MasterType not found";
                }

                data.IsActive = 0;


                await _context.SaveChangesAsync();

                return " Enquiries delete successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating Enquiries", ex);
            }
        }




    }
}
