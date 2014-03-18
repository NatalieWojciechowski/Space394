using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Space394.SceneObjects;
using Space394.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space394.PlayerStates;

namespace Space394
{
    public class PlayerHUD
    {
        private Player player;

        private AutoTexture2D hudBase;
        private AutoTexture2D[] healthBar;
        private AutoTexture2D[] shieldBar;
        private AutoTexture2D[] heatBar;
        private AutoTexture2D esxolusMissileIcon;
        private AutoTexture2D esxolusBombIcon;
        private AutoTexture2D esxolusHexIcon;
        private AutoTexture2D halkMissileIcon;
        private AutoTexture2D halkBombIcon;        
        private AutoTexture2D halkInterceptorMissileIcon;

        private AutoTexture2D takingHitSplash;

        private AutoTexture2D warning;
        private AutoTexture2D overheating;

        private AutoTexture2D esxolusLogo;
        private AutoTexture2D halkLogo;
        private AutoTexture2D esxolusCapitalShip;
        private AutoTexture2D halkCapitalShip;

        private AutoTexture2D timerBox;

        private AutoTexture2D reticule;
        private AutoTexture2D hitConfirmedReticule;

        private AutoTexture2D objectives;

        private Vector2 heatBarPosition = new Vector2(50, 250);

        private Vector2[] esxolusShipPositions;
        private Vector2[] halkShipPositions;
        private int initialShipsEsx;
        private int initialShipsHalk;
        private const int MAX_SHIPS = 5;
        private const int SHIP_DIST = 28;

        private Vector2[] numbers;

        private Vector2[] secondaryNumbers;
        private AutoTexture2D[] secondaryNumbersTextures;

        private Vector2 reticulePosition;
        private Vector2 hitConfirmedReticulePosition;

        private const int HEALTH = 10;
        private const int SHIELDS = 13;
        private const int HEAT = 17;

        private bool drawConfirmed = false;
        public bool DrawConfirmed
        {
            get { return drawConfirmed; }
            set { drawConfirmed = value; }
        }
        private float CONFIRMED_TIME = 0.05f;
        private float confirmedTimer;

        private bool takingHit = false;
        public bool TakingHit
        {
            get { return takingHit; }
            set { takingHit = value; }
        }
        private float hitAlpha = 0;
        private const float HIT_ALPHA_SCALE_IN = 50.0f;
        private const float HIT_ALPHA_SCALE_OUT = 25.0f;
        private bool hitAlphaIn = true;

        private SpriteFont font;
        private Vector2 fontScale;

        public PlayerHUD(Player _player)
        {
            player = _player;

            confirmedTimer = CONFIRMED_TIME;

            esxolusShipPositions = new Vector2[]
            {
                new Vector2(319, 8),
                new Vector2(292, 8),
                new Vector2(264, 8),
                new Vector2(236, 8),
                new Vector2(208, 8)  
            };

            halkShipPositions = new Vector2[]
            {
                new Vector2(469, 8),
                new Vector2(496, 8),
                new Vector2(523, 8),
                new Vector2(550, 8),
                new Vector2(576, 8)
            };

            numbers = new Vector2[]
            {
                new Vector2(370, 27),
                new Vector2(377, 27),
                new Vector2(387, 27), // :
                new Vector2(391, 27),
                new Vector2(398, 27),
                new Vector2(408, 27), // :
                new Vector2(412, 27),
                new Vector2(420, 27)
            };

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i].X *= AutoTexture2D.WidthConversion;
                numbers[i].Y *= AutoTexture2D.HeightConversion;
            }

