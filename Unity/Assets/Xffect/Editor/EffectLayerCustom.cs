using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using Xft;

[CustomEditor(typeof(EffectLayer))]
public class EffectLayerCustom : Editor
{
	
	public static string EditorAssets = "Assets/Xffect/Editor/EditorAssets";
	
    public static string ColorBkgMatPath = "Editor/color_bkg.mat";
	
	public static bool IsSkinLoaded = false;
	
	public static Material ColorBkgMat;
	
	public static GUISkin XSkin;
	public static GUISkin InspectorSkin;
	public static GUIStyle Xtoggle;
	public static GUIStyle Xbutton;
	public static GUIStyle XLabelField;
	public static GUIStyle XArea;
	public static GUIStyle XTexture;
	public static GUIStyle XToggle2;
	public static GUIStyle XInfoArea;
	
	
	public Texture2D GradientTex;
	
	public EffectLayer Script;
	

	
	protected Rect ColorBox;
	
	
	#region serialized property
	protected SerializedProperty StartTime;string TStartTime = "how long will this layer delay before getting start.";
	protected SerializedProperty DebugColor;string TDebugColor = "the debug color of this layer in the esitor scene view.";
	protected SerializedProperty MaxFps;
	protected SerializedProperty ClientTransform;string TClientTransform = "every node's original position is relative to this transform.";
	protected SerializedProperty SyncClient;string TSyncClient = "if you toggle this option, all the nodes' position will in the local space of the client transform.";
	protected SerializedProperty Material;string TMaterial = "specify the material of this Layer.";
	protected SerializedProperty Depth;string TDepth = "the render queue of this material, depth 0 means render queue 3000.";
	protected SerializedProperty RenderType;string TRenderType = "specify the node's render type.";
	
	//Sprite
	protected SerializedProperty SpriteType;string TSpriteType = "Billboard: A quad that will always rotate to face to camera.\nBillboardSelf: It will also face to the camera, but only rotate around its original direction. In fact, it's a Ribbon Trail which has only 2 elements.\nXZ Plane: A quad on the XZ Plane.";
	protected SerializedProperty OriPoint;string TOriPoint = "the pivot of this sprite.";
	protected SerializedProperty SpriteWidth;string TSpriteWidth = "the width of this sprite.";
	protected SerializedProperty SpriteHeight;string TSpriteHeight = "the height of this sprite.";

	
	//Ribbon Trail
	protected SerializedProperty RibbonWidth; string TRibbonWidth = "the width of the trail";
	protected SerializedProperty MaxRibbonElements; string TMaxRibbonElements = "the max num of quads in a trail is 'max elements' -1, so the more elements you set, the more smooth you get. And note this value should always > 2.";
	protected SerializedProperty RibbonLen; string TRibbonLen = "the length of the trail. If the current trail length exceed this length, the trail head will stretch and tail will shrink.";
	protected SerializedProperty FaceToObject; string TFaceToObject = "used for slash trail, make the trail face to object instead of facing to camera.";
	protected SerializedProperty FaceObject; string TFaceObject = "the object to face, can't be null";
	protected SerializedProperty UseRandomRibbon; string TUseRandomRibbon = "give the trail's size some randomness";
	protected SerializedProperty RibbonWidthMin;
	protected SerializedProperty RibbonWidthMax;
	protected SerializedProperty RibbonLenMin;
	protected SerializedProperty RibbonLenMax;
	
	
	//Emitter
	protected SerializedProperty EmitType; string TEmitType = "the shape of the emitter";
    protected SerializedProperty BoxSize;
    protected SerializedProperty EmitPoint;
    protected SerializedProperty Radius;
    protected SerializedProperty CircleDir;
    protected SerializedProperty EmitUniform;
    protected SerializedProperty LineLengthLeft;
    protected SerializedProperty LineLengthRight;
    protected SerializedProperty MaxENodes; string TMaxENodes = "the maximum nodes the emitter can emit.";
    protected SerializedProperty IsNodeLifeLoop; string TIsNodeLifeLoop = "if check this option, then each node's life is infinite.";
    protected SerializedProperty NodeLifeMin;
    protected SerializedProperty NodeLifeMax;
    protected SerializedProperty EmitWay;
    protected SerializedProperty EmitterCurve;
    protected SerializedProperty DiffDistance;
    protected SerializedProperty EmitMesh;
    protected SerializedProperty EmitMeshType;
    protected SerializedProperty ChanceToEmit; string TChanceToEmit = "give the emition some randomness.";
    protected SerializedProperty EmitDuration; string TEmitDuration = "each emition loop's duration. when the time expired, the emition loop count will -1.";
    protected SerializedProperty EmitRate; string TEmitRate = "emit rate of each emition.";
    protected SerializedProperty EmitLoop; string TEmitLoop = "when loop count becomes zero, the emition will stop. if this value < 0, the loop count is infinite.";
    protected SerializedProperty EmitDelay;string TEmitDelay = "specify the delay time after each loop has finished.";
	
	
	protected SerializedProperty DirType;
    protected SerializedProperty DirCenter;
    protected SerializedProperty OriVelocityAxis;
    protected SerializedProperty AngleAroundAxis;
    protected SerializedProperty OriSpeed; string TOriSpeed = "give each node a original speed, the velocity direction is the 'direction type' that you set.";
    protected SerializedProperty AlwaysSyncRotation; string TAlwaysSyncRotation = "each node's original direction is inherited from the client by default, but it will not be influenced by the client's rotation's change during run time, if you want always sync the direction to client, check on this option.";
    protected SerializedProperty IsRandomSpeed;
    protected SerializedProperty SpeedMin;
    protected SerializedProperty SpeedMax;
	
	
	//gravity modifier
	protected SerializedProperty GravityAffectorEnable;
    protected SerializedProperty GravityAftType;
    protected SerializedProperty GravityMagType;
    protected SerializedProperty GravityMag;
    protected SerializedProperty GravityCurve;
    protected SerializedProperty GravityDirection;
    protected SerializedProperty GravityObject;
    protected SerializedProperty IsGravityAccelerate; string TIsGravityAccelerate = "if check on this option, the gravity force will be added to the node's velocity, or, the force will directly change the node's position. e.g, if you want every node be attracted by gravity object, but there has other modifiers on each node, you should uncheck this option to make each node close to garavity object without other influences.";
	
	
	//Bomb Affector
    protected SerializedProperty BombAffectorEnable;
    protected SerializedProperty BombObject;
    protected SerializedProperty BombType;
    protected SerializedProperty BombDecayType;
    protected SerializedProperty BombMagType;
    protected SerializedProperty BombMagnitude;
    protected SerializedProperty BombMagCurve;
    protected SerializedProperty BombAxis;
    protected SerializedProperty BombDecay;
	
	
	protected SerializedProperty AirAffectorEnable;
    protected SerializedProperty AirObject;
    protected SerializedProperty AirMagType;
    protected SerializedProperty AirMagnitude;
    protected SerializedProperty AirMagCurve;
    protected SerializedProperty AirDirection;
    protected SerializedProperty AirAttenuation;
    protected SerializedProperty AirUseMaxDistance;
    protected SerializedProperty AirMaxDistance;
    protected SerializedProperty AirEnableSpread;
    protected SerializedProperty AirSpread;
    protected SerializedProperty AirInheritVelocity; string TAirInheritVelocity = "describes how much of the air object's velocity is added to the magnitude and direction of the air field.";
    protected SerializedProperty AirInheritRotation;
	
	
	protected SerializedProperty VortexAffectorEnable;
    protected SerializedProperty VortexMagType;
    protected SerializedProperty VortexMag;
    protected SerializedProperty VortexCurve;
    protected SerializedProperty VortexDirection;
    protected SerializedProperty VortexInheritRotation;
    protected SerializedProperty VortexObj;
    protected SerializedProperty IsRandomVortexDir;
	protected SerializedProperty IsVortexAccelerate; string TIsVortexAccelerate = "if check on this option, the vortex force will added to the node's velocity, or, the force will directly change the node's position.";
	protected SerializedProperty VortexAttenuation;
	protected SerializedProperty UseVortexMaxDistance;
	protected SerializedProperty VortexMaxDistance;
	
	
	protected SerializedProperty JetAffectorEnable;
    protected SerializedProperty JetMagType;
    protected SerializedProperty JetMag;
    protected SerializedProperty JetCurve;
	
	
	protected SerializedProperty TurbulenceAffectorEnable;
    protected SerializedProperty TurbulenceObject;
    protected SerializedProperty TurbulenceMagType;
    protected SerializedProperty TurbulenceMagnitude;
    protected SerializedProperty TurbulenceMagCurve;
    protected SerializedProperty TurbulenceAttenuation;
    protected SerializedProperty TurbulenceUseMaxDistance;
    protected SerializedProperty TurbulenceMaxDistance;
	
	
	protected SerializedProperty DragAffectorEnable;
    protected SerializedProperty DragObj;
    protected SerializedProperty DragUseDir;
    protected SerializedProperty DragDir;
    protected SerializedProperty DragMag;
    protected SerializedProperty DragUseMaxDist;
    protected SerializedProperty DragMaxDist;
    protected SerializedProperty DragAtten;
	
	
	protected SerializedProperty ConeSize;
    protected SerializedProperty ConeAngle;
    protected SerializedProperty ConeSegment; string TConeSegment = "indicate the number of segments of the bottom circle, the more segment, the more smooth, and the value should always >= 3.";
    protected SerializedProperty UseConeAngleChange;
    protected SerializedProperty ConeDeltaAngle;
	
	
	protected SerializedProperty OriRotationMin;
    protected SerializedProperty OriRotationMax;
    protected SerializedProperty RotAffectorEnable;
    protected SerializedProperty RotateType;
    protected SerializedProperty DeltaRot;
    protected SerializedProperty RotateCurve;
	protected SerializedProperty RandomOriRot;
	
