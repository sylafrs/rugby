using UnityEngine;
using System.Collections;

public class ShaderTransparency : MonoBehaviour {
#if UNITY_EDITOR
	LogFile ShaderGoal = new LogFile();
	private bool clearFile = true;
#endif
	
	public bool Fade;
	public float TimeToFade;
	
	private bool bOnFadeJap = false;
	private bool bOnFadeMao = false;
	private float timeJap = 0f;
	private float timeMao = 0f;
	
	public string NewShader;
	public int NewAlphaJap;
	public int NewAlphaMao;
	public LayerMask MaoGoal;
	public LayerMask JapGoal;
	
	private Shader OldShaderJap;
	private Shader OldShaderMaori;
	private Shader newShader;
	
	private float alphaMaoChangeStatus;
	private float alphaJapChangeStatus;
	
	private bool bMaoGoal = false;
	private bool bOldStatusMaoGoal;
	private bool bJapGoal = false;
	private bool bOldStatusJapGoal;
	
	Team[] teams = new Team[2];
	Vector3 dir;

#if UNITY_EDITOR
	void Awake()
	{
		ShaderGoal.SetName("Assets/Scripts/Game/Autres/LOG/ShaderTransparency");
		if (clearFile)
			ShaderGoal.ClearFile();
	}
#endif
	// Use this for initialization
	void Start () {
		if (Game.instance.northTeam != null)
			OldShaderMaori = Game.instance.northTeam.But.model.renderer.material.shader;
		if (Game.instance.southTeam != null)
			OldShaderJap = Game.instance.southTeam.But.model.renderer.material.shader;
		
		newShader = Shader.Find(NewShader);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (teams[0] == null || teams[1] == null)
		{
			teams[0] = Game.instance.northTeam;
			teams[1] = Game.instance.southTeam;
		}
		
		playerBehindGoal();
		updateFade();
		updateTime();
		
		/*
		foreach(var t in teams)
		{
			foreach(Unit u in t)
			{
				dir = u.transform.position - this.transform.position;
				//a player is behind Maori goal
				if (Physics.Raycast(this.transform.position, dir, dir.magnitude, MaoGoal))
				{
					bMaoGoal = true;
				}
				//a player cannot be behind Maori goal and Jap goal
				else if (Physics.Raycast(this.transform.position, dir, dir.magnitude, JapGoal))
				{
					bJapGoal = true;;
				}
			}
		}
		
		FadeGoal(MaoGoal);
		FadeGoal(JapGoal);
		
		if (!bOnFadeMao)
			timeMao = 0f;
		else if (timeMao >= TimeToFade)
			timeMao = TimeToFade;
		else
			timeMao += Time.deltaTime;
		
		if (!bOnFadeJap)
			timeJap = 0f;
		else if (timeJap >= TimeToFade)
			timeJap = TimeToFade;
		else
			timeJap += Time.deltaTime;
		*/
	}
	/*
	void FadeGoal(LayerMask goal)
	{
		
		if (goal == MaoGoal)
		{
			//Fade
			if (bMaoGoal)
			{
				//change shader
				if (Fade)
				{
					float alpha;
					alpha = 255f - (NewAlphaJap*timeJap/TimeToFade);
					Game.instance.northTeam.But.model.renderer.material = SwapAlpha(Game.instance.northTeam.But.model.renderer.material, alpha/255f);
				}
				else
				{
					Game.instance.northTeam.But.model.renderer.material = SwapAlpha(Game.instance.northTeam.But.model.renderer.material, NewAlphaMao/255f);
				}
				Game.instance.northTeam.But.model.renderer.material.shader = newShader;
			}
			//return before fade
			else
			{
				if (Fade)
				{
					float alpha;
					alpha = 255f - (NewAlphaMao*(TimeToFade-timeJap)/TimeToFade);
					Game.instance.northTeam.But.model.renderer.material = SwapAlpha(Game.instance.northTeam.But.model.renderer.material, alpha/255f);
				}
				else
				{
					Game.instance.southTeam.But.model.renderer.material = SwapAlpha(Game.instance.southTeam.But.model.renderer.material, 1f);
				}
				Game.instance.northTeam.But.model.renderer.material.shader = OldShaderMaori;
			}
		}
		else if (goal == JapGoal)
		{
			//Fade
			if (bJapGoal)
			{
				float alpha = 0f;
				if (Fade)
				{
					alpha = 255f - (NewAlphaJap*time/TimeToFade);
					Game.instance.southTeam.But.model.renderer.material = SwapAlpha(Game.instance.southTeam.But.model.renderer.material, alpha/255f);
				}
				else
				{
					Game.instance.southTeam.But.model.renderer.material = SwapAlpha(Game.instance.southTeam.But.model.renderer.material, NewAlphaJap/255f);
				}
				Game.instance.southTeam.But.model.renderer.material.shader = newShader;
			}
			else
			{
				float alpha = 0f;
				if (Fade)
				{
					alpha = 255f - (NewAlphaJap*(TimeToFade-time)/TimeToFade);
					Game.instance.southTeam.But.model.renderer.material = SwapAlpha(Game.instance.southTeam.But.model.renderer.material, alpha/255f);
				}
				else
				{
					Game.instance.southTeam.But.model.renderer.material = SwapAlpha(Game.instance.southTeam.But.model.renderer.material, 1f);
				}
				if (Others.nearlyEqual(alpha, 255f, 0.1f))
					Game.instance.southTeam.But.model.renderer.material.shader = OldShaderJap;
				
			}
		}
	}
	*/
	Material SwapAlpha(Material mat, float alpha)
	{
		Material res = mat;
		Color col;
		col = res.color;
		col.a = alpha;
		res.color = col;
		return res;
	}
	
