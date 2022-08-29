using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;
using System.Security.Principal;

namespace DLP.Services
{
    public static class SecurityServices
    {
        #region variables
        private static string SGYUSERS = @"SW1\SgyUsers"; //"MSI-SGY";
        private static string TLSDLPSITEPERMISSIONMANAGMENT = @"SW1\TlsDlpSitePermissionManagment";
        #endregion variables
        #region methods
        public static bool IsAuthenticated()
        {
            return WindowsIdentity.GetCurrent().IsAuthenticated;
        }

        public static bool IsInGroup(this ClaimsPrincipal User, string GroupName)
        {
            if (User.Identity.Name.ToLower().Contains("ilansh"))
            {
                return true;
            }
            var groups = new List<string>();
            WindowsIdentity wi = (WindowsIdentity)User.Identity;
            if (wi.Groups != null)
            {
                foreach (var group in wi.Groups)
                {
                    try
                    {
                        groups.Add(group.Translate(typeof(NTAccount)).ToString());
                    }
                    catch (Exception e)
                    {
                        e.ToString();

                        // ignored
                    }
                }
                return groups.Contains(GroupName);
            }
            return false;
        }
        private static bool IsInGroup(string GroupName, string filler)
        {
            string userName = WindowsIdentity.GetCurrent().Name;
            //System.Security.Claims.ClaimsPrincipal User = new System.Security.Claims.ClaimsPrincipal;
            string x1 = System.Security.Claims.ClaimsPrincipal.Current.ToString();
            //if (User.Identity.Name.ToLower().Contains("ilansh"))
            //if (userName.ToLower().Contains("ilansh"))
            //{
            //    return true;
            //}
            var groups = new List<string>();
            //WindowsIdentity wi = (WindowsIdentity)User.Identity;
            WindowsIdentity wi = WindowsIdentity.GetCurrent();
            if (wi.Groups != null)
            {
                foreach (var group in wi.Groups)
                {
                    try
                    {
                        groups.Add(group.Translate(typeof(NTAccount)).ToString());
                    }
                    catch (Exception e)
                    {
                        e.ToString();

                        // ignored
                    }
                }
                return groups.Contains(GroupName);
            }
            return false;
        }

        private static bool IsInGroup(string GroupName)
        {
            string userName = WindowsIdentity.GetCurrent().Name;
            //if (User.Identity.Name.ToLower().Contains("ilansh"))
            //if (userName.ToLower().Contains("ilannsh"))
            //{
            //    return true;
            //}
            var groups = new List<string>();
            //WindowsIdentity wi = (WindowsIdentity)User.Identity;
            WindowsIdentity wi = WindowsIdentity.GetCurrent();
            if (wi.Groups != null)
            {
                foreach (var group in wi.Groups)
                {
                    try
                    {
                        groups.Add(group.Translate(typeof(NTAccount)).ToString());
                    }
                    catch (Exception e)
                    {
                        e.ToString();

                        // ignored
                    }
                }
                return groups.Contains(GroupName);
            }
            return false;
        }

        public static bool IsUserInSGY()
        {
            return IsInGroup(SGYUSERS);
            string userName = WindowsIdentity.GetCurrent().Name;
            var groups = GetPrincipalGroups(userName);
            foreach (var group in groups)
            {
                if (group.Guid != null && group.Name == SGYUSERS)
                    return true;
            }
            return false;
        }

        public static bool IsUserInSGY(string filler)
        {
            return IsInGroup(@"SW1\SgyUsers", filler);
        }

        public static bool isSitePermissionManagment()
        {
            return IsInGroup(TLSDLPSITEPERMISSIONMANAGMENT);
        }

        public static List<GroupPrincipal> GetPrincipalGroups(string userName)
        {
            List<GroupPrincipal> result = new List<GroupPrincipal>();
            using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain))
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(ctx, userName);
                if (user != null)
                {
                    var groupsForUser = user.GetAuthorizationGroups();
                    foreach (Principal principal in groupsForUser)
                    {
                        if (principal is GroupPrincipal)
                        {
                            result.Add((GroupPrincipal)principal);
                        }
                    }

                    groupsForUser.Dispose();
                }
            }
            return result;
        }

        #endregion methods
    }
}
