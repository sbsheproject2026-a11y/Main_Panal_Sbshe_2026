using Application.Repository;
using Application.Repository.LoginRepo;
using Domain;
using Microsoft.AspNetCore.Http;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IMasterTypeRepository MasterType { get; private set; }
        public IMasterTypeDetailRepository MasterTypeDetail { get; private set; }
        public IStateRepository Location { get; private set; }
        public IFrenchiseRepository Frenchise { get; private set; }
        public ILoginRepository Login { get; private set; }
        public IEmployeeRepository Employee { get; private set; }
        public ICourseRepository Course { get; private set; }
        public IMasterSessionRepository MasterSession { get; private set; }
        public IDropDownRepository DropDown { get; private set; }
        public IStudentRepository Student { get; private set; }
        public IDocumentProfileRepository DocumentProfile { get; private set; }
        public ILectureRepository Lecture { get; private set; }
        public IWebsiteContentRepository WebsiteContent { get; private set; }
        public IEnquiriesRepository Enquiries { get; private set; }
        public IAccRepository AccRegistration { get; private set; }



        public UnitOfWork(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;

            MasterType = new MasterTypeRepository(_context);
            MasterTypeDetail = new MasterTypeDetailRepository(_context);
            Location = new StateRepository(_context);
            Frenchise = new FrenchiseRepository(_context);
            Login = new LoginRepository(_context, jwtService);
            Employee = new EmployeeRepository(_context);
            Course = new CourseRepository(_context);
            MasterSession = new MasterSessionRepository(_context);
            DropDown = new DropDownRepository(_context);
            Student = new StudentRepository(_context);
            DocumentProfile = new DocumentProfileRepository(_context);
            Lecture = new LectureRepository(_context);
            WebsiteContent = new WebsiteContentRepository(_context);
            Enquiries = new EnquiriesRepository(_context);
            AccRegistration = new AccRepository(_context);


        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
