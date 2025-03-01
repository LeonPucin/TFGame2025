using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DoubleDCore.Finder
{
    public static class ScriptableObjectFinder
    {
#if UNITY_EDITOR
        public static List<TFindType> FindAllInstances<TFindType>()
        {
            List<TFindType> results = new List<TFindType>();
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(TFindType));

                if (asset is TFindType findAsset)
                    results.Add(findAsset);
            }

            return results;
        }
#endif
    }
}