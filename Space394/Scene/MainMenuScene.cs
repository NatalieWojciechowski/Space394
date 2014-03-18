using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space394.PlayerStates;
using Space394.SceneObjects;

namespace Space394.Scenes
{
    public class MainMenuScene : Scene
    {
        private Texture2D mainMenuTexture;
        private Texture2D skirmishTexture;
        private Texture2D extrasTexture;

        private AutoTexture2D campaign;
        private AutoTexture2D skirmish;
        private AutoTexture2D settings;
        private AutoTexture2D extras;
        private AutoTexture2D exit;

        private AutoTexture2D campaignLit;
        private AutoTexture2D skirmishLit;
        private AutoTexture2D settingsLit;
        private AutoTexture2D extrasLit;
        private AutoTexture2D exitLit;

        private Vector2 campaignPosition;
        private Vector2 skirmishPosition;
        private Vector2 settingsPosition;
        private Vector2 extrasPosition;
        private Vector2 exitPosition;

        private AutoTexture2D engagement;
        private AutoTexture2D contention;
        private AutoTexture2D dissidence;

        private AutoTexture2D engagementLit;
        private AutoTexture2D contentionLit;
        private AutoTexture2D dissidenceLit;

        private Vector2 engagementPosition;
        private Vector2 contentionPosition;
        private Vector2 dissidencePosition;

        private AutoTexture2D dev;
        private AutoTexture2D art;
        private AutoTexture2D trailer;
        private AutoTexture2D credits;

        private AutoTexture2D devLit;
        private AutoTexture2D artLit;
        private AutoTexture2D trailerLit;
        private AutoTexture2D creditsLit;

        private Vector2 devPosition;
        private Vector2 artPosition;
        private Vector2 trailerPosition;
        private Vector2 creditsPosition;

        private AutoTexture2D topShip;
        private AutoTexture2D bottomShip;

        private Vector2 farLeftTopShipPosition;
        private Vector2 leftTopShipPosition;
        private Vector2 middleTopShipPosition;
        private Vector2 rightTopShipPosition;
        private Vector2 farRightTopShipPosition;

        private Vector2 farLeftBottomShipPosition;
        private Vector2 leftBottomShipPosition;
        private Vector2 middleBottomShipPosition;
        private Vector2 rightBottomShipPosition;
        private Vector2 farRightBottomShipPosition;

        private AutoTexture2D buttons;

        private float fade;
        private const float FADE_SPEED = 0.5f;
        private bool fadeIn = true;
        private float fadeDelay;
        private float MIN_FADE = 0.15f;
        private float MIN_DELAY = 0.0f;
        private float MAX_FADE = 1.0f;
        private float MAX_DELAY = 0.01f;
        private bool delaySet = false;

        private bool drawButtons;
        private float buttonsFadeDelay;
        private const float BUTTON_FADE_DELAY = 1.5f;
        
        private float pressButtonFade;
        private const float BUTTON_FADE_SPEED = 0.75f;

        private Space394Game.sceneEnum nextScene;

        public enum state
        {
            fade,
            main,
            skirmish,
            extras
        };
        private state currentState;

        public enum mainSelect
        {
            //campaign,
            skirmish,
            //settings,
            extras,
            exit
        };
        private mainSelect currentMainSelect;

        public enum skirmishSelect
        {
            engagement,
            contention,
            dissidence
        };
        private skirmishSelect currentSkirmishSelect;

        public enum extrasSelect
        {
            //dev,
            //art,
            //trailer,
            credits
        };
        private extrasSelect currentExtrasSelect;

        public MainMenuScene()
            : base()
        {
            LogCat.updateValue("Scene", "MainMenu");
            currentState = state.fade;
            currentMainSelect = mainSelect.skirmish;
            currentSkirmishSelect = skirmishSelect.engagement;
            currentExtrasSelect = extrasSelect.credits;
        }

