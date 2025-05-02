using Arkano.Antifraud.Application;
using Arkano.Antifraud.Infrastructure;
using Arkano.Common.Models;
using Arkano.Worker;
using Confluent.Kafka;
using System.Reflection;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection(nameof(KafkaSettings)));
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig))
);

builder.Services.AddHostedService<AntifraudHostedService>();
var host = builder.Build();
host.Run();
