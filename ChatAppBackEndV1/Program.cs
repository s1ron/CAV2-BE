
using ChatAppBackEndV1.Data.Context;
using ChatAppBackEndV1.Data.Entities;
using ChatAppBackEndV1.Services.FriendService;
using ChatAppBackEndV1.Services.UserService;
using ChatAppBackEndV2.GraphQLResolver;
using ChatAppBackEndV2.Hubs;
using ChatAppBackEndV2.Services.ConversationService;
using ChatAppBackEndV2.Services.FileStorageService;
using ChatAppBackEndV2.Services.MessageService;
using ChatAppBackEndV2.Services.SystemImageService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*builder.Services.AddDbContext<ChatAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatAppV2"));
});*/

builder.Services.AddDbContext<ChatAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatAppV2"));
}, ServiceLifetime.Transient);

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ChatAppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials();
    });
});

builder.Services.AddSignalR();


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IFriendService, FriendService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<ISystemImageService, SystemImageService>();

builder.Services.AddSingleton<IDictionary<Guid, string>>(opts => new Dictionary<Guid, string>());
builder.Services.AddScoped<ChatHub, ChatHub>();

builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType(x => x.Name("Query"))
    .AddType<QueryResolver>()
    .AddMutationType(x => x.Name("Mutation"))
    .AddType<MutationResolver>();



string issuer = builder.Configuration.GetValue<string>("Tokens:Issuer");
string signingKey = builder.Configuration.GetValue<string>("Tokens:Key");
byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = issuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = System.TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
    };
}).AddGoogle(googleOptions =>
{
    IConfigurationSection googleAuthSection = builder.Configuration.GetSection("Authentication:Google");

    googleOptions.ClientId = googleAuthSection["ClientId"];
    googleOptions.ClientSecret = googleAuthSection["ClientSecret"];
    googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
});






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.MapGraphQL();

app.MapHub<ChatHub>("/chat");

app.Run();
