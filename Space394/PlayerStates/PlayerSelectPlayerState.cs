using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space394.PlayerStates
{
    public class PlayerSelectPlayerState : PlayerState
    {
        public PlayerSelectPlayerState(Player _player)
        {
            player = _player;

            player.PlayerShip = null;

            LogCat.updateValue("PlayerState", "PlayerSelect");
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
            return (new TeamSelectPlayerState(_player));
        }

        public override void ProcessInput()
        {
            
        } // End process input

        public override void OnExit()
        {
            
        }
    }
}
