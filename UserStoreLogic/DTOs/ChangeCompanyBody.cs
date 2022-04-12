using DTOs;

namespace UserStoreLogic.DTOs
{
    public class ChangeCompanyBody
    {
        public int Id { get; set; }
        public CompanyDto CompanyDto { get; set; }

        public ChangeCompanyBody(int id, CompanyDto companyDto)
        {
            CompanyDto = companyDto;
        }
    }
}
