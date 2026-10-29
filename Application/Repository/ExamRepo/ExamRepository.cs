using Application.DTOs;
using Domain;
using Microsoft.EntityFrameworkCore;
using Model;
using System;




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
                    .Where(p => p.IsActive == 1 && p.SubjectId == id)
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
                    .Where(p => p.IsActive == 1 && p.Id == id)
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

                data.IsActive = 0;


                await _context.SaveChangesAsync();

                return "Exam delete successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while delete Lecture", ex);
            }
        }


        public async Task<List<ExamGetDTOs>> StudentExamDetails(int id)
        {
            try
            {
                var data = await _context.Paper
                    .Where(p => p.IsActive == 1 && p.CousreId == id)
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
                         
                    })
                    .ToListAsync();

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
                    {  Id = p.Id,
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


        //----------------------Master Type Drop Down----------------------------------------


    }
}
