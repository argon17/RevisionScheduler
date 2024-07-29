using DotNetEnv;
using Microsoft.OpenApi.Models;
using RevisionScheduler.Core.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo { Title = "PizzaStore API", Description = "Making the Pizzas you love", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI(c =>
   {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "RevisionScheduler API V1");
   });
}

TopicSetReader topicSetReader = new ();
TopicSetWriter topicSetWriter = new ();

Env.Load();
Env.TraversePath().Load();

string? dbPath = Environment.GetEnvironmentVariable("DBPATH")
                ?? throw new InvalidDataException("Coulnd't retrieve the DbPath from Environment Variable");

var revisionScheduler = new RevisionScheduler.Core.RevisionScheduler(topicSetReader, topicSetWriter, dbPath, 60);
app.MapGet("/", () => "Hello World!");
app.MapGet("/topics", () => revisionScheduler.GetTopics());
app.MapGet("/topics/{id}", (int id) => revisionScheduler.GetTopics().SingleOrDefault(topic => topic.Id == id));
app.MapPost("/topics", (Topic topic) => revisionScheduler.AddTopic(topic));
// app.MapPut("/topics", (topic topic) => /* Update the data store using the `topic` instance */);
app.MapDelete("/topics/{id}", (int id) => revisionScheduler.DeleteTopic(id));
app.Run();
