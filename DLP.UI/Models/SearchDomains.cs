using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace DLP.UI.Models
{
    public class SearchDomains
    {
        [Display(Name = "כתובת אימייל לחיפוש:")]
        [Required(ErrorMessage = "חובה להזין כתובת אי-מייל תקנית")]
        //[EmailAddress(ErrorMessage = "כתובת המייל שהזנת אינה תקינה")]
        //[RegularExpression(@"@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
        [RegularExpression(@"\b.*[.].*(\s|\z)", ErrorMessage = "חובה להזין דומיין או כתובת אי-מייל תקנית")]
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

        //public List<Entities.TLS> Tls { get; set; }
        //public List<Entities.DLP> Dlp { get; set; }

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