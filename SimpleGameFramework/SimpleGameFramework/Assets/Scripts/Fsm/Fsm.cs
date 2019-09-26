

using System;

public class Fsm<T> : IFsm where T : class
{
    public string Name { get; private set; }
  

    public Type OwnerType
    {
        get { return typeof(T); }
    }

    public bool IsDestroy { get; private set; }

    public float CurStateTime { get; private set; }

    public void Shutdown()
    {
        
    }

    public void Update(float elapseSec, float realElapseSec)
    {
        
    }
}
