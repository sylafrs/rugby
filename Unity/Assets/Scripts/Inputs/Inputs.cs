using UnityEngine;
using System.Collections;

[System.Serializable]
public class InputTouch
{
    public KeyCode keyboard;
    public XBOX_BUTTONS xbox;
}

[System.Serializable]
public class InputDirection
{
    public KeyBoardDirection keyboard;
    public XBOX_DIRECTION xbox;
}

[System.Serializable]
public class KeyBoardDirection
{
    public KeyCode up, down, right, left;

    public static int GetRight(KeyBoardDirection d)
    {
        return (Input.GetKey(d.right) ? 1 : 0) + (Input.GetKey(d.left) ? -1 : 0);
    }

    public static int GetUp(KeyBoardDirection d)
    {
        return (Input.GetKey(d.up) ? 1 : 0) + (Input.GetKey(d.down) ? -1 : 0);
    }
}