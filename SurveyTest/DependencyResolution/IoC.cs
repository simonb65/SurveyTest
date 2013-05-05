using StructureMap;
namespace SurveyTest 
{
    public static class IoC 
    {
        public static IContainer Initialize() 
        {
            var cont = new Container(cfg => cfg.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            }));

            return cont;
        }
    }
}