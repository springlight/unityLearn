using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField]
    Shape[] prefabs;
    [SerializeField]
    Material[] materials;
    [SerializeField]
    bool recycle;
    Scene poolScene;
    List<Shape>[] pools;//每一个类型一个数组
    public Shape Get(int shapeId = 0,int materialId = 0)
    {
        Shape instance = null;
        if (recycle)
        {
            
            if(pools == null)
            {
                Debug.LogError("开始创建对象池");
                CreatePools();
            }
            List<Shape> pool = pools[shapeId];
            int lastIdx = pool.Count - 1;
            if(lastIdx >= 0)
            {
                instance = pool[lastIdx];
                instance.gameObject.SetActive(true);
                pool.RemoveAt(lastIdx);
            }
            else
            {
                instance = Instantiate(prefabs[shapeId]);
                instance.ShapeId = shapeId;
                //把创建的shape移动到缓存场景
                SceneManager.MoveGameObjectToScene(instance.gameObject, poolScene);
            }

        }
        else
        {
            instance = Instantiate(prefabs[shapeId]);
            instance.ShapeId = shapeId;
        }
       
        instance.SetMaterial(materials[materialId], materialId);
        return instance;
    }

    public Shape GetRandom()
    {
        return Get(Random.Range(0, prefabs.Length), Random.Range(0, materials.Length));
    }
    //重新编辑之后，代码会再次调用一遍，重新编译会让pool置成null
    void CreatePools()
    {
        pools = new List<Shape>[prefabs.Length];
        for(int i = 0;i< pools.Length; i++)
        {
            pools[i] = new List<Shape>();
        }
        if (Application.isEditor)
        {
            poolScene = SceneManager.GetSceneByName(name);
            //重新编译之后，pool为空，会重新创建pool。所以要把重新编译之后的
            //shape重新加入到pool中

            if (poolScene.isLoaded)
            {
                GameObject[] rootObjects = poolScene.GetRootGameObjects();
                for(int i = 0; i < rootObjects.Length; i++)
                {
                    Shape pooledShape = rootObjects[i].GetComponent<Shape>();
                    if(!pooledShape.gameObject.activeSelf)
                     pools[pooledShape.ShapeId].Add(pooledShape);
                }
                return;
            }
               
        }
      
        poolScene = SceneManager.CreateScene(name);


    }

    public void Reclaim(Shape shapeToRecycle)
    {
        if (recycle)
        {
            if (pools == null)
            {
                CreatePools();
            }
            shapeToRecycle.gameObject.SetActive(false);
            pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
        }
        else
        {
            Destroy(shapeToRecycle.gameObject);
        }
    }
}
