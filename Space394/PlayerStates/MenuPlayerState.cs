using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space394.PlayerStates
{
    public class MenuPlayerState : PlayerState
    {
        public MenuPlayerState(Player _player)
        {
            player = _player;
            player.TeamNotSet = true;
            LogCat.updateValue("PlayerState", "Menu");
            player.PlayerShip = null;
        }

        public override void Update(float deltaTime)
        {
            
        }

        public override void Draw(PlayerCamera camera)
        {
            
        }

        public override PlayerState getNextState(Player _player)
        {
            OnExit();
            return (new LoginPlayerState(_player));
        }

        public override void ProcessInput()
        {
            
        }

        public override void OnExit()
        {

        }
    }
}
