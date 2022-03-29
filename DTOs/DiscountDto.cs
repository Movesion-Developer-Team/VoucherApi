
using System.Xml.Schema;
using Enum;

namespace DTOs
{
    public class DiscountDto
    {
        public string Code { get; set; }
        public int PlayerId { get; set; }
        public string? LinkTermsAndConditions { get; set; }
        public string? UnityOfMeasurement { get; set; }
        public float DiscountValue { get; set; }
        public int NumberOfUsagePerAgency { get; set; }
        public int NumberOfUsagePerUser { get; set; }
        public int InitialPrice { get; set; }
        public int FinalPrice { get; set; }

        public DiscountType DiscountType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
