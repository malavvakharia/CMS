using Campaign.Data;
using Campaign.IBL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Campaign.BL
{
    public class LoginRepository:ILoginRepository
    {
        CampaignManagementSystemEntities db;
        public LoginRepository()
        {
            db = new CampaignManagementSystemEntities();
        }
        public int AddUser(UserLogin objUserModel)
        {
            objUserModel.Key = Random(0000, 100000);
            objUserModel.Password = EncryptData(objUserModel.Password, (objUserModel.Key + 100) * 4 / (objUserModel.Key - 90));
            db.UserLogins.Add(objUserModel);
            return db.SaveChanges();
        }

        public IEnumerable<UserLogin> GetAllUsers()
        {
            var objUserData = db.UserLogins;
            return objUserData;
        }

        public UserLogin GetUserByEmail(string Email)
        {
            var objUserData = db.UserLogins.Where(a => a.Email.Equals(Email)).FirstOrDefault();
          
            return objUserData;
        }
        public UserLogin GetUserByEmailPassword(string Email, string Password)
        {
            var usr = GetUserByEmail(Email);
            int Key;
            if (usr == null)
            {
               Key  = 123;
            }
             Key = ((usr.Key + 100) * 4 / (usr.Key - 90));
            string encPwd = EncryptData(Password, Key);
            var objUserData = db.UserLogins.Where(a => a.Email.Equals(Email) && a.Password.Equals(encPwd)).FirstOrDefault();

            return objUserData;
        }
        public string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(Random(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public int Random(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public string EncryptData(string szPlainText, int szEncryptionKey)
        {
            StringBuilder szInputStringBuild = new StringBuilder(szPlainText);
            StringBuilder szOutStringBuild = new StringBuilder(szPlainText.Length);
            char Textch;
            for (int iCount = 0; iCount < szPlainText.Length; iCount++)
            {
                Textch = szInputStringBuild[iCount];
                Textch = (char)(Textch ^ szEncryptionKey);
                szOutStringBuild.Append(Textch);
            }
            return szOutStringBuild.ToString();
        }
        public bool CheckEmail(string email)
        {
            string emailRegex = @"(^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$)";

            Regex re = new Regex(emailRegex);


            if (re.IsMatch(email))
                return (true);
            else
                return (false);
        }
        public bool CheckPassword(string pwd)
        {
            string pwdRegex = @"(^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}$)";

            Regex re = new Regex(pwdRegex);


            if (re.IsMatch(pwd))
                return (true);
            else
                return (false);
        }
        public bool CheckName(string name)
        {
            string nameRegex = @"(^^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$)";

            Regex re = new Regex(nameRegex);


            if (re.IsMatch(name))
                return (true);
            else
                return (false);
        }

        public string UpdateAndReturnUserPassword(UserLogin objUser)
        {
            string pwd = RandomPassword();

            objUser.Password = EncryptData(pwd, (objUser.Key + 100) * 4 / (objUser.Key - 90));
            db.Entry(objUser).State = EntityState.Modified;
            db.SaveChanges();
            return pwd;
        }

    }
}
