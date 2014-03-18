using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Space394
{
    public static class InputManager
    {
        private const int MAX_PLAYERS = 4;

        private const int CENTER_X = 400;
        private const int CENTER_Y = 240;
        private const int MIN_XY = 10;
        private const int MAX_XY = 50;

        private const float KEYBOARD_DEGREE = 1.0f;

        // Keyboard keys
        private static bool[] player1Keys;
        private static bool[] player2Keys;
        private static bool[] player3Keys;
        private static bool[] player4Keys;

        // XBox keys
        private static Dictionary<Buttons, Boolean> XBoxButtons1;
        private static Dictionary<Buttons, Boolean> XBoxButtons2;
        private static Dictionary<Buttons, Boolean> XBoxButtons3;
        private static Dictionary<Buttons, Boolean> XBoxButtons4;

        #region GamePad Numbers
        // | operator can be used
        /*
            DPadUp = 1,
            DPadDown = 2,
            DPadLeft = 4,
            DPadRight = 8,
            Start = 16,
            Back = 32,
            LeftStick = 64,
            RightStick = 128,
            LeftShoulder = 256,
            RightShoulder = 512,
            BigButton = 2048,
            A = 4096,
            B = 8192,
            X = 16384,
            Y = 32768,
            LeftThumbstickLeft = 2097152,
            RightTrigger = 4194304,
            LeftTrigger = 8388608,
            RightThumbstickUp = 16777216,
            RightThumbstickDown = 33554432,
            RightThumbstickRight = 67108864,
            RightThumbstickLeft = 134217728,
            LeftThumbstickUp = 268435456,
            LeftThumbstickDown = 536870912,
            LeftThumbstickRight = 1073741824,
        */
        #endregion

        #region Mouse
        private static int mouseX;
        private static int mouseY;
        public static int getMouseX() { return mouseX; }
        public static int getMouseY() { return mouseY; }
        public static bool isMouseLeft() { return ((mouseX+MIN_XY) < CENTER_X); }
        public static bool isMouseRight() { return ((mouseX-MIN_XY) > CENTER_X); }
        public static bool isMouseUp() { return ((mouseY+MIN_XY) < CENTER_Y); }
        public static bool isMouseDown() { return ((mouseY-MIN_XY) > CENTER_Y); }
        public static void centerMouse() { /*Mouse.SetPosition(CENTER_X, CENTER_Y);*/ }
        public static void bindMouse() { /*Mouse.SetPosition((int)(MathHelper.Clamp(mouseX, CENTER_X-MAX_XY, CENTER_X+MAX_XY)), (int)(MathHelper.Clamp(mouseY, CENTER_Y-MAX_XY, CENTER_Y+MAX_XY)));*/ }

        private static bool leftClickReleased;
        public static bool isLeftClickDown() { return false; } // { return (Mouse.GetState().LeftButton == ButtonState.Pressed); }
        public static bool isLeftClickUp() { return false; } // { return (Mouse.GetState().LeftButton == ButtonState.Released); }
        public static bool isLeftClickPressed() { return false; } // { return (Mouse.GetState().LeftButton == ButtonState.Pressed && leftClickReleased); }

        private static bool rightClickReleased;
        public static bool isRightClickDown() { return false; } // { return (Mouse.GetState().RightButton == ButtonState.Pressed); }
        public static bool isRightClickUp() { return false; } // { return (Mouse.GetState().RightButton == ButtonState.Released); }
        public static bool isRightClickPressed() { return false; } // { return (Mouse.GetState().RightButton == ButtonState.Pressed && rightClickReleased); }
        #endregion

        #region Keyboard Keys
        private static bool escKeyReleased;
        public static bool isKeyEscDown() { return (Keyboard.GetState().IsKeyDown(Keys.Escape)); }
        public static bool isKeyEscUp() { return (Keyboard.GetState().IsKeyUp(Keys.Escape)); }
        public static bool isKeyEscPressed() { return (Keyboard.GetState().IsKeyDown(Keys.Escape) && escKeyReleased); }

        private static bool enterKeyReleased;
        public static bool isKeyEnterDown() { return (Keyboard.GetState().IsKeyDown(Keys.Enter)); }
        public static bool isKeyEnterUp() { return (Keyboard.GetState().IsKeyUp(Keys.Enter)); }
        public static bool isKeyEnterPressed() { return (Keyboard.GetState().IsKeyDown(Keys.Enter) && enterKeyReleased); }

        private static bool spaceKeyReleased;
        public static bool isKeySpaceDown() { return (Keyboard.GetState().IsKeyDown(Keys.Space)); }
        public static bool isKeySpaceUp() { return (Keyboard.GetState().IsKeyUp(Keys.Space)); }
        public static bool isKeySpacePressed() { return (Keyboard.GetState().IsKeyDown(Keys.Space) && spaceKeyReleased); }

        private static bool backspaceKeyReleased;
        public static bool isKeyBackspaceDown() { return (Keyboard.GetState().IsKeyDown(Keys.Back)); }
        public static bool isKeyBackspaceUp() { return (Keyboard.GetState().IsKeyUp(Keys.Back)); }
        public static bool isKeyBackspacePressed() { return (Keyboard.GetState().IsKeyDown(Keys.Back) && backspaceKeyReleased); }

        private static bool rightShiftKeyReleased;
        public static bool isKeyRightShiftDown() { return (Keyboard.GetState().IsKeyDown(Keys.RightShift)); }
        public static bool isKeyRightShiftUp() { return (Keyboard.GetState().IsKeyUp(Keys.RightShift)); }
        public static bool isKeyRightShiftPressed() { return (Keyboard.GetState().IsKeyDown(Keys.RightShift) && rightShiftKeyReleased); }

        private static bool leftShiftKeyReleased;
        public static bool isKeyLeftShiftDown() { return (Keyboard.GetState().IsKeyDown(Keys.LeftShift)); }
        public static bool isKeyLeftShiftUp() { return (Keyboard.GetState().IsKeyUp(Keys.LeftShift)); }
        public static bool isKeyLeftShiftPressed() { return (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && leftShiftKeyReleased); }

        private static bool leftCtrlKeyReleased;
        public static bool isKeyLeftCtrlDown() { return (Keyboard.GetState().IsKeyDown(Keys.LeftControl)); }
        public static bool isKeyLeftCtrlUp() { return (Keyboard.GetState().IsKeyUp(Keys.LeftControl)); }
        public static bool isKeyLeftCtrlPressed() { return (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && leftCtrlKeyReleased); }

        private static bool qKeyReleased;
        public static bool isKeyQDown() { return (Keyboard.GetState().IsKeyDown(Keys.Q)); }
        public static bool isKeyQUp() { return (Keyboard.GetState().IsKeyUp(Keys.Q)); }
        public static bool isKeyQPressed() { return (Keyboard.GetState().IsKeyDown(Keys.Q) && qKeyReleased); }

        private static bool eKeyReleased;
        public static bool isKeyEDown() { return (Keyboard.GetState().IsKeyDown(Keys.E)); }
        public static bool isKeyEUp() { return (Keyboard.GetState().IsKeyUp(Keys.E)); }
        public static bool isKeyEPressed() { return (Keyboard.GetState().IsKeyDown(Keys.E) && eKeyReleased); }

        private static bool zKeyReleased;
        public static bool isKeyZDown() { return (Keyboard.GetState().IsKeyDown(Keys.Z)); }
        public static bool isKeyZUp() { return (Keyboard.GetState().IsKeyUp(Keys.Z)); }
        public static bool isKeyZPressed() { return (Keyboard.GetState().IsKeyDown(Keys.Z) && zKeyReleased); }

        private static bool cKeyReleased;
        public static bool isKeyCDown() { return (Keyboard.GetState().IsKeyDown(Keys.C)); }
        public static bool isKeyCUp() { return (Keyboard.GetState().IsKeyUp(Keys.C)); }
        public static bool isKeyCPressed() { return (Keyboard.GetState().IsKeyDown(Keys.C) && cKeyReleased); }
        
        private static bool wKeyReleased;
        public static bool isKeyWDown() { return (Keyboard.GetState().IsKeyDown(Keys.W)); }
        public static bool isKeyWUp() { return (Keyboard.GetState().IsKeyUp(Keys.W)); }
        public static bool isKeyWPressed() { return (Keyboard.GetState().IsKeyDown(Keys.W) && wKeyReleased); }

        private static bool sKeyReleased;
        public static bool isKeySDown() { return (Keyboard.GetState().IsKeyDown(Keys.S)); }
        public static bool isKeySUp() { return (Keyboard.GetState().IsKeyUp(Keys.S)); }
        public static bool isKeySPressed() { return (Keyboard.GetState().IsKeyDown(Keys.S) && sKeyReleased); }

        private static bool aKeyReleased;
        public static bool isKeyADown() { return (Keyboard.GetState().IsKeyDown(Keys.A)); }
        public static bool isKeyAUp() { return (Keyboard.GetState().IsKeyUp(Keys.A)); }
        public static bool isKeyAPressed() { return (Keyboard.GetState().IsKeyDown(Keys.A) && aKeyReleased); }

        private static bool dKeyReleased;
        public static bool isKeyDDown() { return (Keyboard.GetState().IsKeyDown(Keys.D)); }
        public static bool isKeyDUp() { return (Keyboard.GetState().IsKeyUp(Keys.D)); }
        public static bool isKeyDPressed() { return (Keyboard.GetState().IsKeyDown(Keys.D) && dKeyReleased); }
        
        private static bool upKeyReleased;
        public static bool isKeyUpDown() { return (Keyboard.GetState().IsKeyDown(Keys.Up)); }
        public static bool isKeyUpUp() { return (Keyboard.GetState().IsKeyUp(Keys.Up)); }
        public static bool isKeyUpPressed() { return (Keyboard.GetState().IsKeyDown(Keys.Up) && upKeyReleased); }

        private static bool downKeyReleased;
        public static bool isKeyDownDown() { return (Keyboard.GetState().IsKeyDown(Keys.Down)); }
        public static bool isKeyDownUp() { return (Keyboard.GetState().IsKeyUp(Keys.Down)); }
        public static bool isKeyDownPressed() { return (Keyboard.GetState().IsKeyDown(Keys.Down) && downKeyReleased); }

        private static bool leftKeyReleased;
        public static bool isKeyLeftDown() { return (Keyboard.GetState().IsKeyDown(Keys.Left)); }
        public static bool isKeyLeftUp() { return (Keyboard.GetState().IsKeyUp(Keys.Left)); }
        public static bool isKeyLeftPressed() { return (Keyboard.GetState().IsKeyDown(Keys.Left) && leftKeyReleased); }

        private static bool rightKeyReleased;
        public static bool isKeyRightDown() { return (Keyboard.GetState().IsKeyDown(Keys.Right)); }
        public static bool isKeyRightUp() { return (Keyboard.GetState().IsKeyUp(Keys.Right)); }
        public static bool isKeyRightPressed() { return (Keyboard.GetState().IsKeyDown(Keys.Right) && rightKeyReleased); }
        
        private static bool oKeyReleased;
        public static bool isKeyODown() { return (Keyboard.GetState().IsKeyDown(Keys.O)); }
        public static bool isKeyOUp() { return (Keyboard.GetState().IsKeyUp(Keys.O)); }
        public static bool isKeyOPressed() { return (Keyboard.GetState().IsKeyDown(Keys.O) && oKeyReleased); }

        private static bool pKeyReleased;
        public static bool isKeyPDown() { return (Keyboard.GetState().IsKeyDown(Keys.P)); }
        public static bool isKeyPUp() { return (Keyboard.GetState().IsKeyUp(Keys.P)); }
        public static bool isKeyPPressed() { return (Keyboard.GetState().IsKeyDown(Keys.P) && pKeyReleased); }

        private static bool keypad8KeyReleased;
        public static bool isKeypad8Down() { return (Keyboard.GetState().IsKeyDown(Keys.NumPad8)); }
        public static bool isKeypad8Up() { return (Keyboard.GetState().IsKeyUp(Keys.NumPad8)); }
        public static bool isKeypad8Pressed() { return (Keyboard.GetState().IsKeyDown(Keys.NumPad8) && keypad8KeyReleased); }

        private static bool keypad2KeyReleased;
        public static bool isKeypad2Down() { return (Keyboard.GetState().IsKeyDown(Keys.NumPad2)); }
        public static bool isKeypad2Up() { return (Keyboard.GetState().IsKeyUp(Keys.NumPad2)); }
        public static bool isKeypad2Pressed() { return (Keyboard.GetState().IsKeyDown(Keys.NumPad2) && keypad2KeyReleased); }

        private static bool keypad4KeyReleased;
        public static bool isKeypad4Down() { return (Keyboard.GetState().IsKeyDown(Keys.NumPad4)); }
        public static bool isKeypad4Up() { return (Keyboard.GetState().IsKeyUp(Keys.NumPad4)); }
        public static bool isKeypad4Pressed() { return (Keyboard.GetState().IsKeyDown(Keys.NumPad4) && keypad4KeyReleased); }

        private static bool keypad6KeyReleased;
        public static bool isKeypad6Down() { return (Keyboard.GetState().IsKeyDown(Keys.NumPad6)); }
        public static bool isKeypad6Up() { return (Keyboard.GetState().IsKeyUp(Keys.NumPad6)); }
        public static bool isKeypad6Pressed() { return (Keyboard.GetState().IsKeyDown(Keys.NumPad6) && keypad6KeyReleased); }
        #endregion

        #region XBox Buttons
        public static bool isButtonDown(PlayerIndex number, Buttons button) { return (GamePad.GetState(number).IsButtonDown(button)); }
        public static bool isButtonUp(PlayerIndex number, Buttons button) { return (GamePad.GetState(number).IsButtonUp(button)); }
        public static bool isButtonPressed(PlayerIndex number, Buttons button)
        {
            bool value = false;
            switch (button)
            {
                case Buttons.A:
                    value = isAPressed(number);
                    break;
                case Buttons.B:
                    value = isBPressed(number);
                    break;
                case Buttons.X:
                    value = isXPressed(number);
                    break;
                case Buttons.Y:
                    value = isYPressed(number);
                    break;
                case Buttons.BigButton:
                    value = isBigButtonPressed(number);
                    break;
                case Buttons.Start:
                    value = isStartPressed(number);
                    break;
                case Buttons.Back:
                    value = isBackPressed(number);
                    break;
            }
            return value;
        }

        private static bool[] aReleased;
        public static bool isADown(PlayerIndex number) { return (GamePad.GetState(number).Buttons.A == ButtonState.Pressed); }
        public static bool isAUp(PlayerIndex number) { return (GamePad.GetState(number).Buttons.A == ButtonState.Released); }
        public static bool isAPressed(PlayerIndex number) { return ((GamePad.GetState(number).Buttons.A == ButtonState.Pressed) && aReleased[(int)number]); }

        private static bool[] bReleased;
        public static bool isBDown(PlayerIndex number) { return (GamePad.GetState(number).Buttons.B == ButtonState.Pressed); }
        public static bool isBUp(PlayerIndex number) { return (GamePad.GetState(number).Buttons.B == ButtonState.Released); }
        public static bool isBPressed(PlayerIndex number) { return ((GamePad.GetState(number).Buttons.B == ButtonState.Pressed) && bReleased[(int)number]); }

        private static bool[] xReleased;
        public static bool isXDown(PlayerIndex number) { return (GamePad.GetState(number).Buttons.X == ButtonState.Pressed); }
        public static bool isXUp(PlayerIndex number) { return (GamePad.GetState(number).Buttons.X == ButtonState.Released); }
        public static bool isXPressed(PlayerIndex number) { return ((GamePad.GetState(number).Buttons.X == ButtonState.Pressed) && xReleased[(int)number]); }

        private static bool[] yReleased;
        public static bool isYDown(PlayerIndex number) { return (GamePad.GetState(number).Buttons.Y == ButtonState.Pressed); }
        public static bool isYUp(PlayerIndex number) { return (GamePad.GetState(number).Buttons.Y == ButtonState.Released); }
        public static bool isYPressed(PlayerIndex number) { return ((GamePad.GetState(number).Buttons.Y == ButtonState.Pressed) && yReleased[(int)number]); }

        private static bool[] lbReleased; // left bumper
        public static bool isLBDown(PlayerIndex number) { return (GamePad.GetState(number).Buttons.LeftShoulder == ButtonState.Pressed); }
        public static bool isLBUp(PlayerIndex number) { return (GamePad.GetState(number).Buttons.LeftShoulder == ButtonState.Released); }
        public static bool isLBPressed(PlayerIndex number) { return ((GamePad.GetState(number).Buttons.LeftShoulder == ButtonState.Pressed) && lbReleased[(int)number]); }
        
        private static bool[] rbReleased; // right bumper
        public static bool isRBDown(PlayerIndex number) { return (GamePad.GetState(number).Buttons.RightShoulder == ButtonState.Pressed); }
        public static bool isRBUp(PlayerIndex number) { return (GamePad.GetState(number).Buttons.RightShoulder == ButtonState.Released); }
        public static bool isRBPressed(PlayerIndex number) { return ((GamePad.GetState(number).Buttons.RightShoulder == ButtonState.Pressed) && rbReleased[(int)number]); }
        
        private static bool[] ltReleased; // left trigger
        public static bool isLTDown(PlayerIndex number) { return (GamePad.GetState(number).Triggers.Left > 0); }
        public static bool isLTUp(PlayerIndex number) { return (GamePad.GetState(number).Triggers.Left == 0); }
        public static bool isLTPressed(PlayerIndex number) { return ((GamePad.GetState(number).Triggers.Left > 0) && ltReleased[(int)number]); }

        private static bool[] rtReleased; // right trigger
        public static bool isRTDown(PlayerIndex number) { return (GamePad.GetState(number).Triggers.Right > 0); }
        public static bool isRTUp(PlayerIndex number) { return (GamePad.GetState(number).Triggers.Right == 0); }
        public static bool isRTPressed(PlayerIndex number) { return ((GamePad.GetState(number).Triggers.Right > 0) && rtReleased[(int)number]); }

        private static bool[] bigButtonReleased;
        public static bool isBigButtonDown(PlayerIndex number) { return (GamePad.GetState(number).Buttons.BigButton == ButtonState.Pressed); }
        public static bool isBigButtonUp(PlayerIndex number) { return (GamePad.GetState(number).Buttons.BigButton == ButtonState.Released); }
        public static bool isBigButtonPressed(PlayerIndex number) { return ((GamePad.GetState(number).Buttons.BigButton == ButtonState.Pressed) && bigButtonReleased[(int)number]); }

        private static bool[] startReleased;
        public static bool isStartDown(PlayerIndex number) { return (GamePad.GetState(number).Buttons.Start == ButtonState.Pressed); }
        public static bool isStartUp(PlayerIndex number) { return (GamePad.GetState(number).Buttons.Start == ButtonState.Released); }
        public static bool isStartPressed(PlayerIndex number) { return ((GamePad.GetState(number).Buttons.Start == ButtonState.Pressed) && startReleased[(int)number]); }

        private static bool[] backReleased;
        public static bool isBackDown(PlayerIndex number) { return (GamePad.GetState(number).Buttons.Back == ButtonState.Pressed); }
        public static bool isBackUp(PlayerIndex number) { return (GamePad.GetState(number).Buttons.Back == ButtonState.Released); }
        public static bool isBackPressed(PlayerIndex number) { return ((GamePad.GetState(number).Buttons.Back == ButtonState.Pressed) && backReleased[(int)number]); }

        private static bool[] leftStickReleased;
        public static bool isLeftStickDown(PlayerIndex number) { return (GamePad.GetState(number).Buttons.LeftStick == ButtonState.Pressed); }
        public static bool isLeftStickUp(PlayerIndex number) { return (GamePad.GetState(number).Buttons.LeftStick == ButtonState.Released); }
        public static bool isLeftStickPressed(PlayerIndex number) { return ((GamePad.GetState(number).Buttons.LeftStick == ButtonState.Pressed) && leftStickReleased[(int)number]); }

        private static bool[] rightStickReleased;
        public static bool isRightStickDown(PlayerIndex number) { return (GamePad.GetState(number).Buttons.RightStick == ButtonState.Pressed); }
        public static bool isRightStickUp(PlayerIndex number) { return (GamePad.GetState(number).Buttons.RightStick == ButtonState.Released); }
        public static bool isRightStickPressed(PlayerIndex number) { return ((GamePad.GetState(number).Buttons.RightStick == ButtonState.Pressed) && rightStickReleased[(int)number]); }

        private static bool[] leftStickXVectorZeroedOut;
        private static bool[] leftStickYVectorZeroedOut;
        public static float leftStickXVector(PlayerIndex number) { return (GamePad.GetState(number).ThumbSticks.Left.X); }
        public static float leftStickYVector(PlayerIndex number) { return (GamePad.GetState(number).ThumbSticks.Left.Y); }
        public static bool isLeftStickXVectorZeroedOut(PlayerIndex number) { return leftStickXVectorZeroedOut[(int)number]; }
        public static bool isLeftStickYVectorZeroedOut(PlayerIndex number) { return leftStickYVectorZeroedOut[(int)number]; }

        private static bool[] rightStickXVectorZeroedOut;
        private static bool[] rightStickYVectorZeroedOut;
        public static float rightStickXVector(PlayerIndex number) { return (GamePad.GetState(number).ThumbSticks.Right.X); }
        public static float rightStickYVector(PlayerIndex number) { return (GamePad.GetState(number).ThumbSticks.Right.Y); }
        public static bool isRightStickXVectorZeroedOut(PlayerIndex number) { return rightStickXVectorZeroedOut[(int)number]; }
        public static bool isRightStickYVectorZeroedOut(PlayerIndex number) { return rightStickYVectorZeroedOut[(int)number]; }

        private static bool[] dPadUpReleased;
        public static bool isDPadUpDown(PlayerIndex number) { return (GamePad.GetState(number).DPad.Up == ButtonState.Pressed); }
        public static bool isDPadUpUp(PlayerIndex number) { return (GamePad.GetState(number).DPad.Up == ButtonState.Released); }
        public static bool isDPadUpPressed(PlayerIndex number) { return ((GamePad.GetState(number).DPad.Up == ButtonState.Pressed) && dPadUpReleased[(int)number]); }

        private static bool[] dPadDownReleased;
        public static bool isDPadDownDown(PlayerIndex number) { return (GamePad.GetState(number).DPad.Down == ButtonState.Pressed); }
        public static bool isDPadDownUp(PlayerIndex number) { return (GamePad.GetState(number).DPad.Down == ButtonState.Released); }
        public static bool isDPadDownPressed(PlayerIndex number) { return ((GamePad.GetState(number).DPad.Down == ButtonState.Pressed) && dPadDownReleased[(int)number]); }

        private static bool[] dPadLeftReleased;
        public static bool isDPadLeftDown(PlayerIndex number) { return (GamePad.GetState(number).DPad.Left == ButtonState.Pressed); }
        public static bool isDPadLeftUp(PlayerIndex number) { return (GamePad.GetState(number).DPad.Left == ButtonState.Released); }
        public static bool isDPadLeftPressed(PlayerIndex number) { return ((GamePad.GetState(number).DPad.Left == ButtonState.Pressed) && dPadLeftReleased[(int)number]); }

        private static bool[] dPadRightReleased;
        public static bool isDPadRightDown(PlayerIndex number) { return (GamePad.GetState(number).DPad.Right == ButtonState.Pressed); }
        public static bool isDPadRightUp(PlayerIndex number) { return (GamePad.GetState(number).DPad.Right == ButtonState.Released); }
        public static bool isDPadRightPressed(PlayerIndex number) { return ((GamePad.GetState(number).DPad.Right == ButtonState.Pressed) && dPadRightReleased[(int)number]); }
        #endregion

        #region Combined Buttons

        #region Directional

        #region Combined Stick

        #region Left Combined Stick

        public static bool isCombinedLeftCombinedStickDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (isCombinedLeftLeftStickDown(index) || isCombinedLeftRightStickDown(index))
            {
                down = true;
            }
            else
            {
                
            }
            return down;
        }

        public static bool isCombinedLeftCombinedStickUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (isCombinedLeftLeftStickUp(index) && isCombinedLeftRightStickUp(index))
            {
                up = true;
            }
            else
            {

            }
            return up;
        }

        public static bool isCombinedLeftCombinedStickPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (isCombinedLeftLeftStickPressed(index) || isCombinedLeftRightStickPressed(index))
            {
                pressed = true;
            }
            else
            {
                
            }
            return pressed;
        }

        #endregion

        #region Right Combined Stick

        public static bool isCombinedRightCombinedStickDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (isCombinedRightLeftStickDown(index) || isCombinedRightRightStickDown(index))
            {
                down = true;
            }
            else
            {

            }
            return down;
        }

        public static bool isCombinedRightCombinedStickUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (isCombinedRightLeftStickUp(index) && isCombinedRightRightStickUp(index))
            {
                up = true;
            }
            else
            {

            }
            return up;
        }

        public static bool isCombinedRightCombinedStickPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (isCombinedRightLeftStickPressed(index) || isCombinedRightRightStickPressed(index))
            {
                pressed = true;
            }
            else
            {

            }
            return pressed;
        }

        #endregion

        #region Up Combined Stick

        public static bool isCombinedUpCombinedStickDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (isCombinedUpLeftStickDown(index) || isCombinedUpRightStickDown(index))
            {
                down = true;
            }
            else
            {

            }
            return down;
        }

        public static bool isCombinedUpCombinedStickUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (isCombinedUpLeftStickUp(index) && isCombinedUpRightStickUp(index))
            {
                up = true;
            }
            else
            {

            }
            return up;
        }

        public static bool isCombinedUpCombinedStickPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (isCombinedUpLeftStickPressed(index) || isCombinedUpRightStickPressed(index))
            {
                pressed = true;
            }
            else
            {

            }
            return pressed;
        }

        #endregion

        #region Down Combined Stick

        public static bool isCombinedDownCombinedStickDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (isCombinedDownLeftStickDown(index) || isCombinedDownRightStickDown(index))
            {
                down = true;
            }
            else
            {

            }
            return down;
        }

        public static bool isCombinedDownCombinedStickUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (isCombinedDownLeftStickUp(index) && isCombinedDownRightStickUp(index))
            {
                up = true;
            }
            else
            {

            }
            return up;
        }

        public static bool isCombinedDownCombinedStickPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (isCombinedDownLeftStickPressed(index) || isCombinedDownRightStickPressed(index))
            {
                pressed = true;
            }
            else
            {

            }
            return pressed;
        }

        #endregion

        #endregion

        #region Left Stick

        #region Left Left Stick

        public static bool isCombinedLeftLeftStickDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (leftStickXVector((PlayerIndex)index) < 0);
            }
            else
            {
                down = (isKeyADown());
            }
            return down;
        }

        public static bool isCombinedLeftLeftStickUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isLeftStickXVectorZeroedOut((PlayerIndex)index));
            }
            else
            {
                up = (isKeyAUp());
            }
            return up;
        }

        public static bool isCombinedLeftLeftStickPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isLeftStickXVectorZeroedOut((PlayerIndex)index) && leftStickXVector((PlayerIndex)index) < 0);
            }
            else
            {
                pressed = (isKeyAPressed());
            }
            return pressed;
        }

        #endregion

        #region Right Left Stick

        public static bool isCombinedRightLeftStickDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (leftStickXVector((PlayerIndex)index) > 0);
            }
            else
            {
                down = (isKeyDDown());
            }
            return down;
        }

        public static bool isCombinedRightLeftStickUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isLeftStickXVectorZeroedOut((PlayerIndex)index));
            }
            else
            {
                up = (isKeyDUp());
            }
            return up;
        }

        public static bool isCombinedRightLeftStickPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isLeftStickXVectorZeroedOut((PlayerIndex)index) && leftStickXVector((PlayerIndex)index) > 0);
            }
            else
            {
                pressed = (isKeyDPressed());
            }
            return pressed;
        }

        #endregion

        #region Up Left Stick

        public static bool isCombinedUpLeftStickDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (leftStickYVector((PlayerIndex)index) > 0);
            }
            else
            {
                down = (isKeyWDown()/* || isMouseUp()*/);
            }
            return down;
        }

        public static bool isCombinedUpLeftStickUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isLeftStickYVectorZeroedOut((PlayerIndex)index));
            }
            else
            {
                up = (isKeyWUp()/* && !isMouseUp()*/);
            }
            return up;
        }

        public static bool isCombinedUpLeftStickPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isLeftStickYVectorZeroedOut((PlayerIndex)index) && leftStickYVector((PlayerIndex)index) > 0);
            }
            else
            {
                pressed = (isKeyWPressed()/* || isMouseUp()*/);
            }
            return pressed;
        }

        #endregion

        #region Down Left Stick

        public static bool isCombinedDownLeftStickDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (leftStickYVector((PlayerIndex)index) < 0);
            }
            else
            {
                down = (isKeySDown()/* || isMouseDown()*/);
            }
            return down;
        }

        public static bool isCombinedDownLeftStickUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isLeftStickYVectorZeroedOut((PlayerIndex)index));
            }
            else
            {
                up = (isKeySUp()/* && !isMouseDown()*/);
            }
            return up;
        }

        public static bool isCombinedDownLeftStickPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isLeftStickYVectorZeroedOut((PlayerIndex)index) && leftStickYVector((PlayerIndex)index) < 0);
            }
            else
            {
                pressed = (isKeySPressed()/* || isMouseDown()*/);
            }
            return pressed;
        }

        #endregion

        #region Degree

        public static float LeftStickXDegree(Player.ControllerIndex index)
        {
            float degree = 0;
            if (index != Player.ControllerIndex.Keyboard)
            {
                degree = GamePad.GetState((PlayerIndex)index).ThumbSticks.Left.X;
            }
            else
            {
                if (isKeyADown() && !isKeyDDown())
                {
                    degree = -KEYBOARD_DEGREE;
                }
                else if (isKeyDDown() && !isKeyADown())
                {
                    degree = KEYBOARD_DEGREE;
                }
                else { }
            }
            return degree;
        }

        public static float LeftStickYDegree(Player.ControllerIndex index)
        {
            float degree = 0;
            if (index != Player.ControllerIndex.Keyboard)
            {
                degree = GamePad.GetState((PlayerIndex)index).ThumbSticks.Left.Y;
            }
            else
            {
                if (isKeySDown() && !isKeyWDown())
                {
                    degree = -KEYBOARD_DEGREE;
                }
                else if (isKeyWDown() && !isKeySDown())
                {
                    degree = KEYBOARD_DEGREE;
                }
                else { }
            }
            return degree;
        }

        #endregion

        #endregion

        #region Right Stick

        #region Left Right Stick

        public static bool isCombinedLeftRightStickDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (rightStickXVector((PlayerIndex)index) < 0);
            }
            else
            {
                down = (isKeyLeftDown()/* || isMouseLeft()*/);
            }
            return down;
        }

        public static bool isCombinedLeftRightStickUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isRightStickXVectorZeroedOut((PlayerIndex)index));
            }
            else
            {
                up = (isKeyLeftUp()/* && !isMouseLeft()*/);
            }
            return up;
        }

        public static bool isCombinedLeftRightStickPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isRightStickXVectorZeroedOut((PlayerIndex)index) && rightStickXVector((PlayerIndex)index) < 0);
            }
            else
            {
                pressed = (isKeyLeftPressed()/* || isMouseLeft()*/);
            }
            return pressed;
        }

        #endregion

        #region Right Right Stick

        public static bool isCombinedRightRightStickDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (rightStickXVector((PlayerIndex)index) > 0);
            }
            else
            {
                down = (isKeyRightDown()/* || isMouseRight()*/);
            }
            return down;
        }

        public static bool isCombinedRightRightStickUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isRightStickXVectorZeroedOut((PlayerIndex)index));
            }
            else
            {
                up = (isKeyRightUp()/* && !isMouseRight()*/);
            }
            return up;
        }

        public static bool isCombinedRightRightStickPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isRightStickXVectorZeroedOut((PlayerIndex)index) && rightStickXVector((PlayerIndex)index) > 0);
            }
            else
            {
                pressed = (isKeyRightPressed()/* || isMouseRight()*/);
            }
            return pressed;
        }

        #endregion

        #region Up Right Stick

        public static bool isCombinedUpRightStickDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (rightStickYVector((PlayerIndex)index) > 0);
            }
            else
            {
                down = (isKeyUpDown());
            }
            return down;
        }

        public static bool isCombinedUpRightStickUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isRightStickYVectorZeroedOut((PlayerIndex)index));
            }
            else
            {
                up = (isKeyUpUp());
            }
            return up;
        }

        public static bool isCombinedUpRightStickPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isRightStickYVectorZeroedOut((PlayerIndex)index) && rightStickYVector((PlayerIndex)index) > 0);
            }
            else
            {
                pressed = (isKeyUpPressed());
            }
            return pressed;
        }

        #endregion

        #region Down Right Stick

        public static bool isCombinedDownRightStickDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (rightStickYVector((PlayerIndex)index) < 0);
            }
            else
            {
                down = (isKeyDownDown());
            }
            return down;
        }

        public static bool isCombinedDownRightStickUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isRightStickYVectorZeroedOut((PlayerIndex)index));
            }
            else
            {
                up = (isKeyDownUp());
            }
            return up;
        }

        public static bool isCombinedDownRightStickPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isRightStickYVectorZeroedOut((PlayerIndex)index) && rightStickYVector((PlayerIndex)index) < 0);
            }
            else
            {
                pressed = (isKeyDownPressed());
            }
            return pressed;
        }

        #endregion

        #region Degree

        public static float RightStickXDegree(Player.ControllerIndex index)
        {
            float degree = 0;
            if (index != Player.ControllerIndex.Keyboard)
            {
                if (isLBDown((PlayerIndex)index) && !isRBDown((PlayerIndex)index))
                {
                    degree = -KEYBOARD_DEGREE;
                }
                else if (isRBDown((PlayerIndex)index) && !isLBDown((PlayerIndex)index))
                {
                    degree = KEYBOARD_DEGREE;
                }
                else
                {
                    degree = GamePad.GetState((PlayerIndex)index).ThumbSticks.Right.X;
                }
            }
            else
            {
                if ((isKeyLeftDown() || isKeyQDown()) && !(isKeyRightDown() || isKeyEDown()))
                {
                    degree = -KEYBOARD_DEGREE;
                }
                else if ((isKeyRightDown() || isKeyEDown()) && !(isKeyLeftDown() || isKeyQDown()))
                {
                    degree = KEYBOARD_DEGREE;
                }
                else { }
            }
            return degree;
        }

        public static float RightStickYDegree(Player.ControllerIndex index)
        {
            float degree = 0;
            if (index != Player.ControllerIndex.Keyboard)
            {
                degree = GamePad.GetState((PlayerIndex)index).ThumbSticks.Right.Y;
            }
            else
            {
                if (isKeyDownDown() && !isKeyUpDown())
                {
                    degree = -KEYBOARD_DEGREE;
                }
                else if (isKeyUpDown() && !isKeyDownDown())
                {
                    degree = KEYBOARD_DEGREE;
                }
                else { }
            }
            return degree;
        }

        #endregion

        #endregion

        #endregion

        #region Buttons

        #region Super Confirm

        public static bool isCombinedSuperConfirmDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (isStartDown((PlayerIndex)index));
            }
            else
            {
                down = (isKeyEnterDown());
            }
            return down;
        }

        public static bool isCombinedSuperConfirmUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isStartUp((PlayerIndex)index));
            }
            else
            {
                up = (isKeyEnterUp());
            }
            return up;
        }

        public static bool isCombinedSuperConfirmPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isStartPressed((PlayerIndex)index));
            }
            else
            {
                pressed = (isKeyEnterPressed());
            }
            return pressed;
        }

        #endregion

        #region Confirm

        public static bool isCombinedConfirmDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (isADown((PlayerIndex)index));
            }
            else
            {
                down = (isKeyEnterDown());
            }
            return down;
        }

        public static bool isCombinedConfirmUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isAUp((PlayerIndex)index));
            }
            else
            {
                up = (isKeyEnterUp());
            }
            return up;
        }

        public static bool isCombinedConfirmPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isAPressed((PlayerIndex)index));
            }
            else
            {
                pressed = (isKeyEnterPressed());
            }
            return pressed;
        }

        #endregion

        #region Unconfirm

        public static bool isCombinedUnconfirmDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (isBDown((PlayerIndex)index));
            }
            else
            {
                down = (isKeyBackspaceDown());
            }
            return down;
        }

        public static bool isCombinedUnconfirmUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isBUp((PlayerIndex)index));
            }
            else
            {
                up = (isKeyBackspaceUp());
            }
            return up;
        }

        public static bool isCombinedUnconfirmPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isBPressed((PlayerIndex)index));
            }
            else
            {
                pressed = (isKeyBackspacePressed());
            }
            return pressed;
        }

        #endregion

        #region Primary Fire

        public static bool isCombinedPrimaryFireDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (isADown((PlayerIndex)index));
            }
            else
            {
                down = (isKeySpaceDown()/* || isLeftClickDown()*/);
            }
            return down;
        }

        public static bool isCombinedPrimaryFireUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isAUp((PlayerIndex)index));
            }
            else
            {
                up = (isKeySpaceUp()/* && isLeftClickUp()*/);
            }
            return up;
        }

        public static bool isCombinedPrimaryFirePressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isAPressed((PlayerIndex)index));
            }
            else
            {
                pressed = (isKeySpacePressed()/* || isLeftClickPressed()*/);
            }
            return pressed;
        }

        #endregion

        #region Secondary Fire

        // Secondary
        public static bool isCombinedSecondaryFireDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (isBDown((PlayerIndex)index));
            }
            else
            {
                down = (isKeyLeftCtrlDown()/* || isRightClickDown()*/);
            }
            return down;
        }

        public static bool isCombinedSecondaryFireUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isBUp((PlayerIndex)index));
            }
            else
            {
                up = (isKeyLeftCtrlUp()/* && isRightClickUp()*/);
            }
            return up;
        }

        public static bool isCombinedSecondaryFirePressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isBPressed((PlayerIndex)index));
            }
            else
            {
                pressed = (isKeyLeftCtrlPressed()/* || isRightClickPressed()*/);
            }
            return pressed;
        }

        #endregion

        #region Boost

        public static bool isCombinedBoostDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (isRTDown((PlayerIndex)index));
            }
            else
            {
                down = (isKeyCDown());
            }
            return down;
        }

        public static bool isCombinedBoostUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isRTUp((PlayerIndex)index));
            }
            else
            {
                up = (isKeyCUp());
            }
            return up;
        }

        public static bool isCombinedBoostPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isRTPressed((PlayerIndex)index));
            }
            else
            {
                pressed = (isKeyCPressed());
            }
            return pressed;
        }

        #endregion

        #region Brake

        public static bool isCombinedBrakeDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (isLTDown((PlayerIndex)index));
            }
            else
            {
                down = (isKeyZDown());
            }
            return down;
        }

        public static bool isCombinedBrakeUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isLTUp((PlayerIndex)index));
            }
            else
            {
                up = (isKeyZUp());
            }
            return up;
        }

        public static bool isCombinedBrakePressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isLTPressed((PlayerIndex)index));
            }
            else
            {
                pressed = (isKeyZPressed());
            }
            return pressed;
        }

        #endregion

        #region Bumpers

        #region Left Bumper

        public static bool isCombinedLeftBumperDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (isLBDown((PlayerIndex)index));
            }
            else
            {
                down = (isKeyQDown());
            }
            return down;
        }

        public static bool isCombinedLeftBumperUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isLBUp((PlayerIndex)index));
            }
            else
            {
                up = (isKeyQUp());
            }
            return up;
        }

        public static bool isCombinedLeftBumperPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isLBPressed((PlayerIndex)index));
            }
            else
            {
                pressed = (isKeyQPressed());
            }
            return pressed;
        }

        #endregion

        #region Right Bumper

        public static bool isCombinedRightBumperDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (isRBDown((PlayerIndex)index));
            }
            else
            {
                down = (isKeyEDown());
            }
            return down;
        }

        public static bool isCombinedRightBumperUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isRBUp((PlayerIndex)index));
            }
            else
            {
                up = (isKeyEUp());
            }
            return up;
        }

        public static bool isCombinedRightBumperPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isRBPressed((PlayerIndex)index));
            }
            else
            {
                pressed = (isKeyEPressed());
            }
            return pressed;
        }

        #endregion

        #endregion

        #region Stick Click

        #region Left Stick

        public static bool isCombinedLeftStickDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (isLeftStickDown((PlayerIndex)index));
            }
            else
            {
                down = (isKeyODown());
            }
            return down;
        }

        public static bool isCombinedLeftStickUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isLeftStickUp((PlayerIndex)index));
            }
            else
            {
                up = (isKeyOUp());
            }
            return up;
        }

        public static bool isCombinedLeftStickPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isLeftStickPressed((PlayerIndex)index));
            }
            else
            {
                pressed = (isKeyOPressed());
            }
            return pressed;
        }

        #endregion

        #region Right Stick

        public static bool isCombinedRightStickDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (isRightStickDown((PlayerIndex)index));
            }
            else
            {
                down = (isKeyPDown());
            }
            return down;
        }

        public static bool isCombinedRightStickUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isRightStickUp((PlayerIndex)index));
            }
            else
            {
                up = (isKeyPUp());
            }
            return up;
        }

        public static bool isCombinedRightStickPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isRightStickPressed((PlayerIndex)index));
            }
            else
            {
                pressed = (isKeyPPressed());
            }
            return pressed;
        }

        #endregion

        #endregion

        #region Toggle Objectives

        public static bool isCombinedToggleObjectivesDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (isYDown((PlayerIndex)index));
            }
            else
            {
                down = (isKeyODown());
            }
            return down;
        }

        public static bool isCombinedToggleObjectivesUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isYUp((PlayerIndex)index));
            }
            else
            {
                up = (isKeyOUp());
            }
            return up;
        }

        public static bool isCombinedToggleObjectivesPressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isYPressed((PlayerIndex)index));
            }
            else
            {
                pressed = (isKeyOPressed());
            }
            return pressed;
        }

        #endregion

        #region Pause

        public static bool isCombinedPauseDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (isStartDown((PlayerIndex)index));
            }
            else
            {
                down = (isKeyEscDown());
            }
            return down;
        }

        public static bool isCombinedPauseUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isStartUp((PlayerIndex)index));
            }
            else
            {
                up = (isKeyEscUp());
            }
            return up;
        }

        public static bool isCombinedPausePressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isStartPressed((PlayerIndex)index));
            }
            else
            {
                pressed = (isKeyEscPressed());
            }
            return pressed;
        }

        #endregion

        #region Escape

        public static bool isCombinedEscapeDown(Player.ControllerIndex index)
        {
            bool down = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                down = (isBackDown((PlayerIndex)index));
            }
            else
            {
                down = (isKeyEscDown());
            }
            return down;
        }

        public static bool isCombinedEscapeUp(Player.ControllerIndex index)
        {
            bool up = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                up = (isBackUp((PlayerIndex)index));
            }
            else
            {
                up = (isKeyEscUp());
            }
            return up;
        }

        public static bool isCombinedEscapePressed(Player.ControllerIndex index)
        {
            bool pressed = false;
            if (index != Player.ControllerIndex.Keyboard)
            {
                pressed = (isBackPressed((PlayerIndex)index));
            }
            else
            {
                pressed = (isKeyEscPressed());
            }
            return pressed;
        }

        #endregion

        #endregion

        #endregion

        #region Special Buttons
        public static bool isConfirmationKeyPressed()
        {
            return (isKeyEnterPressed()
                || isAPressed(PlayerIndex.One)
                || isAPressed(PlayerIndex.Two)
                || isAPressed(PlayerIndex.Three)
                || isAPressed(PlayerIndex.Four));
        }

        public static bool isUnconfirmationKeyPressed()
        {
            return (isKeyBackspacePressed()
                || isBPressed(PlayerIndex.One)
                || isBPressed(PlayerIndex.Two)
                || isBPressed(PlayerIndex.Three)
                || isBPressed(PlayerIndex.Four));
        }

        public static bool isSuperConfirmationKeyPressed()
        {
            return (isKeyEnterPressed()
                || (isStartPressed(PlayerIndex.One)
                || isStartPressed(PlayerIndex.Two)
                || isStartPressed(PlayerIndex.Three)
                || isStartPressed(PlayerIndex.Four)));
        }

        public static bool isQuitKeyPressed()
        {
            return (isKeyEscPressed()
                || isBackPressed(PlayerIndex.One)
                || isBackPressed(PlayerIndex.Two)
                || isBackPressed(PlayerIndex.Three)
                || isBackPressed(PlayerIndex.Four));
        }

        public static bool isDebugUpPressed()
        {
            return (isKeypad8Pressed()
                || isDPadUpPressed(PlayerIndex.One)
                || isDPadUpPressed(PlayerIndex.Two)
                || isDPadUpPressed(PlayerIndex.Three)
                || isDPadUpPressed(PlayerIndex.Four));
        }

        public static bool isDebugDownPressed()
        {
            return (isKeypad2Pressed()
                || isDPadDownPressed(PlayerIndex.One)
                || isDPadDownPressed(PlayerIndex.Two)
                || isDPadDownPressed(PlayerIndex.Three)
                || isDPadDownPressed(PlayerIndex.Four));
        }

        public static bool isDebugLeftPressed()
        {
            return (isKeypad4Pressed()
                || isDPadLeftPressed(PlayerIndex.One)
                || isDPadLeftPressed(PlayerIndex.Two)
                || isDPadLeftPressed(PlayerIndex.Three)
                || isDPadLeftPressed(PlayerIndex.Four));
        }

        public static bool isDebugRightPressed()
        {
            return (isKeypad6Pressed()
                || isDPadRightPressed(PlayerIndex.One)
                || isDPadRightPressed(PlayerIndex.Two)
                || isDPadRightPressed(PlayerIndex.Three)
                || isDPadRightPressed(PlayerIndex.Four));
        }
        #endregion

        public static void Initialize()
        {
            qKeyReleased = false;
            eKeyReleased = false;
            oKeyReleased = false;
            pKeyReleased = false;
            zKeyReleased = false;
            cKeyReleased = false;
            wKeyReleased = false;
            sKeyReleased = false;
            dKeyReleased = false;
            aKeyReleased = false;
            upKeyReleased = false;
            downKeyReleased = false;
            leftKeyReleased = false;
            rightKeyReleased = false;
            enterKeyReleased = false;
            escKeyReleased = false;
            backspaceKeyReleased = false;
            rightShiftKeyReleased = false;
            leftShiftKeyReleased = false;
            leftCtrlKeyReleased = false;
            spaceKeyReleased = false;
            keypad2KeyReleased = false;
            keypad4KeyReleased = false;
            keypad6KeyReleased = false;
            keypad8KeyReleased = false;
            aReleased = new bool[] { false, false, false, false };
            bReleased = new bool[] { false, false, false, false };
            xReleased = new bool[] { false, false, false, false };
            yReleased = new bool[] { false, false, false, false };
            lbReleased = new bool[] { false, false, false, false };
            rbReleased = new bool[] { false, false, false, false };
            ltReleased = new bool[] { false, false, false, false };
            rtReleased = new bool[] { false, false, false, false };
            bigButtonReleased = new bool[] { false, false, false, false };
            startReleased = new bool[] { false, false, false, false };
            backReleased = new bool[] { false, false, false, false };
            leftStickReleased = new bool[] { false, false, false, false };
            rightStickReleased = new bool[] { false, false, false, false };
            dPadUpReleased = new bool[] { false, false, false, false };
            dPadDownReleased = new bool[] { false, false, false, false };
            dPadLeftReleased = new bool[] { false, false, false, false };
            dPadRightReleased = new bool[] { false, false, false, false };
            leftStickXVectorZeroedOut = new bool[] { false, false, false, false };
            leftStickYVectorZeroedOut = new bool[] { false, false, false, false };
            rightStickXVectorZeroedOut = new bool[] { false, false, false, false };
            rightStickYVectorZeroedOut = new bool[] { false, false, false, false };

            XBoxButtons1 = new Dictionary<Buttons, bool>();
            foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
            {
                XBoxButtons1.Add(button, false);
            }

            XBoxButtons2 = new Dictionary<Buttons, bool>();
            foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
            {
                XBoxButtons2.Add(button, false);
            }

            XBoxButtons3 = new Dictionary<Buttons, bool>();
            foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
            {
                XBoxButtons3.Add(button, false);
            }

            XBoxButtons4 = new Dictionary<Buttons, bool>();
            foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
            {
                XBoxButtons4.Add(button, false);
            }

#if WINDOWS
            player1Keys = new bool[sizeof(Keys)];
            for (int i = 0; i < player1Keys.Length; i++) { player1Keys[i] = false; }
            player2Keys = new bool[sizeof(Keys)];
            for (int i = 0; i < player2Keys.Length; i++) { player2Keys[i] = false; }
            player3Keys = new bool[sizeof(Keys)];
            for (int i = 0; i < player3Keys.Length; i++) { player3Keys[i] = false; }
            player4Keys = new bool[sizeof(Keys)];
            for (int i = 0; i < player4Keys.Length; i++) { player4Keys[i] = false; }
            mouseX = CENTER_X;
            mouseY = CENTER_Y;
            leftClickReleased = false;
            rightClickReleased = false;
#endif
        }
        
        public static void FlushInput()
        {
            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                aReleased[i] = (GamePad.GetState((PlayerIndex)i).Buttons.A == ButtonState.Released);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                bReleased[i] = (GamePad.GetState((PlayerIndex)i).Buttons.B == ButtonState.Released);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                xReleased[i] = (GamePad.GetState((PlayerIndex)i).Buttons.X == ButtonState.Released);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                yReleased[i] = (GamePad.GetState((PlayerIndex)i).Buttons.Y == ButtonState.Released);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                lbReleased[i] = (GamePad.GetState((PlayerIndex)i).Buttons.LeftShoulder == ButtonState.Released);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                rbReleased[i] = (GamePad.GetState((PlayerIndex)i).Buttons.RightShoulder == ButtonState.Released);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                ltReleased[i] = (GamePad.GetState((PlayerIndex)i).Triggers.Left == 0);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                rtReleased[i] = (GamePad.GetState((PlayerIndex)i).Triggers.Right == 0);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                bigButtonReleased[i] = (GamePad.GetState((PlayerIndex)i).Buttons.BigButton == ButtonState.Released);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                startReleased[i] = (GamePad.GetState((PlayerIndex)i).Buttons.Start == ButtonState.Released);
            }
            
            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                backReleased[i] = (GamePad.GetState((PlayerIndex)i).Buttons.Back == ButtonState.Released);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                leftStickReleased[i] = (GamePad.GetState((PlayerIndex)i).Buttons.LeftStick == ButtonState.Released);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                rightStickReleased[i] = (GamePad.GetState((PlayerIndex)i).Buttons.RightStick == ButtonState.Released);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                dPadUpReleased[i] = (GamePad.GetState((PlayerIndex)i).DPad.Up == ButtonState.Released);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                dPadDownReleased[i] = (GamePad.GetState((PlayerIndex)i).DPad.Down == ButtonState.Released);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                dPadLeftReleased[i] = (GamePad.GetState((PlayerIndex)i).DPad.Left == ButtonState.Released);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                dPadRightReleased[i] = (GamePad.GetState((PlayerIndex)i).DPad.Right == ButtonState.Released);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                leftStickXVectorZeroedOut[i] = (leftStickXVector((PlayerIndex)i) == 0);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                leftStickYVectorZeroedOut[i] = (leftStickYVector((PlayerIndex)i) == 0);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                rightStickXVectorZeroedOut[i] = (rightStickXVector((PlayerIndex)i) == 0);
            }

            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                rightStickYVectorZeroedOut[i] = (rightStickYVector((PlayerIndex)i) == 0);
            }

            /*foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
            {
                XBoxButtons1[button] = GamePad.GetState(PlayerIndex.One).IsButtonUp(button);
            }

            foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
            {
                XBoxButtons2[button] = GamePad.GetState(PlayerIndex.Two).IsButtonUp(button);
            }

            foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
            {
                XBoxButtons3[button] = GamePad.GetState(PlayerIndex.Three).IsButtonUp(button);
            }

            foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
            {
                XBoxButtons4[button] = GamePad.GetState(PlayerIndex.Four).IsButtonUp(button);
            }*/

