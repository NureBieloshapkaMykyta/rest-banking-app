using Bank.Server.Api.Extensions;
using Bank.Server.Business;
using Bank.Server.Infrastructure;
using Bank.Server.Persistence;
using Bank.Server.Shared.Helpers.MappingProfiles;
using Bank.Server.Shared.Options;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddAutoMapper(options =>
{
    options.AddProfile<AccountProfile>();
    options.AddProfile<TransactionProfile>();
});

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGeneration();

builder.Services.AddBusiness();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);

builder.Services.Configure<AccountOptions>(builder.Configuration.GetSection(AccountOptions.SectionName));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
