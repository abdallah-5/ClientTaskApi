
using ClientTask.Core.Interfaces.IRepositories;
using ClientTask.Core.Interfaces.IServices;
using ClientTask.Core.Services;
using ClientTask.Infrastructure.Data.Repositories;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace ClientTask.API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {



          



            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IPolygonService, PolygonService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();

            services.AddHttpContextAccessor();


           

            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));

            //services.AddCors(options => {
            //    options.AddPolicy("AllowSpecificOrigin", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            //});

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHttpClient();


            

            return services;
        }
    }
}
