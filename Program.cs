using Movies.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Movies.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Movies.Components.Account;
using Movies;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MoviesContext") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<MoviesContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddQuickGridEntityFrameworkAdapter();;

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddSingleton<DialogState>();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IdentityUserAccessor>();

builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

builder.Services.AddIdentityCore<AuthUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<MoviesContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<AuthUser>, IdentityNoOpEmailSender>();

var configuration = builder.Configuration;
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
});

builder.Services.AddTransient<IEmailSender, EmailSender>();
//builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.Configure<AuthMessageSenderOptions>(options =>
{
    options.SendGridKey = builder.Configuration.GetValue<string>("SendGridKey");
});

builder.Services.ConfigureApplicationCookie(o => {
    o.ExpireTimeSpan = TimeSpan.FromDays(5);
    o.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();;

app.Run();
