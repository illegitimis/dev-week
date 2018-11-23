# dev-week

endava dev week code optimization contest

## todo

- ConfigureServices swagger gen

```cs
 services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc(V1, new Info
        {
            Title = $"{NAME} {V1}",
            Version = V1,
            Contact = new Contact 
            { 
                Email = "andrei.ciprian@gmail.com",
                Name = "Andrei Ciprian Popescu",
                Url = "http://andreipopescu.tk/" 
            },
            Description = DESCRIPTION,
            TermsOfService = "<TermsOfService>",
            License = new License { Name = "BSD 3-Clause", Url = "https://github.com/domaindrivendev/Swashbuckle/blob/master/LICENSE" }
        });

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);

        // ConfigureSwaggerGen
        c.DescribeAllEnumsAsStrings();
        c.DescribeAllParametersInCamelCase();
        c.DescribeStringEnumsInCamelCase();
        c.IgnoreObsoleteActions();
        c.IgnoreObsoleteProperties();
        c.OperationFilter<FileUploadOperation>();
    });
```

- ConfigureServices XSRF

```cs
// Angular's default header name for sending the XSRF token.
services.AddAntiforgery(options => options.HeaderName = AntiforgeryHeaderName);
```

- Configure swagger

```cs
const string V1 = "v1";
const string NAME = "dev week api";
const string DESCRIPTION = "A simple ASP.NET Core Web API";

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("http://localhost:5000/", NAME);
    c.RoutePrefix = string.Empty;
    c.DocExpansion(DocExpansion.List);
    c.DisplayRequestDuration();
    c.DocumentTitle = DESCRIPTION;
});

```