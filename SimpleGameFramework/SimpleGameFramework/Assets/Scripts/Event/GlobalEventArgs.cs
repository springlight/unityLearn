using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GlobalEventArgs : EventArgs, IReference
{
    /// <summary>
    /// 事件类型Id
    /// </summary>
    public abstract int Id { get; }
    public abstract void Clear();

}
