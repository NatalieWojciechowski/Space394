using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space394.PlayerStates
{
    public class LoginPlayerState : PlayerState
    {
        public LoginPlayerState(Player _player)
        {
            player = _player;

            player.PlayerShip = null;

            LogCat.updateValue("PlayerState", "Menu");
        }

        public override PlayerState getNextState(Player _player)
        {
            OnExit();
            return (new TeamSelectPlayerState(_player));
        }

        public override void OnExit()
        {

        }

        public override void ProcessInput()
        {
            
        }

        public override void Update(float deltaTime)
        {
            ProcessInput();
        }

        public override void Draw(PlayerCamera camera)
        {

        }
    }
}