	protected SerializedProperty CMesh;
	
	protected SerializedProperty RandomOriScale;
	protected SerializedProperty OriScaleXMin;
    protected SerializedProperty OriScaleXMax;
    protected SerializedProperty OriScaleYMin;
    protected SerializedProperty OriScaleYMax;
    protected SerializedProperty ScaleAffectorEnable;
    protected SerializedProperty ScaleType;
    protected SerializedProperty DeltaScaleX;
    protected SerializedProperty DeltaScaleY;
    protected SerializedProperty ScaleXCurve;
    protected SerializedProperty ScaleYCurve;
    
    protected SerializedProperty UseSameScaleCurve;
	
	
	protected SerializedProperty ColorAffectorEnable;
	protected SerializedProperty ColorGradualTimeLength;
    protected SerializedProperty ColorGradualType;
	protected SerializedProperty ColorChangeType;
	//protected SerializedProperty ColorParam;
	protected SerializedProperty ColorGradualCurve;
	protected SerializedProperty Color1;
	
	
	
	protected SerializedProperty UVAffectorEnable;
    protected SerializedProperty UVType;
    protected SerializedProperty OriTopLeftUV;
    protected SerializedProperty OriUVDimensions;
    protected SerializedProperty Cols;
    protected SerializedProperty Rows;
    protected SerializedProperty LoopCircles;
    protected SerializedProperty UVTime;
	protected SerializedProperty RandomStartFrame;
	protected SerializedProperty UVRotAffectorEnable;
    protected SerializedProperty RandomUVRotateSpeed;
    protected SerializedProperty UVRotXSpeed;
    protected SerializedProperty UVRotYSpeed;
    protected SerializedProperty UVRotXSpeedMax;
    protected SerializedProperty UVRotYSpeedMax;
	
	
	protected SerializedProperty SpriteUVStretch; string TSpriteUVStretch = "specify the uv stretch direction.";
	protected SerializedProperty StretchType; string TStretchType = "specify the uv stretch direction.";
	
	
	protected SerializedProperty UseCollisionDetection;
    protected SerializedProperty ParticleRadius;
    protected SerializedProperty CollisionType;
    protected SerializedProperty CollisionAutoDestroy;
    protected SerializedProperty EventReceiver;
    protected SerializedProperty EventHandleFunctionName;
    protected SerializedProperty CollisionGoal;
    protected SerializedProperty ColliisionPosRange;
    protected SerializedProperty CollisionLayer;
    //protected SerializedProperty CollisionCollider;
	
	
	#endregion
	
	protected static XEditorTool mXEditor;
	
	
	
