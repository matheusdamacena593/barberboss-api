using BarberBoss.Application.AutoMapper;
using BarberBoss.Application.UseCases.Billings.Delete;
using BarberBoss.Application.UseCases.Billings.GetAll;
using BarberBoss.Application.UseCases.Billings.GetById;
using BarberBoss.Application.UseCases.Billings.Register;
using BarberBoss.Application.UseCases.Billings.Reports.Excel;
using BarberBoss.Application.UseCases.Billings.Reports.Pdf;
using BarberBoss.Application.UseCases.Billings.Update;
using BarberBoss.Application.UseCases.Login.DoLogin;
using BarberBoss.Application.UseCases.Users.ChangePassword;
using BarberBoss.Application.UseCases.Users.Delete;
using BarberBoss.Application.UseCases.Users.Profile;
using BarberBoss.Application.UseCases.Users.Register;
using BarberBoss.Application.UseCases.Users.Update;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBoss.Application
{
    public static class DependencyInjectionException
    {
        public static void AddApplication(this IServiceCollection services)
        {
            AddAutoMapper(services);
            AddUseCases(services);
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapping));
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterBillingUseCase, RegisterBillingUseCase>();
            services.AddScoped<IGetAllBillingsUseCase, GetAllBillingsUseCase>();
            services.AddScoped<IGetBillingByIdUseCase, GetBillingByIdUseCase>();
            services.AddScoped<IDeleteBillingUseCase, DeleteBillingUseCase>();
            services.AddScoped<IUpdateBillingUseCase, UpdateBillingUseCase>();
            services.AddScoped<IGenerateBillingsReportExcelUseCase, GenerateBillingsReportExcelUseCase>();
            services.AddScoped<IGenerateBillingsReportPdfUseCase, GenerateBillingsReportPdfUseCase>();
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddScoped<IDeleteUserAccountUseCase, DeleteUserAccountUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        }
    }
}