        public override void Initialize()
        {
            if (System.DateTime.Now.Millisecond % 2 == 0)
            {
                mainMenuTexture = ContentLoadManager.loadTexture("Textures/Screens/main_esx");
                skirmishTexture = ContentLoadManager.loadTexture("Textures/Screens/skirmish_esx");
                extrasTexture = ContentLoadManager.loadTexture("Textures/Screens/extras_esx");

                campaignPosition = new Vector2(280, 120);
                skirmishPosition = new Vector2(286, 181);
                settingsPosition = new Vector2(278, 237);
                extrasPosition = new Vector2(269, 290);
                exitPosition = new Vector2(254, 340);

                campaign = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_esx_campaign"), campaignPosition);
                skirmish = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_esx_skirmish"), skirmishPosition);
                settings = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_esx_settings"), settingsPosition);
                extras = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_esx_extras"), extrasPosition);
                exit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_esx_exit"), exitPosition);

                campaignLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_esx_campaign_lit"), campaignPosition);
                skirmishLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_esx_skirmish_lit"), skirmishPosition);
                settingsLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_esx_settings_lit"), settingsPosition);
                extrasLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_esx_extras_lit"), extrasPosition);
                exitLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_esx_exit_lit"), exitPosition);

                engagementPosition = new Vector2(286, 181);
                contentionPosition = new Vector2(278, 237);
                dissidencePosition = new Vector2(269, 290);

                engagement = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_esx_engagement"), engagementPosition);
                contention = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_esx_contention"), contentionPosition);
                dissidence = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_esx_dissidence"), dissidencePosition);

                engagementLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_esx_engagement_lit"), engagementPosition);
                contentionLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_esx_contention_lit"), contentionPosition);
                dissidenceLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_esx_dissidence_lit"), dissidencePosition);

                devPosition = new Vector2(280, 120);
                artPosition = new Vector2(286, 181);
                trailerPosition = new Vector2(278, 237);
                creditsPosition = new Vector2(269, 290);

                dev = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_esx_dev_int"), devPosition);
                art = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_esx_art"), artPosition);
                trailer = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_esx_trailer"), trailerPosition);
                credits = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_esx_credits"), creditsPosition);

                devLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_esx_dev_int_lit"), devPosition);
                artLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_esx_art_lit"), artPosition);
                trailerLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_esx_trailer_lit"), trailerPosition);
                creditsLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_esx_credits_lit"), creditsPosition);

                farLeftTopShipPosition = new Vector2(482, 145);
                leftTopShipPosition = new Vector2(530, 145);
                middleTopShipPosition = new Vector2(578, 145);
                rightTopShipPosition = new Vector2(626, 145);
                farRightTopShipPosition = new Vector2(673, 145);

                farLeftBottomShipPosition = new Vector2(484, 277);
                leftBottomShipPosition = new Vector2(531, 277);
                middleBottomShipPosition = new Vector2(579, 277);
                rightBottomShipPosition = new Vector2(627, 277);
                farRightBottomShipPosition = new Vector2(675, 277);

                topShip = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_esx_cap_top"), middleTopShipPosition);
                bottomShip = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_halk_cap_bottom"), middleBottomShipPosition);

                foreach (Player player in Space394Game.GameInstance.Controllers)
                {
                    player.CurrentTeam = Ship.Team.Esxolus;
                }
            }
            else
            {
                mainMenuTexture = ContentLoadManager.loadTexture("Textures/Screens/main_halk");
                skirmishTexture = ContentLoadManager.loadTexture("Textures/Screens/skirmish_halk");
                extrasTexture = ContentLoadManager.loadTexture("Textures/Screens/extras_halk");

                campaignPosition = new Vector2(380, 121);
                skirmishPosition = new Vector2(377, 182);
                settingsPosition = new Vector2(393, 239);
                extrasPosition = new Vector2(414, 290);
                exitPosition = new Vector2(451, 341);

                campaign = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_halk_campaign"), campaignPosition);
                skirmish = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_halk_skirmish"), skirmishPosition);
                settings = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_halk_settings"), settingsPosition);
                extras = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_halk_extras"), extrasPosition);
                exit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_halk_exit"), exitPosition);

                campaignLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_halk_campaign_lit"), campaignPosition);
                skirmishLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_halk_skirmish_lit"), skirmishPosition);
                settingsLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_halk_settings_lit"), settingsPosition);
                extrasLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_halk_extras_lit"), extrasPosition);
                exitLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/main_halk_exit_lit"), exitPosition);

                engagementPosition = new Vector2(350, 182);
                contentionPosition = new Vector2(371, 239);
                dissidencePosition = new Vector2(377, 290);

                engagement = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_halk_engagement"), engagementPosition);
                contention = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_halk_contention"), contentionPosition);
                dissidence = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_halk_dissidence"), dissidencePosition);

                engagementLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_halk_engagement_lit"), engagementPosition);
                contentionLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_halk_contention_lit"), contentionPosition);
                dissidenceLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_halk_dissidence_lit"), dissidencePosition);

                devPosition = new Vector2(283, 121);
                artPosition = new Vector2(301, 182);
                trailerPosition = new Vector2(352, 239);
                creditsPosition = new Vector2(405, 290);

                dev = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_halk_dev_int"), devPosition);
                art = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_halk_art"), artPosition);
                trailer = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_halk_trailer"), trailerPosition);
                credits = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_halk_credits"), creditsPosition);

                devLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_halk_dev_int_lit"), devPosition);
                artLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_halk_art_lit"), artPosition);
                trailerLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_halk_trailer_lit"), trailerPosition);
                creditsLit = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/extras_halk_credits_lit"), creditsPosition);

                farLeftTopShipPosition = new Vector2(75, 152);
                leftTopShipPosition = new Vector2(123, 152);
                middleTopShipPosition = new Vector2(171, 152);
                rightTopShipPosition = new Vector2(219, 152);
                farRightTopShipPosition = new Vector2(266, 152);

                farLeftBottomShipPosition = new Vector2(75, 271);
                leftBottomShipPosition = new Vector2(122, 271);
                middleBottomShipPosition = new Vector2(170, 271);
                rightBottomShipPosition = new Vector2(218, 271);
                farRightBottomShipPosition = new Vector2(266, 271);

                topShip = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_halk_cap_top"), middleTopShipPosition);
                bottomShip = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/skirmish_esx_cap_bottom"), middleBottomShipPosition);

                foreach (Player player in Space394Game.GameInstance.Controllers)
                {
                    player.CurrentTeam = Ship.Team.Halk;
                }
            }

            buttons = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/menu_buttons"), new Vector2(25, 435));

            drawButtons = false;
            buttonsFadeDelay = BUTTON_FADE_DELAY;
            pressButtonFade = 0;
            fade = 0;
            fadeDelay = MAX_DELAY;
            fadeIn = true;
            delaySet = false;

            Space394Game.GameInstance.clearPlayers();

            if (!SoundManager.PlayingMusic)
            {
                SoundManager.StartMenuMusic();
            }
            else { }
            
            base.Initialize();
        }

