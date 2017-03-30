using ITSUPPORTTICKETSYSTEM.Models;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ITSUPPORTTICKETSYSTEM
{
    public static class Database
    {
        private const string SessionKey = "ITSUPPORTTICKETSYSTEM.Database.SessionKey";

        public static ISessionFactory _sessionFactory;
        

        public static ISession Session
        {
            get { return (ISession)HttpContext.Current.Items[SessionKey]; }
        }
        public static void Configure()
        {
            var config = new Configuration();
            config.AddAssembly(Assembly.GetCallingAssembly());
            // configure the connection string
            config.Configure();
            
            //add our mappings
            var mapper = new ModelMapper();
            mapper.AddMapping<UserMap>();
            mapper.AddMapping<RoleMap>();
            mapper.AddMapping<Tag.TagMap>();
            mapper.AddMapping<Priority.PriorityMap>();
            mapper.AddMapping<TicketMap>();
            mapper.AddMapping<TicketStatusMap>();
            mapper.AddMapping<Comments.CommentsMap>();
            mapper.AddMapping<DepartmentMap>();
         
  

            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());           
            //create session factory
            config.AddAssembly(Assembly.GetExecutingAssembly());
            _sessionFactory = config.BuildSessionFactory();

        }
        public static void OpenSession()
        {
            HttpContext.Current.Items[SessionKey] = _sessionFactory.OpenSession();
        }
        public static void CloseSession()
        {
            var session = HttpContext.Current.Items[SessionKey] as ISession;
            if (session != null)
                session.Close();

                HttpContext.Current.Items.Remove(SessionKey);
        }
    }
}