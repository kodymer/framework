using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.EventBus
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EventNameAttribute : Attribute
    {

        public string Name { get; }

        public EventNameAttribute(string name)
        {
            Guard.Against.NullOrWhiteSpace(name);

            Name = name;
        }

        public static string GetOrDefault(Type @event)
        {
            var eventNameAttribute = @event.GetTypeInfo().GetCustomAttributes(typeof(EventNameAttribute), false).FirstOrDefault();
            if (!(eventNameAttribute is null))
            {
                var eventNameAttributeType = eventNameAttribute.GetType();
                var propertyInfo = eventNameAttributeType.GetTypeInfo().GetProperty(nameof(EventNameAttribute.Name), 
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

                if(!(propertyInfo is null))
                {
                    return propertyInfo.GetValue(eventNameAttribute).ToString();
                }
            }

            return @event.FullName;
        }

    }
}
