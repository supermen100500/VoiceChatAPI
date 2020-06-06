using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VoiceChatAPI.Application.Models
{
    public static class AssemblyLoader
    {
        public static ICollection<Assembly> Assemblies { get; private set; } = new List<Assembly>();

        public static void LoadVoiceChatAssemblies(Type entryType)
        {
            Assemblies = FindVoiceChatAPIAssemblies(Assembly.GetAssembly(entryType));
        }

        private static ICollection<Assembly> FindVoiceChatAPIAssemblies(Assembly assembly)
        {
            var loaded = new ConcurrentDictionary<Assembly, bool>();
            loaded.TryAdd(assembly, true);

            foreach (var assemblyName in assembly.GetReferencedAssemblies().Where(a => a.FullName.Contains("VoiceChatAPI")))
            {

                var refAssemblies = FindVoiceChatAPIAssemblies(Assembly.Load(assemblyName));
                foreach (var refAssembly in refAssemblies)
                {
                    loaded.TryAdd(refAssembly, true);
                }
                loaded.TryAdd(Assembly.Load(assemblyName), true);
            }

            return loaded.Keys;
        }
    }
}
