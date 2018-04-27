namespace Core.DTO
{
    using Data;
    using Data.Utils;
    using System;
    public class UserDTO
    {
        public UserDTO() { }
        public UserDTO(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            SecondName = user.SecondName;
            Login = user.Login;
            Password = user.Password.ToString();
            Space = user.Space;
            VkId = user.VkId;
            DiscUsage = user.DiscUsage;
        }
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public float? Space { get; set; }
        public string VkId { get; set; }
        public float? DiscUsage { get; set; }

        public User MapToUser(UserDTO _user)
        {
            var user = new User();
            user.Id = _user.Id ?? Guid.NewGuid();
            if (string.IsNullOrEmpty(_user.FirstName) || string.IsNullOrEmpty(_user.SecondName) || string.IsNullOrEmpty(_user.Login) || string.IsNullOrEmpty(_user.Password))
                throw new Exception("Внутреняя ошибка сервера");
            user.FirstName = _user.FirstName;
            user.SecondName = _user.SecondName;
            user.Login = _user.Login;
            user.Password = HashString.GetHashString(_user.Password);
            user.Space = 262144000;
            user.VkId = _user.VkId;
            user.DiscUsage = _user.DiscUsage ?? 0;

            return user;
        }
        public User MapToUser()
        {
            var user = new User();
            user.Id = this.Id ?? Guid.NewGuid();
            if (string.IsNullOrEmpty(this.FirstName) || string.IsNullOrEmpty(this.SecondName) || string.IsNullOrEmpty(this.Login) || string.IsNullOrEmpty(this.Password))
                throw new Exception("Внутреняя ошибка сервера");
            user.FirstName = this.FirstName;
            user.SecondName = this.SecondName;
            user.Login = this.Login;
            user.Password = HashString.GetHashString(this.Password);
            user.Space = 262144000;
            user.VkId = this.VkId;
            user.DiscUsage = this.DiscUsage ?? 0;

            return user;
        }
    }
}