using UnityEngine;
using UnityEditor;
using Xft;
 
[CustomEditor(typeof(XffectComponent))]
public class XffectComponentCustom : Editor
{
	public static string DefaultMatPath = "Examples/Materials/default.mat";
	
	string LayerName = "EffectLayer";
	
	#region check update
	//remember to change this when released a new version.
	public string curVersion = "3.0.0";
	public string latestVersion;
	public static WWW updateCheckObject; 
	public static string updateURL = "http://shallway.net/products/xffect/version.php";
	protected static System.DateTime _lastUpdateCheck;
	public static System.DateTime lastUpdateCheck {
		get {
			try {
				_lastUpdateCheck = System.DateTime.Parse (EditorPrefs.GetString ("XffectLastUpdateCheck",System.DateTime.UtcNow.ToString ()));
			}
			catch (System.FormatException) {
				_lastUpdateCheck = System.DateTime.UtcNow;
				Debug.LogWarning ("Invalid DateTime string encountered when loading from preferences");
			}
			return _lastUpdateCheck;
		}
		set {
			_lastUpdateCheck = value;
			EditorPrefs.SetString ("XffectLastUpdateCheck", _lastUpdateCheck.ToString ());
		}
	}
	public static bool ParseServerMessage (string result)
	{
		if (string.IsNullOrEmpty (result)) {
			return false;
		}
		string[] sarr = result.Split('\n');
		
		if (sarr == null || sarr.Length == 0)
		{
			//Debug.LogWarning("parse server message error:" + result +"\nXffect Pro");
			return false;
		}
		
		for (int i = 0; i < sarr.Length; i++)
		{
			string str = sarr[i];
			
			if (string.IsNullOrEmpty(str) || str.Contains("*"))
				continue;
			string[] item = str.Split('|');
			if (item.Length != 3)
			{
				//Debug.LogWarning("parse server message error:" + result +"\nXffect Pro");
				continue;
			}
			item[2].Trim('\r');
			
			string version = item[0];
			string version_url = item[1];
			string version_desc = item[2];

			EditorPrefs.SetString ("XffectLatestVersion",version.ToString ());
			EditorPrefs.SetString ("XffectLatestVersionDesc",version_desc);
			EditorPrefs.SetString ("XffectLatestVersionURL",version_url);
			return true;
		}
		
		return true;
	}
	public static void CheckForUpdates () {
		
		if (updateCheckObject != null && updateCheckObject.isDone) {
			
			if (!string.IsNullOrEmpty (updateCheckObject.error)) {
				//Debug.LogWarning ("There was an error while checking for updates\n" +
				//"The error might dissapear if you switch build target from Webplayer to Standalone: " +
				//updateCheckObject.error);
				updateCheckObject = null;
				return;
			}
			ParseServerMessage (updateCheckObject.text);
			updateCheckObject = null;
		}
		
		//if (updateCheckObject == null)
			//updateCheckObject = new WWW (updateURL);
		
		if (System.DateTime.Compare (lastUpdateCheck.AddDays (1f),System.DateTime.UtcNow) < 0) {
			//Debug.Log ("Checking For Updates... " + System.DateTime.UtcNow.ToString ()+"\nXffect Pro");
			updateCheckObject = new WWW (updateURL);
			lastUpdateCheck = System.DateTime.UtcNow;
		}
	}
	#endregion
	
	public static string documentation = "http://shallway.net/forum.php?mod=viewthread&tid=39";
	
	
	public XffectComponent Script;
	
	protected XEditorTool mXEditor;
	
	
	

	protected SerializedProperty LifeTime;
    protected SerializedProperty IgnoreTimeScale;
	protected SerializedProperty EditView;
    protected SerializedProperty Scale;
	
	
	public XEditorTool XEditor
	{
		get{
			if (mXEditor == null)
			{
				mXEditor = new XEditorTool();
			}
			return mXEditor;
		}
	}
	
	void LoadStyle()
	{
		if (EffectLayerCustom.IsSkinLoaded)
			return;
		EffectLayerCustom.LoadStyle();
	}
	
	void OnEnable()
	{
		Script = target as XffectComponent;
		InitSerializedProperty();
		LoadStyle();
		
		if (Application.isEditor && !EditorApplication.isPlaying) {
			EditorApplication.update = Script.Update;
		}
	}
	
	void OnDisable ()
	{
		if (Application.isEditor && !EditorApplication.isPlaying) {
			EditorApplication.update = null;
		}
	}
	
	
	void InitSerializedProperty()
	{
		LifeTime = serializedObject.FindProperty("LifeTime");
		IgnoreTimeScale = serializedObject.FindProperty("IgnoreTimeScale");
		EditView = serializedObject.FindProperty("EditView");
		Scale = serializedObject.FindProperty("Scale");
	}

