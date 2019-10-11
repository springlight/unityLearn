using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask
{

    int SerialId { get; }
    bool Done { get; }
}
