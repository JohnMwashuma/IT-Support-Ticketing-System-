using ITSUPPORTTICKETSYSTEM.Infrastructure;
using System.Web;
using System.Web.Mvc;

namespace ITSUPPORTTICKETSYSTEM
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new TransactionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}