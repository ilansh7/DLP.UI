using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLP.Entities
{
    //DB : Safe-T
    public class Users
    {
        public int AID { get; set; }
        public decimal UserKey { get; set; }
        public string UserName { get; set; }
        public int FolderID { get; set; }
        public string LocalIP { get; set; }
        public string Email { get; set; }
        public bool InActive { get; set; }
        public string Password_ { get; set; }
        public int PasswordType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Tel { get; set; }
        public string Comments { get; set; }
        public int LicenseID { get; set; }
        public int AllowRemoteAccess { get; set; }
        public string FolderPath { get; set; }
        public int Privacy_ { get; set; }
        public string Credentials { get; set; }
        public bool IsAdmin { get; set; }
        public int RuleAdaptorID { get; set; }
        public int NumUserExpiredDate { get; set; }
        public int NumUserExpiredDateType { get; set; }
        public bool IsNumUserExpired { get; set; }
        public DateTime ExpiryDateRange { get; set; }
        public bool IsAllowPasswordChange { get; set; }
        public int TypeOfUser { get; set; }
        public DateTime UserDateWhenChanged { get; set; }
        public DateTime LastDateModified { get; set; }
    }
}
