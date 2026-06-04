using System.Text.Json.Serialization;
using Phymnary.SugarPot.AspNetCore.Api.ExceptionHandler;

namespace Phymnary.SugarPot.AspNetCore;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
)]
[JsonSerializable(typeof(AspErrorDto))]
internal partial class ApiSerializerContext : JsonSerializerContext;
