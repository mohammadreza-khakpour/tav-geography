using Autofac;
using Microsoft.Extensions.Configuration;
using Geography.Services.Provinces;
using Geography.Infrastructure.Application;
using Geography.Persistence.EF;
using Geography.Persistence.EF.Provinces;

namespace Geography.RestApi.Configs
{
    class ServicesConfig : Configuration
    {
        private string _dbConnectionString;

        public override void Initialized()
        {
            _dbConnectionString = AppSettings.GetValue<string>("dbConnectionString");
        }

        public override void ConfigureServiceContainer(ContainerBuilder container)
        {
            container.RegisterAssemblyTypes(typeof(ProvinceAppService).Assembly)
                     .AssignableTo<Service>()
                     .AsImplementedInterfaces()
                     .InstancePerLifetimeScope();

            container.RegisterType<EFUnitOfWork>()
                     .As<UnitOfWork>()
                     .InstancePerLifetimeScope();

            container.RegisterType<EFDataContext>()
                     .WithParameter("connectionString", _dbConnectionString)
                     .AsSelf()
                     .InstancePerLifetimeScope();

            container.RegisterAssemblyTypes(typeof(EFProvinceRepository).Assembly)
                     .AssignableTo<Repository>()
                     .AsImplementedInterfaces()
                     .InstancePerLifetimeScope();
        }
    }
}
