namespace WebApi.Middleware
{
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class FileUploadOperation : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var seeksFile = operation.Parameters?.Any(p => p.Name == "FileName");

            if (seeksFile == true)
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "file",
                    In = "formData",
                    Description = "select file to upload",
                    Required = true,
                    Type = "file"
                });
                operation.Parameters.Add(new NonBodyParameter { Name = "version", In = "path", Type = "string", Description = "api version", Required = true });
                operation.Consumes.Add("multipart/form-data");
            }
        }
    }
}
