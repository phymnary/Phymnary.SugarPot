namespace AspNetCore.Boilerplate.EntityFrameworkCore;

public interface ICurrentUser
{
    Guid? Id { get; set; }
}