        public override void Update(float deltaTime) 
        {
            switch (currentState)
            {
                case state.fade:
                    UpdateFade(deltaTime);
                    break;
                case state.main:
                    UpdateMain(deltaTime);
                    break;
                case state.skirmish:
                    UpdateSkirmish(deltaTime);
                    break;
                case state.extras:
                    UpdateExtras(deltaTime);
                    break;
            }

            base.Update(deltaTime);
        }

        private void UpdateFade(float deltaTime)
        {
            fade = Math.Min(1.0f, fade + FADE_SPEED * deltaTime);

            buttonsFadeDelay -= deltaTime;
            if (buttonsFadeDelay <= 0)
            {
                drawButtons = true;
                if (fadeIn)
                {
                    if (!delaySet)
                    {
                        pressButtonFade += deltaTime * BUTTON_FADE_SPEED;
                        if (pressButtonFade >= MAX_FADE)
                        {
                            pressButtonFade = MAX_FADE;
                            delaySet = true;
                            fadeDelay = MAX_DELAY;
                        }
                        else { }
                    }
                    else
                    {
                        fadeDelay -= deltaTime;
                        if (fadeDelay <= 0)
                        {
                            fadeIn = false;
                            delaySet = false;
                            currentState = state.main;
                        }
                        else { }
                    }
                }
                else
                {
                    if (!delaySet)
                    {
                        pressButtonFade -= deltaTime * BUTTON_FADE_SPEED;
                        if (pressButtonFade <= MIN_FADE)
                        {
                            pressButtonFade = MIN_FADE;
                            delaySet = true;
                            fadeDelay = MIN_DELAY;
                        }
                        else { }
                    }
                    else
                    {
                        fadeDelay -= deltaTime;
                        if (fadeDelay <= 0)
                        {
                            fadeIn = true;
                            delaySet = false;
                        }
                        else { }
                    }
                }
            }
            else { }

            if (InputManager.isConfirmationKeyPressed() || InputManager.isSuperConfirmationKeyPressed()
                || InputManager.isSuperConfirmationKeyPressed())
            {
                drawButtons = true;
                fadeIn = false;
                delaySet = false;
                currentState = state.main;
            }
            else { }
        }