	public override void OnInspectorGUI ()
	{
		
		serializedObject.Update();
		XEditor.BeginCommonArea("xffect main config",Script.gameObject.name,this,true);
		
		EditorGUILayout.Space();
		
		XEditor.DrawToggle("update in editor?","",EditView);
		
		//EditView.boolValue = EditorGUILayout.Toggle("update in editor:", EditView.boolValue,GUILayout.Height(40f));
		
		
		if (EditView.boolValue == true) {
			if (!XffectComponent.IsActive(Script.gameObject)) {
				EditView.boolValue = false;
				Debug.Log ("you need to activate the xffect object: " + Script.gameObject.name + " before updating it in editor.");
			}
		}

		if (EditView.boolValue) {
			Script.EnableEditView ();
		} else {
			Script.DisableEditView ();
		}

		if (EditView.boolValue) {
			if (GUILayout.Button ("Reset")) {
				Script.ResetEditScene ();
			}
		}
		
		XEditor.DrawSeparator();
		
		XEditor.DrawFloat("life(-1 means infinite):","",LifeTime);
		XEditor.DrawToggle("ignore time scale?","",IgnoreTimeScale);
		
		
		EditorGUILayout.Space();
		
		XEditor.DrawFloat("scale:","change this Xffect's scale",Scale);
		
		
		if (GUILayout.Button ("Add Layer")) {
			GameObject layer = new GameObject (LayerName);
			EffectLayer efl = (EffectLayer)layer.AddComponent ("EffectLayer");
			layer.transform.parent = Selection.activeTransform;
			efl.transform.localPosition = Vector3.zero;
			//default to effect layer object.
			efl.ClientTransform = efl.transform;
			efl.GravityObject = efl.transform;
			efl.BombObject = efl.transform;
			efl.TurbulenceObject = efl.transform;
			efl.AirObject = efl.transform;
			efl.VortexObj = efl.transform;
			efl.DirCenter = efl.transform;
			efl.Material = AssetDatabase.LoadAssetAtPath(XEditorTool.GetXffectPath() + DefaultMatPath, typeof(Material)) as Material;
			
			Selection.activeGameObject = layer;
		}
		
		
		if (GUILayout.Button ("Add Event")) {
			GameObject obj = new GameObject ("_Event");
			XftEventComponent xevent = (XftEventComponent)obj.AddComponent ("XftEventComponent");
			xevent.transform.parent = Selection.activeTransform;
			xevent.transform.localPosition = Vector3.zero;
			xevent.RadialBlurShader = Shader.Find ("Xffect/PP/radial_blur_new");
			xevent.GlowCompositeShader = Shader.Find ("Xffect/PP/glow_compose");
			xevent.GlowDownSampleShader = Shader.Find ("Xffect/PP/glow_downsample");
			xevent.GlowBlurShader = Shader.Find ("Xffect/PP/glow_conetap");
			xevent.RadialBlurObj = xevent.transform;
            xevent.ColorInverseShader = Shader.Find ("Xffect/PP/color_inverse");
			
			xevent.GlowPerObjBlendShader = Shader.Find("Xffect/glow_per_obj/blend");
			xevent.GlowPerObjReplacementShader = Shader.Find("Xffect/glow_per_obj/replacement");
			
			Selection.activeGameObject = obj;
		}
		
		
		EditorGUILayout.Space();
		
		XEditor.EndXArea();
		
		DrawInfos();
		
		serializedObject.ApplyModifiedProperties();
	}
	
	
	void DrawInfos()
	{
		XEditor.BeginCommonArea("xffect infos","Info",this,false);
		
		///////////check for update////////////////////////////
		/*CheckForUpdates();
		string latestVersion = EditorPrefs.GetString("XffectLatestVersion",curVersion);
		System.Version lv = new System.Version(latestVersion);
		string url = EditorPrefs.GetString("XffectLatestVersionURL","http://shallway.net");
		if (lv > new System.Version(curVersion))
		{
			string desc = EditorPrefs.GetString("XffectLatestVersionDesc","");
			string info = "There is a new version available, the latest version is:" +  latestVersion + "\nInfo:" + desc;
			XEditor.DrawInfo(info);
			
			if (GUILayout.Button ("Download")) {
				Application.OpenURL (url);
			}
			
		}*/
		//////////////////////////////////////////////////
		
		XEditor.DrawInfo("this is a free edition, the functions and presets are limited, if you like it, buy pro version:)");
		
		EditorGUILayout.Space();
		
		string curversion = "Free Edition:" + curVersion; 
		
		EditorGUILayout.LabelField(curversion);
		EditorGUILayout.LabelField("Arthor: shallway");
		EditorGUILayout.LabelField("Contact: shallwaycn@gmail.com");
		
		if (GUILayout.Button ("Documentation")) {
			Application.OpenURL (documentation);
		}
		
		EditorGUILayout.Space();
		
		XEditor.EndXArea();
	}
	
	
	[MenuItem("GameObject/Create Other/Xffet Object")]
	[MenuItem("Window/Xffect/Create Xffet Object")]
	static void DoCreateXffectObject ()
	{
		GameObject go = new GameObject ("XffectObj");
		go.transform.localScale = Vector3.one;
		go.transform.rotation = Quaternion.identity;
		go.AddComponent<XffectComponent> ();

		Selection.activeGameObject = go;

		GameObject layer = new GameObject ("EffectLayer");
		EffectLayer efl = (EffectLayer)layer.AddComponent ("EffectLayer");
		layer.transform.parent = go.transform;

		efl.transform.localPosition = Vector3.zero;
		//fixed 2012.6.25. default to effect layer object.
		efl.ClientTransform = efl.transform;
		efl.GravityObject = efl.transform;
		efl.BombObject = efl.transform;
		efl.TurbulenceObject = efl.transform;
		efl.AirObject = efl.transform;
		efl.VortexObj = efl.transform;
		efl.DirCenter = efl.transform;
        efl.DragObj = efl.transform;
		
		efl.Material = AssetDatabase.LoadAssetAtPath(XEditorTool.GetXffectPath() + DefaultMatPath, typeof(Material)) as Material;
		
	}
}
