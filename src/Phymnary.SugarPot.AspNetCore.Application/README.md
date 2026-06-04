# Phymnary.SugarPot.AspNetCore.Application

Application-layer primitives for SugarPot ASP.NET Core solutions.

This project currently provides standardized application exceptions that implement `IBusinessException` (through `IApplicationException`) and carry HTTP status semantics plus optional error codes.

## Features

- Common application exception marker: `IApplicationException`
- Predefined exception types with HTTP mappings:
  - `AspBadRequestException` -> `400 BadRequest`
  - `AspUnauthorizedException` -> `401 Unauthorized`
  - `AspForbiddenEndpointException` -> `403 Forbidden`
  - `AspInvalidOperationException` -> `422 UnprocessableEntity`
  - `InternalServiceUnavailableException` -> `503 ServiceUnavailable`
- Optional domain/application error code support via `WithErrorCode(...)`

## Installation

Add a project/package reference to `Phymnary.SugarPot.AspNetCore.Application`.

## Quick Usage

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

## Exception Examples

### Unauthorized

```csharp
throw new AspUnauthorizedException("Missing access token")
    .WithErrorCode("APP_AUTH_UNAUTHORIZED");
```

### Forbidden

```csharp
throw new AspForbiddenEndpointException("You do not have permission to perform this action")
    .WithErrorCode("APP_AUTH_FORBIDDEN");
```

### Invalid operation

```csharp
throw new AspInvalidOperationException("Order cannot be completed in current state")
    .WithErrorCode("APP_ORDER_INVALID_STATE");
```

### Service unavailable

```csharp
throw new InternalServiceUnavailableException("Billing provider is temporarily unavailable")
    .WithErrorCode("APP_BILLING_UNAVAILABLE");
```

## Notes

- This project is focused on application-layer contracts and exception semantics.
- Exception-to-response formatting/serialization should be handled by API middleware or exception handlers in upper layers.

## Target Frameworks

- .NET Standard 2.0
- .NET 8
