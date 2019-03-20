using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;

namespace IdentityServer4.EntityFramework.IntegrationTests
{
    public class FakeOperationalStoreNotification : IOperationalStoreNotification
    {
        public Task PersistedGrantsRemovedAsync(IEnumerable<PersistedGrant> persistedGrants)
        {
            return Task.CompletedTask;
        }
    }
}
