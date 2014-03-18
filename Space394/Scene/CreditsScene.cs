using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space394.PlayerStates;

namespace Space394.Scenes
{
    public class CreditsScene : Scene
    {
        private AutoTexture2D overlay;

        private float timer;
        private const float TIMER = 55.0f;

        private const float SPEED = -30.0f;
        private float speed = SPEED;

        private const float X = 125;
        private float x = X;

        private float y;
        private const float START_Y = 500;

        private const float Y_DIST = 20;
        private const float TITLE_Y_DIST = 30;
        private const float SECTION_Y_DIST = 50;
        private const float GAME_TITLE_Y_DIST = 115;

        private float yDist = Y_DIST;
        private float titleYDist = TITLE_Y_DIST;
        private float sectionYDist = SECTION_Y_DIST;
        private float gameTitleYDist = GAME_TITLE_Y_DIST;

        private SpriteFont gameTitleFont;
        private SpriteFont titleFont;
        private SpriteFont font;

        private Vector2 fontScale;

        private Random random;

        private string space394_title =
            "Space [394]";

        private string grubby_paw_studios_title =
            "Grubby Paw Studios";
        private string[] grubby_paw_studios_members =
        {
            "Corbin Cogswell - Programming / Producer",
            "Natalie Wojciechowski - Programming",
            "Kevin Smick - Art",
            "Chris Santore - Art"
        };
        private List<string> gps_membersList;

        private string audio =
            "Audio";
        private string[] audio_contributors =
        {
            "Brandon Olafsson - Music"
        };
        private List<string> audio_contributorsList;

        private string created_with_title =
            "Created with";
        private string[] created =
        {
            "XNA Game Studio",
            "BePu Physics"
        };
        private List<string> createdList;

        private string playtesters_title =
            "Playtesters";
        private string[] playtesters = 
        {
            "Michelle Raymond",
            "Chloe Rosen",
            "Giselle Lorence",
            "Daniel Rolon",
            "Derek Hearn",
            "Nick Edgerton",
            "Paul Gomez",
            "Sara Pope",
            "Melinda Andrea",
            "Jeffrey Bakken",
            "Colin Mei",
            "Tyler Strickfaden",
            "Nick Maxey",
            "Giovanni Mota",
            "Joe Sweeney",
            "Dylan Norris",
            "Max Arnold",
            "Andrew Underwood"
        };
        private List<string> playtestersList;

        private string special_thanks_title =
            "Special Thanks";
        private string[] special_thankees =
        {
            "Joel Kuczmarski",
            "Stephan Lane",
            "Jake Garcia",
            "Katie Salen",
            "Brian Schrank",
            "DePaul University",
            "Norbo",
            "Various forum and tutorial posters",
            "Michael Ogren"
        };
        private List<string> thankeeList;
        private string special_special_thanks =
            "All our fans";
        private string super_special_special_thanks =
            "And you";

        public CreditsScene()
            : base()
        {
            LogCat.updateValue("Scene", "Credits");
            timer = TIMER;

            random = new Random(System.DateTime.Now.Millisecond);

            y = START_Y;
            y *= AutoTexture2D.HeightConversion;

            speed *= AutoTexture2D.HeightConversion;

            x *= AutoTexture2D.WidthConversion;

            yDist *= AutoTexture2D.HeightConversion;
            titleYDist *= AutoTexture2D.HeightConversion;
            sectionYDist *= AutoTexture2D.HeightConversion;
            gameTitleYDist *= AutoTexture2D.HeightConversion;

            fontScale = new Vector2(AutoTexture2D.WidthConversion, AutoTexture2D.HeightConversion);
        }

