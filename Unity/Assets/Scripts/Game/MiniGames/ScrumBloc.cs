using UnityEngine;
using System.Collections.Generic;

/**
  * @class ScrumBloc
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class ScrumBloc : MonoBehaviour
{
    public Animator north;
    public Animator south;
    public ParticleSystem[] northSmoke;
    public ParticleSystem[] southSmoke;

    private Dictionary<ParticleSystem, float> smokeDurations;
    private List<ParticleSystem> activeSmokes;

    public GameObject prefabHit;
    public Transform [] hitParents;

    public float minDurationSmoke;
    public float maxDurationSmoke;

    [HideInInspector]
    public Vector3 idealPosition;

    [HideInInspector]
    public int nFrame;

    public ScrumBloc()
    {
        activeSmokes = new List<ParticleSystem>();
        smokeDurations = new Dictionary<ParticleSystem, float>();
    }

    public void HitFeedbackRecursively(int i = 0)
    {
        int l = this.hitParents.Length;
        
        GameObject g = GameObject.Instantiate(prefabHit) as GameObject;
        g.transform.parent = this.hitParents[i];
        g.transform.localPosition = Vector3.zero;
        g.SetActive(true);

        Timer.AddTimer(1.5f, () =>
        {
            GameObject.Destroy(g);            
        });

        if (i + 1 < l)
        {
            Timer.AddTimer(0.2f, () =>
            {
                HitFeedbackRecursively(i + 1);
            });
        }        
    }

    public void PushFor(Team t)
    {
        this.HitFeedbackRecursively();

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

        if (smoothPosition)
            UpdatePosition();

        UpdateSmokes();
    }

    const float speed = 0.2f;

    [HideInInspector]
    public bool smoothPosition;

    private void OnEnable()
    {
        smoothPosition = false;

        foreach (ParticleSystem smoke in northSmoke)
        {
            smoke.enableEmission = false;
        }

        foreach (ParticleSystem smoke in southSmoke)
        {
            smoke.enableEmission = false;
        }

        activeSmokes.Clear();
        smokeDurations.Clear();
    }

    public void UpdatePosition()
    {
        //this.transform.position = Vector3.SmoothDamp(this.transform.position, this.idealPosition, ref speed, 0.9f);
        this.transform.position = Vector3.Lerp(this.transform.position, this.idealPosition, Time.deltaTime * speed);
    }

    public void FeedBackSmash(Team t)
    {
        float duration = Random.Range(minDurationSmoke, maxDurationSmoke);
        ParticleSystem[] smokes = (t == t.game.northTeam) ? northSmoke : southSmoke;

        if (smokes.Length == 0)
            return;

        int playerSmoke = Random.Range(0, smokes.Length);

        ParticleSystem smoke = smokes[playerSmoke];

        if (activeSmokes.Contains(smoke))
        {
            smokeDurations[smoke] = duration;
        }
        else
        {
            smokeDurations.Add(smoke, duration);
            smoke.enableEmission = true;
            activeSmokes.Add(smoke);
        }
    }

    public void UpdateSmokes()
    {
        foreach (ParticleSystem smoke in activeSmokes)
        {
            smokeDurations[smoke] -= Time.deltaTime;
            if (smokeDurations[smoke] <= 0)
            {
                smoke.enableEmission = false;
                smokeDurations.Remove(smoke);
                activeSmokes.Remove(smoke);
                break;
            }
        }
    }
}
