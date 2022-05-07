//using Microsoft.AspNetCore.SignalR;
using LAYHER.Backend.API.Helpers;
using LAYHER.Backend.Application.Comun;
using LAYHER.Backend.Application.Inspeccion;
using LAYHER.Backend.Application.ModuloAdministracionUsuarioCliente;
using LAYHER.Backend.Application.ModuloProgramacionUnidad;
using LAYHER.Backend.Application.Notificacion;
using LAYHER.Backend.Domain.Comun.Interfaces;
using LAYHER.Backend.Domain.Inspeccion.Interfaces;
using LAYHER.Backend.Domain.ModuloAdministracionUsuarioCliente.Interfaces;
using LAYHER.Backend.Domain.ModuloProgramacionUnidad.Interfaces;
using LAYHER.Backend.Domain.Notificacion.Interfaces;
using LAYHER.Backend.Infraestructure;
using LAYHER.Backend.Infraestructure.Comun;
using LAYHER.Backend.Infraestructure.ModuloInspeccionAndamios;
using LAYHER.Backend.Infraestructure.ModuloAdministracionUsuarioCliente;
using LAYHER.Backend.Infraestructure.ModuloProgramacionUnidad;
using LAYHER.Backend.Infraestructure.Notificacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.Tasks;
using LAYHER.Backend.Application.ModuloWebShop;
using LAYHER.Backend.Domain.ModuloWebShop.Interfaces;
using LAYHER.Backend.Domain.ModuloFacturacion.Interfaces;
using LAYHER.Backend.Domain.ModuloPreguia_Guia.Interfaces;
using LAYHER.Backend.Infraestructure.ModuloWebShop;
using LAYHER.Backend.Infraestructure.ModuloFacturacion;
using LAYHER.Backend.Infraestructure.ModuloPreguia_Guia;
using LAYHER.Backend.Domain.ModuloProductos.Interfaces;
using LAYHER.Backend.Application.ModuloPlataforma;
using LAYHER.Backend.Infraestructure.ModulosPlataforma;
using LAYHER.Backend.Domain.ModuloPlataforma.Interfaces;
using LAYHER.Backend.Infraestructure.Eventos;
using LAYHER.Backend.Domain.ModuloEventos.Interfaces;
using LAYHER.Backend.Application.ModuloEventos;
using Microsoft.AspNetCore.HttpOverrides;

