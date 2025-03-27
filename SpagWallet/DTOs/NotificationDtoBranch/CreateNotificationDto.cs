
namespace SpagWallet.Application.DTOs.NotificationDtoBranch
{
    public class CreateNotificationDto
    {
        public required Guid UserId { get; set; }
        public required string Message { get; set; }
    }
}
