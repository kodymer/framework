using Castle.Core.Logging;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Vesta.Uow
{
    public class UnitOfWorkInterceptor : IInterceptor
    {
        private ILogger<UnitOfWorkInterceptor> Logger { get; set; }

        public UnitOfWorkInterceptor()
        {
            Logger = NullLogger<UnitOfWorkInterceptor>.Instance;
        }

        private UnitOfWorkAttribute GetUnitOfWorkAttribute(IInvocation invocation)
        {

            return invocation.MethodInvocationTarget.GetCustomAttributes<UnitOfWorkAttribute>(true).FirstOrDefault() ?? invocation.Method.GetCustomAttributes<UnitOfWorkAttribute>().FirstOrDefault();
        }

        public void Intercept(IInvocation invocation)
        {
            Logger.LogInformation("Intercepting {DeclaringType}  witn UnitOfWork interceptor", invocation.InvocationTarget.GetType().Name);
            Logger.LogInformation("Method {MethodName}", invocation.Method.Name);

            var unitOfWorkAttribute = GetUnitOfWorkAttribute(invocation);

            if (invocation.Method.IsPublic &&
                unitOfWorkAttribute is not null)
            {

                Logger.LogDebug("Try change unit of work options.");

                var unitOfWorkPropertyInfo = invocation.InvocationTarget
                   .GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance)
                   .FirstOrDefault(p => p.PropertyType == typeof(IUnitOfWork));

                if (unitOfWorkPropertyInfo is not null)
                {
                    var options = new UnitOfWorkOptions();
                    unitOfWorkAttribute.SetOptions(options);


                    var unitOfWork = unitOfWorkPropertyInfo.GetValue(invocation.InvocationTarget, null) as UnitOfWork;
                    if (unitOfWork is not null && unitOfWork.Options is UnitOfWorkDefaultOptions)
                    {
                        unitOfWork.Initialize(options);

                        Logger.LogDebug("New options setters: {Options}", JsonSerializer.Serialize(options));
                    } 
                    else
                    {
                        Logger.LogWarning("The unit of work already was initialized!", JsonSerializer.Serialize(options));
                        Logger.LogDebug("Current options: {Options}", JsonSerializer.Serialize(options));
                    }
                }
            }

            Logger.LogDebug("Invoking {MethodName} method .", invocation.Method.Name);

            invocation.Proceed();

            Logger.LogDebug("{MethodName} method invoked.", invocation.Method.Name);
        }
    }
}
