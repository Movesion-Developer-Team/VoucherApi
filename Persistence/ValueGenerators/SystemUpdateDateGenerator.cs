using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Persistence.ValueGenerators
{
    public class SystemUpdateDateGenerator : ValueGenerator<DateTimeOffset?>
    {
        
        public SystemUpdateDateGenerator()
        {
           
        }

        public override DateTimeOffset? Next(EntityEntry entry)
        {
            return DateTimeOffset.UtcNow;
        }

        public override bool GeneratesTemporaryValues => false;
    }
}
