using System;
using System.ComponentModel.DataAnnotations;

namespace DLP.Entities
{
    public class DlpTlsInfo
    {
        public int ID { get; set; }
        public int Tls_Id { get; set; }
        public string Tls_Name { get; set; }
        public string Table_Tls { get; set; }
        public string Dane_Option { get; set; }
        public string Certificate { get; set; }
        public int Max_Messages_Per_Connection { get; set; }
        public int Recipient_Minutes { get; set; }
        public int Max_Host_Concurrency { get; set; }
        public string Limit_Type { get; set; }
        public string Ip_Sort_Pref { get; set; }
        public string Limit_Apply { get; set; }
        public string Send_Tls_Req_Alert { get; set; }
        public int Recipient_Limit { get; set; }
        public string Bounce_Validation { get; set; }

    }
}
