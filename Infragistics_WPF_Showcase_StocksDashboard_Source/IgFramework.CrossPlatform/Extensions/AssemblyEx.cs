using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System.Reflection
{
    public static class AssemblyEx
    {
        public static List<Type> GetTypesList(this Assembly assembly)
        {
            var types = assembly.GetTypes().ToList();
            return types;
        }
        /// <summary>
        /// Gets stream of embedded resource file in specified assembly or 
        /// </summary> 
        public static Stream GetResourceStream(this Assembly assembly, string resourceName)
        {
            if (String.IsNullOrEmpty(resourceName))
                throw new ArgumentException("resourceName");

            resourceName = resourceName.ToLower();
            var resurces = assembly.GetManifestResourceNames();
            foreach (var name in resurces)
            {
                if (name.ToLower().EndsWith(resourceName))
                    return assembly.GetManifestResourceStream(name);
            }
            return null;
        }

        //public static Stream GetResourcesDictionary(Assembly assembly, string resourceName)
        //{
        //    var assmblies = Application.
        //}
    }

    //public class EmbeddedResources
    //{
    //    public EmbeddedResources()
    //    {
    //        Dictionary = new Dictionary<string, Assembly>();
    //    }
    //    /// <summary>
    //    /// Gets or sets PropertyName
    //    /// </summary>
    //    public Dictionary<string, Assembly> Dictionary { get; set; }

    //    public void AddAssembly(Assembly assembly)
    //    {
    //        var resurces = assembly.GetManifestResourceNames();
    //        foreach (var resurce in resurces)
    //        {
    //            var key = resurce.ToLower();
    //            if (!Dictionary.ContainsKey(key))
    //            {
    //                Dictionary.Add(key, assembly);
    //            }
    //        }
    //    }
    //}
}