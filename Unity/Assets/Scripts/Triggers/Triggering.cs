using UnityEngine;
using System.Collections;

/**
 * @interface Triggering
 * @brief Interface de la classe qui gèrera le déclenchement
 * @author Sylvain Lafon
 */
public interface Triggering
{
    void Entered(Triggered o, Trigger t);
    void Inside(Triggered o, Trigger t);
    void Left(Triggered o, Trigger t);
}