        private void UpdateMain(float deltaTime)
        {
            foreach (Player player in Space394Game.GameInstance.Controllers)
            {
                if (InputManager.isCombinedDownCombinedStickPressed(player.PlayerNumber))
                {
                    switch (currentMainSelect)
                    {
                        case mainSelect.skirmish:
                            currentMainSelect = mainSelect.extras;
                            break;
                        case mainSelect.extras:
                            currentMainSelect = mainSelect.exit;
                            break;
                        case mainSelect.exit:
                            currentMainSelect = mainSelect.skirmish;
                            break;
                    }
                }
                else if (InputManager.isCombinedUpCombinedStickPressed(player.PlayerNumber))
                {
                    switch (currentMainSelect)
                    {
                        case mainSelect.skirmish:
                            currentMainSelect = mainSelect.exit;
                            break;
                        case mainSelect.extras:
                            currentMainSelect = mainSelect.skirmish;
                            break;
                        case mainSelect.exit:
                            currentMainSelect = mainSelect.extras;
                            break;
                    }
                }
                else { }
            }
            if (InputManager.isConfirmationKeyPressed() || InputManager.isSuperConfirmationKeyPressed())
            {
                switch (currentMainSelect)
                {
                    case mainSelect.skirmish:
                        currentState = state.skirmish;
                        break;
                    case mainSelect.extras:
                        currentState = state.extras;
                        break;
                    case mainSelect.exit:
                        Space394Game.GameInstance.Exit();
                        break;
                }
            }
            else { }
        }

        private void UpdateSkirmish(float deltaTime)
        {
            foreach (Player player in Space394Game.GameInstance.Controllers)
            {
                if (InputManager.isCombinedDownCombinedStickPressed(player.PlayerNumber))
                {
                    switch (currentSkirmishSelect)
                    {
                        case skirmishSelect.engagement:
                            currentSkirmishSelect = skirmishSelect.contention;
                            break;
                        case skirmishSelect.contention:
                            currentSkirmishSelect = skirmishSelect.dissidence;
                            break;
                        case skirmishSelect.dissidence:
                            currentSkirmishSelect = skirmishSelect.engagement;
                            break;
                    }
                }
                else if (InputManager.isCombinedUpCombinedStickPressed(player.PlayerNumber))
                {
                    switch (currentSkirmishSelect)
                    {
                        case skirmishSelect.engagement:
                            currentSkirmishSelect = skirmishSelect.dissidence;
                            break;
                        case skirmishSelect.contention:
                            currentSkirmishSelect = skirmishSelect.engagement;
                            break;
                        case skirmishSelect.dissidence:
                            currentSkirmishSelect = skirmishSelect.contention;
                            break;
                    }
                }
                else { }
            }
            if (InputManager.isConfirmationKeyPressed() || InputManager.isSuperConfirmationKeyPressed())
            {
                switch (currentSkirmishSelect)
                {
                    case skirmishSelect.engagement:
                        LoadingScene.MapToLoad = "SampleSmall";
                        break;
                    case skirmishSelect.contention:
                        LoadingScene.MapToLoad = "SampleMedium";
                        break;
                    case skirmishSelect.dissidence:
                        LoadingScene.MapToLoad = "Sample";
                        break;
                    default: // Just in case
                        LoadingScene.MapToLoad = "SampleSmall";
                        break;
                }
                nextScene = Space394Game.sceneEnum.PlayerSelectScene;
                readyToExit = true;
                foreach (Player player in Space394Game.GameInstance.Controllers)
                {
                    player.nextState();
                }
            }
            else if (InputManager.isUnconfirmationKeyPressed())
            {
                currentState = state.main;
                currentSkirmishSelect = skirmishSelect.engagement;
            }
            else { }
        }

