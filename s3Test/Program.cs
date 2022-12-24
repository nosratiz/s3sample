using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using s3Test.Helper;
using s3Test.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IFileHandler, FileHandler>();

var awsOptions = new AWSOptions()
{
    DefaultClientConfig =
    {
        ServiceURL = "http://localhost:4566",
        UseHttp = true,
    }
};

builder.Services.AddAWSService<IAmazonS3>(awsOptions);


builder.Services.Configure<s3Configuration>(builder.Configuration.GetSection("s3"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();