using API_livechat.Controllers;
using API_livechat.Models;
using API_livechat.Repositories;
using API_livechat.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API_livechat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,              //permette di effettuare un filtraggio di auth
                    ValidateAudience = true,            //permette di effettuare un filtraggio API
                    ValidateLifetime = true,            //Al superamento di una data di scadenza l'autenticazione scade
                    ValidateIssuerSigningKey = true,    //stringa di autenticazione per HMACSHA256 nel SIGNATURE JWT
                    ValidIssuer = "mjserver",
                    ValidAudience = "Users",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("supersecret_key_supersecret_key_supersecret_key")
                        )
                };
            });

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<loginContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<MessageRepository>();
            builder.Services.AddScoped<MessageService>();
            builder.Services.AddScoped<ChatRoomRepository>();
            builder.Services.AddScoped<ChatRoomService>();

            var app = builder.Build();

            app.UseCors(builder =>
            builder
            .WithOrigins("*")
            .AllowAnyMethod()
            .AllowAnyHeader());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();        //abilitazione del middleware di autenticazione
            app.UseAuthorization();

#if DEBUG
            app.UseCors(options =>
                {
                    options
                        .WithOrigins("*")
                        .AllowAnyMethod()
                        .AllowAnyHeader();     
                });
#endif
            app.MapControllers();

            app.Run();
        }
    }
}
