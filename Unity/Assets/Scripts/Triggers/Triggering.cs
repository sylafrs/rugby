using UnityEngine;
using System.Collections;

public interface Triggering
{
    void Entered(Triggered o, Trigger t);
    void Inside(Triggered o, Trigger t);
    void Left(Triggered o, Trigger t);
}



