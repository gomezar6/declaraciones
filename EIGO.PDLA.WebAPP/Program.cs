using EIGO.PDLA.Common.Helpers;
using EIGO.PDLA.Common.Logger;
using EIGO.PDLA.Common.Models;
using EIGO.PDLA.Common.Repositories;
using EIGO.PDLA.WebAPP.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ') ?? builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        options.Events = new OpenIdConnectEvents
        {
            OnTokenValidated = async context =>
            {
                //Calls method to process groups overage claim.
                var overageGroupClaims = await GraphHelper.GetSignedInUsersGroups(context);
                //Calls methos to process CUP
                var cup = await GraphHelper.GetUserCUP(context);
            }
        };
    })
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
    .AddDownstreamWebApi("DownstreamApi", builder.Configuration.GetSection("DownstreamApi"))
    .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
    .AddInMemoryTokenCaches();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    // Handling SameSite cookie according to https://docs.microsoft.com/en-us/aspnet/core/security/samesite
    options.HandleSameSiteCookieCompatibility();
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GroupAdmin",
        policy => policy.RequireAssertion(context =>
            GraphHelper.CheckUsersGroupMembership(context.User, builder.Configuration["Groups:GroupAdmin"])
        ));
    options.AddPolicy("GroupMember",
        policy => policy.RequireAssertion(context =>
            GraphHelper.CheckUsersGroupMembership(context.User, builder.Configuration["Groups:GroupMember"])
        )
        );
    options.AddPolicy("AnyGroup",
        policy => policy.RequireAssertion(context =>
            GraphHelper.CheckUsersGroupMembership(context.User, builder.Configuration["Groups:GroupAdmin"]) ||
            GraphHelper.CheckUsersGroupMembership(context.User, builder.Configuration["Groups:GroupMember"])
        ));
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddDbContext<DeclaracionesContext>(
    options => options.UseSqlServer(builder.Configuration["ConnectionStrings:PDLA"],
    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
    );

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.TryAddScoped<IPdlaLogger, PdlaLogger>();
// BEGIN Entities
builder.Services.TryAddScoped<IEntityRepository<Alerta>, AlertaRepository>();
builder.Services.TryAddScoped<IEntityRepository<Proceso>, ProcesoRepository>();
builder.Services.TryAddScoped<IEntityRepository<Disclaimer>, DisclaimersRepository>();
builder.Services.TryAddScoped<IEntityRepository<Formulario>, FormularioRepository>();
builder.Services.TryAddScoped<IEntityRepository<EstadoProceso>, EstadoProcesoRepository>();
builder.Services.TryAddScoped<IEntityRepository<TipoDeclaracion>, TipoDeclaracionRepository>();
builder.Services.TryAddScoped<IEntityRepository<ProcesoDisclaimerFormulario>, ProcesoDisclaimerFormularioRepository>();
builder.Services.TryAddScoped<IEntityRepository<Sincronizacion>, SincronizacionRepository>();
builder.Services.TryAddScoped<IEntityRepository<Declaracion>, DeclaracionRepository>();
builder.Services.TryAddScoped<IEntityRepository<Funcionario>, FuncionarioRepository>();
builder.Services.TryAddScoped<IEntityRepository<DeclaracionFuncionarioCargos>, DeclaracionFuncionarioCargosRepository>();
builder.Services.TryAddScoped<IEntityRepository<Parentesco>, ParentescoRepository>();
builder.Services.TryAddScoped<IEntityRepository<Pais>, PaisRepository>();
builder.Services.TryAddScoped<IEntityRepository<CatalogoCargos>, CatalogoCargosRepository>();
builder.Services.TryAddScoped<IEntityRepository<CatalogoAnios>, CatalogoAniosRepository>();
builder.Services.TryAddScoped<IEntityRepository<Ciudad>, CiudadRepository>();
builder.Services.TryAddScoped<IEntityRepository<Participacion>, ParticipacionRepository>();
builder.Services.TryAddScoped<IEntityRepository<Familiar>, FamiliarRepository>();
builder.Services.TryAddScoped<IEntityRepository<ProcesosFuncionario>, ProcesosFuncionarioRepository>();
builder.Services.TryAddScoped<IEntityRepository<FuncionarioNacionalidad>, FuncionarioNacionalidadRepository>();
builder.Services.TryAddScoped<IEntityRepository<EstadoDeclaracion>, EstadoDeclaracionRepository>();
builder.Services.TryAddScoped<IEntityRepository<Auditoria>, AuditoriaRepository>();
builder.Services.TryAddScoped<IDeclaracionesService, DeclaracionesService>();
// END

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromDays(7);

});

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddControllers().AddOData(options => options.Select().Filter().OrderBy());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");


    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/Error");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=declaraciones}/{action=Index}/{id?}"
    );
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DeclaracionesFuncionario}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllers();
app.Run();
