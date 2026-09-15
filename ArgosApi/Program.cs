using ArgosApi.Common.Extensions;
using ArgosApi.Data;
using ArgosApi.Infrastructure.Authentication;
using ArgosApi.Infrastructure.Swagger;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddSwaggerDocumentation()
    .AddDatabaseConfiguration(builder.Configuration)
    .AddAuthenticationServices(builder.Configuration)
    .AddCorsPolicy(builder.Configuration);

var app = builder.Build();

app.UseForwardedHeaders();
app.ApplyDatabaseMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
    app.UseHttpsRedirection();
}

app.UseCors(CorsExtensions.PolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run(); 