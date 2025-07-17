using System.Text.RegularExpressions;

namespace SimpleIAM_API.Helper
{
    public static class UserRegistration
    {
        public static (bool IsValid, string? Error) ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return (false, "Email is required.");

            if (email.Length < 6)
                return (false, "Email must be at least 6 characters.");

            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, emailRegex))
                return (false, "Email format is invalid.");

            return (true, null);
        }

        public static (bool IsValid, string? Error) ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password is required.");

            if (password.Length < 6)
                return (false, "Password must be at least 6 characters.");

            if (!char.IsUpper(password[0]))
                return (false, "Password must start with a capital letter.");

            if (!password.Any(char.IsDigit))
                return (false, "Password must contain at least one number.");

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
                return (false, "Password must contain at least one non-alphanumeric character.");

            return (true, null);
        }

        // Optional combined method
        public static (bool IsValid, List<string> Errors) ValidateCredentials(string email, string password)
        {
            var errors = new List<string>();

            var emailCheck = ValidateEmail(email);
            if (!emailCheck.IsValid) errors.Add(emailCheck.Error!);

            var passwordCheck = ValidatePassword(password);
            if (!passwordCheck.IsValid) errors.Add(passwordCheck.Error!);

            return (errors.Count == 0, errors);
        }
    }
}
