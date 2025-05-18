var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



//TODO  利用 Grpc 聚合服务、聚合其他服务的功能
//先让前端云心起来再说吧、前端能运行后再重构后端。
