using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCheck : PostEffectBase
{
    public Shader edgeCheckShader;
    private Material edgeCheckMaterial;

    [Range(0.0f, 1.0f)]
    public float edgesOnly = 0.0f;
    public Color edgeColor = Color.black;
    public Color backgroundColor = Color.white;
    public Material material
    {
        get
        {
            edgeCheckMaterial = CheckShaderAndCreateMaterial(edgeCheckShader, edgeCheckMaterial);
            return edgeCheckMaterial;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(material != null)
        {
            material.SetFloat("_EdgeOnly", edgesOnly);
            material.SetColor("_EdgeColor", edgeColor);
            material.SetColor("_BackgroundColor", backgroundColor);
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
