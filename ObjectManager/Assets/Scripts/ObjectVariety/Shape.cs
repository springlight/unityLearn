using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : PersistableObject
{
    [SerializeField]
    MeshRenderer[] meshRenderers;
    //public Vector3 AngularVelocity { get; set; }
    //public Vector3 Velocity { get; set; }
    int shapeId = int.MinValue;
    List<ShapeBehavior> behaviorList = new List<ShapeBehavior>();
    public int MaterialId { get; private set; }

    public float Age { get; private set; }
    public int ShapeId
    {
        get { return shapeId; }
        set
        {
            if(shapeId == int.MinValue &&value != int.MinValue)
            {
                shapeId = value;
            }
            else
            {
                Debug.LogError("Not allowed to change Id");
            }
        }
    }
    //   private Color color;
    Color[] colors;
  //  MeshRenderer meshRenderer;
    static int colorPropertyId = Shader.PropertyToID("_Color");
    static MaterialPropertyBlock sharedPropertyBlock;
    //void Awake()
    //{
    //    meshRenderer = GetComponent<MeshRenderer>();
    //}

    private void Awake()
    {
        colors = new Color[meshRenderers.Length];
    }

    private ShapeFactory oriFactory;
    public ShapeFactory OriginFactory
    {
        get { return oriFactory; }
        set
        {
            if (oriFactory == null)
                oriFactory = value;
            else
                Debug.LogError("Not allowed to change factory.");
        }
    }

    public void SetMaterial(Material material,int materialId)
    {
        for(int i = 0; i <meshRenderers.Length; i++)
        {
            meshRenderers[i].material = material;
        }
     
        MaterialId = materialId;
    }

    public void SetColor(Color color)
    {
       // this.color = color;

    
        if(sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
        sharedPropertyBlock.SetColor(colorPropertyId, color);
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            colors[i] = color;
            meshRenderers[i].SetPropertyBlock(sharedPropertyBlock);
        }
      
      //  meshRenderer.material.color = color;
    }

    public void SetColor(Color color,int index)
    {
        if(sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
        sharedPropertyBlock.SetColor(colorPropertyId, color);
        colors[index] = color;
        meshRenderers[index].SetPropertyBlock(sharedPropertyBlock);

    }

    public int ColorCount { get { return colors.Length; } }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(colors.Length);
       // writer.Write(color);version = 4
       for(int i =0; i < colors.Length; i++)
        {
            writer.Write(colors[i]);
        }
        //writer.Write(AngularVelocity);
        //writer.Write(Velocity);
        writer.Write(Age);
        writer.Write(behaviorList.Count);
        for(int i = 0; i <behaviorList.Count; i++)
        {
            writer.Write((int)behaviorList[i].BehaviorType);
            behaviorList[i].Save(writer);
        }
    }
    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        if(reader.version >= 5)
        {
            LoadColors(reader);
        }
        else
        {
            SetColor(reader.version > 0 ? reader.ReadColor() : Color.white);
        }
      
        //AngularVelocity = reader.version >= 4 ? reader.ReadVector3() : Vector3.zero;
        //Velocity = reader.version >= 4 ? reader.ReadVector3() : Vector3.zero;
        if(reader.version >= 6)
        {
            Age = reader.ReadFloat();
            int behaviorCnt = reader.ReadInt();
            for(int i = 0; i < behaviorCnt; i++)
            {
                //   AddBehavior((ShapeBehaviorType)reader.ReadInt()).Load(reader);
                ShapeBehavior behavior = ((ShapeBehaviorType)reader.ReadInt()).GetInstance();
                behaviorList.Add(behavior);
                behavior.Load(reader);
            }
        }
        else if(reader.version >= 4)
        {
            AddBehavior<RotationShapeBehavior>().AngularVelocity = reader.ReadVector3();
            AddBehavior<MovementShapeBehavior>().Velocity = reader.ReadVector3();
        }

    }

    private void LoadColors(GameDataReader reader)
    {
        int count = reader.ReadInt();
        int max = count <= colors.Length ? count : colors.Length;
        int i = 0;
        for (; i < max; i++)
        {
            SetColor(reader.ReadColor(), i);
        }
        if(count > max)//存储的数量大于需要的
        {
            for(;i< count; i++)
            {
                reader.ReadColor();//把多余的取出来
            }
        }
        else if( count <max)
        {
            for(; i < max; i++)
            {
                SetColor(Color.white, i);
            }
        }
    }

    //private void FixedUpdate()
    public void GameUpdate()
    {
        //transform.Rotate(AngularVelocity * Time.deltaTime);
        //transform.localPosition += Velocity * Time.deltaTime;
        Age += Time.deltaTime;
        for(int i = 0; i < behaviorList.Count; i++)
        {
            behaviorList[i].GameUpdate(this);
        }
    }

    public void Recycle()
    {
        Age = 0f;
        for(int i =0; i < behaviorList.Count; i++)
        {
            behaviorList[i].Recycle();
           // Destroy(behaviorList[i]);
        }
        behaviorList.Clear();
        OriginFactory.Reclaim(this);
    }

    //public void AddBehavior(ShapeBehavior behavior)
    //{
    //    behaviorList.Add(behavior);
    //}

    public T AddBehavior<T>()where T: ShapeBehavior,new()
    {
        //T behavior = gameObject.AddComponent<T>();
        //behaviorList.Add(behavior);
        T behavior = ShapeBehaviorPool<T>.Get();
        behaviorList.Add(behavior);
        return behavior;
    }

    //private ShapeBehavior AddBehavior(ShapeBehaviorType type)
    //{
    //    switch (type)
    //    {
    //        case ShapeBehaviorType.Movement:
    //            return AddBehavior<MovementShapeBehavior>();
    //        case ShapeBehaviorType.Rotation:
    //            return AddBehavior<RotationShapeBehavior>();
    //    }
    //    return null;
    //}
}


