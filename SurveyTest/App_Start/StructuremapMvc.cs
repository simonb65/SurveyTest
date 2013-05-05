using System.Web.Mvc;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SurveyTest.App_Start.StructuremapMvc), "Start")]

namespace SurveyTest.App_Start 
{
    public static class StructuremapMvc 
    {
        public static void Start() 
        {
            var container = (IContainer) IoC.Initialize();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));
        }
    }
}