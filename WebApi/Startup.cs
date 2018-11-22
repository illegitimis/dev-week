namespace WebApi
{
    using DevWeek.Algo;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerUI;
    using System;
    using System.IO;
    using System.Reflection;
    using WebApi.Middleware;
    using static WebApi.Extensions.Constants;


    public class Startup
    {
        const string V1 = "v1";
        const string NAME = "dev week api";
        const string DESCRIPTION = "A simple ASP.NET Core Web API";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();

            // Angular's default header name for sending the XSRF token.
            //services.AddAntiforgery(options => options.HeaderName = AntiforgeryHeaderName);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc(V1, new Info
            //    {
            //        Title = $"{NAME} {V1}",
            //        Version = V1,
            //        Contact = new Contact { Email = "andrei.ciprian@gmail.com", Name = "Andrei Ciprian Popescu", Url = "http://andreipopescu.tk/" },
            //        Description = DESCRIPTION,
            //        TermsOfService = "<TermsOfService>",
            //        License = new License { Name = "BSD 3-Clause", Url = "https://github.com/domaindrivendev/Swashbuckle/blob/master/LICENSE" }
            //    });

            //    // Set the comments path for the Swagger JSON and UI.
            //    var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);

            //    // ConfigureSwaggerGen
            //    c.DescribeAllEnumsAsStrings();
            //    c.DescribeAllParametersInCamelCase();
            //    c.DescribeStringEnumsInCamelCase();
            //    c.IgnoreObsoleteActions();
            //    c.IgnoreObsoleteProperties();
            //    c.OperationFilter<FileUploadOperation>();
            //});

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUnzip, Unzipper>();
            services.AddSingleton<IRunBackgroundJob, BackgroundJobRunner>();
            services.AddSingleton<IReadQrCode, ZxingQrCodeReader>();
            services.AddSingleton<IPickStockPrice, CheatingStockPricePicker>();             
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseSwagger();

            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("http://localhost:5000/", NAME);
            //    c.RoutePrefix = string.Empty;
            //    c.DocExpansion(DocExpansion.List);
            //    c.DisplayRequestDuration();
            //    c.DocumentTitle = DESCRIPTION;
            //});

            app.UseMvc();
        }
    }
}
