using System.Collections;
using UnityEngine;

[System.Serializable]
public class UISettings
{
	public GameUISettings  GameUI;
	public ScrumUISettings ScrumUI;
	public EndUISettings   EndUI;	
}

[System.Serializable]
public class GameUISettings
{
    public UIPanel MainPanel;

    public MySlider blueSuper;
    public MySlider redSuper;
    public NumberManager timeNumber;

	//for super bar
	//public Texture2D emptyBar;
	//public Texture2D blueBar;
	//public Texture2D redBar;
	
	//GUI custom
	public GUIStyle superOkTextStyle;
	//public GUIStyle gameTimeTextStyle;
	public GUIStyle gameScoreTextStyle;
	//public GUIStyle timeBeforeScrumStyle;
	
	public float ScrumBarMaxDelta = 1.5f;
	
	//public float timeBoxWidthPercentage   = 10;
	//public float timeBoxHeightPercentage  = 5;
	//public float timeBoxXPercentage		= 50;
	//public float timeBoxYPercentage		= 10;
    
	//public float blueGaugeBoxWidthPercentage   = 25;
	//public float blueGaugeBoxHeightPercentage  = 10;
	//public float blueGaugeBoxXPercentage		= 22.5f;
	//public float blueGaugeBoxYPercentage		= 10;
	
	//public float redGaugeBoxWidthPercentage   = 25;
	//public float redGaugeBoxHeightPercentage  = 10;
	//public float redGaugeBoxXPercentage		= 77.5f;
	//public float redGaugeBoxYPercentage		= 10;
	
	public float scoreBoxWidthPercentage  = 20;
	public float scoreBoxHeightPercentage = 15;
	public float scoreBoxXPercentage = 50;
	public float scoreBoxYPercentage = 10;
	
	public float scrumBarBoxWidthPercentage = 50;
	public float scrumBarBoxHeightPercentage = 16;
	public float scrumBarBoxXPercentage = 50;
	public float scrumBarBoxYPercentage = 50;
	
	public float scrumSpecialBoxWidthPercentage = 50;
	public float scrumSpecialBoxHeightPercentage = 16;
	public float scrumSpecialBoxXPercentage = 50;
	public float scrumSpecialBoxYPercentage = 66;
	
	public float scrumTimeBoxWidthPercentage = 50;
	public float scrumTimeBoxHeightPercentage = 16;
	public float scrumTimeBoxXPercentage = 50;
	public float scrumTimeBoxYPercentage = 34;
}

[System.Serializable]
public class ScrumUISettings
{
    public UIPanel ScrumPanel;
    public MySlider sliderScrum;

	public Rect ScrumSpecialRect;
    public Texture2D ScrumSpecialButton;
    //public Rect ScrumBarRect;
    //public Texture2D ScrumRightBar;
    //public Texture2D ScrumLeftBar;
    //public Texture2D ScrumEmptyBar;
    public Texture2D ScrumSpecialFailedButton;
    public Rect ScrumLeftSpecialFailedButtonRect;
    public Rect ScrumRightSpecialFailedButtonRect;
    public float MalusDuration;
}

[System.Serializable]
public class EndUISettings
{
    public Rect ResultRect, ResultButtonRect, ResultScoreRect;
    public GUIStyle ResultStyle, ResultScoreStyle;
    public int btnFontSize;
}