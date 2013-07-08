using UnityEngine;
using System.Collections;

public class TouchUI{
	
	private Game game;
	private TouchManager manager;
    private TouchUISettings settings;
	
	public TouchUI(Game _game)
	{
		game = _game;
        manager = _game.refs.managers.touch;
        settings = _game.settings.UI.TouchUI;
	}

    public void StartUI()
    {
        settings.MainPanel.gameObject.SetActive(true);
    }

    public void EndUI()
    {
        settings.MainPanel.gameObject.SetActive(false);
    }
	
	public void DrawUI()
	{
        int l = settings.right.Length;

        for (int i = 0; i < l; i++)
        {
            settings.right[i].gameObject.SetActive(true);
            settings.left[i].gameObject.SetActive(true);
        }

        settings.croix.gameObject.SetActive(false);
        settings.rond.gameObject.SetActive(false);

        int southChoice, northChoice;

        if(manager.gamerTouch == game.southTeam.Player) 
        {
            southChoice = manager.touchChoice;
            northChoice = manager.interChoice;
        }
        else {
            northChoice = manager.touchChoice;
            southChoice = manager.interChoice;
        }

        bool right = this.game.Referee.IsTouchRight();
        UITexture [] south, north;

        if (right)
        {
            north = this.settings.right;
            south = this.settings.left;
        }
        else
        {
            north = this.settings.left;
            south = this.settings.right;
        }

        if (southChoice != 0 && northChoice != 0)
        {
            TouchManager.Result res = manager.GetResult();
            bool southWin = false;
            bool draw = res == TouchManager.Result.DRAW;

            settings.croix.gameObject.SetActive(!draw);
            settings.rond.gameObject.SetActive(!draw);

            if (res == TouchManager.Result.TOUCH)
            {
                southWin = (manager.gamerTouch == game.southTeam.Player);
            }
            else
            {
                southWin = (manager.gamerTouch != game.southTeam.Player);
            }

            for (int i = 0; i < l; i++)
            {
                if (i != southChoice - 1)
                {
                    south[i].gameObject.SetActive(false);
                }
                else if (!draw)
                {
                    if (southWin)
                    {
                        settings.rond.transform.position = south[i].transform.position + Vector3.forward;
                    }
                    else
                    {
                        settings.croix.transform.position = south[i].transform.position + Vector3.forward;
                    }
                }

                if (i != northChoice - 1)
                {
                    north[i].gameObject.SetActive(false);
                }
                else if (!draw)
                {
                    if (!southWin)
                    {
                        settings.rond.transform.position = north[i].transform.position + Vector3.forward;
                    }
                    else
                    {
                        settings.croix.transform.position = north[i].transform.position + Vector3.forward;
                    }
                }
            }
        }
        else
        {
            if (southChoice != 0)
            {
                for (int i = 0; i < l; i++)
                {
                    south[i].gameObject.SetActive(false);
                }
            }
            if (northChoice != 0)
            {
                for (int i = 0; i < l; i++)
                {
                    north[i].gameObject.SetActive(false);
                }
            }
        }        
	}
}
