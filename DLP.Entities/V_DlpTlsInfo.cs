using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DLP.Entities
{
    public class V_DlpTlsInfo

    {
        //public int ID { get; set; }
        public string Tls_Name { get; set; }
        public int Tls_Id { get; set; }
        public int Dlp_ID { get; set; }
        public int TlsID { get; set; }
        public bool EmailType { get; set; }
        [Display(Name = "דומיין")]
        public string EmailAddress { get; set; }
        public int CompanyType_Id { get; set; }
        [Display(Name = "מוחרג עבור")]
        public string CompanyType { get; set; }
        public int ExcludeType_Id { get; set; }
        [Display(Name = "סוג נמען")]
        public string ExcludeType { get; set; }
        public bool IsAllowEncrypted { get; set; }
    }
}
