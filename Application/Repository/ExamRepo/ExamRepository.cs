using Application.DTOs;
using Domain;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Data;
using System.Data.SqlClient;




namespace Application.Repository
{
    public class ExamRepository : IExamRepository
    {
        private readonly ApplicationDbContext _context;

        public ExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExamGetDTOs>> GetAll(int id)
        {
            try
            {
                var data = await _context.Paper
                    .Where(p => p.IsActive == 1 & p.isDelete == 0 && p.SubjectId == id)
                    .OrderByDescending(p => p.Id)
                    .Select(p => new ExamGetDTOs
                    {
                        Id = p.Id,
                        Title = p.Title,
                        SubTitle = p.SubTitle,
                        Description = p.Description,
                        StartTime = p.StartTime,
                        EndTime = p.EndTime,
                        EntryTime = p.EntryTime,
                        PaperDate = p.PaperDate,
                        TotalQuestions = p.TotalQuestions,
                        TotalMark = p.TotalMark,
                        IsActive = p.IsActive,
                        TotalTimeInMinute = p.TotalTimeInMinute,


                        Questiones = _context.Questiones
                            .Where(q => q.PaperId == p.Id && q.IsActive == 1)
                            .Select(q => new QuestionesGetDTOs
                            {
                                Id = q.Id,
                                Question = q.Question,
                                QuestionMarks = q.QuestionMarks,
                                PaperId = q.PaperId,
                                IsActive = q.IsActive,
                            })
                            .ToList()
                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Exam/Paper data.", ex);
            }
        }

        public async Task<string> CreatePaper(ExamPostDTOs model)
        {
            try
            {

                if (model.CousreId == null || model.CousreId == 0)
                {
                    return "Invalid CourseId";
                }


                var existingActivePapers = await _context.Paper
                    .Where(x => x.SubjectId == model.SubjectId && x.IsActive == 1)
                    .ToListAsync();

                if (existingActivePapers.Any())
                {
                    foreach (var oldPaper in existingActivePapers)
                    {
                        oldPaper.IsActive = 0;
                        oldPaper.UpdateDate = DateTime.Now;
                    }

                    await _context.SaveChangesAsync();
                }


                var newPaper = new Paper
                {
                    Title = model.Title,
                    SubTitle = model.SubTitle,
                    Description = model.Description,
                    TypeId = null,
                    CategoryId = null,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    EntryTime = model.EntryTime,
                    PaperDate = model.PaperDate,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    TotalQuestions = model.TotalQuestions,
                    TotalMark = model.TotalMark,
                    ParentId = model.ParentId ?? null,
                    CousreId = model.CousreId,
                    SubjectId = model.SubjectId,
                    IsActive = model.IsActive,
                    isDelete = 0,
                    TotalTimeInMinute = model.TotalTimeInMinute,
                };

                await _context.Paper.AddAsync(newPaper);
                await _context.SaveChangesAsync();


                if (model.Questiones != null && model.Questiones.Any())
                {
                    foreach (var item in model.Questiones)
                    {
                        if (string.IsNullOrWhiteSpace(item.Question))
                        {
                            return "Question name is required";
                        }

                        var newQuestion = new Questiones
                        {
                            PaperId = newPaper.Id,
                            Question = item.Question,
                            QuestionMarks = item.QuestionMarks,
                            AddedDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            IsActive = 1
                        };

                        await _context.Questiones.AddAsync(newQuestion);
                    }

                    await _context.SaveChangesAsync();
                }

                return "Exam and Questions created successfully";
            }
            catch (Exception ex)
            {

                throw new Exception("Something went wrong: " + ex.Message, ex);
            }
        }

        public async Task<ExamGetDTOs> GetById(int id)
        {
            try
            {
                var data = await _context.Paper
                    .Where(p => p.IsActive == 1 && p.isDelete == 0 && p.Id == id)
                    .Select(p => new ExamGetDTOs
                    {
                        Id = p.Id,
                        Title = p.Title,
                        SubTitle = p.SubTitle,
                        Description = p.Description,
                        StartTime = p.StartTime,
                        EndTime = p.EndTime,
                        EntryTime = p.EntryTime,
                        PaperDate = p.PaperDate,
                        TotalQuestions = p.TotalQuestions,
                        TotalMark = p.TotalMark,
                        IsActive = p.IsActive,
                        CousreId = p.CousreId,
                        SubjectId = p.SubjectId,
                        TotalTimeInMinute = p.TotalTimeInMinute,


                        Questiones = _context.Questiones
                            .Where(q => q.PaperId == p.Id && q.IsActive == 1)
                            .Select(q => new QuestionesGetDTOs
                            {
                                Id = q.Id,
                                Question = q.Question,
                                QuestionMarks = q.QuestionMarks,

                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync();

                return data;

            }
            catch (Exception ex)
            {

                throw new Exception("Error while fetching Exam by id.", ex);
            }
        }

        public async Task<string> Update(ExamPostDTOs model)
        {
            try
            {
                // 1. Check karo ki Exam exist karta hai ya nahi
                var data = await _context.Paper.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (data == null)
                {
                    return "Exam not found with the provided Id.";
                }

                // 2. Paper ki basic details update karo
                data.Title = model.Title;
                data.SubTitle = model.SubTitle;
                data.Description = model.Description;
                data.TypeId = null;
                data.CategoryId = null;
                data.StartTime = model.StartTime;
                data.EndTime = model.EndTime;
                data.EntryTime = model.EntryTime;
                data.PaperDate = model.PaperDate;

                // CreateDate ko change nahi karna hai, sirf UpdateDate change karo
                data.UpdateDate = DateTime.Now;

                data.TotalQuestions = model.TotalQuestions;
                data.TotalMark = model.TotalMark;
                data.ParentId = model.ParentId ?? null;
                data.CousreId = model.CousreId;
                data.SubjectId = model.SubjectId;
                data.IsActive = model.IsActive;
                data.TotalTimeInMinute = model.TotalTimeInMinute;


                var existingQuestions = await _context.Questiones
                    .Where(q => q.PaperId == data.Id)
                    .ToListAsync();

                if (existingQuestions.Any())
                {

                    _context.Questiones.RemoveRange(existingQuestions);


                }

                // 4. Naye Questions add karo
                if (model.Questiones != null && model.Questiones.Any())
                {
                    foreach (var item in model.Questiones)
                    {
                        if (string.IsNullOrWhiteSpace(item.Question))
                        {
                            return "Question name is required";
                        }

                        var dataq = new Questiones
                        {
                            PaperId = data.Id,
                            Question = item.Question,
                            QuestionMarks = item.QuestionMarks,
                            AddedDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            IsActive = 1
                        };

                        await _context.Questiones.AddAsync(dataq);
                    }
                }

                // 5. Saare changes save karo
                await _context.SaveChangesAsync();

                return "Exam updated successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating Exam", ex);
            }
        }

        public async Task<string> Delete(int Id)
        {
            try
            {
                var data = await _context.Paper
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return "Exam not found";
                }

                data.isDelete = 1;


                await _context.SaveChangesAsync();

                return "Exam delete successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while delete Lecture", ex);
            }
        }


        public async Task<List<ExamGetDTOs>> StudentExamDetails(int courseId, int userId)
        {
            try
            {
                var data = await (from p in _context.Paper
                                  where p.IsActive == 1 && p.isDelete == 0 && p.CousreId == courseId
                                  join pa in _context.PaperAttempt.Where(x => x.UserId == userId)
                                  on p.Id equals pa.PaperId into attempts
                                  from pa in attempts.DefaultIfEmpty()
                                  orderby p.Id descending
                                  group new { p, pa } by new { p.Id, p.Title, p.SubTitle, p.Description, p.StartTime, p.EndTime, p.EntryTime, p.PaperDate, p.TotalQuestions, p.TotalMark, p.IsActive, p.TotalTimeInMinute } into g
                                  select new ExamGetDTOs
                                  {
                                      Id = g.Key.Id,
                                      Title = g.Key.Title,
                                      SubTitle = g.Key.SubTitle,
                                      Description = g.Key.Description,
                                      StartTime = g.Key.StartTime,
                                      EndTime = g.Key.EndTime,
                                      EntryTime = g.Key.EntryTime,
                                      PaperDate = g.Key.PaperDate,
                                      TotalQuestions = g.Key.TotalQuestions,
                                      TotalMark = g.Key.TotalMark,
                                      IsActive = g.Key.IsActive,
                                      TotalTimeInMinute = g.Key.TotalTimeInMinute,

                                      // Get the latest attempt from the group
                                      PaperAttemptId = g.OrderByDescending(x => x.pa != null ? x.pa.Id : 0).Select(x => x.pa != null ? x.pa.Id : (int?)null).FirstOrDefault(),
                                      IsAttempted = g.Any(x => x.pa != null) ? 1 : 0,
                                      StartDate = g.OrderByDescending(x => x.pa != null ? x.pa.Id : 0).Select(x => x.pa != null ? x.pa.StartDate : (DateTime?)null).FirstOrDefault(),
                                      EndDate = g.OrderByDescending(x => x.pa != null ? x.pa.Id : 0).Select(x => x.pa != null ? x.pa.EndDate : (DateTime?)null).FirstOrDefault(),
                                      IsCompleted = g.Any(x => x.pa != null && x.pa.IsActive == 1) ? 1 : 0,
                                  }).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Exam/Paper data.", ex);
            }
        }

        public async Task<List<QuestionesGetDTOs>> GetExamQuestionDetails(int id)
        {
            try
            {
                var data = await _context.Questiones
                    .Where(p => p.IsActive == 1 && p.PaperId == id)
                    // .OrderByDescending(p => p.Id)
                    .Select(p => new QuestionesGetDTOs
                    {
                        Id = p.Id,
                        Question = p.Question,
                        QuestionMarks = p.QuestionMarks,


                    })
                    .ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Exam/Paper data.", ex);
            }
        }


        public async Task<PaperAttempt> ExamStart(int Id, int UserId)
        {
            try
            {
                // 1. Check karein ki kya pehle se koi active attempt hai
                var existingAttempt = await _context.PaperAttempt
                    .FirstOrDefaultAsync(x => x.PaperId == Id && x.UserId == UserId && x.IsActive == 1);

                if (existingAttempt != null)
                {
                    // Agar pehle se chalu hai, toh wahi return kar dein
                    return existingAttempt;
                }

                // 2. Agar nahi hai, toh nayi entry banayein
                var newPaper = new PaperAttempt
                {
                    PaperId = Id,
                    UserId = UserId,
                    StartDate = DateTime.Now,
                    IsActive = 0
                };

                await _context.PaperAttempt.AddAsync(newPaper);
                await _context.SaveChangesAsync();

                return newPaper; // Frontend ko ID chahiye hogi, isliye object return karein
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while starting the exam: " + ex.Message, ex);
            }
        }
        public async Task<PaperAttempt> ExamEnd(SubmitExamDto model)
        {
            try
            {
                // 1. Exact usi Attempt ko dhoondein jo abhi active hai
                var existingAttempt = await _context.PaperAttempt
                    .FirstOrDefaultAsync(x =>
                        x.Id == model.AttemptId &&
                        x.PaperId == model.PaperId &&
                        x.UserId == model.UserId);

                // Agar attempt nahi mila to error throw karein
                if (existingAttempt == null)
                {
                    throw new Exception("Active Exam Attempt not found for this student.");
                }

                // 2. End Date aur Time set karein (Jis time usne button dabaya)
                existingAttempt.EndDate = DateTime.Now;
                existingAttempt.IsActive = 1; // Maan lijiye 1 ka matlab Exam Completed hai

                // 3. Agar File upload hui hai, to uski entry Document table me karein
                if (!string.IsNullOrEmpty(model.Path))
                {
                    var newPaperdoc = new PaperAttemptDocument
                    {
                        PaperAttemptId = model.AttemptId, // Link document to this attempt
                        PaperId = model.PaperId,
                        UserId = model.UserId,
                        Path = model.Path, // Controller se aayi hui file ka naam
                        IsActive = 1
                    };

                    await _context.PaperAttemptDocument.AddAsync(newPaperdoc);
                }

                // 4. Changes Save karein
                await _context.SaveChangesAsync();

                // 5. Updated data frontend ko return karein
                return existingAttempt;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while ending the exam: " + ex.Message, ex);
            }
        }


        public async Task<List<StudentGetDTOs>> GetStudentAllResult(int referenceId)
        {
            try
            {
                var data = new List<StudentGetDTOs>();

                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetStudentAllResult", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", referenceId));


                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            data.Add(new StudentGetDTOs
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Code = reader["Code"]?.ToString(),
                                Name = reader["SubjectName"]?.ToString(),
                                Date = reader["PaperDate"]?.ToString(),
                                IssueDate = reader["StartTime"]?.ToString(),
                                IssueYear = reader["EndTime"]?.ToString(),
                                StartDate = reader["StartDate"]?.ToString(),
                                EndDate = reader["EndDate"]?.ToString(),
                                SelfImage = reader["Path"]?.ToString(),

                            });

                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Frenchise data.", ex);
            }
        }

        public async Task<List<StudentGetDTOs>> GetSubmitStudentAllResult(int referenceId)
        {
            try
            {
                var data = new List<StudentGetDTOs>();

                var connectionString = _context.Database.GetConnectionString();

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("GetSubmitStudentAllResult", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", referenceId));


                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            data.Add(new StudentGetDTOs
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Code = reader["EnrollmentNo"]?.ToString(),
                                Name = reader["Name"]?.ToString(),
                                CourseName = reader["CourseName"]?.ToString(),
                                

                            });

                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching Frenchise data.", ex);
            }
        }



        //----------------------Master Type Drop Down----------------------------------------


    }
}
