using DoubleDCore.Periphery.Base;

namespace Infrastructure.Input.Maps
{
    public class UIMap : Map
    {
        private readonly InputLevers _inputControls;

        public UIMap(InputLevers inputControls)
        {
            _inputControls = inputControls;
        }

        protected override void Activate()
        {
            _inputControls.UI.Enable();
        }

        protected override void Deactivate()
        {
            _inputControls.UI.Disable();
        }
    }
}