using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTestAgrs : GlobalEventArgs
{
    public string m_Name;
    public override int Id
    {
        get
        {
           return 1;
        }
    }

    public override void Clear()
    {
        m_Name = string.Empty;
    }


    public EventTestAgrs Fill(string name)
    {
        m_Name = name;
        return this;
    }
}