        private void UpdateExtras(float deltaTime)
        {
            foreach (Player player in Space394Game.GameInstance.Controllers)
            {
                if (InputManager.isCombinedDownCombinedStickPressed(player.PlayerNumber))
                {
                    switch (currentExtrasSelect)
                    {
                        case extrasSelect.credits:
                            currentExtrasSelect = extrasSelect.credits;
                            break;
                    }
                }
                else if (InputManager.isCombinedUpCombinedStickPressed(player.PlayerNumber))
                {
                    switch (currentExtrasSelect)
                    {
                        case extrasSelect.credits:
                            currentExtrasSelect = extrasSelect.credits;
                            break;
                    }
                }
                else { }
            }
            if (InputManager.isConfirmationKeyPressed() || InputManager.isSuperConfirmationKeyPressed())
            {
                switch (currentExtrasSelect)
                {
                    case extrasSelect.credits:
                        nextScene = Space394Game.sceneEnum.CreditsScene;
                        break;
                }
                readyToExit = true;
            }
            else if (InputManager.isUnconfirmationKeyPressed())
            {
                currentState = state.main;
                currentExtrasSelect = extrasSelect.credits;
            }
            else { }
        }

        public override void Draw()
        {
            switch (currentState)
            {
                case state.fade:
                    DrawFade();
                    break;
                case state.main:
                    DrawMain();
                    break;
                case state.skirmish:
                    DrawSkirmish();
                    break;
                case state.extras:
                    DrawExtras();
                    break;
            }
        }

        private void DrawFade() 
        {
            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(mainMenuTexture,
                new Rectangle(Space394Game.GameInstance.GraphicsDevice.Viewport.X, Space394Game.GameInstance.GraphicsDevice.Viewport.Y,
                    Space394Game.GameInstance.GraphicsDevice.Viewport.Width, Space394Game.GameInstance.GraphicsDevice.Viewport.Height), 
                new Color(fade, fade, fade));
            if (drawButtons)
            {
                campaign.Color = new Color(pressButtonFade, pressButtonFade, pressButtonFade);
                campaign.DrawAlreadyBegunMaintainRatio();
                skirmish.Color = new Color(pressButtonFade, pressButtonFade, pressButtonFade);
                skirmish.DrawAlreadyBegunMaintainRatio();
                settings.Color = new Color(pressButtonFade, pressButtonFade, pressButtonFade);
                settings.DrawAlreadyBegunMaintainRatio();
                extras.Color = new Color(pressButtonFade, pressButtonFade, pressButtonFade);
                extras.DrawAlreadyBegunMaintainRatio();
                exit.Color = new Color(pressButtonFade, pressButtonFade, pressButtonFade);
                exit.DrawAlreadyBegunMaintainRatio();
                buttons.Color = new Color(pressButtonFade, pressButtonFade, pressButtonFade);
                buttons.DrawAlreadyBegunMaintainRatio();
            }
            else { }
            spriteBatch.End();

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw();
        }

        private void DrawMain()
        {
            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(mainMenuTexture,
                new Rectangle(Space394Game.GameInstance.GraphicsDevice.Viewport.X, Space394Game.GameInstance.GraphicsDevice.Viewport.Y,
                    Space394Game.GameInstance.GraphicsDevice.Viewport.Width, Space394Game.GameInstance.GraphicsDevice.Viewport.Height), 
                Color.White);

            switch (currentMainSelect)
            {
                case mainSelect.skirmish:
                    campaign.Color = Color.White;
                    campaign.DrawAlreadyBegunMaintainRatio();
                    skirmishLit.Color = Color.White;
                    skirmishLit.DrawAlreadyBegunMaintainRatio();
                    settings.Color = Color.White;
                    settings.DrawAlreadyBegunMaintainRatio();
                    extras.Color = Color.White;
                    extras.DrawAlreadyBegunMaintainRatio();
                    exit.Color = Color.White;
                    exit.DrawAlreadyBegunMaintainRatio();
                    break;
                case mainSelect.extras:
                    campaign.Color = Color.White;
                    campaign.DrawAlreadyBegunMaintainRatio();
                    skirmish.Color = Color.White;
                    skirmish.DrawAlreadyBegunMaintainRatio();
                    settings.Color = Color.White;
                    settings.DrawAlreadyBegunMaintainRatio();
                    extrasLit.Color = Color.White;
                    extrasLit.DrawAlreadyBegunMaintainRatio();
                    exit.Color = Color.White;
                    exit.DrawAlreadyBegunMaintainRatio();
                    break;
                case mainSelect.exit:
                    campaign.Color = Color.White;
                    campaign.DrawAlreadyBegunMaintainRatio();
                    skirmish.Color = Color.White;
                    skirmish.DrawAlreadyBegunMaintainRatio();
                    settings.Color = Color.White;
                    settings.DrawAlreadyBegunMaintainRatio();
                    extras.Color = Color.White;
                    extras.DrawAlreadyBegunMaintainRatio();
                    exitLit.Color = Color.White;
                    exitLit.DrawAlreadyBegunMaintainRatio();
                    break;
            }
            buttons.Color = Color.White;
            buttons.DrawAlreadyBegunMaintainRatio();

            spriteBatch.End();

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw();
        }

