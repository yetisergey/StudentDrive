namespace Core
{
    using Data;
    using Data.Utils;
    using DTO;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Core
    {
        public UserDTO GetUserAuthorize(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login);
            if (user == null)
                throw new Exception("Пользователь не найден. Неправильно введён логин или пароль!");
            var temp = HashString.GetHashString(password);
            if (user.Password != temp)
                throw new Exception("Неправильно введён логин или пароль!");
            return new UserDTO(user);
        }

        public UserDTO GetUserAuthorizeVk(string vkId)
        {
            var user = _context.Users.FirstOrDefault(u => u.VkId == vkId);
            if (user == null)
            {
                throw new Exception("Пользователь не найден. Неправильно введён логин или пароль!");
            }
            return new UserDTO(user);

        }
        public UserDTO GetUserId(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                throw new Exception("Пользователь не найден");
            return new UserDTO(user);
        }
        public List<UserDTO> GetUsers()
        {
            var users = _context.Users.ToList().Select(u => new UserDTO(u)).ToList();
            if (users == null)
                throw new Exception("Пользователи не найдены");
            return users;
        }
        public UserDTO GetUserLogin(string login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login);
            if (user == null)
                throw new Exception("Пользователь не найден");
            return new UserDTO(user);
        }
        public UserDTO AddUser(UserDTO _user)
        {
            var pass = HashString.GetHashString(_user.Password);
            var user = _context.Users.FirstOrDefault(u => u.Login.Equals(_user.Login) && u.Password.Equals(pass));
            if (user != null)
                throw new Exception("Пользователь уже существует");
            var nUser = _user.MapToUser();
            var userStat = new Statistics(0, 0, 0, nUser.Id);
            _context.Users.Add(nUser);
            _context.Statistics.Add(userStat);
            _context.SaveChanges();
            return new UserDTO(nUser);
        }

        public void DeleteUser(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                throw new Exception("Пользователь не найден");
            var userFiles = _context.Files.Where(uf => uf.UserId == id).ToList();
            var userShares = _context.Shares.Where(us => us.OwnerId == id).ToList();
            var toUserShares = _context.Shares.Where(tus => tus.ToUserId == id).ToList();
            var userStat = _context.Statistics.FirstOrDefault(st => st.UserId == id);
            if (userShares.Count != 0)
            {
                foreach (var obj in userShares)
                {
                    _context.Shares.Remove(obj);
                }
            }
            if (toUserShares.Count != 0)
            {
                foreach (var obj in toUserShares)
                {
                    _context.Shares.Remove(obj);
                }
            }
            if (userFiles.Count != 0)
            {
                foreach (var obj in userFiles)
                {
                    _context.Files.Remove(obj);
                }
            }
            _context.Statistics.Remove(userStat);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public UserDTO ChangeUser(UserDTO _user)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == _user.Id);
            if (user == null)
                throw new Exception("Пользователь не найден");
            user.Login = _user.Login;
            user.Password = HashString.GetHashString(_user.Password);
            user.FirstName = _user.FirstName;
            user.SecondName = _user.SecondName;
            _context.SaveChanges();
            return new UserDTO(user);
        }
    }
}