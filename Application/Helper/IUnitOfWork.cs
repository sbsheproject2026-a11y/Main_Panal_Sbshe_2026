using Application.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public interface IUnitOfWork
    {
        IMasterTypeRepository MasterType {  get; }
        IMasterTypeDetailRepository MasterTypeDetail {  get; }
        IStateRepository Location {  get; }

        IFrenchiseRepository Frenchise { get; }
        ILoginRepository Login { get; }
        IEmployeeRepository Employee { get; }
        ICourseRepository Course { get; }
        IMasterSessionRepository MasterSession { get; }
        IDropDownRepository DropDown { get; }
        IStudentRepository Student { get; }
        IDocumentProfileRepository DocumentProfile { get; }
        ILectureRepository Lecture { get; }
        IWebsiteContentRepository WebsiteContent { get; }
        IEnquiriesRepository Enquiries { get; }



        void save();
    }
}
