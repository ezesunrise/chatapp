using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ChatApp.Data;
using ChatApp.Hubs;
using Bot;
using ChatApp.Data.Repository;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using ChatApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IMessageRepository, MessageRepository>();
//builder.Services.AddScoped<Hub<IChatHub>, ChatHub>();

builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddBotServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

var factory = new ConnectionFactory { HostName = builder.Configuration["RabbitMQ:Host"] };
var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare("stockQuotes");

#region consumer
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    using var scope = app.Services.CreateScope();
    var chatHub = scope.ServiceProvider.GetRequiredService<IHubContext<ChatHub, IChatHub>>();
    chatHub.Clients.All.ReceiveMessage(new ChatApp.Pages.MessageModel { Content = message.Trim('\"'), TimeStamp = DateTime.Now, Owner = AppConsts.CHAT_BOT_NAME});
};
channel.BasicConsume(queue: "stockQuotes", autoAck: true, consumer: consumer);
#endregion

app.MapHub<ChatHub>("/chathub");

app.Run();

