using UnityEngine;
using System.Collections.Generic;

/**
  * @class ScrumBloc
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class ScrumBloc : MonoBehaviour {
    public Animator north;
    public Animator south;

    [HideInInspector]
    public Vector3 idealPosition;

    public int nFrame;

    public void PushFor(Team t)
    {
        if (t == t.game.northTeam)
        {
            north.SetBool("in_bool_push", true);
            south.SetBool("in_bool_fail", true);
        }
        else
        {
            south.SetBool("in_bool_push", true);
            north.SetBool("in_bool_fail", true);
        }

        nFrame = 2;
    }

    public void Update()
    {
        if (nFrame > 0)
        {
            nFrame--;
            if (nFrame == 0)
            {
                south.SetBool("in_bool_push", false);
                north.SetBool("in_bool_push", false);
                south.SetBool("in_bool_fail", false);
                north.SetBool("in_bool_fail", false);
            }
        }

        if(smoothPosition)
            UpdatePosition();
    }

    const float speed = 0.2f;
    public bool smoothPosition;

    private void OnEnable()
    {
        smoothPosition = false;
    }

    public void UpdatePosition()
    {
        //this.transform.position = Vector3.SmoothDamp(this.transform.position, this.idealPosition, ref speed, 0.9f);
        this.transform.position = Vector3.Lerp(this.transform.position, this.idealPosition, Time.deltaTime * speed);
    }
}
