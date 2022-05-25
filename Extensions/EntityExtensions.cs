using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Extensions
{
    public static class EntityExtensions
    {
        public static void CheckForNull(this EntityBase? entity, string paramName)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(paramName,$"Not found");
            }

        }

        public static void CheckQueryForNull(this IQueryable<EntityBase>? entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities), $"{nameof(entities)} not found");
            }
        }

        public static void CheckEnumerableForNull(this IEnumerable<EntityBase>? entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities), $"{nameof(entities)} not found");
            }
        }
    }
}
