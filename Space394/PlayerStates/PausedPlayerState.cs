using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space394.PlayerStates
{
    public class PausedPlayerState : PlayerState
    {
        private AutoTexture2D pauseBackgroundEsx;
        private AutoTexture2D optionsBackgroundEsx;
        private AutoTexture2D destructBackgroundEsx;
        private AutoTexture2D leaveBackgroundEsx;
        private AutoTexture2D pauseBackgroundHalk;
        private AutoTexture2D optionsBackgroundHalk;
        private AutoTexture2D destructBackgroundHalk;
        private AutoTexture2D leaveBackgroundHalk;
        private AutoTexture2D resumeEsx;
        private AutoTexture2D resumeHalk;
        private AutoTexture2D optionsEsx;
        private AutoTexture2D optionsHalk;
        private AutoTexture2D voteEsx;
        private AutoTexture2D voteHalk;
        private AutoTexture2D destructEsx;
        private AutoTexture2D destructHalk;
        private AutoTexture2D leaveEsx;
        private AutoTexture2D leaveHalk;
        private AutoTexture2D yesEsx;
        private AutoTexture2D noEsx;  
        private AutoTexture2D yesHalk;
        private AutoTexture2D noHalk;
        private AutoTexture2D resumeEsxLit;
        private AutoTexture2D resumeHalkLit;
        private AutoTexture2D optionsEsxLit;
        private AutoTexture2D optionsHalkLit;
        private AutoTexture2D voteEsxLit;
        private AutoTexture2D voteHalkLit;
        private AutoTexture2D destructEsxLit;
        private AutoTexture2D destructHalkLit;
        private AutoTexture2D leaveEsxLit;
        private AutoTexture2D leaveHalkLit;
        private AutoTexture2D yesEsxLit;
        private AutoTexture2D noEsxLit;
        private AutoTexture2D yesHalkLit;
        private AutoTexture2D noHalkLit;
        private AutoTexture2D settingsEsx;
        private AutoTexture2D settingsEsxLit;
        private AutoTexture2D settingsHalk;
        private AutoTexture2D settingsHalkLit;
        private AutoTexture2D controlsEsx;
        private AutoTexture2D controlsEsxLit;
        private AutoTexture2D controlsHalk;
        private AutoTexture2D controlsHalkLit;
        private AutoTexture2D profileEsx;
        private AutoTexture2D profileEsxLit;
        private AutoTexture2D profileHalk;
        private AutoTexture2D profileHalkLit;

        private AutoTexture2D controlsPC;
        private AutoTexture2D controlsXBox;

        public enum menuState
        {
            main,
            options,
            destruct,
            leave
        };
        private menuState currentMenuState;

        public enum mainState
        {
            resume,
            options,
            //vote,
            destruct,
            leave
        };
        private mainState currentMainState;

        public enum yesNoState
        {
            yes,
            no
        };
        private yesNoState currentYesNoState;

        public enum optionsState
        {
            //settings,
            controls,
            //profile
        };
        private optionsState currentOptionsState;

        private bool drawingControls;
        private bool drawingOtherControls;

        public PausedPlayerState(Player _player)
        {
            player = _player;

            pauseBackgroundEsx = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_esx"), new Vector2(0, 0));
            pauseBackgroundHalk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_halk"), new Vector2(0, 0));
            optionsBackgroundEsx = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/options_esx"), new Vector2(0, 0));
            optionsBackgroundHalk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/options_halk"), new Vector2(0, 0));
            destructBackgroundEsx = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/destruct_esx"), new Vector2(0, 0));
            destructBackgroundHalk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/destruct_halk"), new Vector2(0, 0));
            leaveBackgroundEsx = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/leave_esx"), new Vector2(0, 0));
            leaveBackgroundHalk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/leave_halk"), new Vector2(0, 0));

            resumeEsx = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_esx_resume"), new Vector2(347, 165));
            resumeHalk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_halk_resume"), new Vector2(345, 168));
            optionsEsx = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_esx_options"), new Vector2(347, 202));
            optionsHalk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_halk_options"), new Vector2(347, 204));
            voteEsx = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_esx_vote"), new Vector2(338, 238));
            voteHalk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_halk_vote"), new Vector2(336, 240));
            destructEsx = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_esx_destruct"), new Vector2(330, 274));
            destructHalk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_halk_destruct"), new Vector2(327, 277));
            leaveEsx = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_esx_leave"), new Vector2(332, 310));
            leaveHalk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_halk_leave"), new Vector2(330, 312));    
            yesEsx = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/destruct_esx_yes"), new Vector2(361, 246));
            yesHalk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/destruct_halk_yes"), new Vector2(358, 247));
            noEsx = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/destruct_esx_no"), new Vector2(364, 282));
            noHalk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/destruct_halk_no"), new Vector2(363, 284));
            settingsEsx = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/options_esx_settings"), new Vector2(345, 198));
            settingsHalk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/options_halk_settings"), new Vector2(341, 201));
            controlsEsx = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/options_esx_controls"), new Vector2(343, 235));
            controlsHalk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/options_halk_controls"), new Vector2(340, 237));
            profileEsx = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/options_esx_profile"), new Vector2(349, 271));
            profileHalk = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/options_halk_profile"), new Vector2(347, 273));

            resumeEsxLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_esx_resume_lit"), new Vector2(347, 165));
            resumeHalkLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_halk_resume_lit"), new Vector2(345, 168));
            optionsEsxLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_esx_options_lit"), new Vector2(347, 202));
            optionsHalkLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_halk_options_lit"), new Vector2(347, 204));
            voteEsxLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_esx_vote_lit"), new Vector2(338, 238));
            voteHalkLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_halk_vote_lit"), new Vector2(336, 240));
            destructEsxLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_esx_destruct_lit"), new Vector2(330, 274));
            destructHalkLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_halk_destruct_lit"), new Vector2(327, 277));
            leaveEsxLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_esx_leave_lit"), new Vector2(332, 310));
            leaveHalkLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_halk_leave_lit"), new Vector2(330, 312));
            yesEsxLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/destruct_esx_yes_lit"), new Vector2(361, 246));
            yesHalkLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/destruct_halk_yes_lit"), new Vector2(358, 247));
            noEsxLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/destruct_esx_no_lit"), new Vector2(364, 282));
            noHalkLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/destruct_halk_no_lit"), new Vector2(363, 284));
            settingsEsxLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/options_esx_settings_lit"), new Vector2(345, 198));
            settingsHalkLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/options_halk_settings_lit"), new Vector2(341, 201));
            controlsEsxLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/options_esx_controls_lit"), new Vector2(343, 235));
            controlsHalkLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/options_halk_controls_lit"), new Vector2(340, 237));
            profileEsxLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/options_esx_profile_lit"), new Vector2(349, 271));
            profileHalkLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/options_halk_profile_lit"), new Vector2(347, 273));

            controlsPC = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/controls_PC"), new Vector2(0, 0));
            controlsXBox = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/controls_xbox"), new Vector2(0, 0));

            currentMenuState = menuState.main;
            currentMainState = mainState.resume;
            currentYesNoState = yesNoState.no;
            currentOptionsState = optionsState.controls;
            drawingControls = false;
            drawingOtherControls = false;
        }

        public override void Update(float deltaTime)
        {

        }

        public override void Draw(PlayerCamera camera)
        {
            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;

            spriteBatch.Begin();
            switch (player.CurrentTeam)
            {
                case SceneObjects.Ship.Team.Esxolus:
                    switch (currentMenuState)
                    {
                        case menuState.main:
                            pauseBackgroundEsx.DrawAlreadyBegunMaintainRatio(camera);
                            switch (currentMainState)
                            {
                                case mainState.resume:
                                    resumeEsxLit.DrawAlreadyBegunMaintainRatio(camera);
                                    optionsEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    voteEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    destructEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    leaveEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                                case mainState.options:
                                    resumeEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    optionsEsxLit.DrawAlreadyBegunMaintainRatio(camera);
                                    voteEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    destructEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    leaveEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                                /*case mainState.vote:
                                    resumeEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    optionsEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    voteEsxLit.DrawAlreadyBegunMaintainRatio(camera);
                                    destructEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    leaveEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    break;*/
                                case mainState.destruct:
                                    resumeEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    optionsEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    voteEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    destructEsxLit.DrawAlreadyBegunMaintainRatio(camera);
                                    leaveEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                                case mainState.leave:
                                    resumeEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    optionsEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    voteEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    destructEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    leaveEsxLit.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                            }
                            break;
                        case menuState.options:
                            optionsBackgroundEsx.DrawAlreadyBegunMaintainRatio(camera);
                            switch (currentOptionsState)
                            {
                                case optionsState.controls:
                                    settingsEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    controlsEsxLit.DrawAlreadyBegunMaintainRatio(camera);
                                    profileEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                            }
                            if (drawingControls)
                            {
                                switch (player.PlayerNumber)
                                {
                                    case Player.ControllerIndex.Keyboard:
                                        if (!drawingOtherControls)
                                        {
                                            controlsPC.DrawAlreadyBegunMaintainRatio(camera);
                                        }
                                        else
                                        {
                                            controlsXBox.DrawAlreadyBegunMaintainRatio(camera);
                                        }
                                        break;
                                    default:
                                        if (!drawingOtherControls)
                                        {
                                            controlsXBox.DrawAlreadyBegunMaintainRatio(camera);
                                        }
                                        else
                                        {
                                            controlsPC.DrawAlreadyBegunMaintainRatio(camera);
                                        }
                                        break;
                                }
                            }
                            else { }
                            break;
                        case menuState.destruct:
                            destructBackgroundEsx.DrawAlreadyBegunMaintainRatio(camera);
                            switch (currentYesNoState)
                            {
                                case yesNoState.yes:
                                    yesEsxLit.DrawAlreadyBegunMaintainRatio(camera);
                                    noEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                                case yesNoState.no:
                                    yesEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    noEsxLit.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                            }
                            break;
                        case menuState.leave:
                            leaveBackgroundEsx.DrawAlreadyBegunMaintainRatio(camera);
                            switch (currentYesNoState)
                            {
                                case yesNoState.yes:
                                    yesEsxLit.DrawAlreadyBegunMaintainRatio(camera);
                                    noEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                                case yesNoState.no:
                                    yesEsx.DrawAlreadyBegunMaintainRatio(camera);
                                    noEsxLit.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                            }
                            break;
                    }
                    break;

                case SceneObjects.Ship.Team.Halk:
                    switch (currentMenuState)
                    {
                        case menuState.main:
                            pauseBackgroundHalk.DrawAlreadyBegunMaintainRatio(camera);
                            switch (currentMainState)
                            {
                                case mainState.resume:
                                    resumeHalkLit.DrawAlreadyBegunMaintainRatio(camera);
                                    optionsHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    voteHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    destructHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    leaveHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                                case mainState.options:
                                    resumeHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    optionsHalkLit.DrawAlreadyBegunMaintainRatio(camera);
                                    voteHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    destructHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    leaveHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                                /*case mainState.vote:
                                    resumeHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    optionsHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    voteHalkLit.DrawAlreadyBegunMaintainRatio(camera);
                                    destructHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    leaveHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    break;*/
                                case mainState.destruct:
                                    resumeHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    optionsHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    voteHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    destructHalkLit.DrawAlreadyBegunMaintainRatio(camera);
                                    leaveHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                                case mainState.leave:
                                    resumeHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    optionsHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    voteHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    destructHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    leaveHalkLit.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                            }
                            break;
                        case menuState.options:
                            optionsBackgroundHalk.DrawAlreadyBegunMaintainRatio(camera);
                            switch (currentOptionsState)
                            {
                                case optionsState.controls:
                                    settingsHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    controlsHalkLit.DrawAlreadyBegunMaintainRatio(camera);
                                    profileHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                            }
                            if (drawingControls)
                            {
                                switch (player.PlayerNumber)
                                {
                                    case Player.ControllerIndex.Keyboard:
                                        if (!drawingOtherControls)
                                        {
                                            controlsPC.DrawAlreadyBegunMaintainRatio(camera);
                                        }
                                        else
                                        {
                                            controlsXBox.DrawAlreadyBegunMaintainRatio(camera);
                                        }
                                        break;
                                    default:
                                        if (!drawingOtherControls)
                                        {
                                            controlsXBox.DrawAlreadyBegunMaintainRatio(camera);
                                        }
                                        else
                                        {
                                            controlsPC.DrawAlreadyBegunMaintainRatio(camera);
                                        }
                                        break;
                                }
                            }
                            else { }
                            break;
                        case menuState.destruct:
                            destructBackgroundHalk.DrawAlreadyBegunMaintainRatio(camera);
                            switch (currentYesNoState)
                            {
                                case yesNoState.yes:
                                    yesHalkLit.DrawAlreadyBegunMaintainRatio(camera);
                                    noHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                                case yesNoState.no:
                                    yesHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    noHalkLit.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                            }
                            break;
                        case menuState.leave:
                            leaveBackgroundHalk.DrawAlreadyBegunMaintainRatio(camera);
                            switch (currentYesNoState)
                            {
                                case yesNoState.yes:
                                    yesHalkLit.DrawAlreadyBegunMaintainRatio(camera);
                                    noHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                                case yesNoState.no:
                                    yesHalk.DrawAlreadyBegunMaintainRatio(camera);
                                    noHalkLit.DrawAlreadyBegunMaintainRatio(camera);
                                    break;
                            }
                            break;
                    }
                    break;
            }
            spriteBatch.End();

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        public override PlayerState getNextState(Player _player)
        {
            OnExit();
            return (this);
        }

        public override void ProcessInput()
        {
            switch (currentMenuState)
            {
                case menuState.main:
                    if (InputManager.isCombinedDownCombinedStickPressed(player.PlayerNumber))
                    {
                        switch (currentMainState)
                        {
                            case mainState.resume:
                                currentMainState = mainState.options; // currentMainState = mainState.options;
                                break;
                            case mainState.options:
                                currentMainState = mainState.destruct;
                                break;
                            /*case mainState.vote:
                                currentMainState = mainState.destruct;
                                break;*/
                            case mainState.destruct:
                                currentMainState = mainState.leave;
                                break;
                            case mainState.leave:
                                currentMainState = mainState.resume;
                                break;
                        }
                    }
                    else if (InputManager.isCombinedUpCombinedStickPressed(player.PlayerNumber))
                    {
                        switch (currentMainState)
                        {
                            case mainState.resume:
                                currentMainState = mainState.leave;
                                break;
                            case mainState.options:
                                currentMainState = mainState.resume;
                                break;
                            /*case mainState.vote:
                                currentMainState = mainState.vote;
                                break;*/
                            case mainState.destruct:
                                currentMainState = mainState.options; // currentMainState = mainState.vote;
                                break;
                            case mainState.leave:
                                currentMainState = mainState.destruct;
                                break;
                        }
                    }

                    if (InputManager.isCombinedConfirmPressed(player.PlayerNumber))
                    {
                        switch (currentMainState)
                        {
                            case mainState.resume:
                                currentMenuState = menuState.main;
                                currentMainState = mainState.resume;
                                player.unpause();
                                break;
                            case mainState.options:
                                currentMenuState = menuState.options;
                                currentOptionsState = optionsState.controls;
                                break;
                            /*case mainState.vote:
                                currentMainState = mainState.vote;
                                break;*/
                            case mainState.destruct:
                                currentMenuState = menuState.destruct;
                                currentYesNoState = yesNoState.no;
                                break;
                            case mainState.leave:
                                currentMenuState = menuState.leave;
                                currentYesNoState = yesNoState.no;
                                break;
                        }
                    }
                    else if (InputManager.isCombinedUnconfirmPressed(player.PlayerNumber))
                    {
                        currentMenuState = menuState.main;
                        currentMainState = mainState.resume;
                        player.unpause();
                    }
                    else { }
                    break;
                case menuState.options:
                    if (InputManager.isCombinedDownCombinedStickPressed(player.PlayerNumber))
                    {
                        switch (currentOptionsState)
                        {
                            case optionsState.controls:
                                currentOptionsState = optionsState.controls;
                                break;
                        }
                    }
                    else if (InputManager.isCombinedUpCombinedStickPressed(player.PlayerNumber))
                    {
                        switch (currentOptionsState)
                        {
                            case optionsState.controls:
                                currentOptionsState = optionsState.controls;
                                break;
                        }
                    }

                    if (InputManager.isCombinedConfirmPressed(player.PlayerNumber))
                    {
                        switch (currentOptionsState)
                        {
                            case optionsState.controls:
                                if (!drawingControls)
                                {
                                    drawingControls = true;
                                }
                                else
                                {
                                    drawingOtherControls = !drawingOtherControls; // Toggle
                                }
                                break;
                        }
                    }
                    else if (InputManager.isCombinedUnconfirmPressed(player.PlayerNumber))
                    {
                        if (drawingControls)
                        {
                            drawingControls = false;
                            drawingOtherControls = false;
                        }
                        else
                        {
                            currentMenuState = menuState.main;
                        }
                    }
                    else { }
                    break;
                case menuState.leave:
                    if (InputManager.isCombinedUpCombinedStickPressed(player.PlayerNumber)
                        || InputManager.isCombinedDownCombinedStickPressed(player.PlayerNumber))
                    {
                        switch (currentYesNoState)
                        {
                            case yesNoState.yes:
                                currentYesNoState = yesNoState.no;
                                break;
                            case yesNoState.no:
                                currentYesNoState = yesNoState.yes;
                                break;
                        }
                    }
                    else { }

                    if (InputManager.isCombinedConfirmPressed(player.PlayerNumber))
                    {
                        switch (currentYesNoState)
                        {
                            case yesNoState.yes:
                                currentMenuState = menuState.main;
                                currentMainState = mainState.resume;
                                player.unpause();
                                //Space394Game.GameInstance.decreasePlayers((int)player.PlayerNumber);
                                Space394Game.GameInstance.CurrentScene.ForceReadyToExit(); // Temporary
                                break;
                            case yesNoState.no:
                                currentMenuState = menuState.main;
                                break;
                        }
                    }
                    else if (InputManager.isCombinedUnconfirmPressed(player.PlayerNumber))
                    {
                        currentMenuState = menuState.main;
                    }
                    else { }
                    break;
                case menuState.destruct:
                    if (InputManager.isCombinedUpCombinedStickPressed(player.PlayerNumber)
                        || InputManager.isCombinedDownCombinedStickPressed(player.PlayerNumber))
                    {
                        switch (currentYesNoState)
                        {
                            case yesNoState.yes:
                                currentYesNoState = yesNoState.no;
                                break;
                            case yesNoState.no:
                                currentYesNoState = yesNoState.yes;
                                break;
                        }
                    }
                    else { }

                    if (InputManager.isCombinedConfirmPressed(player.PlayerNumber))
                    {
                        switch (currentYesNoState)
                        {
                            case yesNoState.yes:
                                currentMenuState = menuState.main;
                                currentMainState = mainState.resume;
                                player.unpause();
                                if (player.PlayerShip != null)
                                {
                                    player.PlayerShip.Health = 0;
                                }
                                else { }
                                break;
                            case yesNoState.no:
                                currentMenuState = menuState.main;
                                break;
                        }
                    }
                    else if (InputManager.isCombinedUnconfirmPressed(player.PlayerNumber))
                    {
                        currentMenuState = menuState.main;
                    }
                    else { }
                    break;
            }
            if (InputManager.isCombinedPausePressed(player.PlayerNumber))
            {
                currentMenuState = menuState.main;
                currentMainState = mainState.resume;
                player.unpause();
            }
            else { }
        }

        public override void OnExit()
        {

        }
    }
}
