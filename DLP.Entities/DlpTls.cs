using System.Collections.Generic;

namespace DLP.Entities
{
    public class DlpTls
    {
        public string SearchText { get; set; }                              // input Search text
        public int EmpCompany { get; set; }                                 // User.IsInGroup(@"SW1\SgyUsers") ? 2 : 1;
        public bool isSitePermissionManagment { get; set; }                 // User.IsInGroup(@"SW1\TlsDlpSitePermissionManagment")
        public List<Entities.V_DlpTlsInfo> v_DlpTlsInfo { get; set; }
        public string SearchDomain { get; set; }                            // domain part of SearchText
        public int RecipientCompany { get; set; }                           // V_DlpTlsInfo.CompanyType or 5 if domain exsist
        public bool IsDomainSearch { get; set; }                            // Search text is Domain
        public bool IsEmailSearch { get; set; }                             // Search text is Email
        public bool IsTls { get; set; }
        public bool IsDlp { get; set; }
        public bool IsEmailDlp { get; set; }
        public bool IsDomainDlp { get; set; }                               // Email exist in DLP
        public bool IsSafetPass { get; set; }
        public bool IsAllowEncryption { get; set; }
        public List<Entities.TLS> Tls { get; set; }
        public List<Entities.DLP> Dlp { get; set; }
        public int ID { get; set; }
        public string EmailAddress { get; set; }
        public string CompanyType { get; set; }
        public string ExcludeType { get; set; }

        public string GetCompany(int comp)
        {
            switch (comp)
            {
                case 1:
                case 101:
                    return "מבטח סימון";

                case 2:
                case 102:
                    return "שגיא יוגב";

                default:
                    return "כל החברות";
            }
        }
        public string GetEmpCompany(int empComp)
        {
            switch (empComp)
            {
                case 1:
                case 101:
                    return "מבטח סימון";

                case 2:
                case 102:
                    return "שגיא יוגב";

                default:
                    return "כל החברות";
            }
        }
    }
}
