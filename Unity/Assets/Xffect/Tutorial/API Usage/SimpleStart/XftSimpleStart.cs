using UnityEngine;
using System.Collections;

public class XftSimpleStart : MonoBehaviour {
 
    public XffectComponent YourEffect;
    
	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = Instantiate(YourEffect.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
            XffectComponent fireEffect = go.GetComponent<XffectComponent>();
            //after you instantiate a Xffect Object, you need to call Active() to activate it
            //and when there are no active nodes in the scene, this Xffect Object will automatically become non-active
            fireEffect.Active();
            
            //if you fired a loop Xffect Object
            //  fireEffect.Deactive();
        }
	}
    
    
    void OnGUI()
    {
        GUI.Label(new Rect(150, 0, 350, 25), "just click mouse button to instantiate a new Effect!");
    }
}
