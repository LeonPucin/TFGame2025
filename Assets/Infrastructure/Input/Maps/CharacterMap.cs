using DoubleDCore.Periphery.Base;
using UnityEngine;

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

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        protected override void Deactivate()
        {
            _inputControls.Character.Disable();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}