using Infrastructure.Data.Contexts;
using Infrastructure.GraphQL.Mutations;
using Infrastructure.GraphQL.ObjectTypes;
using Infrastructure.GraphQL.Query;
using Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Http.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication() // Korrekt metod för ASP.NET Core Integration
    .ConfigureServices((hostContext, services) =>
    {
        // Application Insights
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Cosmos DB context factory
        services.AddPooledDbContextFactory<DataContext>(options =>
        {
            var cosmosUri = hostContext.Configuration["COSMOS_URI"];
            var cosmosDbName = hostContext.Configuration["COSMOS_DBNAME"];
            options.UseCosmos(cosmosUri, cosmosDbName)
                   .UseLazyLoadingProxies();
        });

        // Application services
        services.AddScoped<ICourseService, CourseService>();

        // GraphQL setup
        services.AddGraphQLFunction()
                .AddQueryType<Query>()
                .AddMutationType<CourseMutation>()
                .AddType<CourseType>();

        // Ensure database is created
        var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DataContext>>();
        using var context = dbContextFactory.CreateDbContext();
        context.Database.EnsureCreated();
    })
    .ConfigureLogging(logging =>
    {
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Information);
    })
    .Build();

host.Run();
