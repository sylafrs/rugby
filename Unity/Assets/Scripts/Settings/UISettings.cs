using System.Collections;
using UnityEngine;

[System.Serializable]
public class UISettings
{
	public GameUISettings  GameUI;
	public ScrumUISettings ScrumUI;
	public EndUISettings   EndUI;
    public TouchUISettings TouchUI;
}

[System.Serializable]
public class GameUISettings
{
    public UIPanel MainPanel;

    public MySlider blueSuper;
    public MySlider redSuper;
    public NumberManager timeNumber;

    public NumberManager redScore;
    public NumberManager blueScore;

    public GameObject j1, j2;
	
	public float ScrumBarMaxDelta = 1.5f;
    public float OffsetSideIndicator = 1;
}

[System.Serializable]
public class TouchUISettings
{
    public UIPanel MainPanel;
    public UITexture [] left, right;
    public UITexture croix, rond;
}

[System.Serializable]
public class ScrumUISettings
{
    public UIPanel ScrumPanel;
    public MySlider sliderScrum;

    public UITexture ScrumSpecial;
    public UITexture ScrumLeftSpecialFailed;
    public UITexture ScrumRightSpecialFailed;

    public float MalusDuration;
}

[System.Serializable]
public class EndUISettings
{
    public UITexture MaoriWins;
    public UITexture JapanWins;
    public UITexture Draw;
    public NumberManager NorthScore;
    public NumberManager SouthScore;
    public UIPanel EndPanel;
    public GameObject Abtn;
    public GameObject DegradeCam;
    public GameObject FlagMaori, FlagJap;
    public GameObject JapanWin, MaoriWin;
}