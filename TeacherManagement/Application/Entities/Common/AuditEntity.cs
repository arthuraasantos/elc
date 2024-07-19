namespace Core.Entities.Common
{
    public abstract class AuditEntity
    {
        public Guid Id { get; set; }
        public Guid CreatedUserId { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UpdatedUserId { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? DeleteUserId { get; set; }
        public string? DeleteUser { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
