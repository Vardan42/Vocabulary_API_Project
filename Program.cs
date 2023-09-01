using Microsoft.EntityFrameworkCore;
using Vocabulary_API_Project.Data;
using Vocabulary_API_Project.Mapping;
using Vocabulary_API_Project.Repository.Classes;
using Vocabulary_API_Project.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddAutoMapper(typeof(MappingConfiguration));

builder.Services.AddScoped<IWordARepository, WordARepository>();
builder.Services.AddScoped<IWordBRepository,WordBRepository>();
builder.Services.AddScoped<IWordAllRepository, WordAllRepository>();
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
