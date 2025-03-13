using System.Collections.Generic;
using DoubleDCore.Extensions;
using DoubleDCore.TimeTools;
using UnityEngine;

namespace DoubleDCore.Debugging
{
    public class ScreenDebug : MonoBehaviour
    {
        [SerializeField] private DebugMessageText _prefab;
        [SerializeField] private Canvas _canvas;

        private static readonly Dictionary<int, DebugMessageText> Texts = new();

        private static ScreenDebug _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        public static void Log(int key, string message, Vector3 worldPoint, Color color = default,
            float duration = 1f, float scale = 1)
        {
            bool hasInstance = Texts.TryGetValue(key, out var messageInstance);
            var debugText = hasInstance ? messageInstance : CreateDebugText();

            debugText.Text.text = message.Color(color);
            debugText.RectTransform.localScale = scale * Vector3.one;
            debugText.Factor = _instance._canvas.scaleFactor;
            debugText.WorldPoint = worldPoint;

            if (key == -1)
            {
                debugText.Timer.Start(duration, onEnd: () => Destroy(debugText.gameObject));
                return;
            }

            debugText.Timer.Start(duration, onEnd: () => ClearText(key));

            if (hasInstance == false)
                Texts.Add(key, debugText);
        }

        private static void ClearText(int key)
        {
            var isSuccess = Texts.TryGetValue(key, out var text);

            if (isSuccess == false)
                return;

            Texts.Remove(key);
            Destroy(text.gameObject);
        }

        private static DebugMessageText CreateDebugText()
        {
            var init =
                Instantiate(_instance._prefab, Vector3.zero, Quaternion.identity, _instance._canvas.transform);

            init.Initialize(new Timer(TimeBindingType.RealTime));

            return init;
        }
    }
}