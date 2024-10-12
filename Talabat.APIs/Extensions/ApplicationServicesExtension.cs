﻿using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.RepositoriesContract;
using Talabat.Repository;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApllicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddAutoMapper(typeof(MappingProfile));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                    .SelectMany(P => P.Value.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToList();

                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }

        public static WebApplication AddSwaggerMiddleWare(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
