using UnityEngine;
using XInputDotNetPure;
using System.Collections.Generic;

public enum XBOX_DIRECTION
{
    NONE, Pad, StickRight, StickLeft
}

public enum XBOX_BUTTONS
{
    NONE, A, B, Back, LeftShoulder, LeftStick, RightShoulder, RightStick, Start, X, Y, TriggerL, TriggerR, PAD_up, PAD_down, PAD_left, PAD_right
}

[AddComponentMenu("Inputs/XboxInputs")]
public class XboxInputs : myMonoBehaviour{

    public const int NB_DIRECTION = 3;

    public static InputDirection.Direction GetDirection(XBOX_DIRECTION direction, GamePadState pad)
    {
        InputDirection.Direction d = new InputDirection.Direction(0, 0);
        
        if (!pad.IsConnected)
        {
            return d;
        }
        
        switch (direction)
        {            
            case XBOX_DIRECTION.Pad:
                if (pad.DPad.Down == ButtonState.Pressed)
                    d.y--;
                if (pad.DPad.Up == ButtonState.Pressed)
                    d.y++;
                if (pad.DPad.Right == ButtonState.Pressed)
                    d.x++;
                if (pad.DPad.Left == ButtonState.Pressed)
                    d.x--;
                break;
            case XBOX_DIRECTION.StickLeft:
                d.x = pad.ThumbSticks.Left.X;
                d.y = pad.ThumbSticks.Left.Y;
                break;
            case XBOX_DIRECTION.StickRight:
                d.x = pad.ThumbSticks.Right.X;
                d.y = pad.ThumbSticks.Right.Y;
                break;
        }

        return d;
    }
    
    public const int NB_BUTTONS = 16;

    public static bool GetButton(XBOX_BUTTONS button, GamePadState pad)
    {
        if (!pad.IsConnected)
        {
            return false;
        }

        switch (button)
        {
            case XBOX_BUTTONS.NONE:
                return false;
            case XBOX_BUTTONS.A:
                return pad.Buttons.A == ButtonState.Pressed;
            case XBOX_BUTTONS.B:
                return pad.Buttons.B == ButtonState.Pressed;
            case XBOX_BUTTONS.Back:
                return pad.Buttons.Back == ButtonState.Pressed;
            case XBOX_BUTTONS.LeftShoulder:
                return pad.Buttons.LeftShoulder == ButtonState.Pressed;
            case XBOX_BUTTONS.LeftStick:
                return pad.Buttons.LeftStick == ButtonState.Pressed;
            case XBOX_BUTTONS.RightShoulder:
                return pad.Buttons.RightShoulder == ButtonState.Pressed;
            case XBOX_BUTTONS.RightStick:
                return pad.Buttons.RightStick == ButtonState.Pressed;
            case XBOX_BUTTONS.Start:
                return pad.Buttons.Start == ButtonState.Pressed;
            case XBOX_BUTTONS.X:
                return pad.Buttons.X == ButtonState.Pressed;
            case XBOX_BUTTONS.Y:
                return pad.Buttons.Y == ButtonState.Pressed;
            case XBOX_BUTTONS.TriggerL:
                return (pad.Triggers.Left > 0.5f);
            case XBOX_BUTTONS.TriggerR:
                return (pad.Triggers.Right > 0.5f);
            case XBOX_BUTTONS.PAD_up:
                return pad.DPad.Up == ButtonState.Pressed;
            case XBOX_BUTTONS.PAD_down:
                return pad.DPad.Down == ButtonState.Pressed;
            case XBOX_BUTTONS.PAD_left:
                return pad.DPad.Left == ButtonState.Pressed;
            case XBOX_BUTTONS.PAD_right:
                return pad.DPad.Right == ButtonState.Pressed;
        }

        return false;
    }

    // -------------------------------------------  //   
 
    public class Controller {
        private bool[] prevState;
        private PlayerIndex index;

        public bool IsConnected
        {
            get
            {
                return pad.IsConnected;
            }
            private set { }
        }
        
        public Controller(int i)
        {
            index = (PlayerIndex)i;
            prevState = new bool[XboxInputs.NB_BUTTONS];
        }

        private GamePadState framePad;        
        public GamePadState pad
        {
            get
            {
                if (padToUpdate)
                {
                    padToUpdate = false;
                    pad = GamePad.GetState(index);
                }

                return framePad;
            }
            private set
            {
                framePad = value;
            }
        }
        private bool padToUpdate = true;

        public InputDirection.Direction GetDirection(XBOX_DIRECTION dir)
        {
            return XboxInputs.GetDirection(dir, pad);
        }

        public bool GetButton(XBOX_BUTTONS btn) {
            return XboxInputs.GetButton(btn, pad);
        }

        public bool GetButtonDown(XBOX_BUTTONS btn)
        {
            return XboxInputs.GetButton(btn, pad) && !prevState[(int)btn];
        }

        public bool GetButtonUp(XBOX_BUTTONS btn)
        {
            return !XboxInputs.GetButton(btn, pad) && prevState[(int)btn];
        }

        public void Update()
        {
            this.padToUpdate = true;
            this.UpdateButtons();
        }

        private void UpdateButtons()
        {
            // Si la manette de la frame précédente était connectée.
            //if (framePad.IsConnected) 
            //{
                for (int i = 0; i < XboxInputs.NB_BUTTONS; i++)
                {
                    prevState[i] = GetButton((XBOX_BUTTONS)i);
                }
            //}
        }
		
		void Vibrate(float left, float right) {
			GamePad.SetVibration(index, pad.Triggers.Left, pad.Triggers.Right);
		}
    }

    // -------------------------------------------  //

    const int MAX_CONTROLLERS = 4;
   
    public int nConnectedControllers;

    public bool [] checkedControllers;
    public Controller[] controllers;

    bool inited = false;

    public void Start()
    {
        if(!inited)
            this.init();
    }
   
    void init()
    {
        inited = true;

        checkedControllers = new bool[MAX_CONTROLLERS];
        controllers = new Controller[MAX_CONTROLLERS];
        for (int i = 0; i < MAX_CONTROLLERS; i++)
        {
            controllers[i] = new Controller(i);
        }

        this.CheckAll();
    }

    void Update()
    {        
        for (int i = 0; i < MAX_CONTROLLERS; i++)
        {
            //if (checkedControllers[i])
            {
                controllers[i].Update();
            }
        }        
    }

    public void NoNeedToCheck(int index)
    {
        if (!inited)
            init();

        checkedControllers[index] = false;
    }

    public void NeedToCheck(int index)
    {
        if (!inited)
            init();

        checkedControllers[index] = true;
    }

    public void CheckAll()
    {
        if (!inited)
            init();

        for (int i = 0; i < MAX_CONTROLLERS; i++)
        {
            checkedControllers[i] = true;
        }
    }

    public void CheckNone()
    {
        if (!inited)
            init();

        for (int i = 0; i < MAX_CONTROLLERS; i++)
        {
            checkedControllers[i] = false;
        }
    }
}
