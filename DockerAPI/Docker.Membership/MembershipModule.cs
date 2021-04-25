using Autofac;
using Docker.Membership.Contexts;
using Docker.Membership.Seed;
using Docker.Membership.Services;
using Microsoft.AspNetCore.Http;

namespace Docker.Membership
{
    public class MembershipModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public MembershipModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            //Registering services
            builder.RegisterType<AccountService>().As<IAccountService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>()
                .SingleInstance();

            //Register seeds
            builder.RegisterType<ApplicationUserSeed>().AsSelf()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
