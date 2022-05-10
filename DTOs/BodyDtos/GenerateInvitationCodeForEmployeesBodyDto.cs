namespace DTOs.BodyDtos
{
    public class GenerateInvitationCodeForEmployeesBodyDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int CompanyId { get; set; }
    }
}
