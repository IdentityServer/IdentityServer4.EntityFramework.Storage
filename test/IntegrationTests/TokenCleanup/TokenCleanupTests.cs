using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer4.EntityFramework.IntegrationTests.TokenCleanup
{
    public class TokenCleanupTests : IntegrationTest<TokenCleanupTests, PersistedGrantDbContext, OperationalStoreOptions>
    {
        private readonly DateTime _now;

        public TokenCleanupTests(DatabaseProviderFixture<PersistedGrantDbContext> fixture) : base(fixture)
        {
            _now = DateTime.UtcNow;
            foreach (var options in TestDatabaseProviders.SelectMany(x => x.Select(y => (DbContextOptions<PersistedGrantDbContext>)y)).ToList())
            {
                using (var context = new PersistedGrantDbContext(options, StoreOptions))
                {
                    context.Database.EnsureCreated();
                }
            }
        }

        [Theory, MemberData(nameof(TestDatabaseProviders))]
        public async Task RemoveExpiredGrantsAsync_ExpiredTokensCleanedUp(DbContextOptions<PersistedGrantDbContext> options)
        {
            using (var context = new PersistedGrantDbContext(options, StoreOptions))
            {
                var grants = new PersistedGrant[100];
                for (var i = 0; i < grants.Length; i++)
                {
                    grants[i] = new PersistedGrant
                    {
                        Key = Guid.NewGuid().ToString("N"),
                        Type = i % 4 == 0 ? "authorization_code" : "refresh_token",
                        SubjectId = i.ToString(),
                        ClientId = "test-client",
                        CreationTime = _now.AddDays(-60),
                        // 34 tokens should be cleaned up.
                        Expiration = _now.AddDays(i % 3 == 0 ? -1 : 1),
                        Data = "grant data"
                    };
                }
                context.PersistedGrants.AddRange(grants);
                await context.SaveChangesAsync();
            }

            var customStoreOptions = new OperationalStoreOptions
            {
                TokenCleanupBatchSize = 5
            };
            using (var context = new PersistedGrantDbContext(options, StoreOptions))
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddSingleton<IPersistedGrantDbContext>(context);
                serviceCollection.AddSingleton<IOperationalStoreNotification, FakeOperationalStoreNotification>();

                var cleanup = new EntityFramework.TokenCleanup(
                    serviceCollection.BuildServiceProvider(),
                    new FakeLogger<EntityFramework.TokenCleanup>(),
                    customStoreOptions);

                await cleanup.RemoveExpiredGrantsAsync();
            }

            using (var context = new PersistedGrantDbContext(options, StoreOptions))
            {
                var grants = await context.PersistedGrants.ToListAsync();
                Assert.Equal(100 - 34, grants.Count);
            }
        }
    }
}
