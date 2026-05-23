namespace BarberBoss.Domain.Entities.BaseEntities
{
    public interface IBaseEntity
    {
        DateTime CreatedAt { get; set; }

        DateTime? UpdatedAt { get; set; }
    }
}
