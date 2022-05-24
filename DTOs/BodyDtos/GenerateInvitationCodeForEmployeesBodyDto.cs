namespace DTOs.BodyDtos
{
    public class GenerateInvitationCodeForEmployeesBodyDto
    {
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public int CompanyId { get; set; }
    }
}
