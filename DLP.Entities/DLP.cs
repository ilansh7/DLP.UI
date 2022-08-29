using System;
using System.ComponentModel.DataAnnotations;

namespace DLP.Entities
{
    public class DLP
    {
        public int ID { get; set; }
        public int TlsID { get; set; }
        public bool EmailType { get; set; }
        public string EmailAddress { get; set; }
        public string CompanyType { get; set; }
        public int ExcludeType { get; set; }
        public bool IsAllowEncrypted { get; set; }
    }
}

