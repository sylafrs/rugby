using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Xft;
using UnityEditor;

//patch for version below 3.0.0
public class XffectPatch300 : Editor 
{
	
	[MenuItem("Window/Xffect/Patch/Ver3.0.0/Patch Selected Object")]
	public static void PatchForSelectObject()
	{
		GameObject curSelect = Selection.activeGameObject;
		
		EditorUtility.DisplayProgressBar("Xffect Patch", "patching:" +curSelect.name + ", please wait...", 0.95f);
		
		DoPatch(curSelect.transform);
		
		EditorUtility.ClearProgressBar();
	}
	
	
	[MenuItem("Window/Xffect/Patch/Ver3.0.0/Patch Current Project")]
	public static void PatchForPrefabs()
	{
		// check all prefabs to see if we can find any objects we are interested in
		List<string> allPrefabPaths = new List<string>();
		Stack<string> paths = new Stack<string>();
		paths.Push(Application.dataPath);
		while (paths.Count != 0)
		{
			string path = paths.Pop();
			string[] files = Directory.GetFiles(path, "*.prefab");
			foreach (var file in files)
			{
				allPrefabPaths.Add(file.Substring(Application.dataPath.Length - 6));
			}
			
			foreach (string subdirs in Directory.GetDirectories(path)) 
				paths.Push(subdirs);
		}	
		
		// Check all prefabs
		int currPrefabCount = 1;
		foreach (string prefabPath in allPrefabPaths)
		{
			EditorUtility.DisplayProgressBar("Xffect Patch", "Searching and patching xffect prefabs in project folder, please wait...", (float)currPrefabCount / (float)(allPrefabPaths.Count));
			
			GameObject iterGo = AssetDatabase.LoadAssetAtPath( prefabPath, typeof(GameObject) ) as GameObject;
			if (!iterGo) continue;
			
			if (DoPatch(iterGo.transform))
			{
				//IMPORTANT: MAKE THIS PREFAB TO BE RE-IMPORTED!
				EditorUtility.SetDirty(iterGo);
			}
				

			++currPrefabCount;

			//if (currPrefabCount % 100 == 0)
			{
                iterGo = null;
				EditorUtility.UnloadUnusedAssets();
				System.GC.Collect();
			}
		}
		
		
		// unload all unused assets
		EditorUtility.UnloadUnusedAssets();
		System.GC.Collect();
		
		EditorUtility.DisplayProgressBar("Xffect Patch", "Saving...", 1f);
		AssetDatabase.Refresh();
		AssetDatabase.SaveAssets();
		
		EditorUtility.ClearProgressBar();
	}

	public static bool DoPatch(Transform xffect)
	{
		bool needToReimport = false;
		Object[] deps = EditorUtility.CollectDeepHierarchy (new Object[]{xffect.gameObject as Object});
		
		foreach(Object obj in deps)
		{
			if (obj == null || obj.GetType() != typeof(GameObject))
				continue;
			GameObject go = obj as GameObject;
			EffectLayer efl = go.GetComponent<EffectLayer>();
			if (efl != null)
			{
				DoPatch(efl);
				needToReimport = true;
			}
			
			XftEventComponent xevent = go.GetComponent<XftEventComponent>();
			if (xevent != null)
			{
				DoPatch(xevent);
				needToReimport = true;
			}
		}
		
		return needToReimport;
	}
	
	public static string GetPath(Transform current)
	{
		if (current.parent == null)
			return "/" + current.name;
		
		return GetPath(current.parent) + "/" + current.name;
	}
	
	
	public static void DoPatch(XftEventComponent xevent)
	{
		if (xevent.EventType == XftEventType.CameraGlow && xevent.GlowColorGradualType != COLOR_GRADUAL_TYPE.CURVE)
		{
			Debug.LogWarning("[upgrade note]color gradual type is deprecated in glow event, please re-fill the gradual curve:" + GetPath(xevent.transform));
		}
		
		if (xevent.EventType == XftEventType.CameraColorInverse)
		{
			xevent.Type = XEventType.CameraEffect;
			xevent.CameraEffectType = CameraEffectEvent.EType.ColorInverse;
		}
		else if (xevent.EventType == XftEventType.CameraGlow)
		{
			xevent.Type = XEventType.CameraEffect;
			xevent.CameraEffectType = CameraEffectEvent.EType.Glow;
		}
		else if (xevent.EventType == XftEventType.CameraRadialBlur)
		{
			xevent.Type = XEventType.CameraEffect;
			xevent.CameraEffectType = CameraEffectEvent.EType.RadialBlur;
		}
		else if (xevent.EventType == XftEventType.CameraRadialBlurMask)
		{
			xevent.Type = XEventType.CameraEffect;
			xevent.CameraEffectType = CameraEffectEvent.EType.RadialBlurMask;
		}
		else if (xevent.EventType == XftEventType.CameraShake && xevent.Type == XEventType.CameraShake)
		{
			xevent.Type = XEventType.CameraShake;
		}
		else if (xevent.EventType == XftEventType.Light)
		{
			xevent.Type = XEventType.Light;
		}
		else if (xevent.EventType == XftEventType.Sound)
		{
			xevent.Type = XEventType.Sound;
		}
		else if (xevent.EventType == XftEventType.TimeScale)
		{
			xevent.Type = XEventType.TimeScale;
		}
	}
	
	public static void DoPatch(EffectLayer layer)
	{
		Debug.Log("patching for effect layer:" + GetPath(layer.transform));
		
		if (layer.ColorAffectType == 0)
		{
			layer.ColorChangeType = COLOR_CHANGE_TYPE.Constant;
			layer.ColorAffectorEnable = false;
		}
		else if (layer.ColorAffectType == 1)
		{
			layer.ColorParam.Colors.Clear();
			
			layer.ColorParam.AddColorKey(0f,layer.Color1);
			layer.ColorParam.AddColorKey(1f,layer.Color2);
			
			layer.ColorChangeType = COLOR_CHANGE_TYPE.Gradient;
			
			layer.ColorAffectorEnable = true;
		}
		else if (layer.ColorAffectType == 2)
		{
			layer.ColorParam.Colors.Clear();
			layer.ColorParam.AddColorKey(0f,layer.Color1);
			layer.ColorParam.AddColorKey(0.25f,layer.Color2);
			layer.ColorParam.AddColorKey(0.5f,layer.Color3);
			layer.ColorParam.AddColorKey(0.75f,layer.Color4);
			layer.ColorParam.AddColorKey(1f,layer.Color5);
			
			layer.ColorChangeType = COLOR_CHANGE_TYPE.Gradient;
			layer.ColorAffectorEnable = true;
		}
		
		if (layer.Material == null)
		{
			layer.Material = AssetDatabase.LoadAssetAtPath(XEditorTool.GetXffectPath() + XffectComponentCustom.DefaultMatPath, 
				typeof(Material)) as Material;
		}
	}
	
	
}
