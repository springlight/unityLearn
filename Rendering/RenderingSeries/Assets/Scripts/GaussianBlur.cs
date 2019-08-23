﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianBlur : PostEffectBase
{
    public Shader gaosiShader;
    private Material gaosiMaterial;

    public Material material
    {
        get
        {
            gaosiMaterial = CheckShaderAndCreateMaterial(gaosiShader, gaosiMaterial);
            return gaosiMaterial;
        }
    }

    [Range(0, 4)]
    public int iterations = 3;//迭代次数
    [Range(0.2f, 0.3f)]
    public float blurSpeed = 0.6f;
    [Range(1, 8)]
    public int downSample = 2;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(material != null)
        {
            int rtW = source.width/downSample;
            int rtH = source.height/downSample;
            RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
            buffer0.filterMode = FilterMode.Bilinear;
            Graphics.Blit(source, buffer0);
            for(int i = 0; i < iterations; i++)
            {
                material.SetFloat("_BlurSize", 1.0f + i * blurSpeed);
                RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
                Graphics.Blit(buffer0, buffer1, material, 0);
                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
                buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);

                Graphics.Blit(buffer0, buffer1, material, 1);
                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;

            }
            Graphics.Blit(buffer0, destination);
            RenderTexture.ReleaseTemporary(buffer0);
            //Graphics.Blit(source, buffer,material, 0);
            //Graphics.Blit(buffer, destination, material, 1);
            //RenderTexture.ReleaseTemporary(buffer);

        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