#if WINDOWS
            qKeyReleased = isKeyQUp();
            eKeyReleased = isKeyEUp();
            oKeyReleased = isKeyOUp();
            pKeyReleased = isKeyPUp();
            zKeyReleased = isKeyZUp();
            cKeyReleased = isKeyCUp();
            wKeyReleased = isKeyWUp();
            sKeyReleased = isKeySUp();
            dKeyReleased = isKeyDUp();
            aKeyReleased = isKeyAUp();
            upKeyReleased = isKeyUpUp();
            downKeyReleased = isKeyDownUp();
            leftKeyReleased = isKeyLeftUp();
            rightKeyReleased = isKeyRightUp();
            enterKeyReleased = isKeyEnterUp();
            escKeyReleased = isKeyEscUp();
            backspaceKeyReleased = isKeyBackspaceUp();
            rightShiftKeyReleased = isKeyRightShiftUp();
            leftShiftKeyReleased = isKeyLeftShiftUp();
            leftCtrlKeyReleased = isKeyLeftCtrlUp();
            spaceKeyReleased = isKeySpaceUp();
            keypad2KeyReleased = isKeypad2Up();
            keypad4KeyReleased = isKeypad4Up();
            keypad6KeyReleased = isKeypad6Up();
            keypad8KeyReleased = isKeypad8Up();
            
            //for (int i = 0; i < player1Keys.Length; i++) { player1Keys[i] = Keyboard.GetState().IsKeyDown((Keys)i); }
            //for (int i = 0; i < player2Keys.Length; i++) { player2Keys[i] = Keyboard.GetState().IsKeyDown((Keys)i); }
            //for (int i = 0; i < player3Keys.Length; i++) { player3Keys[i] = Keyboard.GetState().IsKeyDown((Keys)i); }
            //for (int i = 0; i < player4Keys.Length; i++) { player4Keys[i] = Keyboard.GetState().IsKeyDown((Keys)i); }

            mouseX = Mouse.GetState().X;
            mouseY = Mouse.GetState().Y;
            leftClickReleased = isLeftClickUp();
            rightClickReleased = isRightClickUp();
#endif
        }
    }
}
