using BarberBoss.Domain.Entities.BaseEntities;
using BarberBoss.Domain.Enums;

namespace BarberBoss.Domain.Entities
{
    public class User : IBaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Guid UserIdentifier { get; set; }
        public string Role { get; set; } = Roles.TEAM_MEMBER;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
