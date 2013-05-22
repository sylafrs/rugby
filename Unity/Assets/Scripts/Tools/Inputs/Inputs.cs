using UnityEngine;
using System.Collections;

[System.Serializable]
public class InputTouch
{
    public KeyCode keyboardP1;
	public KeyCode keyboardP2;
    public XBOX_BUTTONS xbox;   
	
	public KeyCode keyboard(Team t) {
		Game g = t.game;
		if(t == g.southTeam)
			return keyboardP1;
		return keyboardP2;
	}
}

[System.Serializable]
public class InputDirection
{
    public struct Direction
    {
        public float x, y;
        public Direction(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public KeyBoardDirection keyboardP1;
	public KeyBoardDirection keyboardP2;
	
	public KeyBoardDirection keyboard(Team t) {
		Game g = t.game;
		if(t == g.southTeam)
			return keyboardP1;
		return keyboardP2;
	}
	
    public XBOX_DIRECTION xbox;   
}

[System.Serializable]
public class KeyBoardDirection
{
    public KeyCode up, down, right, left;

    public int GetRight()
    {
        return (Input.GetKey(right) ? 1 : 0) + (Input.GetKey(left) ? -1 : 0);
    }

    public int GetUp()
    {
        return (Input.GetKey(up) ? 1 : 0) + (Input.GetKey(down) ? -1 : 0);
    }

    public InputDirection.Direction GetDirection()
    {
        return new InputDirection.Direction(this.GetRight(), this.GetUp());
    }    
}