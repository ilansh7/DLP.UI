using System.ComponentModel.DataAnnotations;

namespace DLP.UI.Models
{
    public class V_DlpTlsInfo
    {
        public string SearchText { get; set; }
        public string Tls_Name { get; set; }
        public int Tls_Id { get; set; }
        public int Dlp_ID { get; set; }
        public int TlsID { get; set; }
        public int EmailType { get; set; }
        [Display(Name = "דומיין")]
        public string EmailAddress { get; set; }
        public int CompanyType_Id { get; set; }
        [Display(Name = "מוחרג עבור")]
        public string CompanyType { get; set; }
        public int ExcludeType_Id { get; set; }
        [Display(Name = "סוג נמען")]
        public string ExcludeType { get; set; }
        public bool IsAllowEncrypted { get; set; }
        public bool IsSafeT { get; set; }
    }
}