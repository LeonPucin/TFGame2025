using DoubleDCore.Periphery.Base;

namespace Infrastructure.Input.Maps
{
    public class CharacterMap : Map
    {
        private readonly InputLevers _inputControls;

        public CharacterMap(InputLevers inputControls)
        {
            _inputControls = inputControls;
        }

        protected override void Activate()
        {
            _inputControls.Character.Enable();
        }

        protected override void Deactivate()
        {
            _inputControls.Character.Disable();
        }
    }
}