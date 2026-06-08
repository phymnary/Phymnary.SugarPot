# Phymnary.SugarPot.AspNetCore.Application

Application-layer primitives for SugarPot ASP.NET Core solutions.

This project provides standardized application exceptions that implement `IBusinessException` (via `IApplicationException`) and carry HTTP status semantics plus optional error codes. Use these types in application services to express HTTP-aware error outcomes; mapping to HTTP responses is the responsibility of API middleware or exception handlers in upper layers.

## Features

- Marker interface: `IApplicationException`
- Predefined exception types with HTTP mappings:
  - `AspBadRequestException` -> `400 BadRequest`
  - `AspUnauthorizedException` -> `401 Unauthorized`
  - `AspForbiddenEndpointException` -> `403 Forbidden`
  - `AspInvalidOperationException` -> `422 Unprocessable Entity`
  - `InternalServiceUnavailableException` -> `503 Service Unavailable`
- Optional error code support via `WithErrorCode(...)`

## Installation

`dotnet add package Phymnary.SugarPot.AspNetCore.Application`

## Quick usage

```csharp
using Phymnary.SugarPot.AspNetCore.Exceptions;

public static class UserAppService
{
    public static void ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new AspBadRequestException("Name is required")
                .WithErrorCode("APP_VALIDATION_NAME_REQUIRED");
        }
    }
}
```

## Exception examples

Unauthorized:

```csharp
throw new AspUnauthorizedException("Missing access token")
    .WithErrorCode("APP_AUTH_UNAUTHORIZED");
```

Forbidden:

```csharp
throw new AspForbiddenEndpointException("You do not have permission to perform this action")
    .WithErrorCode("APP_AUTH_FORBIDDEN");
```

Invalid operation:

```csharp
throw new AspInvalidOperationException("Order cannot be completed in current state")
    .WithErrorCode("APP_ORDER_INVALID_STATE");
```

Service unavailable:

```csharp
throw new InternalServiceUnavailableException("Billing provider is temporarily unavailable")
    .WithErrorCode("APP_BILLING_UNAVAILABLE");
```

## Mapping to HTTP

These exception types are intended to be translated to HTTP responses by an exception handling middleware. A typical middleware maps the exception's semantic type to the corresponding HTTP status code and returns a consistent error payload with optional error code and message.

## Target frameworks

This library targets multiple TFMs used across the solution to maximize compatibility:

- `net8.0`
- `net9.0`
- `net10.0`

Check the csproj for the exact TFMs used by this project.

## Contributing

Contributions, bug reports and feature requests are welcome. Please open issues or pull requests on the repository.

## License

See the repository root for license information.