	public static string TDELAY = "how long will this layer delay before getting start.";

	
	public XEditorTool XEditor
	{
		get{
			if (mXEditor == null)
			{
				mXEditor = new XEditorTool();
			}
				
			mXEditor.MyEditor = this;
			return mXEditor;
		}
	}
	public static bool LoadStyle()
	{
		if (IsSkinLoaded)
			return true;
		
		//first check if we've stored the right path
		string storedpath = EditorPrefs.GetString("xffect editor assets:");
		if (!string.IsNullOrEmpty(storedpath))
		{
			EditorAssets = storedpath;
		}
		
		string projectPath = Application.dataPath;
		if (projectPath.EndsWith ("/Assets")) {
			projectPath = projectPath.Remove (projectPath.Length-("Assets".Length));
		}
		
		//if can't load at the default path or the last stored path, then re-check it.
		if (!System.IO.File.Exists (projectPath + EditorAssets + "/Xskin.guiskin")) {
			//Initiate search
			System.IO.DirectoryInfo sdir = new System.IO.DirectoryInfo (Application.dataPath);
			
			Queue<System.IO.DirectoryInfo> dirQueue = new Queue<System.IO.DirectoryInfo>();
			dirQueue.Enqueue (sdir);
			
			bool found = false;
			while (dirQueue.Count > 0) {
				System.IO.DirectoryInfo dir = dirQueue.Dequeue ();
				if (System.IO.File.Exists (dir.FullName + "/Xskin.guiskin")) {
					string path = dir.FullName.Replace ('\\','/');
					found = true;
					//Remove data path from string to make it relative
					path = path.Replace (projectPath,"");
					
					if (path.StartsWith ("/")) {
						path = path.Remove (0,1);
					}
					
					EditorAssets = path;
					Debug.Log ("Located editor assets folder to '"+EditorAssets+"'");
					
					EditorPrefs.SetString("xffect editor assets:",EditorAssets);
					
					break;
				}
				System.IO.DirectoryInfo[] dirs = dir.GetDirectories ();
				for (int i=0;i<dirs.Length;i++) {
					dirQueue.Enqueue (dirs[i]);
				}
			}
			
			if (!found) {
				Debug.LogWarning ("Could not locate editor assets folder\nXffect");
				return false;
			}
		}
		
		XSkin = AssetDatabase.LoadAssetAtPath (EditorAssets + "/Xskin.guiskin",typeof(GUISkin)) as GUISkin;
		InspectorSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
		Xtoggle = XSkin.FindStyle ("xtoggle");
		Xbutton = XSkin.FindStyle ("xbutton");
		XLabelField = XSkin.FindStyle("xlabelfield");
		XArea = XSkin.FindStyle("xarea");
		XTexture = XSkin.FindStyle("xtexture");
		XToggle2 = XSkin.FindStyle("xtoggle2");
		
		//XInfoArea = XSkin.FindStyle("xinfoarea");
		
		XInfoArea = InspectorSkin.FindStyle("HelpBox")?? InspectorSkin.FindStyle("Box");
		
		IsSkinLoaded = true;
		
		
		//load color bkg mat
		
		ColorBkgMat = AssetDatabase.LoadAssetAtPath(XEditorTool.GetXffectPath() + ColorBkgMatPath,typeof(Material)) as Material;
		
		return true;
	}
	
