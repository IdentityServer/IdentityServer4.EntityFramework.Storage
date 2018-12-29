// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4.EntityFramework.Storage
{
    /// <summary>
    /// Extension methods to add EF database support to IdentityServer.
    /// </summary>
    public static class IdentityServerEntityFrameworkBuilderExtensions
    {
        /// <summary>
        /// Add Configuration DbContext to the DI system.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddConfigurationDbContext(this IServiceCollection services,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
        {
            return services.AddConfigurationDbContext<ConfigurationDbContext>(storeOptionsAction);
        }

        /// <summary>
        /// Add Configuration DbContextPool to the DI system.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddConfigurationDbContextPool(this IServiceCollection services,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
        {
            return services.AddConfigurationDbContextPool<ConfigurationDbContext>(storeOptionsAction);
        }

        /// <summary>
        /// Add Configuration DbContext to the DI system.
        /// </summary>
        /// <typeparam name="TContext">The IConfigurationDbContext to use.</typeparam>
        /// <param name="services"></param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddConfigurationDbContext<TContext>(this IServiceCollection services,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IConfigurationDbContext
        {
            services.AddConfigurationInfrastructure<TContext>(storeOptionsAction, out var storeOptions);

            if (storeOptions.ResolveDbContextOptions != null)
            {
                services.AddDbContext<TContext>(storeOptions.ResolveDbContextOptions);
            }
            else
            {
                services.AddDbContext<TContext>(dbCtxBuilder =>
                {
                    storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder);
                });
            }

            return services;
        }

        /// <summary>
        /// Add Configuration DbContextPool to the DI system.
        /// </summary>
        /// <typeparam name="TContext">The IConfigurationDbContext to use.</typeparam>
        /// <param name="services"></param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddConfigurationDbContextPool<TContext>(this IServiceCollection services,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IConfigurationDbContext
        {
            services.AddConfigurationInfrastructure<TContext>(storeOptionsAction, out var storeOptions);

            if (storeOptions.ResolveDbContextOptions != null)
            {
                services.AddDbContextPool<TContext>(storeOptions.ResolveDbContextOptions);
            }
            else
            {
                services.AddDbContextPool<TContext>(dbCtxBuilder =>
                {
                    storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder);
                });
            }

            return services;
        }

        /// <summary>
        /// Adds Configuration to the DI system, but does not register DbContext.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="storeOptionsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddConfigurationInfrastructure<TContext>(this IServiceCollection services,
            Action<ConfigurationStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IConfigurationDbContext
        {
            return services.AddConfigurationInfrastructure<TContext>(storeOptionsAction, out _);
        }

        /// <summary>
        /// Adds Configuration Store to the DI system, but does not register DbContext.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="storeOptionsAction"></param>
        /// <param name="storeOptions"></param>
        /// <returns></returns>
        private static IServiceCollection AddConfigurationInfrastructure<TContext>(this IServiceCollection services,
            Action<ConfigurationStoreOptions> storeOptionsAction, out ConfigurationStoreOptions storeOptions)
            where TContext : DbContext, IConfigurationDbContext
        {
            storeOptions = new ConfigurationStoreOptions();
            services.AddSingleton(storeOptions);
            storeOptionsAction?.Invoke(storeOptions);
            services.AddScoped<IConfigurationDbContext, TContext>();

            return services;
        }

        /// <summary>
        /// Adds operational DbContext to the DI system.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddOperationalDbContext(this IServiceCollection services,
            Action<OperationalStoreOptions> storeOptionsAction = null)
        {
            return services.AddOperationalDbContext<PersistedGrantDbContext>(storeOptionsAction);
        }

        /// <summary>
        /// Adds operational DbContextPool to the DI system.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddOperationalDbContextPool(this IServiceCollection services,
            Action<OperationalStoreOptions> storeOptionsAction = null)
        {
            return services.AddOperationalDbContextPool<PersistedGrantDbContext>(storeOptionsAction);
        }

        /// <summary>
        /// Adds operational DbContext to the DI system.
        /// </summary>
        /// <typeparam name="TContext">The IPersistedGrantDbContext to use.</typeparam>
        /// <param name="services"></param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddOperationalDbContext<TContext>(this IServiceCollection services,
            Action<OperationalStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IPersistedGrantDbContext
        {
            services.AddOperationalInfrastructure<TContext>(storeOptionsAction, out var storeOptions);

            if (storeOptions.ResolveDbContextOptions != null)
            {
                services.AddDbContext<TContext>(storeOptions.ResolveDbContextOptions);
            }
            else
            {
                services.AddDbContext<TContext>(dbCtxBuilder =>
                {
                    storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder);
                });
            }

            return services;
        }

        /// <summary>
        /// Adds operational DbContextPool to the DI system.
        /// </summary>
        /// <typeparam name="TContext">The IPersistedGrantDbContext to use.</typeparam>
        /// <param name="services"></param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddOperationalDbContextPool<TContext>(this IServiceCollection services,
            Action<OperationalStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IPersistedGrantDbContext
        {
            services.AddOperationalInfrastructure<TContext>(storeOptionsAction, out var storeOptions);

            if (storeOptions.ResolveDbContextOptions != null)
            {
                services.AddDbContextPool<TContext>(storeOptions.ResolveDbContextOptions);
            }
            else
            {
                services.AddDbContextPool<TContext>(dbCtxBuilder =>
                {
                    storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder);
                });
            }

            return services;
        }

        /// <summary>
        /// Adds Operational Store to the DI system, but does not register DbContext.
        /// </summary>
        /// <typeparam name="TContext">The IPersistedGrantDbContext to use.</typeparam>
        /// <param name="services"></param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddOperationalInfrastructure<TContext>(this IServiceCollection services,
            Action<OperationalStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IPersistedGrantDbContext
        {
            return services.AddOperationalInfrastructure<TContext>(storeOptionsAction, out _);
        }

        /// <summary>
        /// Adds Configuration Store to the DI system, but does not register DbContext.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="storeOptionsAction"></param>
        /// <param name="storeOptions"></param>
        /// <returns></returns>
        private static IServiceCollection AddOperationalInfrastructure<TContext>(this IServiceCollection services,
            Action<OperationalStoreOptions> storeOptionsAction, out OperationalStoreOptions storeOptions)
            where TContext : DbContext, IPersistedGrantDbContext
        {
            storeOptions = new OperationalStoreOptions();
            services.AddSingleton(storeOptions);
            storeOptionsAction?.Invoke(storeOptions);
            services.AddScoped<IPersistedGrantDbContext, TContext>();
            services.AddSingleton<TokenCleanup>();

            return services;
        }

        /// <summary>
        /// Adds an implementation of the IOperationalStoreNotification to the DI system.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddOperationalStoreNotification<T>(this IServiceCollection services)
           where T : class, IOperationalStoreNotification
        {
            services.AddTransient<IOperationalStoreNotification, T>();
            return services;
        }
    }
}
