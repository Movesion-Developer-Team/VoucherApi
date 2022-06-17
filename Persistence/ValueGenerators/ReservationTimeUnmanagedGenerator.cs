using Core.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Persistence.ValueGenerators
{
    public class ReservationTimeUnmanagedGenerator : ValueGenerator<DateTimeOffset?>
    {
        public override DateTimeOffset? Next(EntityEntry entry)
        {
            var reserved = entry.Property(nameof(DiscountCode.TemporaryReserved)).CurrentValue;
            if ((bool)reserved)
            {
                return DateTimeOffset.UtcNow;
            }

            return null;
        }

        public override bool GeneratesTemporaryValues => false;

        public ReservationTimeUnmanagedGenerator()
        {
            
        }
        

    }
}
