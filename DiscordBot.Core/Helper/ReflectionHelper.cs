using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscordBot.Core.Helper
{
    public static class ReflectionHelper
    {
        public static IEnumerable<Type> GetClassesFromBaseClass<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(T)))
                .ToList();
        }
    }
}