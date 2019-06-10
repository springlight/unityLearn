using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : PersistableObject
{
    [SerializeField]
    MeshRenderer[] meshRenderers;
    public Vector3 AngularVelocity { get; set; }
    public Vector3 Velocity { get; set; }
    int shapeId = int.MinValue;
    public int MaterialId { get; private set; }
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
       // writer.Write(color);version = 4
       for(int i =0; i < colors.Length; i++)
        {
            writer.Write(colors[i]);
        }
        writer.Write(AngularVelocity);
        writer.Write(Velocity);
    }
    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        if(reader.version >= 5)
        {
            for(int i =0; i <colors.Length; i++)
            {
                SetColor(reader.ReadColor(), i);
            }
        }
        else
        {
            SetColor(reader.version > 0 ? reader.ReadColor() : Color.white);
        }
      
        AngularVelocity = reader.version >= 4 ? reader.ReadVector3() : Vector3.zero;
        Velocity = reader.version >= 4 ? reader.ReadVector3() : Vector3.zero;

    }

    //private void FixedUpdate()
    public void GameUpdate()
    {
        transform.Rotate(AngularVelocity * Time.deltaTime);
        transform.localPosition += Velocity * Time.deltaTime;
    }
}
