
namespace SpagWallet.Application.DTOs.NotificationDtoBranch
{
    public class GetUserNotificationsDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
