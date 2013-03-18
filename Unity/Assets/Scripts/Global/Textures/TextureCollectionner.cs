using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NamedTexture {
	public string name;
	public Texture texture;
}

/**
  * @class TextureCollectionner
  * @brief Collects textures.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
[AddComponentMenu("Scripts/Textures/Collectionner")]
public class TextureCollectionner : MonoBehaviour {
		
	public NamedTexture [] collection;
	public Renderer target;
	
	public Texture GetTextureByName(string name) {
		for(int i = 0; i < collection.Length; i++) {
			if(collection[i].name.Equals(name)) {
				return collection[i].texture;	
			}
		}
		
		return null;
	}
		
	public bool ApplyTexture(string name) {
		Texture t = GetTextureByName(name);
	
		if(t == null) {
			return false;
		}
		
		if(target == null)
			target = this.renderer;
		
		target.renderer.material.mainTexture = t;
			
		return true;
	}
}
