using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace latihanwebesemka.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int Role { get; set; }

        public string? PhotoPath { get; set; }
    }

    public class UserLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class FileUpload
    {
        public IFormFile file { get; set; }
    }

    public enum OptionGenderEnum
    {
        Female,
        Male
    }

    public class UserQueryModel
    {
        public OptionGenderEnum OptionGender { get; set; }

    }

    internal sealed class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                model.Enum.Clear();
                Enum
                   .GetNames(context.Type)
                   .ToList()
                   .ForEach(name => model.Enum.Add(new OpenApiString($"{name}")));
                model.Type = "int";
                model.Format = String.Empty;
            }
        }
    }
}
