
namespace Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();  
        public string? Action { get; set; }  
        public Guid? PerformedBy { get; set; }  
        public DateTime PerformedAt { get; set; } = DateTime.UtcNow; 
        public string? Details { get; set; }

        public AuditLog() { }

        public AuditLog(string action, Guid performedBy, string details)
        {
            Action = action;
            PerformedBy = performedBy;
            PerformedAt = DateTime.UtcNow;
            Details = details;
        }
    }
}