	void playerBehindGoal()
	{
		bool oldMao = bMaoGoal;
		bool oldJap = bJapGoal;
		
		bMaoGoal = false;
		bJapGoal = false;
		
		foreach(var t in teams)
		{
			foreach(Unit u in t)
			{
				dir = u.transform.position - this.transform.position;
				//a player is behind Maori goal
                if (MaoGoal != 0)
                {
                    if (Physics.Raycast(this.transform.position, dir, dir.magnitude, MaoGoal))
                    {
                        bMaoGoal = true;
                    }
                }
                //a player cannot be behind Maori goal and Jap goal
                if (JapGoal != 0)
                {
                    if (Physics.Raycast(this.transform.position, dir, dir.magnitude, JapGoal))
                    {
                        bJapGoal = true; ;
                    }
                }
			}
		}
		
		if (oldMao != bMaoGoal)
		{
			alphaMaoChangeStatus = Game.instance.northTeam.But.model.renderer.material.color.a*255f;
		#if UNITY_EDITOR
			ShaderGoal.WriteLine("Maori goal change status : " + oldMao + " becomes " + bMaoGoal);
			ShaderGoal.WriteLine("Alpha : " + alphaMaoChangeStatus);
		#endif
			timeMao = 0f;
		}
		
		if (oldJap != bJapGoal)
		{
			alphaJapChangeStatus = Game.instance.southTeam.But.model.renderer.material.color.a*255f;
		#if UNITY_EDITOR
			ShaderGoal.WriteLine("Jap goal change status : " + oldJap + " becomes " + bJapGoal);
			ShaderGoal.WriteLine("Alpha : " + alphaJapChangeStatus);
		#endif
			timeJap = 0f;
		}

	}
	
