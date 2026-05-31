using System.Text.Json.Serialization;
using AspNetCore.Boilerplate.Api.ExceptionHandler;

namespace AspNetCore.Boilerplate;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
)]
[JsonSerializable(typeof(AspErrorDto))]
internal partial class AspBoilerplateJsonSerializerContext : JsonSerializerContext;
