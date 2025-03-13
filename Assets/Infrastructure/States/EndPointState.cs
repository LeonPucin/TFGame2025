using DoubleDCore.Automation.Base;
using UnityEngine;

namespace Infrastructure.States
{
    public class EndPointState : IState
    {
        public void Enter()
        {
            Application.Quit();
        }

        public void Exit()
        {
        }
    }
}