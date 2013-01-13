using UnityEngine;
using System.Collections;

/**
  * @interface Debugable
  * @brief Interface à faire implémenter par les Components qui veulent être affichés dans DebugWindow
  * @see DebugWindow
  * @author Sylvain Lafon  
  */
public interface Debugable
{
    /// Ce qui sera affiché dans la DebugWindow.
    void ForDebugWindow();

    /*
     *  Vous pourrez ainsi afficher des informations et faire
     *  des entrées utilisateurs pour débugger sans vous prendre la tête !
     */

    /*
     * Exemple :
     * 
     * public void ForDebugWindow() {
     *     #if UNITY_EDITOR
     *          EditorGUILayout.LabelField("Plop, je m'appelle : " + this.name);
     *     #endif
     * }
     */
}