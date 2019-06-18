using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShapeBehavior : MonoBehaviour {

    public abstract void GameUpdate(Shape shape);
    public abstract void Save(GameDataWriter write);
    public abstract void Load(GameDataReader reader);

}
