using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Skill> skills = new List<Skill>();


    private void Start()
    {
        skills.Add(new FirePunch(this));
        skills.Add(new KiBlast(this));
    }

    private void Update()
    {
        foreach(Skill skill in skills)
        {
            skill.Update(Time.deltaTime);
        }


    }

    public void ResetSkill()
    {
        foreach(Skill skill in skills)
        {
            skill.Reset();
        }

        Debug.LogError("所有招式都被重置了");
    }
}
