using BarberBoss.Api;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Tests.Resources;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public BillingIdentityManager Billing_Member_Team { get; private set; } = default!;
        public BillingIdentityManager Billing_Admin { get; private set; } = default!;
        public UserIdentityManager User_Team_Member {  get; private set; } = default!;
        public UserIdentityManager User_Admin {  get; private set; } = default!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<BarberBossDbContext>(config =>
                    {
                        config.UseInMemoryDatabase("InMemoryDbForTesting");
                        config.UseInternalServiceProvider(provider);
                    });

                    var scope = services.BuildServiceProvider().CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<BarberBossDbContext>();
                    var passwordEncrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                    var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();
                    
                    StartDatabase(dbContext, passwordEncrypter, tokenGenerator);

                    
                });
        }

        private void StartDatabase(
            BarberBossDbContext dbContext,
            IPasswordEncripter passwordEncrypter,
            IAccessTokenGenerator accessTokenGenerator)
        {
            var userTeamMember = AddUserTeamMember(dbContext, passwordEncrypter, accessTokenGenerator);
            //var expenseTeamMember = AddExpenses(dbContext, userTeamMember, expenseId: 1, tagId: 1);
            //Expense_Member_Team = new ExpenseIdentityManager(expenseTeamMember);

            var userAdmin = AddUserAdmin(dbContext, passwordEncrypter, accessTokenGenerator);
            //var expenseAdmin = AddExpenses(dbContext, userAdmin, expenseId: 2, tagId: 2);
            //Expense_Admin = new ExpenseIdentityManager(expenseAdmin);

            dbContext.SaveChanges();
        }

        private User AddUserTeamMember(
            BarberBossDbContext dbContext,
            IPasswordEncripter passwordEncrypter,
            IAccessTokenGenerator accessTokenGenerator)
        {
            var user = UserBuilder.Build();
            user.Id = 1;
            var password = user.Password;

            user.Password = passwordEncrypter.Encrypt(user.Password);

            dbContext.Users.Add(user);

            var token = accessTokenGenerator.Generate(user);

            User_Team_Member = new UserIdentityManager(user, password, token);

            return user;
        }

        private User AddUserAdmin(
            BarberBossDbContext dbContext,
            IPasswordEncripter passwordEncrypter,
            IAccessTokenGenerator accessTokenGenerator)
        {
            var user = UserBuilder.Build(Roles.ADMIN);
            user.Id = 2;
            var password = user.Password;

            user.Password = passwordEncrypter.Encrypt(user.Password);

            dbContext.Users.Add(user);

            var token = accessTokenGenerator.Generate(user);

            User_Admin = new UserIdentityManager(user, password, token);

            return user;
        }

        //private Billing AddBillings(BarberBossDbContext dbContext, User user, long expenseId, long tagId)
        //{
        //    var expense = ExpenseBuilder.Build(user);
        //    expense.Id = expenseId;

        //    foreach (var tag in expense.Tags)
        //    {
        //        tag.Id = tagId;
        //        tag.ExpenseId = expenseId;
        //    }

        //    dbContext.Expenses.Add(expense);

        //    return expense;
        //}
    }
}
