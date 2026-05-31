namespace AspNetCore.Boilerplate.EntityFrameworkCore;

public interface ICurrentTenant
{
    Guid? Id { get; set; }
}
