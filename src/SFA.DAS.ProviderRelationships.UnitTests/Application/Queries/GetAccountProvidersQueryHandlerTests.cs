using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class GetAccountProvidersQueryHandlerTests : FluentTest<GetAccountProvidersQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingGetAddedProvidersQuery_ThenShouldReturnGetAddedProvidersQueryResult()
        {
            return RunAsync(f => f.SetAccountProviders(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                
                r.AccountProviders.Should().NotBeNull().And.BeEquivalentTo(
                    new AccountProviderSummaryDto
                    {
                        Id = f.AccountProvider.Id,
                        ProviderName = f.Provider.Name
                    });
            });
        }

        [Test]
        public Task Handle_WhenHandlingGetAddedProvidersQueryNoProvidersAdded_ThenShouldReturnGetAddedProvidersQueryResponseWithEmptyProvidersList()
        {
            return RunAsync(f => f.Handle(), (f, r) => 
            {
                r.Should().NotBeNull();
                r.AccountProviders.Should().NotBeNull().And.BeEmpty();
            });
        }
    }

    public class GetAccountProvidersQueryHandlerTestsFixture
    {
        public GetAccountProvidersQuery Query { get; set; }
        public IRequestHandler<GetAccountProvidersQuery, GetAccountProvidersQueryResult> Handler { get; set; }
        public Account Account { get; set; }
        public Provider Provider { get; set; }
        public AccountProvider AccountProvider { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IConfigurationProvider ConfigurationProvider { get; set; }

        public GetAccountProvidersQueryHandlerTestsFixture()
        {
            Query = new GetAccountProvidersQuery(1);
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            
            ConfigurationProvider = new MapperConfiguration(c =>
            {
                c.AddProfile<AccountProviderMappings>();
                c.AddProfile<ProviderMappings>();
            });
            
            Handler = new GetAccountProvidersQueryHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ConfigurationProvider);
        }

        public Task<GetAccountProvidersQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken.None);
        }

        public GetAccountProvidersQueryHandlerTestsFixture SetAccountProviders()
        {
            Account = new AccountBuilder().WithId(Query.AccountId);
            Provider = new ProviderBuilder().WithUkprn(12345678).WithName("Foo");
            
            AccountProvider = new AccountProviderBuilder()
                .WithId(2)
                .WithAccountId(Account.Id)
                .WithProviderUkprn(Provider.Ukprn);

            Db.Accounts.Add(Account);
            Db.Providers.Add(Provider);
            Db.AccountProviders.Add(AccountProvider);
            Db.SaveChanges();

            return this;
        }
    }
}