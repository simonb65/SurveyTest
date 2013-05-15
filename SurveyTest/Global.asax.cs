using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SurveyTest
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new[] { "SurveyTest.Controllers" }
            );
        }

        public static void InitialIoc()
        {
            var container = IoC.Initialize();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));
        }

        protected void Application_Start()
        {
            InitialIoc();

            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default
            Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var questionDefModelBinger = DependencyResolver.Current.GetService<SurveyTest.Areas.Admin.Models.QuestionDefModelBinder>();
            ModelBinders.Binders.Add(typeof(SurveyTest.Models.QuestionDef), questionDefModelBinger);

            var surveyRunModelBinder = DependencyResolver.Current.GetService<SurveyTest.Models.SurveyRunModelBinder>();
            ModelBinders.Binders.Add(typeof(SurveyTest.Models.SurveyRunModel), surveyRunModelBinder);
        }
    }
}