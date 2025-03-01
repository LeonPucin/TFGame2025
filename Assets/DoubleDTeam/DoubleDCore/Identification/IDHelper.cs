using System.Collections.Generic;
using System.Linq;
using DoubleDCore.Finder;

namespace DoubleDCore.Identification
{
#if UNITY_EDITOR
    public static class IDHelper
    {
        public static bool HasDuplicate(string id)
        {
            var allObjects = GetIdentifyingScriptableObjects();
            var allID = allObjects.Select(o => o.ID);

            return allID.Count(i => i == id) > 1;
        }

        public static bool HasID(string id)
        {
            var allObjects = GetIdentifyingScriptableObjects();
            var allID = allObjects.Select(o => o.ID);

            return allID.Contains(id);
        }

        private static IEnumerable<IIdentifying> GetIdentifyingScriptableObjects()
        {
            return ScriptableObjectFinder.FindAllInstances<IIdentifying>();
        }

        public static int GetOrder(string id, int order = 0)
        {
            return HasID(id + order) ? GetOrder(id, order + 1) : order;
        }

        public static string GetUniqueID(string prefix, char separator = '/')
        {
            string result = prefix + separator;

            return result + GetOrder(result);
        }
    }
#endif
}