using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;

namespace Extensions
{
    public static class EntityExtensions
    {
        public static void CheckForNull(this EntityBase? entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} not found");
            }
        }
    }
}
