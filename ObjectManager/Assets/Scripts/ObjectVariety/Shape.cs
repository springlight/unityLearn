using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : PersistableObject
{
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
    private Color color;
    MeshRenderer meshRenderer;
    static int colorPropertyId = Shader.PropertyToID("_Color");
    static MaterialPropertyBlock sharedPropertyBlock;
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    public void SetMaterial(Material material,int materialId)
    {
        meshRenderer.material = material;
        MaterialId = materialId;
    }

    public void SetColor(Color color)
    {
        this.color = color;

       // var propertyBlock = new MaterialPropertyBlock();
        //propertyBlock.SetColor("_Color", color);
        if(sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
        sharedPropertyBlock.SetColor(colorPropertyId, color);
        meshRenderer.SetPropertyBlock(sharedPropertyBlock);
      //  meshRenderer.material.color = color;
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(color);
        writer.Write(AngularVelocity);
        writer.Write(Velocity);
    }
    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        SetColor(reader.version > 0 ? reader.ReadColor() : Color.white);
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
