using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITaskAgent<T> where T:ITask{

	T Task { get; }
    void Initialize();
    void Update(float elapseSeconds, float realElapseSeconds);

    void Shutdown();

    void Start(T task);

    void Reset();
}
