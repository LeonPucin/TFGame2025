using TriInspector;
using UnityEditor;
using UnityEngine;

namespace DoubleDCore.Identification
{
    public abstract class IdentifyingScriptable : ScriptableObject, IIdentifying
    {
        [ReadOnly, SerializeField] private string _id;

        public string ID => _id;

        protected abstract string GetIDPrefix();

#if UNITY_EDITOR
        [Button]
        public void ResetID()
        {
            if (EditorUtility.DisplayDialog("Generate new ID",
                    "Are you sure you want to generate ID?", "Yes", "No") == false)
            {
                return;
            }

            if (string.IsNullOrEmpty(_id) == false && IDHelper.HasDuplicate(_id) == false)
            {
                if (EditorUtility.DisplayDialog("Change unique ID",
                        "Are you sure you want to regenerate ID?", "Yes", "No") == false)
                {
                    return;
                }
            }

            Undo.RecordObject(this, "Change ID");

            _id = IDHelper.GetUniqueID(GetIDPrefix());

            EditorUtility.SetDirty(this);
        }
#endif
    }
}