using DoubleDCore.UI;
using DoubleDCore.UI.Base;

namespace Game.Source.UI.Pages
{
    public class DefaultPointerPage : MonoPage, IUIPage
    {
        public override void Initialize()
        {
            SetCanvasState(true);
        }

        public void Open()
        {
            SetCanvasState(true);
        }

        public override void Close()
        {
            SetCanvasState(false);
        }
    }
}