        private void DrawSkirmish()
        {
            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(skirmishTexture,
                new Rectangle(Space394Game.GameInstance.GraphicsDevice.Viewport.X, Space394Game.GameInstance.GraphicsDevice.Viewport.Y,
                    Space394Game.GameInstance.GraphicsDevice.Viewport.Width, Space394Game.GameInstance.GraphicsDevice.Viewport.Height), 
                Color.White);

            switch (currentSkirmishSelect)
            {
                case skirmishSelect.engagement:
                    engagementLit.DrawAlreadyBegunMaintainRatio();
                    contention.DrawAlreadyBegunMaintainRatio();
                    dissidence.DrawAlreadyBegunMaintainRatio();

                    topShip.Position = middleTopShipPosition;
                    topShip.DrawAlreadyBegunMaintainRatio();

                    bottomShip.Position = middleBottomShipPosition;
                    bottomShip.DrawAlreadyBegunMaintainRatio();
                    break;
                case skirmishSelect.contention:
                    engagement.DrawAlreadyBegunMaintainRatio();
                    contentionLit.DrawAlreadyBegunMaintainRatio();
                    dissidence.DrawAlreadyBegunMaintainRatio();

                    topShip.Position = leftTopShipPosition;
                    topShip.DrawAlreadyBegunMaintainRatio();
                    topShip.Position = rightTopShipPosition;
                    topShip.DrawAlreadyBegunMaintainRatio();

                    bottomShip.Position = leftBottomShipPosition;
                    bottomShip.DrawAlreadyBegunMaintainRatio();
                    bottomShip.Position = rightBottomShipPosition;
                    bottomShip.DrawAlreadyBegunMaintainRatio();
                    break;
                case skirmishSelect.dissidence:
                    engagement.DrawAlreadyBegunMaintainRatio();
                    contention.DrawAlreadyBegunMaintainRatio();
                    dissidenceLit.DrawAlreadyBegunMaintainRatio();

                    topShip.Position = farLeftTopShipPosition;
                    topShip.DrawAlreadyBegunMaintainRatio();
                    topShip.Position = middleTopShipPosition;
                    topShip.DrawAlreadyBegunMaintainRatio();
                    topShip.Position = farRightTopShipPosition;
                    topShip.DrawAlreadyBegunMaintainRatio();

                    bottomShip.Position = farLeftBottomShipPosition;
                    bottomShip.DrawAlreadyBegunMaintainRatio();
                    bottomShip.Position = middleBottomShipPosition;
                    bottomShip.DrawAlreadyBegunMaintainRatio();
                    bottomShip.Position = farRightBottomShipPosition;
                    bottomShip.DrawAlreadyBegunMaintainRatio();
                    break;
            }
            buttons.Color = Color.White;
            buttons.DrawAlreadyBegunMaintainRatio();

            spriteBatch.End();

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw();
        }

        private void DrawExtras()
        {
            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(extrasTexture,
                new Rectangle(Space394Game.GameInstance.GraphicsDevice.Viewport.X, Space394Game.GameInstance.GraphicsDevice.Viewport.Y,
                    Space394Game.GameInstance.GraphicsDevice.Viewport.Width, Space394Game.GameInstance.GraphicsDevice.Viewport.Height), 
                Color.White);

            switch (currentExtrasSelect)
            {
                case extrasSelect.credits:
                    dev.DrawAlreadyBegunMaintainRatio();
                    art.DrawAlreadyBegunMaintainRatio();
                    trailer.DrawAlreadyBegunMaintainRatio();
                    creditsLit.DrawAlreadyBegunMaintainRatio();
                    break;
            }
            buttons.Color = Color.White;
            buttons.DrawAlreadyBegunMaintainRatio();

            spriteBatch.End();

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw();
        }

        public override Space394Game.sceneEnum GetNextScene()
        {
            return nextScene;
        }

        public override void Exit(Scene nextScene)
        {
            SceneObjects.Clear();
            base.Exit(nextScene);
        }
    }
}
