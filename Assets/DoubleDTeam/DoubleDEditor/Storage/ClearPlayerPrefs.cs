using UnityEditor;
using UnityEngine;

namespace DoubleDEditor.Storage
{
    public static class ClearPlayerPrefs
    {
        [MenuItem("Tools/Clear player prefs")]
        public static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}