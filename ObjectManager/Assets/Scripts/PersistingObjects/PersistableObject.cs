using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 读写顺序必须一致
/// </summary>
[DisallowMultipleComponent]
public class PersistableObject : MonoBehaviour
{
    /// <summary>
    /// 写文件
    /// </summary>
    /// <param name="writer"></param>
    public virtual void Save(GameDataWriter writer)
    {
        writer.Write(transform.localPosition);
        writer.Write(transform.localRotation);
        writer.Write(transform.localScale);
    }

    public virtual void Load(GameDataReader reader)
    {
        transform.localPosition = reader.ReadVector3();
        transform.localRotation = reader.ReadQuaternion();
        transform.localScale = reader.ReadVector3();
    }
}
