using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnMenuEvenArgs : GameEventArgs
{

    public static readonly int EventId = typeof(ReturnMenuEvenArgs).GetHashCode();

    public override int Id
    {
        get
        {
            return EventId;
        }
    }

    public override void Clear()
    {
    }
}
