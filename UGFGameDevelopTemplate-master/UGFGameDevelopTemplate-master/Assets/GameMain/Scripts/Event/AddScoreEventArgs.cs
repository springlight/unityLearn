using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScoreEventArgs : GameEventArgs
{
    public static readonly int EvtId = typeof(AddScoreEventArgs).GetHashCode();
    public override int Id
    {
        get
        {
            return EvtId;
        }
    }


    public int AddCount { get; private set; }

    public AddScoreEventArgs Fill(int addCount)
    {
        AddCount = addCount;
        return this;
    }

    public override void Clear()
    {
        AddCount = 0;
    }
}
