using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace DLP.Data
{
    public class SafeT
    {
        #region variables
        private string TLSDLP_CONNSTR = "TlsDlpWebContext";
        private string SAFET_CONNSTR = "SafeT";
        private string MVS_FOLDERID = "2322";
        private string SGY_FOLDERID = "2349";
        #endregion variables
        #region propertirs
        public string SafeTConnStr
        {
            get { return ConfigurationManager.ConnectionStrings[SAFET_CONNSTR].ToString(); }
        }
        public string TlsDlpConnStr
        {
            get { return ConfigurationManager.ConnectionStrings[TLSDLP_CONNSTR].ToString(); }
        }
        #endregion propertirs
        #region methods
        public bool IsSafeTPass(string email, int company)
        {
            //return true;
            using (var connection = new SqlConnection(SafeTConnStr))
            {
                string query = string.Empty;
                if (company == 1)
                {
                    query = string.Concat("where Email like '", email, "' and FolderID = ", MVS_FOLDERID);
                }
                else
                {
                    query = string.Concat("where Email like '", email, "' and FolderID = ", SGY_FOLDERID);
                }

                var users = connection.GetList<Entities.Users>(query).AsList();
                if (users.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion methods
    }
}
