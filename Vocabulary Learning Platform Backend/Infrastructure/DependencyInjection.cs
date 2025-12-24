using Application.Interfaces;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),

                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // First check Authorization header
                        var token = context.Request.Headers["Authorization"].FirstOrDefault();

                        // If not in Authorization header, check "Bearer" header (lowercase)
                        if (string.IsNullOrEmpty(token))
                        {
                            token = context.Request.Headers["Bearer"].FirstOrDefault();
                        }

                        // If still not found, check cookies
                        if (string.IsNullOrEmpty(token))
                        {
                            token = context.Request.Cookies["Bearer"];
                        }

                        // Clean the token if it has "Bearer " prefix
                        if (!string.IsNullOrEmpty(token))
                        {
                            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                token = token.Substring("Bearer ".Length).Trim();
                            }

                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDeckRepository, DeckRepository>();
            services.AddScoped<IWordRepository, WordRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDeckService, DeckService>();
            services.AddScoped<IWordService, WordService>();
            services.AddScoped<IReviewService, ReviewService>();

            services.AddScoped<LeaderboardService>();

            services.AddAuthorization(Options =>
            {
                Options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                Options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
                Options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                          .AllowAnyMethod()                     // Allow all HTTP methods
                          .AllowAnyHeader()                     // Allow all headers
                          .AllowCredentials();
                });
            });

            return services;
        }
    }
}
