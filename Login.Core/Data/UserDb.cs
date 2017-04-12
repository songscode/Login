using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Login.Common;
using Login.Core.Entity;
using PetaPoco;

namespace Login.Core.Data
{
    public class UserDb: BaseDb
    {
        private UserDb()
        {
            
        }
        public static UserDb New()
        {
            return new UserDb();
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetByUserId(int userId)
        {
            using (var db=NewDB())
            {
                var user = db.SingleOrDefault<User>(new Sql().Where("userid=@0", userId));
                return user;
            }
        }

        public bool Validate(ref string username, string password)
        {
            if (username.IsTrimmedEmpty() || string.IsNullOrEmpty(password))
                return false;

            username = username.TrimToEmpty();

            var user = GetByUsername(username);

            if (user != null)
                return ValidateExistingUser(ref username, password, user);
            return false;
        }

        private User GetByUsername(string username)
        {
            using (var db = NewDB())
            {
                var user = db.Query<User>(new Sql().From("Users").Where("username=@0",username)).FirstOrDefault();
                return user;
            }
            
        }

        private bool ValidateExistingUser(ref string username, string password, User user)
        {
            username = user.Username;

            if (user.IsActive != 1)
            {
                if (Log.IsInfoEnabled)
                    Log.Error(String.Format("Inactive user login attempt: {0}", username));
                return false;
            }

            // prevent more than 50 invalid login attempts in 30 minutes
            var throttler = new Throttler("ValidateUser:" + username.ToLowerInvariant(), TimeSpan.FromMinutes(30), 50);
            if (!throttler.Check())
                return false;
            

            Func<bool> validatePassword = () => CalculateHash(password, user.PasswordSalt)
                .Equals(user.PasswordHash, StringComparison.OrdinalIgnoreCase);

            if (user.Source == "site" || user.Source == "sign" )
            {
                if (validatePassword())
                {
                    throttler.Reset();
                    return true;
                }

                return false;
            }

            if (user.Source != "ldap")
                throw new ArgumentOutOfRangeException("userSource");

            if (!string.IsNullOrEmpty(user.PasswordHash) &&
                user.LastDirectoryUpdate != null &&
                user.LastDirectoryUpdate.Value.AddHours(1) >= DateTime.Now)
            {
                if (validatePassword())
                {
                    throttler.Reset();
                    return true;
                }

            }

            return false;
        }
        

        /// <summary>
        /// SHA512加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string ComputeSHA512(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException();

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(s);
#if COREFX
            var sha512 = SHA512.Create();
#else
            var sha512 = System.Security.Cryptography.SHA512Managed.Create();
#endif
            buffer = sha512.ComputeHash(buffer);

            return System.Convert.ToBase64String(buffer).Substring(0, 86); // strip padding
        }
        /// <summary>
        /// 密码加盐生成加密
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string CalculateHash(string password, string salt)
        {
            return ComputeSHA512(password + salt);
        }
        /// <summary>
        /// 密码加密后，生成的密文及盐salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GenerateHash(string password, ref string salt)
        {
            salt = salt ?? Membership.GeneratePassword(5, 1);
            return CalculateHash(password, salt);
        }
    }
}
