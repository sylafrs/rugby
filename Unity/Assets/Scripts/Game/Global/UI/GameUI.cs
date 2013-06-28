using UnityEngine;
using System;
using System.Collections;

public class GameUI{

    private Game game; 
	
	public GameUI(Game _game)
	{
		game = _game;
	}

	public void DrawUI(float blueProgress, float redProgress)
	{
        GameUISettings settings = game.settings.UI.GameUI;

        settings.blueSuper.percent = blueProgress;
		settings.redSuper.percent = redProgress;
		
        settings.blueScore.number = game.southTeam.nbPoints;
        settings.redScore.number = game.northTeam.nbPoints;

		int reverseTime = (int)(game.settings.Global.Game.period_time - game.Referee.IngameTime);
		
        settings.timeNumber.number = reverseTime;

        ShowOutsideScreenUnit();
    }

    public void ShowOutsideScreenUnit()
    {
        GameUISettings settings = game.settings.UI.GameUI;

        Unit south = game.southTeam.Player.Controlled;
        ShowUnit(south, settings.j1.gameObject);

        Unit north = game.northTeam.Player.Controlled;
        ShowUnit(north, settings.j2.gameObject);    
    }

    public void ShowUnit(Unit u, GameObject indic)
    {
        if (u == null)
            return;

        Camera orthoCam = GameObject.FindGameObjectWithTag("UICamera").camera;

        indic.SetActive(true);

        Vector3 pos = WorldToNormalizedViewportPoint(
            Camera.main,
            u.transform.position
        );

        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);

        float rot = 0;

        // Bas
        if (pos.y == 0)
        {
            rot = 0;
        }
        // Haut
        else if (pos.y == 1)
        {
            rot = 180;
        }
        // Gauche
        else if (pos.x == 0)
        {
            rot = 270;
        }
        // Droite
        else if (pos.x == 1)
        {
            rot = 90;
        }
        // Inside
        else
        {
            indic.SetActive(false);
            return;
        }

        indic.transform.localRotation = Quaternion.EulerAngles(0, 0, rot * Mathf.Deg2Rad);   
        
        indic.transform.position = NormalizedViewportToWorldPoint(
            orthoCam,
            pos
        );             
    }

   // public void ShowOutsideScreenUnit()
   // {
   //     GameUISettings settings = game.settings.UI.GameUI;
   //
   //     Vector3 v = ShowOutsideScreenUnit(game.southTeam.Player.Controlled);
   //     settings.j1.transform.position = v;
   //
   //     v = ShowOutsideScreenUnit(game.northTeam.Player.Controlled);
   // 
   // 
   // }
   //    
   // private Vector3 ShowOutsideScreenUnit(Unit u) {
   //
   //     Camera perpCam = Camera.main;
   //     Camera orthoCam = GameObject.FindGameObjectWithTag("UICamera").camera;
   //
   //     Vector3 pos = perpCam.WorldToViewportPoint(u.transform.position);
   //
   //     pos.x = Mathf.Clamp01(pos.x);
   //     pos.y = Mathf.Clamp01(pos.y);
   //
   //     return orthoCam.ViewportToWorldPoint(pos);
   // }


    public static Vector3 WorldToNormalizedViewportPoint(Camera camera, Vector3 point)
    {
        // Use the default camera matrix to normalize XY,
        // but Z will be distance from the camera in world units
        point = camera.WorldToViewportPoint(point);

        if (camera.isOrthoGraphic)
        {
            // Convert world units into a normalized Z depth value
            // based on orthographic projection
            point.z = (2 * (point.z - camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane)) - 1f;
        }
        else
        {
            // Convert world units into a normalized Z depth value
            // based on perspective projection
            point.z = ((camera.farClipPlane + camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane))
            + (1 / point.z) * (-2 * camera.farClipPlane * camera.nearClipPlane / (camera.farClipPlane - camera.nearClipPlane));
        }

        return point;
    }

    public static Vector3 NormalizedViewportToWorldPoint(Camera camera, Vector3 point)
    {
        if (camera.isOrthoGraphic)
        {
            // Convert normalized Z depth value into world units
            // based on orthographic projection
            point.z = (point.z + 1f) * (camera.farClipPlane - camera.nearClipPlane) * 0.5f + camera.nearClipPlane;
        }
        else
        {
            // Convert normalized Z depth value into world units
            // based on perspective projection
            point.z = ((-2 * camera.farClipPlane * camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane)) /
            (point.z - ((camera.farClipPlane + camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane)));
        }

        // Use the default camera matrix which expects normalized XY but world unit Z
        return camera.ViewportToWorldPoint(point);
    }
}
