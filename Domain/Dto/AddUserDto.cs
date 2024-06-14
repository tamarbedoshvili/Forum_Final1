using Final.Enum;
namespace Final.Dto
{
    public class AddUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public EUserRole Role { get; set; }
    }
}