namespace LAYHER.Backend.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Layher Suite", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });

            var appSettingsSection = Configuration.GetSection("AppSettings");

            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            //var userId = context.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                            //if (userId == null)
                            //{
                            //    // return unauthorized if user no longer exists
                            //    context.Fail("Unauthorized");
                            //}

                            //if (context.Request.Headers.ContainsKey("Authorization"))
                            //{
                            //    var tokenHandler = new JwtSecurityTokenHandler();
                            //    var token_id = context.Request.Headers["Authorization"].ToString().Split(' ')[1];
                            //    var claim = tokenHandler.ReadJwtToken(token_id).Claims.FirstOrDefault(s => s.Type.ToString().Equals("unique_name"));

                            //    // Validar que no este en lista negra
                            //    var seguridadService = context.HttpContext.RequestServices.GetRequiredService<SeguridadApp>();
                            //    bool estaEnListaNegra = seguridadService.validarTokenEnListaNegra(token_id);
                            //    if (estaEnListaNegra)
                            //    {
                            //        context.Response.StatusCode = 401;
                            //    }
                            //}
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddScoped<ICustomConnection, CustomConnection>(_ => new CustomConnection(Configuration["ConnectionStrings:LayherSuite"]));

            //Notificación
            services.AddTransient<NotificacionApp>();
            services.AddScoped<INotificacionProvider, FirebaseCloudMessageManager>();
            services.AddScoped<INotificacionRepository, NotificacionRepository>();
            services.AddScoped<IMailManager, MailManager>();

            //Usuario
            services.AddTransient<UsuarioApp>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            //Perfil
            services.AddTransient<PerfilApp>();
            services.AddScoped<IPerfilRepository, PerfilRepository>();
            //Permiso
            services.AddTransient<PermisoApp>();
            services.AddScoped<IPermisoRepository, PermisoRepository>();
            //MaestroPersona
            services.AddTransient<MaestroPersonasApp>();
            services.AddScoped<IMaestroPersonasRepository, MaestroPersonasRepository>();
            //MaestroProyecto
            services.AddTransient<MaestroProyectosApp>();
            services.AddScoped<IMaestroProyectosRepository, MaestroProyectosRepository>();
            //Zona
            services.AddTransient<ZonaApp>();
            services.AddScoped<IZonaRepository, ZonaRepository>();
            //RegionAlmacen
            services.AddTransient<AreaAlmacenApp>();
            services.AddScoped<IAreaAlmacenRepository, AreaAlmacenRepository>();
            //RegionAlmacen
            services.AddTransient<TipoUnidadApp>();
            services.AddScoped<ITipoUnidadRepository, TipoUnidadRepository>();
            //TipoImportacion
            services.AddTransient<TipoImportacionApp>();
            services.AddScoped<ITipoImportacionRepository, TipoImportacionRepository>();
            //TipoProgramacion
            services.AddTransient<TipoProgramacionApp>();
            services.AddScoped<ITipoProgramacionRepository, TipoProgramacionRepository>();
            //TipoEstado
            services.AddTransient<TipoEstadoApp>();
            services.AddScoped<ITipoEstadoRepository, TipoEstadoRepository>();
            //Metraje
            services.AddTransient<MetrajeApp>();
            services.AddScoped<IMetrajeRepository, MetrajeRepository>();
            //Especificacion
            services.AddTransient<EspecificacionApp>();
            services.AddScoped<IEspecificacionRepository, EspecificacionRepository>();
            //TipoMontacarga
            services.AddTransient<TipoMontacargaApp>();
            services.AddScoped<ITipoMontacargaRepository, TipoMontacargaRepository>();
            //TipoDocumento
            services.AddTransient<TipoDocumentoApp>();
            services.AddScoped<ITipoDocumentoRepository, TipoDocumentoRepository>();
            //Cotizacion
            services.AddTransient<CotizacionApp>();
            services.AddScoped<ICotizacionRepository, CotizacionRepository>();
            //AdministracionUsuarioCliente
            services.AddTransient<AdministracionUsuarioClienteApp>();
            //ProgramacionUnidad
            services.AddTransient<ProgramacionUnidadApp>();
            services.AddScoped<IProgramacionUnidadRepository, ProgramacionUnidadRepository>();
            //Formulario
            services.AddTransient<FormularioApp>();
            services.AddScoped<IFormularioRepository, FormularioRepository>();
            //Adjunto
            services.AddTransient<AdjuntoApp>();
            services.AddScoped<IAdjuntoRepository, AdjuntoRepository>();
            //Consideracion
            services.AddTransient<ConsideracionApp>();
            services.AddScoped<IConsideracionRepository, ConsideracionRepository>();
            //ProgramacionTiempo
            services.AddTransient<ProgramacionTiempoApp>();
            services.AddScoped<IProgramacionTiempoRepository, ProgramacionTiempoRepository>();
            // services.AddScoped<IAdministracionUsuarioClienteRepository, AdministracionUsuarioClienteRepository>();
            //Seguridad
            services.AddTransient<SeguridadApp>();
            services.AddScoped<ISeguridadRepository, SeguridadRepository>();

            //Inspeccion
            services.AddTransient<InspeccionAndamioApp>();
            services.AddScoped<IInspeccionAndamioRepository, InspeccionAndamioRepository>();
            services.AddScoped<ICheckListRepository, CheckListRepository>();
            services.AddScoped<ICumplimientoRepository, CumplimientoRepository>();
            services.AddScoped<IRegistroFotograficoRepository, RegistroFotograficoRepository>();
            services.AddScoped<IConfiguracionAppRepository, ConfiguracionAppRepository>();

            //WEBSHOP 
            services.AddTransient<WebShopApp>();
            services.AddScoped<IWebShopRepository, WebShopRepository>();
            services.AddScoped<IFacturacionRepository, FacturacionRepository>();
            services.AddScoped<IControlCantidadesPreguiaGuiaRepository, PreGuia_guiaRepository>();
            services.AddScoped<IProductoRepository, PreGuia_guiaRepository>();

            //Plataforma 
            services.AddTransient<PlataformaApp>();
            services.AddScoped<IPlataformaRepository, PlataformaRepository>();


            //Plataforma 
            services.AddTransient<EventosApp>();
            services.AddScoped<IEventosRepository, EventosRepository>();


            services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                options.RespectBrowserAcceptHeader = true;
                // options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                // options.InputFormatters.Add(new XmlDataContractSerializerInputFormatter(options));
                // options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
                // options.ReturnHttpNotAcceptable = true;
                // options.RespectBrowserAcceptHeader = true;
                // options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                // // options.OutputFormatters.RemoveType(typeof(SystemTextJsonOutputFormatter));
                // // options.InputFormatters.RemoveType(typeof(SystemTextJsonInputFormatter));
            });
            // .AddXmlSerializerFormatters()
            // .AddNewtonsoftJson(options =>
            // {
            //     options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            // });

            //services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Layher Suite V1");
                });
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
            app.UseRouting();

            //string[] sitiosSeguros = Configuration.GetSection("CORS").Get<string[]>();
            //if (sitiosSeguros != null && sitiosSeguros.Length > 0)
            //{
            //    app.UseCors(builder =>
            //    {
            //        builder.WithOrigins(sitiosSeguros)
            //            .AllowAnyHeader()
            //            .AllowAnyMethod()
            //            .AllowCredentials();
            //    });
            //}
            //else
            //{
            app.UseCors("AllowAll");
            //}
            app.UseStaticFiles();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();//.RequireAuthorization();
                //endpoints.MapHub<NotificacionesHub>("/hubs/notificaciones", options =>
                //{
                //    options.Transports =
                //        HttpTransportType.WebSockets |
                //        HttpTransportType.LongPolling;
                //});
            });
        }
    }
}
