using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Campaign.Data;


namespace Campaign.IBL
{
    public interface ILoginRepository
    {
        IEnumerable<UserLogin> GetAllUsers();
        int AddUser(UserLogin objUserLogin);

        UserLogin GetUserByEmail(string Email);
        UserLogin GetUserByEmailPassword(string Email, string Password);
        string UpdateAndReturnUserPassword(UserLogin objUser);
        string RandomPassword();
        string RandomString(int size, bool lowerCase);
        int Random(int min, int max);
        string EncryptData(string szPlainText, int szEncryptionKey);
        bool CheckEmail(string email);
        bool CheckPassword(string pwd);
        bool CheckName(string name);
    }
}
