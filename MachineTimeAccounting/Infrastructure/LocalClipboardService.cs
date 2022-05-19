using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MachineTimeAccounting
{
    public static class LocalClipboardService
    {
        static List<object> _objects = new List<object>();

        public static void Set<T>(List<T> objects) => _objects = objects.DeepClone().Cast<object>().ToList();

        public static List<T> Get<T>() => _objects.OfType<T>().ToList().DeepClone().ToList();

        public static T DeepClone<T>(this T obj) => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
    }
}
