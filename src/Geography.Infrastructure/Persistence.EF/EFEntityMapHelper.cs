using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Geography.Infrastructure.Persistence.EF
{
    public static class EFEntityMapHelper
    {
        private static readonly MethodInfo ApplyEntityConfigurationMethod =
            typeof(ModelBuilder).GetMethods().First(_ =>
                _.Name.Equals("ApplyConfiguration") &&
                _.GetParameters().FirstOrDefault()?.ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

        private static readonly MethodInfo ApplyQueryConfigurationMethod =
            typeof(ModelBuilder).GetMethods().First(_ =>
                _.Name.Equals("ApplyConfiguration") &&
                _.GetParameters().FirstOrDefault()?.ParameterType.GetGenericTypeDefinition() == typeof(IQueryTypeConfiguration<>));

        public static void ApplyAssemblyConfigurations(this ModelBuilder modelBuilder, Assembly assembly)
        {
            ApplyModelBuilderConfigurations(modelBuilder, assembly);
            ApplyEntityConfigurations(modelBuilder, assembly);
            ApplyQueryConfigurations(modelBuilder, assembly);
        }
        
        public static void ApplyQueryConfigurations(ModelBuilder modelBuilder, Assembly assembly)
        {
            assembly.GetTypes()
                .Where(_ => !_.IsInterface && !_.IsAbstract)
                .Select(_ => new
                {
                    Type = _,
                    Interfaces = _.GetInterfaces().Where(_ =>
                        _.IsGenericType &&
                        _.GetGenericTypeDefinition() == typeof(IQueryTypeConfiguration<>))
                })
                .SelectMany(_ => _.Interfaces.Select(i => new { Type = _.Type, Interface = i }))
                .ForEach(_ =>
                {
                    var configurator = Activator.CreateInstance(_.Type);
                    var queryModelType = _.Interface.GetGenericArguments().First();
                    ApplyQueryConfigurationMethod
                        .MakeGenericMethod(queryModelType)
                        .Invoke(modelBuilder, new object[] { configurator });
                });
        }

        public static void ApplyEntityConfigurations(ModelBuilder modelBuilder, Assembly assembly)
        {
            assembly.GetTypes()
                .Where(_ => !_.IsInterface && !_.IsAbstract)
                .Select(_ => new
                {
                    Type = _,
                    Interfaces = _.GetInterfaces().Where(_ =>
                        _.IsGenericType &&
                        _.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                })
                .SelectMany(_ => _.Interfaces.Select(i => new { Type = _.Type, Interface = i }))
                .ForEach(_ =>
                {
                    var configurator = Activator.CreateInstance(_.Type);
                    var entityType = _.Interface.GetGenericArguments().First();
                    ApplyEntityConfigurationMethod
                        .MakeGenericMethod(entityType)
                        .Invoke(modelBuilder, new object[] { configurator });
                });
        }
        //
        public static void ApplyModelBuilderConfigurations(ModelBuilder modelBuilder, Assembly assembly)
        {
            assembly.GetTypes()
                .Where(_ => _.IsInterface == false && _.IsAbstract == false)
                .Where(typeof(IModelBuilderConfiguration).IsAssignableFrom)
                .ForEach(type =>
                {
                    var configurator = Activator.CreateInstance(type) as IModelBuilderConfiguration;
                    configurator.Configure(modelBuilder);
                });
        }
        //
    }

    public interface IModelBuilderConfiguration
    {
        void Configure(ModelBuilder modelBuilder);
    }
}
