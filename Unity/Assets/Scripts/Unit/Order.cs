using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Order  {
    public enum TYPE
    {
        RIEN,
        DEPLACER,
        PASSER,
        SHOOTER,
        PLAQUER,
        SUIVRE,
        ATTAQUER,
        PRESSER,
        ASSISTER
    }
    public enum TYPE_DEPLACEMENT
    {
        MARCHE,
        COURSE,
        SPRINT
    }

    public TYPE type;
    public TYPE_DEPLACEMENT deplacement;
    public Unit target;
    public float power;
}
