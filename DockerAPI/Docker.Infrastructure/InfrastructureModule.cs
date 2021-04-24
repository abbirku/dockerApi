using Autofac;
using Docker.Infrastructure.Context;
using Docker.Infrastructure.Repositories;
using Docker.Infrastructure.Seed;
using Docker.Infrastructure.Services;
using Docker.Infrastructure.UnitOfWorks;

namespace Docker.Infrastructure
{
    public class InfrastructureModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public InfrastructureModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApiContext>()
                   .WithParameter("connectionString", _connectionString)
                   .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                   .InstancePerLifetimeScope();

            //Registering repositories
            builder.RegisterType<WebCamImageRepository>().As<IWebCamImageRepository>()
                .InstancePerLifetimeScope();

            //Registering UnitOfWorks
            builder.RegisterType<ApiUnitOfWork>().As<IApiUnitOfWork>()
                .InstancePerLifetimeScope();

            //Registering services
            builder.RegisterType<WebCamImageCaptureService>().As<IWebCamImageCaptureService>()
                .InstancePerLifetimeScope();

            //Registering Adapters

            //Register seeds
            builder.RegisterType<WebCamSeed>().AsSelf()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
