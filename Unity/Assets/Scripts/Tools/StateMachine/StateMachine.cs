using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

/**
 * @class StateMachine
 * @brief Machine d'états finis (partie globale)
 * @author Sylvain Lafon
 */
[AddComponentMenu("Scripts/AI/StateMachine")]
public partial class StateMachine : myMonoBehaviour, Debugable {

    protected System.Collections.Generic.List<State> list = new System.Collections.Generic.List<State>();
    public State BaseState;

    public int GetStackLength()
    {
        return list.Count;
    }

    public string GetStateName(int i)
    {
        if (i < list.Count && i >= 0) 
          return list[i].GetName();
       
        return null;
    }
 
    public void SetFirstState(State first)
    {
        if (list.Count > 0)
            throw new System.ApplicationException("SetFirstState(" + first + ") : " + list[0] + " �tait d�j� l� ! ");
        list.Add(first);
        list[0].OnEnter();
    }

    // Update is called once per frame
    void Update()
    {
        System.Collections.Generic.List<State> listcopy;

        listcopy = new System.Collections.Generic.List<State>(list);
        foreach (State tmp in listcopy)
        {
            if (list.Contains(tmp))
                tmp.OnUpdate();
        }
    }

    public void state_change_me(State current, State newstate)
    {
        int index = list.IndexOf(current);
        if (index == -1)
        {
            Debug.LogError("state_change_me(" + current + ", " + newstate + ") : " + current + " introuvable !");
            return;
        }
        while (index > 0)
        {
            list[index].OnLeave();
            index--;            
        }
        current.OnLeave();
        index = list.IndexOf(current);
        if (index == -1)
        {
            Debug.LogError("state_change_son(" + current + ", " + newstate + ") : " + current + ".OnLeave() ne doit pas changer l'�tat !");
            return;
        }
        list.RemoveRange(0, index + 1);
        list.Insert(0, newstate);
        newstate.OnEnter();
        index = list.IndexOf(newstate);
        if (index != -1 && list.Count > index + 1)
            list[index + 1].OnChildChanged();
    }

    public void state_change_son(State current, State newstate)
    {
        int index = list.IndexOf(current);
        if (index == -1)
        {
            Debug.LogError("state_change_son(" + current + ", " + newstate + ") : " + current + " introuvable !");
            return;
        }
        while (index > 0)
        {
            index--; 
            list[index].OnLeave();                       
        }
        index = list.IndexOf(current);
        if (index == -1)
        {
            Debug.LogError("state_change_son(" + current + ", " + newstate + ") : " + current + ".OnLeave() ne doit pas changer l'�tat !");
            return;
        }
        list.RemoveRange(0, index);
        list.Insert(0, newstate);
        newstate.OnEnter();
    }

    public void state_kill_me(State current)
    {
        int index = list.IndexOf(current);
        if (index == -1)
        {
            Debug.LogError("state_kill_me(" + current + ") : " + current + " introuvable !");
            return;
        }
        while (index > 0)
        {
            index--;
            list[index].OnLeave();
        }
        current.OnLeave();
        index = list.IndexOf(current);
        if (index == -1)
        {
            Debug.LogError("state_kill_me(" + current + ") : " + current + ".OnLeave() ne doit pas changer l'�tat !");
            return;
        }
        list.RemoveRange(0, index + 1);
        if (list.Count > 0)
            list[0].OnChildLeaved();
        else
            Debug.LogError("state_kill_me(" + current + ") : Attends, la liste est vide !!! AAAAAAAAAHHH !!!!!!!!!!");
    }

    public void ForDebugWindow()
    {
#if UNITY_EDITOR
        int i = 0;
        string str = this.GetStateName(i);
        while (str != null)
        {
            EditorGUILayout.LabelField(str);       
            str = this.GetStateName(++i);
        }     
#endif
    }
}
