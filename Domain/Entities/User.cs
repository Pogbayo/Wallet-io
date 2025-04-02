
using Domain.Entities;
using SpagWallet.Domain.Enums.UserEnums;

namespace SpagWallet.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public virtual BankAccount? BankAccount { get; init; }
        public virtual Wallet? Wallet { get; init; }

        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string TransferPin { get; set; }

        public DateTime DateOfBirth { get; set; }
        public UserRoleEnum Role { get; set; } = UserRoleEnum.User;
        public virtual Kyc? Kyc { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public User() { }
        public User(string firstname,string lastname,string email,string passwordhash,string transferpin,DateTime dateofbirth) 
        {

            if (string.IsNullOrWhiteSpace(firstname)) throw new ArgumentException("First name is required.");
            if (string.IsNullOrWhiteSpace(lastname)) throw new ArgumentException("Last name is required.");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.");
            if (string.IsNullOrWhiteSpace(passwordhash)) throw new ArgumentException("Password hash is required.");
            if (string.IsNullOrWhiteSpace(transferpin) || transferpin.Length != 4) throw new ArgumentException("PIN must be exactly 4 digits.");

            Id = Guid.NewGuid();
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            DateOfBirth = dateofbirth;
            PasswordHash = passwordhash;
            TransferPin = transferpin;
            Wallet = null;
        }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        public bool IsAdmin()
        {
            return UserRoleEnum.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail)) throw new ArgumentException("Email cannot be empty.");
            Email = newEmail;
        }

        public void UpdatePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash)) throw new ArgumentException("Password cannot be empty.");
            PasswordHash = newPasswordHash;
        }

        public void UpdatePin(string newPin)
        {
            if (string.IsNullOrWhiteSpace(newPin) || newPin.Length != 4)
                throw new ArgumentException("PIN must be exactly 4 digits.");
            TransferPin = newPin;
        }
    }

}
