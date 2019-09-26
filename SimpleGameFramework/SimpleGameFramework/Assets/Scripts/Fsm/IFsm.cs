
using System;
/// <summary>
/// 状态机接口
/// </summary>
public interface IFsm
{
    /// <summary>
    /// 状态机名字
    /// </summary>
    string Name { get; }
    /// <summary>
    /// 状态机持有者
    /// </summary>
    Type OwnerType { get; }
    /// <summary>
    /// 状态机是否被销毁
    /// </summary>
    bool IsDestroy { get; }
    /// <summary>
    /// 当前状态运行时间
    /// </summary>
    float CurStateTime { get; }

    /// <summary>
    /// 状态机轮询
    /// </summary>
    /// <param name="elapseSec"></param>
    /// <param name="realElapseSec"></param>
    void Update(float elapseSec, float realElapseSec);

    /// <summary>
    /// 关闭并清理状态机
    /// </summary>
    void Shutdown();
}
