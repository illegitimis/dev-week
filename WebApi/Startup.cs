namespace WebApi
{
    using DevWeek.Algo;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection; 


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
            services.AddMvcCore();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // services.AddSingleton<IRunBackgroundJob, BackgroundJobRunner>();
            services.AddSingleton<IReadQrCode, ZxingQrCodeReader>();
            services.AddSingleton<IPickStockPrice, CheatingStockPricePicker>();

            // services.AddSingleton<IProcessZip, HangfireLikeZipProcessor>();
            // services.AddSingleton<IProcessZip, ParallelCpuBoundZipProcessor>();
            // services.AddSingleton<IProcessZip, ParallelForEachZipProcessor>();
            // services.AddSingleton<IProcessZip, BlockingCollectionZipProcessor>();
            services.AddSingleton<IProcessZip, SequentialZipProcessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(new DeveloperExceptionPageOptions() { SourceCodeLineCount = 13 });
            }

            app.UseMvc();
        }
    }
}
