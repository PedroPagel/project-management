namespace Project.Management.Api.Messaging
{
    public class UserCreationMessage
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
