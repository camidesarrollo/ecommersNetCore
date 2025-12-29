namespace Ecommers.Domain.Common
{
    public abstract class BaseEntity<TId>
    {
        public required TId Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActive { get; set; } = false;

        public void SoftActive()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
