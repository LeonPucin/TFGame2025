using DoubleDCore.UI.Base;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DoubleDCore.UI
{
    [RequireComponent(typeof(Canvas)), RequireComponent(typeof(GraphicRaycaster)), RequireComponent(typeof(PageBinder))]
    public abstract class MonoPage : MonoBehaviour, IPage
    {
        [ReadOnly, SerializeField] private Canvas _canvas;
        [ReadOnly, SerializeField] private GraphicRaycaster _graphicRaycaster;
        [SerializeField] private bool _graphicsRaycasterEnabled = true;

        protected bool PageIsDisplayed;

        public bool IsDisplayed => PageIsDisplayed;

        public Canvas Canvas => _canvas;
        public GraphicRaycaster GraphicRaycaster => _graphicRaycaster;

        protected virtual void OnValidate()
        {
            _canvas = GetComponent<Canvas>();
            _graphicRaycaster = GetComponent<GraphicRaycaster>();
        }

        public virtual void Initialize()
        {
        }

        public virtual void Close()
        {
        }

        public virtual void Reset()
        {
        }

        protected void SetCanvasState(bool isActive)
        {
            PageIsDisplayed = isActive;

            GraphicRaycaster.enabled = _graphicsRaycasterEnabled && isActive;
            Canvas.enabled = isActive;
        }
    }
}