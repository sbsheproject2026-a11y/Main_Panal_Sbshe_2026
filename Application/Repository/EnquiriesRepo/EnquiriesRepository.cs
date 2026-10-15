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
                        EnquiryDate = x.EnquiryDate.Value.ToString("dd/MM/yyyy hh:mm tt"),
                        Email = x.Email,
                        Course = x.Course,
                        Address = x.Address,
                        Status = x.Status,
                        Subject = x.Subject,
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
                    Subject = model.Subject ?? null,
                    Course = model.Course ?? null,
                    EnquiryDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CourseId = model.CourseId ?? null,
                    Address = model.Address ?? null,
                    Status = model.Status ?? null,
                    Remarks = model.Remarks ?? null,
                    TypeId = model.TypeId,
                    IsActive = 1

                };

                await _context.Enquiries.AddAsync(data);
                await _context.SaveChangesAsync();

                return "successfully submit";
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


        public async Task<List<RTIApplicationsGetDTOs>> GetRTIAll()
        {
            try
            {
                var data = await (from p in _context.RTIApplications
                                  join d in _context.RTIDocuments  on p.Id equals d.RTIApplicationsId into docGroup
                                  from d in docGroup.DefaultIfEmpty()
                                  join mtd in _context.MasterTypeDetail on p.StatusId equals mtd.Id into mtd1
                                  from mtd in mtd1.DefaultIfEmpty()
                                  join mtd2 in _context.MasterTypeDetail on p.ApplicantTypeId equals mtd2.Id into mtd12
                                  from mtd2 in mtd12.DefaultIfEmpty()
                                  where p.IsActive == 1
                                  orderby p.Id descending
                                  select new RTIApplicationsGetDTOs
                                  {
                                      Id = p.Id,
                                      TrackingId = p.TrackingId,
                                      ApplicantTypeId = p.ApplicantTypeId,
                                      ApplicantType = mtd2.Name,
                                      StatusId = p.StatusId,
                                      Name = p.Name,
                                      MobileNo = p.MobileNo,
                                      Email = p.Email,
                                      Address = p.Address,
                                      Subject = p.Subject,
                                      StatusName= mtd.Name,
                                      InfoRequired = p.InfoRequired,
                                      Code = p.Code,
                                      Remarks = p.Remarks,
                                      CreatedAt = p.CreatedAt,
                                      UpdatedAt = p.UpdatedAt,
                                      FilePath         = d.FilePath,
                                      FilePath1 = d.LetterHeadImage,
                                  })
                                 .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching RTI data.", ex);
            }
        }

        public async Task<RTIApplicationsGetDTOs> GetRTIByTraickId(string id)
        {
            try
            {
                var data = await (from p in _context.RTIApplications
                                  join mtd in _context.MasterTypeDetail  on p.StatusId equals mtd.Id into mtd1
                                  from mtd in mtd1.DefaultIfEmpty()
                                  join mtd2 in _context.MasterTypeDetail  on p.ApplicantTypeId equals mtd2.Id into mtd12
                                  from mtd2 in mtd12.DefaultIfEmpty()
                                  where p.IsActive == 1  && (p.TrackingId == id
                                      || p.MobileNo == id  || p.Email == id)
                                  orderby p.Id descending
                                  select new RTIApplicationsGetDTOs
                                  {
                                      Id = p.Id,
                                      TrackingId = p.TrackingId,
                                      ApplicantTypeId = p.ApplicantTypeId,
                                      ApplicantType = mtd2 != null ? mtd2.Name : null,
                                      StatusId = p.StatusId,
                                      StatusName = mtd != null ? mtd.Name : null,
                                      Name = p.Name,
                                      MobileNo = p.MobileNo,
                                      Email = p.Email,
                                      Address = p.Address,
                                      Subject = p.Subject,
                                      InfoRequired = p.InfoRequired,
                                      Code = p.Code,
                                      Remarks = p.Remarks,
                                      CreatedAt = p.CreatedAt,
                                      UpdatedAt = p.UpdatedAt,

                                      // ✅ Sub-query (no duplicates)
                                      FilePath = _context.RTIDocuments
                                          .Where(x => x.RTIApplicationsId == p.Id
                                                   && x.FilePath != null)
                                          .Select(x => x.FilePath)
                                          .FirstOrDefault(),

                                      FilePath1 = _context.RTIDocuments
                                          .Where(x => x.RTIApplicationsId == p.Id
                                                   && x.LetterHeadImage != null)
                                          .Select(x => x.LetterHeadImage)
                                          .FirstOrDefault(),
                                  })
                                   .FirstOrDefaultAsync();  

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching RTI data.", ex);
            }
        }

        public async Task<string> RTICreate(RTIApplicationsDTOs model)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Generate Tracking ID (last + 1)
                var lastRecord = await _context.RTIApplications
                    .Where(x => x.TrackingId != null && x.TrackingId.StartsWith("RTI"))
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                int nextNumber = 1;
                if (lastRecord != null && !string.IsNullOrEmpty(lastRecord.TrackingId))
                {
                    // Extract number part: "RTI0001" → 1
                    var numberPart = lastRecord.TrackingId.Replace("RTI", "").Trim();
                    if (int.TryParse(numberPart, out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }

                // Format: RTI0001, RTI0002, ...
                var trackingId = "RTI" + nextNumber.ToString("D4");   // D4 = 4 digit padding

                // 2. Create RTI Application
                var rtidata = new RTIApplications
                {
                    TrackingId = trackingId,
                    Name = model.Name,
                    MobileNo = model.MobileNo ?? null,
                    Email = model.Email ?? null,
                    Subject = model.Subject ?? null,
                    InfoRequired = model.InfoRequired ?? null,
                    Address = model.Address ?? null,
                    Remarks = model.Remarks ?? null,
                    ApplicantTypeId = model.ApplicantTypeId,
                    StatusId = 138,
                    Code = model.Code ?? null,
                    CreatedAt = DateTime.Now,
                    IsActive=1



                };

                await _context.RTIApplications.AddAsync(rtidata);
                await _context.SaveChangesAsync();

                // 3. Create RTI Documents



                var rTIDocuments = new RTIDocuments
                {
                    RTIApplicationsId = rtidata.Id,
                    TypeId = model.TypeId,

                    FilePath = model.FilePath,
                    LetterHeadImage = model.FilePath1,

                    CreatedAt = DateTime.Now,
                 


                };

                await _context.RTIDocuments.AddAsync(rTIDocuments);


                await _context.SaveChangesAsync();




                await transaction.CommitAsync();
                return "Successfully submitted. Tracking ID: " + trackingId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return "Something went wrong: " + ex.Message;
            }
        }


        public async Task<bool> UpdateRTIStatus(UpdateRTIStatusDTO model)
        {
            try
            {
                var record = await _context.RTIApplications
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (record == null)
                {
                    throw new Exception("RTI record not found");
                }

                record.StatusId = model.StatusId;
                record.Remarks = model.Remarks;
                record.UpdatedAt = DateTime.Now;

                _context.RTIApplications.Update(record);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating RTI status: " + ex.Message, ex);
            }
        }


        public async Task<string> DeleteRTI(int Id)
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
