using Application.DTOs;

namespace Application.Repository
{
    public interface IDocumentProfileRepository
    {

        
        Task<StudentGetDTOs> GetDiplomaPrintData(int id);
        Task<StudentGetDTOs> GetMarksheetPrintData(string id);
        Task<List<GetStudentSubjectMarksGetDTOs>> PrintStudentSubjectMarks(string id);
        Task<List<GetStudentSubjectMarksGetDTOs>> PrintStudentSubjectAdmitCard(string id);

    }
}