        public override void Initialize()
        {
            SoundManager.StopMusic();

            overlay = new AutoTexture2D(ContentLoadManager.loadTexture("Textures/credits_overlay"), Vector2.Zero);

            gameTitleFont = Space394Game.GameInstance.Content.Load<SpriteFont>("Fonts/CreditsGameTitleFont");
            titleFont = Space394Game.GameInstance.Content.Load<SpriteFont>("Fonts/CreditsTitleFont");
            font = Space394Game.GameInstance.Content.Load<SpriteFont>("Fonts/CreditsFont");

            List<string> sortList = new List<string>();

            gps_membersList = new List<string>();
            foreach (string s in grubby_paw_studios_members)
            {
                sortList.Add(s);
            }

            while (sortList.Count > 0)
            {
                int j = random.Next() % sortList.Count;
                gps_membersList.Add(sortList[j]);
                sortList.RemoveAt(j);
            }

            audio_contributorsList = new List<string>();
            foreach (string s in audio_contributors)
            {
                sortList.Add(s);
            }

            while (sortList.Count > 0)
            {
                int j = random.Next() % sortList.Count;
                audio_contributorsList.Add(sortList[j]);
                sortList.RemoveAt(j);
            }

            createdList = new List<string>();
            foreach (string s in created)
            {
                sortList.Add(s);
            }

            while (sortList.Count > 0)
            {
                int j = random.Next() % sortList.Count;
                createdList.Add(sortList[j]);
                sortList.RemoveAt(j);
            }

            playtestersList = new List<string>();
            foreach (string s in playtesters)
            {
                sortList.Add(s);
            }

            while (sortList.Count > 0)
            {
                int j = random.Next() % sortList.Count;
                playtestersList.Add(sortList[j]);
                sortList.RemoveAt(j);
            }

            thankeeList = new List<string>();
            foreach (string s in special_thankees)
            {
                sortList.Add(s);
            }

            while (sortList.Count > 0)
            {
                int j = random.Next() % sortList.Count;
                thankeeList.Add(sortList[j]);
                sortList.RemoveAt(j);
            }

            base.Initialize();
        }

        public override void Update(float deltaTime)
        {
            timer -= deltaTime;

            y += (speed * deltaTime);

            if (InputManager.isUnconfirmationKeyPressed() ||
                InputManager.isConfirmationKeyPressed() ||
                InputManager.isSuperConfirmationKeyPressed()
                || timer <= 0)
            {
                readyToExit = true;
            }
            else { }

            base.Update(deltaTime);
        }

        public override void Draw()
        {
            Space394Game.GameInstance.GraphicsDevice.Clear(Color.Black);

            SpriteBatch spriteBatch = Space394Game.GameInstance.SpriteBatch;

            spriteBatch.Begin();

            float currentY = y;

            spriteBatch.DrawString(gameTitleFont, space394_title, new Vector2(x, currentY), Color.White, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            currentY += gameTitleYDist;

            spriteBatch.DrawString(titleFont, grubby_paw_studios_title, new Vector2(x, currentY), Color.White, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            currentY += titleYDist;

            foreach (string member in gps_membersList)
            {
                spriteBatch.DrawString(font, member, new Vector2(x, currentY), Color.White, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
                currentY += yDist;
            }

            currentY += sectionYDist;

            spriteBatch.DrawString(titleFont, audio, new Vector2(x, currentY), Color.White, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            currentY += titleYDist;

            foreach (string contributor in audio_contributorsList)
            {
                spriteBatch.DrawString(font, contributor, new Vector2(x, currentY), Color.White, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
                currentY += yDist;
            }

            currentY += sectionYDist;

            spriteBatch.DrawString(titleFont, created_with_title, new Vector2(x, currentY), Color.White, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            currentY += titleYDist;

            foreach (string creation in createdList)
            {
                spriteBatch.DrawString(font, creation, new Vector2(x, currentY), Color.White, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
                currentY += yDist;
            }

            currentY += sectionYDist;

            spriteBatch.DrawString(titleFont, playtesters_title, new Vector2(x, currentY), Color.White, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            currentY += titleYDist;

            foreach (string tester in playtestersList)
            {
                spriteBatch.DrawString(font, tester, new Vector2(x, currentY), Color.White, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
                currentY += yDist;
            }

            currentY += sectionYDist;

            spriteBatch.DrawString(titleFont, special_thanks_title, new Vector2(x, currentY), Color.White, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            currentY += titleYDist;

            foreach (string thankee in thankeeList)
            {
                spriteBatch.DrawString(font, thankee, new Vector2(x, currentY), Color.White, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
                currentY += yDist;
            }

            spriteBatch.DrawString(font, special_special_thanks, new Vector2(x, currentY), Color.White, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);
            currentY += yDist;

            spriteBatch.DrawString(font, super_special_special_thanks, new Vector2(x, currentY), Color.White, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0);

            overlay.DrawAlreadyBegunMaintainRatio();

            spriteBatch.End();

            Space394Game.GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw();
        }

        public override Space394Game.sceneEnum GetNextScene()
        {
            return Space394Game.sceneEnum.MainMenuScene;
        }

        public override void Exit(Scene nextScene)
        {
            SceneObjects.Clear();
            base.Exit(nextScene);
        }
    }
}