	void updateFade()
	{
		float alpha = 0f;
		if (bMaoGoal)
		{
			bOnFadeMao = true;
			//fade the mao goal
			if (Fade)
			{
				alpha = alphaMaoChangeStatus - ((255f-NewAlphaMao)*timeMao/TimeToFade);
				if (alpha <= NewAlphaMao)
					alpha = NewAlphaMao;
				Game.instance.northTeam.But.model.renderer.material = SwapAlpha(Game.instance.northTeam.But.model.renderer.material, alpha/255f);
			}
			else
			{
				Game.instance.northTeam.But.model.renderer.material = SwapAlpha(Game.instance.northTeam.But.model.renderer.material, NewAlphaMao/255f);
			}
			//change shader
			Game.instance.northTeam.But.model.renderer.material.shader = newShader;
			
		}
		else
		{
			//if the mao goal was on fade inverse the fade
			if (bOnFadeMao)
			{
				if (Fade)
				{
					alpha = alphaMaoChangeStatus + ((255f-NewAlphaMao)*timeMao/TimeToFade);
					if (alpha >= 255f)
						alpha = 255f;
					Game.instance.northTeam.But.model.renderer.material = SwapAlpha(Game.instance.northTeam.But.model.renderer.material, alpha/255f);
				}
				else
				{
					Game.instance.northTeam.But.model.renderer.material = SwapAlpha(Game.instance.northTeam.But.model.renderer.material, 1f);
				}
				
				if (Others.nearlyEqual(Game.instance.northTeam.But.model.renderer.material.color.a, 1f, 0.0001f))
					bOnFadeMao = false;
			}
			//reaffect initial shader
			else
			{
				Game.instance.northTeam.But.model.renderer.material.shader = OldShaderMaori;
			}
		}
		
		if (bJapGoal)
		{
			bOnFadeJap = true;
			//fade the jap goal
			if (Fade)
			{
				alpha = alphaJapChangeStatus - ((255f-NewAlphaJap)*timeJap/TimeToFade);
				if (alpha <= NewAlphaJap)
					alpha = NewAlphaJap;
				Game.instance.southTeam.But.model.renderer.material = SwapAlpha(Game.instance.southTeam.But.model.renderer.material, alpha/255f);
			}
			else
			{
				Game.instance.southTeam.But.model.renderer.material = SwapAlpha(Game.instance.southTeam.But.model.renderer.material, NewAlphaJap/255f);
			}
			//change shader
			Game.instance.southTeam.But.model.renderer.material.shader = newShader;
			
		}
		else
		{
			//if the jap goal was on fade inverse the fade
			if (bOnFadeJap)
			{
				if (Fade)
				{
					alpha = alphaJapChangeStatus + ((255f-NewAlphaJap)*timeJap/TimeToFade);
					if (alpha >= 255f)
						alpha = 255f;
					Game.instance.southTeam.But.model.renderer.material = SwapAlpha(Game.instance.southTeam.But.model.renderer.material, alpha/255f);
				}
				else
				{
					Game.instance.southTeam.But.model.renderer.material = SwapAlpha(Game.instance.southTeam.But.model.renderer.material, 1f);
				}
				
				if (Others.nearlyEqual(Game.instance.southTeam.But.model.renderer.material.color.a, 1f, 0.0001f))
					bOnFadeJap = false;
			}
			//reaffect initial shader
			else
			{
				Game.instance.southTeam.But.model.renderer.material.shader = OldShaderJap;
			}
		}
#if UNITY_EDITOR
	if (ShaderGoal.GetLength() > 0)
	{
		ShaderGoal.WriteLine("bMaoGoal : " + bMaoGoal + " alpha : " + alpha + " time : " + timeMao + "\t\t" + 
				"bJapGoal : " + bJapGoal + " alpha : " + alpha + " time : " + timeJap);
	}
#endif
	}
	
	void updateTime()
	{
		if (!bOnFadeMao)
			timeMao = 0f;
		else if (timeMao >= TimeToFade)
			timeMao = TimeToFade;
		else
			timeMao += Time.deltaTime;
		
		if (!bOnFadeJap)
			timeJap = 0f;
		else if (timeJap >= TimeToFade)
			timeJap = TimeToFade;
		else
			timeJap += Time.deltaTime;
	}
}
