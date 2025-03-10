using DoubleDCore.UI;
using DoubleDCore.UI.Base;

namespace Game.Source.UI.Pages
{
    public class ThrowTipPage : MonoPage, IUIPage
    {
        public override void Initialize()
        {
            SetCanvasState(false);
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