            secondaryNumbers = new Vector2[]
            {
                new Vector2(27, 415),
                new Vector2(35, 415),
                new Vector2(43, 415)
            };
        }

        public void InitializeGraphics()
        {
            hudBase = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_base"), new Vector2(8, 379));

            healthBar = new AutoTexture2D[]
            {
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_armor_tic"), new Vector2(88, 431)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_armor_tic"), new Vector2(95, 431)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_armor_tic"), new Vector2(102, 431)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_armor_tic"), new Vector2(109, 431)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_armor_tic"), new Vector2(116, 431)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_armor_tic"), new Vector2(123, 431)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_armor_tic"), new Vector2(130, 431)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_armor_tic"), new Vector2(137, 431)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_armor_tic"), new Vector2(144, 431)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_armor_cap"), new Vector2(151, 431))
            };
            shieldBar = new AutoTexture2D[]
            {
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_shields_tic"), new Vector2(88, 409)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_shields_tic"), new Vector2(95, 409)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_shields_tic"), new Vector2(102, 409)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_shields_tic"), new Vector2(109, 409)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_shields_tic"), new Vector2(116, 409)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_shields_tic"), new Vector2(123, 409)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_shields_tic"), new Vector2(130, 409)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_shields_tic"), new Vector2(137, 409)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_shields_tic"), new Vector2(144, 409)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_shields_tic"), new Vector2(151, 409)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_shields_tic"), new Vector2(158, 409)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_shields_tic"), new Vector2(165, 409)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_shields_cap"), new Vector2(172, 409))
            };
            heatBar = new AutoTexture2D[]
            {
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_1"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_2"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_3"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_4"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_5"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_6"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_7"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_8"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_9"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_10"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_11"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_12"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_13"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_14"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_15"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_16"), new Vector2(9, 380)),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_heat_17"), new Vector2(9, 380))
            };
            secondaryNumbersTextures = new AutoTexture2D[]
            {
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_secondary_count_0"), secondaryNumbers[0]),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_secondary_count_1"), secondaryNumbers[0]),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_secondary_count_2"), secondaryNumbers[0]),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_secondary_count_3"), secondaryNumbers[0]),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_secondary_count_4"), secondaryNumbers[0]),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_secondary_count_5"), secondaryNumbers[0]),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_secondary_count_6"), secondaryNumbers[0]),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_secondary_count_7"), secondaryNumbers[0]),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_secondary_count_8"), secondaryNumbers[0]),
                new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_secondary_count_9"), secondaryNumbers[0]),
            };

            objectives = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/current_objective"), new Vector2(646, 13));

            timerBox = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_timer"), new Vector2(346, 8));

            esxolusMissileIcon = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_esx_missile_icon"), new Vector2(43, 413));
            esxolusBombIcon = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_esx_bomb_icon"), new Vector2(21, 398));
            esxolusHexIcon = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_esx_hex_icon"), new Vector2(32, 418));
            halkMissileIcon = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_halk_weapon_icon"), new Vector2(40, 415));
            halkInterceptorMissileIcon = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_halk_weapon_icon"), new Vector2(40, 415));
            halkBombIcon = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/HUD_halk_bomb_icon"), new Vector2(38, 413));

            warning = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/warning"), new Vector2(110, 379));
            overheating = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/overheating"), new Vector2(200, 386));

            initialShipsEsx = ((GameScene)Space394Game.GameInstance.CurrentScene).getSpawnPointsRemaining(Ship.Team.Esxolus);
            initialShipsHalk = ((GameScene)Space394Game.GameInstance.CurrentScene).getSpawnPointsRemaining(Ship.Team.Halk);

            int esxLogoX = 168;
            int halkLogoX = 608; //602;

            for (int i = 0; i < (MAX_SHIPS - initialShipsEsx); i++)
            {
                esxLogoX += SHIP_DIST;
            }

            for (int i = 0; i < (MAX_SHIPS - initialShipsHalk); i++)
            {
                halkLogoX -= SHIP_DIST;
            }

            esxolusLogo = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/HUD_esxolus_logo"), new Vector2(esxLogoX, 8));
            halkLogo = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/HUD_halk_logo"), new Vector2(halkLogoX, 7));

            esxolusCapitalShip = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/HUD_esxolus_capital_icon"), Vector2.Zero);
            halkCapitalShip = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/Screens/HUD_halk_capital_icon"), Vector2.Zero);

            takingHitSplash = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/taking_hit"), Vector2.Zero);

            font = Space394Game.GameInstance.Content.Load<SpriteFont>("Fonts/AgencyFB");
            fontScale = new Vector2(AutoTexture2D.WidthConversion, AutoTexture2D.HeightConversion);

            reticulePosition = new Vector2(800 / 2, 480 / 2);
            Texture2D reticuleTexture = ContentLoadManager.loadTexture("Textures/reticule_neutral");
            reticulePosition = new Vector2(reticulePosition.X - reticuleTexture.Width / 2, reticulePosition.Y - reticuleTexture.Height / 2);
            reticule = new AutoTexture2D(reticuleTexture, reticulePosition);

            hitConfirmedReticulePosition = new Vector2(800 / 2, 480 / 2);
            Texture2D hitConfirmedTex = ContentLoadManager.loadTexture("Textures/hit_confirmed_circle");
            hitConfirmedReticulePosition = new Vector2(hitConfirmedReticulePosition.X - hitConfirmedTex.Width / 2, hitConfirmedReticulePosition.Y - hitConfirmedTex.Height / 2);
            hitConfirmedReticule = new AutoTexture2D(hitConfirmedTex, hitConfirmedReticulePosition);
        }

        public void Update(float deltaTime) // , GameCamera camera)
        {
            if (drawConfirmed)
            {
                confirmedTimer -= deltaTime;
                if (confirmedTimer < 0)
                {
                    drawConfirmed = false;
                    confirmedTimer = CONFIRMED_TIME;
                }
            }
            else { }

            if (takingHit)
            {
                if (hitAlphaIn)
                {
                    hitAlpha += HIT_ALPHA_SCALE_IN * deltaTime;
                    if (hitAlpha >= 1)
                    {
                        hitAlpha = 1;
                        hitAlphaIn = false;
                    }
                    else { }
                }
                else
                {
                    hitAlpha -= HIT_ALPHA_SCALE_OUT * deltaTime;
                    if (hitAlpha <= 0)
                    {
                        hitAlpha = 0;
                        hitAlphaIn = true;
                        takingHit = false;
                    }
                    else { }
                }
            } else { }
        }

        public void DrawTray(GameCamera camera)
        {
            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;

            SpriteBatch batch = Space394Game.GameInstance.SpriteBatch;
            batch.Begin();

            esxolusLogo.DrawAlreadyBegunMaintainRatio(camera);
            halkLogo.DrawAlreadyBegunMaintainRatio(camera);

            timerBox.DrawAlreadyBegunMaintainRatio(camera);

            if (player.ObjectivesDrawActive && !(player.CurrentState is TeamSelectPlayerState || player.CurrentState is SpectatorPlayerState))
            {
                objectives.DrawAlreadyBegunMaintainRatio(camera);
            }
            else { }

            for (int i = 0; i < initialShipsEsx; i++)
            {
                esxolusCapitalShip.Position = esxolusShipPositions[i];
                SpawnShip ship = ((GameScene)Space394Game.GameInstance.CurrentScene).getSpawnShip(Ship.Team.Esxolus, i);
                float ratio = ship.Health / ship.MaxHealth;
                #region Decide color
                Color color = new Color(50, 50, 50);
                if (ratio == 1.0f)
                {
                    color = new Color(0, 167, 58);
                }
                else if (ratio >= .9f) 
                {
                    color = new Color(15, 179, 0);
                }
                else if (ratio >= .8f)
                {
                    color = new Color(54, 179, 0);
                }
                else if (ratio >= .7f)
                {
                    color = new Color(92, 179, 0);
                }
                else if (ratio >= .6f)
                {
                    color = new Color(131, 179, 0);
                }
                else if (ratio >= .5f)
                {
                    color = new Color(173, 179, 0);
                }
                else if (ratio >= .4f)
                {
                    color = new Color(179, 145, 0);
                }
                else if (ratio >= .3f)
                {
                    color = new Color(179, 101, 0);
                }
                else if (ratio >= .2f)
                {
                    color = new Color(179, 65, 0);
                }
                else if (ratio >= .1f)
                {
                    color = new Color(179, 27, 0);
                }
                else { /* Dark gray */ }
                #endregion
                esxolusCapitalShip.Color = color;
                esxolusCapitalShip.DrawAlreadyBegunMaintainRatio(camera);
            }

            for (int i = 0; i < initialShipsHalk; i++)
            {
                halkCapitalShip.Position = halkShipPositions[i];
                SpawnShip ship = ((GameScene)Space394Game.GameInstance.CurrentScene).getSpawnShip(Ship.Team.Halk, i);
                float ratio = ship.Health / ship.MaxHealth;
                #region Decide color
                Color color = new Color(50, 50, 50);
                if (ratio == 1.0f)
                {
                    color = new Color(0, 167, 58);
                }
                else if (ratio >= .9f)
                {
                    color = new Color(15, 179, 0);
                }
                else if (ratio >= .8f)
                {
                    color = new Color(54, 179, 0);
                }
                else if (ratio >= .7f)
                {
                    color = new Color(92, 179, 0);
                }
                else if (ratio >= .6f)
                {
                    color = new Color(131, 179, 0);
                }
                else if (ratio >= .5f)
                {
                    color = new Color(173, 179, 0);
                }
                else if (ratio >= .4f)
                {
                    color = new Color(179, 145, 0);
                }
                else if (ratio >= .3f)
                {
                    color = new Color(179, 101, 0);
                }
                else if (ratio >= .2f)
                {
                    color = new Color(179, 65, 0);
                }
                else if (ratio >= .1f)
                {
                    color = new Color(179, 27, 0);
                }
                else { /* Dark gray */ }
                #endregion
                halkCapitalShip.Color = color;
                halkCapitalShip.DrawAlreadyBegunMaintainRatio(camera);
            }

            int index = 0;
            float timer = ((GameScene)Space394Game.GameInstance.CurrentScene).WaveTimer;
            int timerInt = (int)(timer * 10000);
            timerInt -= (timerInt / 1000000);
            batch.DrawString(font, "" + ((timerInt % 1000000) / 100000), numbers[index++], Color.Lime, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            timerInt -= (timerInt / 100000);
            batch.DrawString(font, "" + ((timerInt % 100000) / 10000), numbers[index++], Color.Lime, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            timerInt -= (timerInt / 10000);
            batch.DrawString(font, ":", numbers[index++], Color.Lime, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            batch.DrawString(font, "" + ((timerInt % 10000) / 1000), numbers[index++], Color.Lime, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            timerInt -= (timerInt / 1000);
            batch.DrawString(font, "" + ((timerInt % 1000) / 100), numbers[index++], Color.Lime, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            timerInt -= (timerInt / 100);
            batch.DrawString(font, ":", numbers[index++], Color.Lime, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            batch.DrawString(font, "" + ((timerInt % 100) / 10), numbers[index++], Color.Lime, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            timerInt -= (timerInt / 10);
            batch.DrawString(font, "" + (((timerInt % 10)) / 1), numbers[index++], Color.Lime, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            // timerInt -= (timerInt / 1);

            batch.End();

            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
        }

        public void DrawShipHUD(GameCamera camera)
        {
            Space394Game.GameInstance.GraphicsDevice.Viewport = camera.ViewPort;
            
            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;
            spriteBatch.Begin();

            if (takingHit)
            {
                takingHitSplash.setAlpha((byte)hitAlpha);
                takingHitSplash.DrawAlreadyBegunMaintainRatio(camera);
            }
            else { }

            hudBase.DrawAlreadyBegunMaintainRatio(camera);

            if (player.PlayerShip is EsxolusAssaultFighter)
            {
                esxolusMissileIcon.DrawAlreadyBegunMaintainRatio(camera);
            }
            else if (player.PlayerShip is EsxolusBomber)
            {
                esxolusBombIcon.DrawAlreadyBegunMaintainRatio(camera);
            }
            else if (player.PlayerShip is EsxolusInterceptor)
            {
                esxolusHexIcon.DrawAlreadyBegunMaintainRatio(camera);
            }
            else if (player.PlayerShip is HalkAssaultFighter)
            {
                halkMissileIcon.DrawAlreadyBegunMaintainRatio(camera);
            }
            else if (player.PlayerShip is HalkBomber)
            {
                halkBombIcon.DrawAlreadyBegunMaintainRatio(camera);
            }
            else if (player.PlayerShip is HalkInterceptor)
            {
                halkInterceptorMissileIcon.DrawAlreadyBegunMaintainRatio(camera);
            }
            else { }

            int numHealth = (int)(player.PlayerShip.Health / player.PlayerShip.MaxHealth * HEALTH);
            byte alphaHealth = (byte)(((player.PlayerShip.Health / player.PlayerShip.MaxHealth) % HEALTH) * 255);

            int i;
            for (i = 0; i < numHealth; i++)
            {
                healthBar[i].setAlpha(255);
                healthBar[i].DrawAlreadyBegunMaintainRatio(camera);
            }
            if (i < HEALTH && i > 0)
            {
                //healthBar[i].setAlpha(alphaHealth);
                healthBar[i].DrawAlreadyBegunMaintainRatio(camera);
            }
            else { }

            int numShields = (int)(player.PlayerShip.Shields / player.PlayerShip.MaxShields * SHIELDS);
            byte alphaShields = (byte)(((player.PlayerShip.Shields / player.PlayerShip.MaxShields) % SHIELDS) * 255);

            for (i = 0; i < numShields; i++)
            {
                shieldBar[i].setAlpha(255);
                shieldBar[i].DrawAlreadyBegunMaintainRatio(camera);
            }
            if (i < SHIELDS && i > 0)
            {
                //shieldBar[i].setAlpha(alphaShields);
                shieldBar[i].DrawAlreadyBegunMaintainRatio(camera);
            }
            else { }

            int numHeat = (int)(player.PlayerShip.Heat / player.PlayerShip.getOverheatHeat() * HEAT);
            byte alphaHeat = (byte)(((player.PlayerShip.Heat / player.PlayerShip.getOverheatHeat()) % HEAT) * 255);

            for (i = 0; i < numHeat; i++)
            {
                heatBar[i].setAlpha(255);
                heatBar[i].DrawAlreadyBegunMaintainRatio(camera);
            }
            if (i < HEAT && i > 0)
            {
                //heatBar[i].setAlpha(alphaHeat);
                heatBar[i].DrawAlreadyBegunMaintainRatio(camera);
            }
            else { }

            if (player.PlayerShip.Heat >= (player.PlayerShip.HeatWarningThreshold))
            {
                warning.DrawAlreadyBegunMaintainRatio(camera);
                if (player.PlayerShip.Heat >= (player.PlayerShip.HeatDamageThreshold))
                {
                    overheating.DrawAlreadyBegunMaintainRatio(camera);
                }
                else { }
            }
            else { }

            int index = 0;
            int num;
            int secondaries = player.PlayerShip.SecondaryAmmo;
            int secondariesTotal = secondaries;
            secondaries -= (secondaries / 1000) * 1000;
            if (secondariesTotal > 99)
            {
                num = ((secondaries % 1000) / 100);
                secondaryNumbersTextures[num].Position = secondaryNumbers[index];
                secondaryNumbersTextures[num].DrawAlreadyBegunMaintainRatio(camera);
                // spriteBatch.DrawString(font, "" + ((secondaries % 1000) / 100), secondaryNumbers[index++], Color.White);
                secondaries -= (secondaries / 100) * 100;
            }
            else { }
            index++;
            if (secondariesTotal > 9)
            {
                num = ((secondaries % 100) / 10);
                secondaryNumbersTextures[num].Position = secondaryNumbers[index];
                secondaryNumbersTextures[num].DrawAlreadyBegunMaintainRatio(camera);
                // spriteBatch.DrawString(font, "" + ((secondaries % 100) / 10), secondaryNumbers[index++], Color.White);
                secondaries -= (secondaries / 10) * 10;
            }
            else { }
            index++;
            num = (((secondaries % 10)) / 1);
            secondaryNumbersTextures[num].Position = secondaryNumbers[index];
            secondaryNumbersTextures[num].DrawAlreadyBegunMaintainRatio(camera);
            // spriteBatch.DrawString(font, "" + (((secondaries % 10)) / 1), secondaryNumbers[index++], Color.White);

            reticule.DrawAlreadyBegunMaintainRatio(camera);
            if (drawConfirmed)
            {
                hitConfirmedReticule.DrawAlreadyBegunMaintainRatio(camera);
            }
            else { }

            spriteBatch.End();

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Space394Game.GameInstance.GraphicsDevice.Viewport = Space394Game.GameInstance.DefaultViewPort;
        }

        public void Draw(GameCamera camera)
        {
            if (player.PlayerShip != null && player.PlayerShip.PlayerControlled)
            {
                foreach (SceneObject item in Space394Game.GameInstance.CurrentScene.SceneObjects.Values)
                {
                    if (item is Ship && item.Health > 0)
                    {
                        if (((PlayerCamera)camera).PlayerShip != null)
                        {
                            ((Ship)item).DrawReticule(camera);
                        }
                        else { }
                    }
                    else { }
                }
            }
            else { }
        }
    }
}
