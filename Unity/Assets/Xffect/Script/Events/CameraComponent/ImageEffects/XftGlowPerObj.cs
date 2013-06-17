using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class XftGlowPerObj : MonoBehaviour
{
	public float glowIntensity = 1.5f;
	
	public int blurIterations = 3;
	
	public float blurSpread = 0.7f;
	
	public Color glowTint = new Color(1,1,1,1);
	
	protected XftEventComponent m_client = null;
	
	
	protected RenderTexture TempRenderTex;
	protected RenderTexture TempRenderGlow;
	protected GameObject m_shaderCamera;

	protected Camera ShaderCamera
	{
		get{
			if (m_shaderCamera == null)
			{
				m_shaderCamera = new GameObject("ShaderCamera", typeof(Camera));
				m_shaderCamera.camera.enabled = false;
				m_shaderCamera.hideFlags = HideFlags.HideAndDontSave;
			}
			
			return m_shaderCamera.camera;
		}
	}
	
	public Shader ReplacementShader;
	
	
	#region glow
    public Shader compositeShader;
    Material m_CompositeMaterial = null;
	protected Material compositeMaterial {
		get {
			if (m_CompositeMaterial == null) {
                m_CompositeMaterial = new Material(compositeShader);
				m_CompositeMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_CompositeMaterial;
		} 
	}
	

    public Shader blurShader;
    Material m_BlurMaterial = null;
	protected Material blurMaterial {
		get {
			if (m_BlurMaterial == null) {
                m_BlurMaterial = new Material(blurShader);
                m_BlurMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_BlurMaterial;
		} 
	}
	
	
	public Shader downsampleShader;
	Material m_DownsampleMaterial = null;
	protected Material downsampleMaterial {
		get {
			if (m_DownsampleMaterial == null) {
				m_DownsampleMaterial = new Material( downsampleShader );
				m_DownsampleMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_DownsampleMaterial;
		} 
	}
	
	#endregion
	
	//final blend the temp render tecture and dest texture.
	public Shader blendShader;
	Material m_blendMaterial = null;
	protected Material blendMaterial {
		get {
			if (m_blendMaterial == null) {
				m_blendMaterial = new Material( blendShader );
				m_blendMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_blendMaterial;
		} 
	}	
	
	void ReleaseRenderTex()
	{
		if (TempRenderGlow != null) {
			RenderTexture.ReleaseTemporary (TempRenderGlow);
			TempRenderGlow = null;
		}
			
		if (TempRenderTex != null){
			RenderTexture.ReleaseTemporary (TempRenderTex);
			TempRenderTex = null;
		}
	}
	
	void PrepareRenderTex()
	{
		if (TempRenderTex == null) {
			TempRenderTex = RenderTexture.GetTemporary ((int)camera.pixelWidth, (int)camera.pixelHeight, 24);
		}
		
		if (TempRenderGlow == null) {
			TempRenderGlow = RenderTexture.GetTemporary ((int)camera.pixelWidth, (int)camera.pixelHeight, 24);
		}
	}

	void OnPreRender()
	{
		if (!enabled || !gameObject.active)
		return;
		
		PrepareRenderTex();

		Camera cam = ShaderCamera;
		cam.CopyFrom (camera);
		cam.backgroundColor = Color.black;

		cam.targetTexture = TempRenderTex;
		cam.RenderWithShader(ReplacementShader,"XftEffect");
	}
	
	void Awake()
	{
		this.enabled = false;
	}
	
	public void Init(XftEventComponent client)
	{
		
		m_client = client;
		
		ReplacementShader = client.GlowPerObjReplacementShader;
		blendShader = client.GlowPerObjBlendShader;
		
		if (m_blendMaterial == null)
		{
			m_blendMaterial = new Material( blendShader );
			m_blendMaterial.hideFlags = HideFlags.HideAndDontSave;
		}
		
		if (m_DownsampleMaterial == null)
		{
			downsampleShader = client.GlowDownSampleShader;
			m_DownsampleMaterial = new Material( downsampleShader );
			m_DownsampleMaterial.hideFlags = HideFlags.HideAndDontSave;
		}

		if (m_CompositeMaterial == null)
		{
			compositeShader = client.GlowCompositeShader;
			m_CompositeMaterial = new Material(compositeShader);
			m_CompositeMaterial.hideFlags = HideFlags.HideAndDontSave;
		}

		if (m_BlurMaterial == null)
		{
			blurShader = client.GlowBlurShader;
			m_BlurMaterial = new Material(blurShader);
			m_BlurMaterial.hideFlags = HideFlags.HideAndDontSave;
		}
		SetClientParam();
	}
	
	void SetClientParam()
	{
		glowIntensity = m_client.GlowIntensity;
		blurIterations = m_client.GlowBlurIterations;
		blurSpread = m_client.GlowBlurSpread;
		glowTint = m_client.GlowColorStart;
	}
	
	public bool CheckSupport()
	{
		bool ret = true;
		if (!SystemInfo.supportsImageEffects)
		{
			ret = false;
		}
		
		// Disable the effect if no downsample shader is setup
		if( downsampleShader == null )
		{
			Debug.Log ("No downsample shader assigned! Disabling glow.");
			ret = false;
		}
		// Disable if any of the shaders can't run on the users graphics card
		else
		{		
			if( !blurMaterial.shader.isSupported )
				ret = false;
			if( !compositeMaterial.shader.isSupported )
				ret = false;
			if( !downsampleMaterial.shader.isSupported )
				ret = false;
		}
		
		return ret;
	}
	
	
	public void FourTapCone (RenderTexture source, RenderTexture dest, int iteration)
	{
		float off = 0.5f + iteration*blurSpread;
		Graphics.BlitMultiTap (source, dest, blurMaterial,
            new Vector2( off, off),
			new Vector2(-off, off),
            new Vector2( off,-off),
            new Vector2(-off,-off)
		);
	}
	
	private void DownSample4x (RenderTexture source, RenderTexture dest)
	{
		downsampleMaterial.color = new Color( glowTint.r, glowTint.g, glowTint.b, glowTint.a/4.0f );
		Graphics.Blit (source, dest, downsampleMaterial);
	}
	
	void RenderGlow(RenderTexture source, RenderTexture destination)
	{
		// Clamp parameters to sane values
		glowIntensity = Mathf.Clamp( glowIntensity, 0.0f, 10.0f );
		blurIterations = Mathf.Clamp( blurIterations, 0, 30 );
		blurSpread = Mathf.Clamp( blurSpread, 0.5f, 1.0f );
		
		RenderTexture buffer = RenderTexture.GetTemporary(source.width/4, source.height/4, 0);
		RenderTexture buffer2 = RenderTexture.GetTemporary(source.width/4, source.height/4, 0);
		
		// Copy source to the 4x4 smaller texture.
		DownSample4x (source, buffer);
		
		// Blur the small texture
		float extraBlurBoost = Mathf.Clamp01( (glowIntensity - 1.0f) / 4.0f );
		blurMaterial.color = new Color( 1F, 1F, 1F, 0.25f + extraBlurBoost );
		
		bool oddEven = true;
		for(int i = 0; i < blurIterations; i++)
		{
			if( oddEven )
				FourTapCone (buffer, buffer2, i);
			else
				FourTapCone (buffer2, buffer, i);
			oddEven = !oddEven;
		}
		Graphics.Blit(source,destination);
				
		if( oddEven )
			BlitGlow(buffer, destination);
		else
			BlitGlow(buffer2, destination);
		
		RenderTexture.ReleaseTemporary(buffer);
		RenderTexture.ReleaseTemporary(buffer2);
	}
	
	// Called by the camera to apply the image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		//only glow the particle "layer"
		RenderGlow(TempRenderTex,TempRenderGlow);
		
		blendMaterial.SetTexture("_GlowTex",TempRenderGlow);
		//blendMaterial.SetTexture("_MainTex",source);
		Graphics.Blit (source, destination,blendMaterial);
		
		//Graphics.Blit (TempRenderGlow, destination);
		
		ReleaseRenderTex();
	}
	
	
	public void BlitGlow( RenderTexture source, RenderTexture dest )
	{
		compositeMaterial.color = new Color(1F, 1F, 1F, Mathf.Clamp01(glowIntensity));
		Graphics.Blit (source, dest, compositeMaterial);
	}	
}
