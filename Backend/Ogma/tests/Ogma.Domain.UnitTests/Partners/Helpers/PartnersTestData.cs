using Ogma.Domain.SharedKernel.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogma.Domain.UnitTests.Partners.Helpers;

internal static class PartnersTestData
{
    public static long NextId() => Random.Shared.NextInt64(1, long.MaxValue);

    public static EntityMetadata GetMetadata() => new EntityMetadata(DateTime.UtcNow, DateTime.UtcNow, 1);
}
