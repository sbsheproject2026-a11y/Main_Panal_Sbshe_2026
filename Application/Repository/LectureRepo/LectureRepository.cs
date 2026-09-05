using Application.DTOs;
using Domain;
using Microsoft.EntityFrameworkCore;
using Model;




namespace Application.Repository
{
    public class LectureRepository : ILectureRepository
    {
        private readonly ApplicationDbContext _context;

        public LectureRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LectureGetDTOs>> GetAll(int id)
        {
            try
            {
                var data = await _context.Lecture.Where(x => x.IsActive == 1 && x.CourseId== id)
                    .Select(x => new LectureGetDTOs
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Code = x.Code,
                        TheoryMarks = x.TheoryMarks,
                        PracticalMarks = x.PracticalMarks,
                        AssignmentMarks = x.AssignmentMarks,
                        MinMarks = x.MinMarks,
                        MaxMarks = x.MaxMarks,
                        CourseId = x.CourseId,


                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching Lecture data.", ex);
            }
        }

        public async Task<string> CreateSubject(LecturePostDTOs model)
        {
            try
            {

                if (model.CourseId == null || model.CourseId == 0)
                {
                    return "Invalid CourseId";
                }
                if (model.Subjects == null || model.Subjects.Count == 0)
                {
                    return "At least one subject is required";
                }

                 


                foreach (var item in model.Subjects)
                {
                    if (string.IsNullOrWhiteSpace(item.SubjectName))
                    {
                        return "Subject name is required";
                    }


                    var data = new Lecture
                    {
                        CourseId = model.CourseId,

                        Title = item.SubjectName,
                        Code = item.SubjectCode,
                        TheoryMarks = model.TheoryMarks,
                        PracticalMarks = model.PracticalMarks,
                        AssignmentMarks = model.AssignmentMarks,

                        MinMarks = model.MinMarks,
                        MaxMarks = model.MaxMarks,

                        CreatedAt = DateTime.UtcNow,

                        IsActive = 1
                    };

                    await _context.Lecture.AddAsync(data);
                }

                await _context.SaveChangesAsync();

                return "Subjects created successfully";
            }
            catch (Exception ex)
            {
                return "Something went wrong: " + ex.Message;
            }
        }
       
        
        public async Task<LectureGetDTOs> GetById(int id)
        {
            try
            {
                var data = await _context.Lecture
                    .Where(x => x.Id == id)
                    .Select(x => new LectureGetDTOs
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Code = x.Code,
                        TheoryMarks = x.TheoryMarks,
                        PracticalMarks = x.PracticalMarks,
                        AssignmentMarks = x.AssignmentMarks,
                        MinMarks = x.MinMarks,
                        MaxMarks = x.MaxMarks,
                        CourseId = x.CourseId,
                    })
                    .FirstOrDefaultAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Lecture by id.", ex);
            }
        }

        public async Task<string> Update(LecturePostDTOs model)
        {
            try
            {
                var data = await _context.Lecture
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (data == null)
                {
                    return "Lecture not found";
                }

                data.Title = model.Title;
                data.Code = model.Code;
                data.TheoryMarks = model.TheoryMarks;
                data.PracticalMarks = model.PracticalMarks;
                data.AssignmentMarks = model.AssignmentMarks;
                data.MinMarks = model.MinMarks;
                data.MaxMarks = model.MaxMarks;
                

                await _context.SaveChangesAsync();

                return "Lecture updated successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating Lecture", ex);
            }
        }
        public async Task<string> Delete(int Id)
        {
            try
            {
                var data = await _context.Lecture
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return "Lecture not found";
                }

                data.IsActive = 0;


                await _context.SaveChangesAsync();

                return "Lecture delete successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while delete Lecture", ex);
            }
        }



        //----------------------Master Type Drop Down----------------------------------------


    }
}