	void InitSerializedProperty()
	{
		ClientTransform = serializedObject.FindProperty("ClientTransform");
		SyncClient = serializedObject.FindProperty("SyncClient");
		Material = serializedObject.FindProperty("Material");
		RenderType = serializedObject.FindProperty("RenderType");
		StartTime = serializedObject.FindProperty("StartTime");
		MaxFps = serializedObject.FindProperty("MaxFps");
		DebugColor = serializedObject.FindProperty("DebugColor");
		Depth = serializedObject.FindProperty("Depth");
		
		SpriteType = serializedObject.FindProperty("SpriteType");
		OriPoint = serializedObject.FindProperty("OriPoint");
		SpriteWidth = serializedObject.FindProperty("SpriteWidth");
		SpriteHeight = serializedObject.FindProperty("SpriteHeight");
		
		RibbonWidth = serializedObject.FindProperty("RibbonWidth");
		MaxRibbonElements = serializedObject.FindProperty("MaxRibbonElements");
		RibbonLen = serializedObject.FindProperty("RibbonLen");
		FaceToObject = serializedObject.FindProperty("FaceToObject");
		FaceObject = serializedObject.FindProperty("FaceObject");
		UseRandomRibbon = serializedObject.FindProperty("UseRandomRibbon");
		RibbonWidthMin = serializedObject.FindProperty("RibbonWidthMin");
		RibbonWidthMax = serializedObject.FindProperty("RibbonWidthMax");
		RibbonLenMin = serializedObject.FindProperty("RibbonLenMin");
		RibbonLenMax = serializedObject.FindProperty("RibbonLenMax");
		
		EmitType = serializedObject.FindProperty("EmitType");
		BoxSize = serializedObject.FindProperty("BoxSize");
		EmitPoint = serializedObject.FindProperty("EmitPoint");
		Radius = serializedObject.FindProperty("Radius");
		CircleDir = serializedObject.FindProperty("CircleDir");
		EmitUniform = serializedObject.FindProperty("EmitUniform");
		LineLengthLeft = serializedObject.FindProperty("LineLengthLeft");
		LineLengthRight = serializedObject.FindProperty("LineLengthRight");
		MaxENodes = serializedObject.FindProperty("MaxENodes");
		IsNodeLifeLoop = serializedObject.FindProperty("IsNodeLifeLoop");
		NodeLifeMin = serializedObject.FindProperty("NodeLifeMin");
		NodeLifeMax = serializedObject.FindProperty("NodeLifeMax");
		EmitWay = serializedObject.FindProperty("EmitWay");
		EmitterCurve = serializedObject.FindProperty("EmitterCurve");
		DiffDistance = serializedObject.FindProperty("DiffDistance");
		EmitMesh = serializedObject.FindProperty("EmitMesh");
		EmitMeshType = serializedObject.FindProperty("EmitMeshType");
		ChanceToEmit = serializedObject.FindProperty("ChanceToEmit");
		EmitDuration = serializedObject.FindProperty("EmitDuration");
		EmitRate = serializedObject.FindProperty("EmitRate");
		EmitLoop = serializedObject.FindProperty("EmitLoop");
		EmitDelay = serializedObject.FindProperty("EmitDelay");
		
		DirType = serializedObject.FindProperty("DirType");
		DirCenter = serializedObject.FindProperty("DirCenter");
		OriVelocityAxis = serializedObject.FindProperty("OriVelocityAxis");
		AngleAroundAxis = serializedObject.FindProperty("AngleAroundAxis");
		OriSpeed = serializedObject.FindProperty("OriSpeed");
		AlwaysSyncRotation = serializedObject.FindProperty("AlwaysSyncRotation");
		IsRandomSpeed = serializedObject.FindProperty("IsRandomSpeed");
		SpeedMin = serializedObject.FindProperty("SpeedMin");
		SpeedMax = serializedObject.FindProperty("SpeedMax");
		
		
		GravityAffectorEnable = serializedObject.FindProperty("GravityAffectorEnable");
		GravityAftType = serializedObject.FindProperty("GravityAftType");
		GravityMagType = serializedObject.FindProperty("GravityMagType");
		GravityMag = serializedObject.FindProperty("GravityMag");
		GravityCurve = serializedObject.FindProperty("GravityCurve");
		GravityDirection = serializedObject.FindProperty("GravityDirection");
		GravityObject = serializedObject.FindProperty("GravityObject");
		IsGravityAccelerate = serializedObject.FindProperty("IsGravityAccelerate");
		
		BombAffectorEnable = serializedObject.FindProperty("BombAffectorEnable");
		BombObject = serializedObject.FindProperty("BombObject");
		BombType = serializedObject.FindProperty("BombType");
		BombDecayType = serializedObject.FindProperty("BombDecayType");
		BombMagType = serializedObject.FindProperty("BombMagType");
		BombMagnitude = serializedObject.FindProperty("BombMagnitude");
		BombMagCurve = serializedObject.FindProperty("BombMagCurve");
		BombAxis = serializedObject.FindProperty("BombAxis");
		BombDecay = serializedObject.FindProperty("BombDecay");
		
		AirAffectorEnable = serializedObject.FindProperty("AirAffectorEnable");
		AirObject = serializedObject.FindProperty("AirObject");
		AirMagType = serializedObject.FindProperty("AirMagType");
		AirMagnitude = serializedObject.FindProperty("AirMagnitude");
		AirMagCurve = serializedObject.FindProperty("AirMagCurve");
		AirDirection = serializedObject.FindProperty("AirDirection");
		AirAttenuation = serializedObject.FindProperty("AirAttenuation");
		AirUseMaxDistance = serializedObject.FindProperty("AirUseMaxDistance");
		AirMaxDistance = serializedObject.FindProperty("AirMaxDistance");
		AirEnableSpread = serializedObject.FindProperty("AirEnableSpread");
		AirSpread = serializedObject.FindProperty("AirSpread");
		AirInheritVelocity = serializedObject.FindProperty("AirInheritVelocity");
		AirInheritRotation = serializedObject.FindProperty("AirInheritRotation");
		
		
		VortexAffectorEnable = serializedObject.FindProperty("VortexAffectorEnable");
		VortexMagType = serializedObject.FindProperty("VortexMagType");
		VortexMag = serializedObject.FindProperty("VortexMag");
		VortexCurve = serializedObject.FindProperty("VortexCurve");
		VortexDirection = serializedObject.FindProperty("VortexDirection");
		VortexInheritRotation = serializedObject.FindProperty("VortexInheritRotation");
		VortexObj = serializedObject.FindProperty("VortexObj");
		IsRandomVortexDir = serializedObject.FindProperty("IsRandomVortexDir");
		IsVortexAccelerate = serializedObject.FindProperty("IsVortexAccelerate");
		VortexAttenuation = serializedObject.FindProperty("VortexAttenuation");
		UseVortexMaxDistance = serializedObject.FindProperty("UseVortexMaxDistance");
		VortexMaxDistance = serializedObject.FindProperty("VortexMaxDistance");
		
		JetAffectorEnable = serializedObject.FindProperty("JetAffectorEnable");
		JetMagType = serializedObject.FindProperty("JetMagType");
		JetMag = serializedObject.FindProperty("JetMag");
		JetCurve = serializedObject.FindProperty("JetCurve");
		
		
		TurbulenceAffectorEnable = serializedObject.FindProperty("TurbulenceAffectorEnable");
		TurbulenceObject = serializedObject.FindProperty("TurbulenceObject");
		TurbulenceMagType = serializedObject.FindProperty("TurbulenceMagType");
		TurbulenceMagnitude = serializedObject.FindProperty("TurbulenceMagnitude");
		TurbulenceMagCurve = serializedObject.FindProperty("TurbulenceMagCurve");
		TurbulenceAttenuation = serializedObject.FindProperty("TurbulenceAttenuation");
		TurbulenceUseMaxDistance = serializedObject.FindProperty("TurbulenceUseMaxDistance");
		TurbulenceMaxDistance = serializedObject.FindProperty("TurbulenceMaxDistance");
		
		
		DragAffectorEnable = serializedObject.FindProperty("DragAffectorEnable");
		DragObj = serializedObject.FindProperty("DragObj");
		DragUseDir = serializedObject.FindProperty("DragUseDir");
		DragDir = serializedObject.FindProperty("DragDir");
		DragMag = serializedObject.FindProperty("DragMag");
		DragUseMaxDist = serializedObject.FindProperty("DragUseMaxDist");
		DragMaxDist = serializedObject.FindProperty("DragMaxDist");
		DragAtten = serializedObject.FindProperty("DragAtten");
		
		
		ConeSize = serializedObject.FindProperty("ConeSize");
		ConeAngle = serializedObject.FindProperty("ConeAngle");
		ConeSegment = serializedObject.FindProperty("ConeSegment");
		UseConeAngleChange = serializedObject.FindProperty("UseConeAngleChange");
		ConeDeltaAngle = serializedObject.FindProperty("ConeDeltaAngle");
		
		CMesh = serializedObject.FindProperty("CMesh");
		
		
		OriRotationMin = serializedObject.FindProperty("OriRotationMin");
		OriRotationMax = serializedObject.FindProperty("OriRotationMax");
		RotAffectorEnable = serializedObject.FindProperty("RotAffectorEnable");
		RotateType = serializedObject.FindProperty("RotateType");
		DeltaRot = serializedObject.FindProperty("DeltaRot");
		RotateCurve = serializedObject.FindProperty("RotateCurve");
		RandomOriRot = serializedObject.FindProperty("RandomOriRot");
		
		
		RandomOriScale = serializedObject.FindProperty("RandomOriScale");
		OriScaleXMin = serializedObject.FindProperty("OriScaleXMin");
		OriScaleXMax = serializedObject.FindProperty("OriScaleXMax");
		OriScaleYMin = serializedObject.FindProperty("OriScaleYMin");
		OriScaleYMax = serializedObject.FindProperty("OriScaleYMax");
		ScaleAffectorEnable = serializedObject.FindProperty("ScaleAffectorEnable");
		ScaleType = serializedObject.FindProperty("ScaleType");
		DeltaScaleX = serializedObject.FindProperty("DeltaScaleX");
		DeltaScaleY = serializedObject.FindProperty("DeltaScaleY");
		ScaleXCurve = serializedObject.FindProperty("ScaleXCurve");
		ScaleYCurve = serializedObject.FindProperty("ScaleYCurve");
		
		UseSameScaleCurve = serializedObject.FindProperty("UseSameScaleCurve");
		
		
		ColorAffectorEnable = serializedObject.FindProperty("ColorAffectorEnable");
		ColorGradualTimeLength = serializedObject.FindProperty("ColorGradualTimeLength");
		ColorGradualType = serializedObject.FindProperty("ColorGradualType");
		ColorChangeType = serializedObject.FindProperty("ColorChangeType");
		ColorGradualCurve = serializedObject.FindProperty("ColorGradualCurve");
		Color1 = serializedObject.FindProperty("Color1");
		
		
		UVAffectorEnable = serializedObject.FindProperty("UVAffectorEnable");
		UVType = serializedObject.FindProperty("UVType");
		OriTopLeftUV = serializedObject.FindProperty("OriTopLeftUV");
		OriUVDimensions = serializedObject.FindProperty("OriUVDimensions");
		Cols = serializedObject.FindProperty("Cols");
		Rows = serializedObject.FindProperty("Rows");
		LoopCircles = serializedObject.FindProperty("LoopCircles");
		UVTime = serializedObject.FindProperty("UVTime");
		RandomStartFrame = serializedObject.FindProperty("RandomStartFrame");
		UVRotAffectorEnable = serializedObject.FindProperty("UVRotAffectorEnable");
		RandomUVRotateSpeed = serializedObject.FindProperty("RandomUVRotateSpeed");
		UVRotXSpeed = serializedObject.FindProperty("UVRotXSpeed");
		UVRotYSpeed = serializedObject.FindProperty("UVRotYSpeed");
		UVRotXSpeedMax = serializedObject.FindProperty("UVRotXSpeedMax");
		UVRotYSpeedMax = serializedObject.FindProperty("UVRotYSpeedMax");
		
		SpriteUVStretch = serializedObject.FindProperty("SpriteUVStretch");
		StretchType = serializedObject.FindProperty("StretchType");
		
		
		UseCollisionDetection = serializedObject.FindProperty("UseCollisionDetection");
		ParticleRadius = serializedObject.FindProperty("ParticleRadius");
		CollisionType = serializedObject.FindProperty("CollisionType");
		CollisionAutoDestroy = serializedObject.FindProperty("CollisionAutoDestroy");
		EventReceiver = serializedObject.FindProperty("EventReceiver");
		EventHandleFunctionName = serializedObject.FindProperty("EventHandleFunctionName");
		CollisionGoal = serializedObject.FindProperty("CollisionGoal");
		ColliisionPosRange = serializedObject.FindProperty("ColliisionPosRange");
		CollisionLayer = serializedObject.FindProperty("CollisionLayer");
		//CollisionCollider = serializedObject.FindProperty("CollisionCollider");
		
	}
	
	
	void DrawCollision()
	{
		XEditor.BeginXArea("Collision",XArea, XEditorTool.MinHeight,XEditorTool.EArea.CheckBox, UseCollisionDetection);
		
		XEditor.DrawFloat("node radius:","",ParticleRadius);
		XEditor.DrawToggle("auto destroy","",CollisionAutoDestroy);
		
		XEditor.DrawSeparator();
		

		EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("collision type:",""),XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
        CollisionType.enumValueIndex = (int)(COLLITION_TYPE)EditorGUILayout.EnumPopup((COLLITION_TYPE)CollisionType.enumValueIndex);
		EditorGUILayout.EndHorizontal();
		
		if ((COLLITION_TYPE)CollisionType.enumValueIndex == COLLITION_TYPE.Sphere)
		{
			XEditor.DrawTransform("collision goal:","",CollisionGoal);
			XEditor.DrawFloat("goal radius:","",ColliisionPosRange);
		}
		else
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("layer mask:",EffectLayerCustom.XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
			CollisionLayer.intValue = EditorGUILayout.LayerField(CollisionLayer.intValue);
			EditorGUILayout.EndHorizontal();
		}
		
		XEditor.DrawSeparator();
		
		XEditor.DrawTransform("event receiver:","",EventReceiver);
		
		EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("event handle function:",EffectLayerCustom.XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
        EventHandleFunctionName.stringValue = EditorGUILayout.TextField(EventHandleFunctionName.stringValue);
        EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		
		XEditor.EndXArea();
	}
	
	void DrawUVConfig()
	{
		
		XEditor.BeginXArea("UV Config",XArea, XEditorTool.MinHeight,XEditorTool.EArea.AlwaysToggle, null,1f);
		
		if (RenderType.intValue != 2 && RenderType.intValue != 3)
		{
			XEditor.DrawVector2Field("top left uv:","",OriTopLeftUV);
		}
		else
		{
			GUI.contentColor = Color.yellow;
			XEditor.DrawVector2Field("bottom left uv:","",OriTopLeftUV);
			GUI.contentColor = Color.white;
		}
		
		XEditor.DrawVector2Field("uv dimensions:","",OriUVDimensions);
		
		EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("uv change type:",""),XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
        UVType.intValue = (int)(UVTYPE)EditorGUILayout.EnumPopup((UVTYPE)UVType.intValue);
		EditorGUILayout.EndHorizontal();
		
		UVAffectorEnable.boolValue = false;
		UVRotAffectorEnable.boolValue = false;
		
		if (UVType.intValue == 1)
		{
			XEditor.DrawInt("x tile:","",Cols);
			XEditor.DrawInt("y tile:","",Rows);
			XEditor.DrawFloat("time(-1 is node life):","",UVTime);
			XEditor.DrawInt("loop(-1 is infinite):","",LoopCircles);
			UVAffectorEnable.boolValue = true;
		}
		else if (UVType.intValue == 2)
		{
			if (XEditor.DrawToggle("random speed?","",RandomUVRotateSpeed))
			{
				XEditor.DrawFloat("speed X min:","",UVRotXSpeed);
				XEditor.DrawFloat("speed X max:","",UVRotXSpeedMax);
				
				XEditor.DrawFloat("speed Y min:","",UVRotYSpeed);
				XEditor.DrawFloat("speed Y max:","",UVRotYSpeedMax);
			}
			else
			{
				XEditor.DrawFloat("speed X:","",UVRotXSpeed);
				XEditor.DrawFloat("speed Y:","",UVRotYSpeed);
			}
			UVRotAffectorEnable.boolValue = true;
		}
		
		if (UVType.intValue != 0)
		{
			XEditor.DrawSeparator();
			XEditor.DrawToggle("random start frame?","",RandomStartFrame);
		}
		
		EditorGUILayout.Space();
		
		XEditor.EndXArea();
	}

	void DrawColorConfig()
	{
		
		XEditorTool.XArea m = XEditor.BeginXArea("Color Config",XArea, XEditorTool.MinHeight,XEditorTool.EArea.AlwaysToggle, null,1f);
		
		
		
		EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("color change type:",""),XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
        ColorChangeType.enumValueIndex = (int)(COLOR_CHANGE_TYPE)EditorGUILayout.EnumPopup((COLOR_CHANGE_TYPE)ColorChangeType.enumValueIndex);
        EditorGUILayout.EndHorizontal();
		
		if ((COLOR_CHANGE_TYPE)ColorChangeType.enumValueIndex == COLOR_CHANGE_TYPE.Constant)
		{
			ColorAffectorEnable.boolValue = false;
			XEditor.DrawColor("color:","",Color1);
		}
		else
		{
			if((COLOR_CHANGE_TYPE)ColorChangeType.enumValueIndex == COLOR_CHANGE_TYPE.Gradient)
			{
				XEditor.DrawInfo("Tips: you can edit the gradient color while updating xffect in editor");
				XEditor.DrawFloat("time(-1 is node life):","",ColorGradualTimeLength);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(new GUIContent("gradient mode:",""),XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
				ColorGradualType.enumValueIndex = (int)(COLOR_GRADUAL_TYPE)EditorGUILayout.EnumPopup((COLOR_GRADUAL_TYPE)ColorGradualType.enumValueIndex);
				EditorGUILayout.EndHorizontal();
				if ((COLOR_GRADUAL_TYPE)ColorGradualType.enumValueIndex == COLOR_GRADUAL_TYPE.CURVE)
				{
					XEditor.DrawCurve("t curve:","",ColorGradualCurve,true);
				}
			}
			

			
			ColorAffectorEnable.boolValue = true;
			if (m.Open)
			{
				XEditorTool.RefreshGradientTex(ref GradientTex,Script.ColorParam,this);
			}
			
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Edit",GUILayout.Width(XEditorTool.LabelWidth)))
			{
				XGradientEditor.Show(Script.ColorParam,GradientTex,this);
			}
			
			//use a dummy label field to get the rect.
			EditorGUILayout.LabelField("",EffectLayerCustom.XLabelField,GUILayout.ExpandWidth(true));
			if (Event.current.type == EventType.Repaint )
			{
				ColorBox = GUILayoutUtility.GetLastRect();
			}
			EditorGUILayout.EndHorizontal();
			if (m.Open)
				EditorGUI.DrawPreviewTexture(ColorBox, GradientTex,EffectLayerCustom.ColorBkgMat);
		}
		

		
		XEditor.EndXArea();
	}
	
	void DrawScaleConfig()
	{
		XEditor.BeginXArea("Scale Config",XArea, XEditorTool.MinHeight,XEditorTool.EArea.AlwaysToggle, null,1f);
		
		if (XEditor.DrawToggle("random start scale?","",RandomOriScale))
		{
			XEditor.DrawFloat("start scale x min:","",OriScaleXMin);
			XEditor.DrawFloat("start scale x max:","",OriScaleXMax);
			XEditor.DrawSeparator();
			XEditor.DrawFloat("start scale y min:","",OriScaleYMin);
			XEditor.DrawFloat("start scale y max:","",OriScaleYMax);
		}
		else
		{
			XEditor.DrawFloat("start scale x:","",OriScaleXMin);
			XEditor.DrawFloat("start scale y:","",OriScaleYMin);
			OriScaleXMax.floatValue = OriScaleXMin.floatValue;
			OriScaleYMax.floatValue = OriScaleYMin.floatValue;
		}
		
		XEditor.DrawSeparator();
		
		EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("scale change type:",""),XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
        ScaleType.enumValueIndex = (int)(RSTYPE)EditorGUILayout.EnumPopup((RSTYPE)ScaleType.enumValueIndex);
        EditorGUILayout.EndHorizontal();
		
		if ((RSTYPE)ScaleType.enumValueIndex == RSTYPE.NONE)
		{
			ScaleAffectorEnable.boolValue = false;
		}
		else if ((RSTYPE)ScaleType.enumValueIndex == RSTYPE.SIMPLE)
		{
			ScaleAffectorEnable.boolValue = true;
			XEditor.DrawFloat("delta scaleX per s:","",DeltaScaleX);
			XEditor.DrawFloat("delta scaleY per s:","",DeltaScaleY);
		}
		else
		{
			ScaleAffectorEnable.boolValue = true;
			if (XEditor.DrawToggle("same curve?","",UseSameScaleCurve))
			{
				XEditor.DrawCurve("scaleXY curve:","",ScaleXCurve);
			}
			else
			{
				XEditor.DrawCurve("scaleX curve:","",ScaleXCurve);
				XEditor.DrawCurve("scaleY curve:","",ScaleYCurve);
			}
		}
		
		EditorGUILayout.Space();
		
		XEditor.EndXArea();
	}
	
	
	void DrawRotationConfig()
	{
		XEditor.BeginXArea("Rotation Config",XArea, XEditorTool.MinHeight,XEditorTool.EArea.AlwaysToggle, null,3f);
		
		if (XEditor.DrawToggle("random start rotation?","",RandomOriRot))
		{
			XEditor.DrawInt("start rotation min:","",OriRotationMin);
			XEditor.DrawInt("start rotation max:","",OriRotationMax);
		}
		else
		{
			XEditor.DrawInt("start rotation:","",OriRotationMin);
			OriRotationMax.intValue = OriRotationMin.intValue;
		}
		
		XEditor.DrawSeparator();
		
		EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("rotation change type:",""),XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
        RotateType.enumValueIndex = (int)(RSTYPE)EditorGUILayout.EnumPopup((RSTYPE)RotateType.enumValueIndex);
        EditorGUILayout.EndHorizontal();
		
		if ((RSTYPE)RotateType.enumValueIndex == RSTYPE.NONE)
		{
			RotAffectorEnable.boolValue = false;
		}
		else if ((RSTYPE)RotateType.enumValueIndex == RSTYPE.SIMPLE)
		{
			RotAffectorEnable.boolValue = true;
			XEditor.DrawFloat("delta angle per second:","",DeltaRot);
			if (RenderType.intValue == 0 && SpriteType.intValue == 1)
			{
				XEditor.DrawInfo("warning: billboard self sprite is not recommended to rotate, it might not work properly.");
			}
		}
		else
		{
			RotAffectorEnable.boolValue = true;
			XEditor.DrawCurve("angle by curve:","",RotateCurve);
			if (RenderType.intValue == 0 && SpriteType.intValue == 1)
			{
				XEditor.DrawInfo("warning: billboard self sprite is not recommended to rotate, it might not work properly.");
			}
		}
		
		XEditor.EndXArea();
	}
	
	void DrawCustomMesh()
	{
		XEditor.BeginXArea("CustomMesh Config",XArea, XEditorTool.MinHeight,XEditorTool.EArea.AlwaysToggle, null);
		
		XEditor.DrawMesh("custom mesh:","",CMesh);
		
		XEditor.EndXArea();
	}
	
	void DrawConeConfig()
	{
		XEditor.BeginXArea("Cone",XArea, XEditorTool.MinHeight,XEditorTool.EArea.AlwaysToggle, null);
		
		XEditor.DrawVector2Field("size:","",ConeSize);
		XEditor.DrawInt("segment(>=3)",TConeSegment,ConeSegment);
		
		if(XEditor.DrawToggle("angle change?","",UseConeAngleChange))
		{
			XEditor.DrawCurve("angle change curve:","",ConeDeltaAngle);
		}
		
		XEditor.EndXArea();
			
	}
	
	void DrawDragModifier()
	{
		XEditor.BeginXArea("Drag Modifier",XArea, XEditorTool.MinHeight,XEditorTool.EArea.CheckBox, DragAffectorEnable,2f);
		
		XEditor.DrawInfo("this funtion is not included in free edition.");
		
		XEditor.EndXArea();
	}
	
	void DrawTurbulenceAffector()
	{
		XEditor.BeginXArea("Turbulence Modifier",XArea, XEditorTool.MinHeight,XEditorTool.EArea.CheckBox, TurbulenceAffectorEnable);
		XEditor.DrawInfo("this funtion is not included in free edition.");
		
		XEditor.EndXArea();
	}
	
	void DrawAccelerateAffector()
	{
		
		XEditor.BeginXArea("Acceleration Modifier",XArea, XEditorTool.MinHeight,XEditorTool.EArea.CheckBox, JetAffectorEnable);
		XEditor.DrawEnumMagType("magnitude type:","",JetMagType);
		
		XEditor.DrawInfo("this funtion is not included in free edition.");
		
		
		XEditor.EndXArea();
	}
	
	void DrawVortexAffector()
	{
		XEditor.BeginXArea("Vortex Modifier",XArea, XEditorTool.MinHeight,XEditorTool.EArea.CheckBox, VortexAffectorEnable,1f);
		
		XEditor.DrawTransform("position:","",VortexObj);
		
		if (!XEditor.DrawToggle("random direction?","",IsRandomVortexDir))
		{
			XEditor.DrawVector3Field("direction:","",VortexDirection);
			
			XEditor.DrawToggle("inherit rotation?","",VortexInheritRotation);
		}
		
		XEditor.DrawEnumMagType("magnitude type:","",VortexMagType);
		
		if ((MAGTYPE)VortexMagType.enumValueIndex == MAGTYPE.Curve)
		{
			XEditor.DrawCurve("magnitude curve:","",VortexCurve);
		}
		else
		{
			XEditor.DrawFloat("magnitude:","",VortexMag);
		}
		
		if (XEditor.DrawToggle("use max distance?","",UseVortexMaxDistance))
		{
			XEditor.DrawFloat("max distance:","",VortexMaxDistance);
		}
		XEditor.DrawSlider("attenuation:","",VortexAttenuation,0f,1f);
		
		XEditor.DrawToggle("accelerate?",TIsVortexAccelerate,IsVortexAccelerate);
		
		XEditor.EndXArea();
	}
	
	void DrawAirAffector()
	{
		
		XEditor.BeginXArea("Air Modifier",XArea, XEditorTool.MinHeight,XEditorTool.EArea.CheckBox, AirAffectorEnable);
		
		XEditor.DrawInfo("this funtion is not included in free edition.");

		XEditor.EndXArea();
		
	}
	
	
	
	void DrawBombAffector()
	{
		
		XEditor.BeginXArea("Bomb Modifier",XArea, XEditorTool.MinHeight,XEditorTool.EArea.CheckBox, BombAffectorEnable);
		
		XEditor.DrawInfo("this funtion is not included in free edition.");
		
		XEditor.EndXArea();
		
	}
	
	void DrawAffectors()
	{
		XEditor.BeginXArea("Modifier",XArea,XEditorTool.MinHeight,XEditorTool.EArea.AlwaysToggle,null);
		DrawGravityAffector();
		
		DrawBombAffector();
		
		DrawAirAffector();
		
		DrawVortexAffector();
		
		DrawAccelerateAffector();
		
		DrawTurbulenceAffector();
		
		DrawDragModifier();
		
		XEditor.EndXArea();
	}
	
	void DrawGravityAffector()
	{
		
		XEditor.BeginXArea("Gravity Modifier",XArea, XEditorTool.MinHeight,XEditorTool.EArea.CheckBox, GravityAffectorEnable);

		XEditor.DrawInfo("this funtion is not included in free edition.");
		
		XEditor.EndXArea();
		
	}
	
	void DrawDirectionConfig()
	{
		XEditor.BeginXArea("Direction Config",XArea,XEditorTool.MinHeight,XEditorTool.EArea.AlwaysToggle,null);
		
		EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("direction type:",XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
        DirType.enumValueIndex = (int)(DIRECTION_TYPE)EditorGUILayout.EnumPopup((DIRECTION_TYPE)DirType.enumValueIndex);
        EditorGUILayout.EndHorizontal();
		
		if ((DIRECTION_TYPE)DirType.enumValueIndex == DIRECTION_TYPE.Planar)
		{
			XEditor.DrawVector3Field("original direction:","",OriVelocityAxis);
		}
		else if ((DIRECTION_TYPE)DirType.enumValueIndex == DIRECTION_TYPE.Sphere)
		{
			XEditor.DrawTransform("direction center:","",DirCenter);
		}
		else if ((DIRECTION_TYPE)DirType.enumValueIndex == DIRECTION_TYPE.Cone)
		{
			XEditor.DrawVector3Field("cone axis:","",OriVelocityAxis);
			XEditor.DrawInt("angle:","",AngleAroundAxis);
		}
		else if ((DIRECTION_TYPE)DirType.enumValueIndex == DIRECTION_TYPE.Cylindrical)
		{
			XEditor.DrawVector3Field("cylindrical axis:","",OriVelocityAxis);
			XEditor.DrawTransform("direction center:","",DirCenter);
		}
		
		XEditor.DrawToggle("inherit client rotation?",TAlwaysSyncRotation,AlwaysSyncRotation);
		
		XEditor.DrawSeparator();
		
		if(XEditor.DrawToggle("random speed?","",IsRandomSpeed))
		{
			XEditor.DrawFloat("speed min:","",SpeedMin);
			XEditor.DrawFloat("speed max:","",SpeedMax);
		}
		else
		{
			XEditor.DrawFloat("original speed:",TOriSpeed,OriSpeed);
		}
		XEditor.EndXArea();
	}
	
	void DrawEmitterConfig()
	{
		XEditor.BeginXArea("Emitter Config",XArea,XEditorTool.MinHeight,XEditorTool.EArea.AlwaysToggle,null);
		
		EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("emitter shape:",TEmitType),XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
        EmitType.intValue = (int)(EMITTYPE)EditorGUILayout.EnumPopup((EMITTYPE)EmitType.intValue);
        EditorGUILayout.EndHorizontal();
		
		if (EmitType.intValue == 0)
		{
			XEditor.DrawVector3Field("emit pos:","",EmitPoint);
		}
		else if (EmitType.intValue == 1)
		{
			XEditor.DrawVector3Field("box center:","",EmitPoint);
			XEditor.DrawVector3Field("box size:","",BoxSize);
		}
		else if (EmitType.intValue == 2)
		{
			XEditor.DrawVector3Field("sphere center:","",EmitPoint);
			XEditor.DrawFloat("sphere radius:","",Radius);
		}
		else if (EmitType.intValue == 3)
		{
			XEditor.DrawVector3Field("circle center:","",EmitPoint);
			XEditor.DrawVector3Field("circle direction:","",CircleDir);
			XEditor.DrawFloat("circle radius:","",Radius);
			XEditor.DrawToggle("emit uniformly?","",EmitUniform);
		}
		else if (EmitType.intValue == 4)
		{
			XEditor.DrawVector3Field("line center:","",EmitPoint);
			
			XEditor.DrawFloat("left length:","",LineLengthLeft);
			
			XEditor.DrawFloat("right length:","",LineLengthRight);
		}
		else if (EmitType.intValue == 5)
		{
			XEditor.DrawMesh("mesh:","",EmitMesh);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("mesh emit pos:",XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
			EmitMeshType.intValue = (int)(MESHEMIT_TYPE)EditorGUILayout.EnumPopup((MESHEMIT_TYPE)EmitMeshType.intValue);
			EditorGUILayout.EndHorizontal();
			XEditor.DrawToggle("emit uniformly?","",EmitUniform);
		}
		
		XEditor.DrawSeparator();
		
		XEditor.DrawInt("max nodes:", TMaxENodes,MaxENodes);
		XEditor.DrawToggle("is node life infinite?", TIsNodeLifeLoop,IsNodeLifeLoop);
		if (!IsNodeLifeLoop.boolValue)
		{
			XEditor.DrawFloat("node life min:","",NodeLifeMin);
			XEditor.DrawFloat("node life max:","",NodeLifeMax);
		}
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("emit method:",XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
		EmitWay.enumValueIndex = (int)(EEmitWay)EditorGUILayout.EnumPopup((EEmitWay)EmitWay.enumValueIndex);
		EditorGUILayout.EndHorizontal();
		
		if ((EEmitWay)EmitWay.enumValueIndex == EEmitWay.ByDistance)
		{
			XEditor.DrawFloat("diff distance:", "",DiffDistance);
		}
		else if ((EEmitWay)EmitWay.enumValueIndex == EEmitWay.ByRate)
		{
			XEditor.DrawSlider("chance:",TChanceToEmit,ChanceToEmit,1,100);
			XEditor.DrawFloat("emit duration:", TEmitDuration,EmitDuration);
			XEditor.DrawFloat("emit rate:",TEmitRate, EmitRate);
			XEditor.DrawInt("loop count(-1 infinite):",TEmitLoop,EmitLoop);
			XEditor.DrawFloat("delay after each loop:",TEmitDelay,EmitDelay);
		}
		else
		{
			XEditor.DrawCurve("emit curve:","",EmitterCurve);
		}
		
		XEditor.EndXArea();
	}
	
	void DrawRibbonTrail()
	{
		XEditor.BeginXArea("Ribbon Trail",XArea,XEditorTool.MinHeight,XEditorTool.EArea.AlwaysToggle,null);
		
		if (XEditor.DrawToggle("random start size?",TUseRandomRibbon,UseRandomRibbon))
		{
			XEditor.DrawFloat("width min:","",RibbonWidthMin);
			XEditor.DrawFloat("width max:","",RibbonWidthMax);
			
			XEditor.DrawFloat("length min:","",RibbonLenMin);
			XEditor.DrawFloat("length max:","",RibbonLenMax);
		}
		else
		{
			XEditor.DrawFloat("width:",TRibbonWidth,RibbonWidth);
			XEditor.DrawFloat("trail length:",TRibbonLen,RibbonLen);
		}
		
		XEditor.DrawSeparator();
		
		XEditor.DrawInt("max elements(>=3):",TMaxRibbonElements,MaxRibbonElements);
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(new GUIContent("uv stretch:",TStretchType),XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
		StretchType.intValue = (int)(STRETCH_TYPE)EditorGUILayout.EnumPopup((STRETCH_TYPE)StretchType.intValue);
		EditorGUILayout.EndHorizontal();
		
		if (XEditor.DrawToggle("slash trail?",TFaceToObject,FaceToObject))
		{
			XEditor.DrawTransform("face to object:",TFaceObject,FaceObject);
		}
		
		XEditor.EndXArea();
	}
	
	
	void DrawSpriteConfig()
	{
		XEditor.BeginXArea("Sprite",XArea,XEditorTool.MinHeight,XEditorTool.EArea.AlwaysToggle,null);
		EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("sprite type:",TSpriteType),XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
        SpriteType.intValue = (int)(STYPE)EditorGUILayout.EnumPopup((STYPE)SpriteType.intValue);
        EditorGUILayout.EndHorizontal();

		if (SpriteType.intValue == 1 || SpriteType.intValue == 2)
		{
			XEditor.DrawInfo("the heading direction is based on 'Direction Config'");
		}
		
		if (SpriteType.intValue == 1)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(new GUIContent("uv stretch:",TSpriteUVStretch),XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
			SpriteUVStretch.intValue = (int)(STRETCH_TYPE)EditorGUILayout.EnumPopup((STRETCH_TYPE)SpriteUVStretch.intValue);
			EditorGUILayout.EndHorizontal();
		}
		
		EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("pivot",TOriPoint),XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
        OriPoint.intValue = (int)(ORIPOINT)EditorGUILayout.EnumPopup((ORIPOINT)OriPoint.intValue);
        EditorGUILayout.EndHorizontal();
		
		XEditor.DrawFloat("width:",TSpriteWidth,SpriteWidth);
		XEditor.DrawFloat("height:",TSpriteHeight,SpriteHeight);
		
		
		XEditor.EndXArea();
	}
	
	
	
	
	void OnEnable()
	{
		Script = target as EffectLayer;
		InitSerializedProperty();
		LoadStyle();
	}
	
	
	void DrawMainConfig()
	{
		XEditor.BeginXArea("Effect Layer",XArea,XEditorTool.MinHeightBig,XEditorTool.EArea.Texture,null);
		
		XEditor.DrawMaterial("material",TMaterial,Material);
		XEditor.DrawInt("depth:",TDepth,Depth);
		
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("render type:",TRenderType),XLabelField,GUILayout.Width(XEditorTool.LabelWidth));
        RenderType.intValue = (int)(RENDER_TYPE)EditorGUILayout.EnumPopup((RENDER_TYPE)RenderType.intValue);
        EditorGUILayout.EndHorizontal();
		
		XEditor.DrawFloat("delay(s):",TStartTime,StartTime);
		XEditor.DrawColor("debug color:",TDebugColor,DebugColor);
		
		XEditor.DrawTransform("client:",TClientTransform,ClientTransform);
		XEditor.DrawToggle("sync pos to client?",TSyncClient,SyncClient);

	
		
		XEditor.EndXArea();
	}
	
	
	public override void OnInspectorGUI ()
	{
		serializedObject.Update();
		
		DrawMainConfig();
		
		if (RenderType.intValue == 0)
			DrawSpriteConfig();
		else if (RenderType.intValue == 1)
			DrawRibbonTrail();
		else if (RenderType.intValue == 2)
			DrawConeConfig();
		else
			DrawCustomMesh();
		
		DrawEmitterConfig();
		
		DrawDirectionConfig();
		
		DrawUVConfig();
		
		if (RenderType.intValue != 1)
		{
			DrawRotationConfig();
			DrawScaleConfig();
		}
		
		DrawColorConfig();
		
		DrawAffectors();
		
		DrawCollision();
		
		serializedObject.ApplyModifiedProperties();
	}
}
