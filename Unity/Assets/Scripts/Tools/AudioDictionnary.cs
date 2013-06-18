using UnityEngine;
using System.Collections.Generic;

/**
  * @class AudioDictionnary
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class AudioDictionnary : MonoBehaviour {

    Dictionary<string, AudioSource> dico;

    public AudioDictionnary()
    {
        dico = new Dictionary<string, AudioSource>();
    }

    public AudioSource this[string index]
    {
        get
        {
            if (dico.ContainsKey(index))
            {
                return dico[index];
            }
            else
            {
                return this.Create(index);
            }
        }
        set
        {
            if (dico.ContainsKey(index))
            {
                dico[index] = value;
            }
            else
            {
                dico.Add(index, value);
            }
        }
    }

    public AudioSource Create(string index)
    {
        if(dico.ContainsKey(index)) {
            return dico[index];
        }

        AudioSource src = this.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        this[index] = src;
        return src;
    }

    public void Destroy(string index)
    {
        AudioSource src = dico[index];
        dico.Remove(index);
        GameObject.Destroy(src);
    }
}
