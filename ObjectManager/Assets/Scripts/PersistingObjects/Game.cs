using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : PersistableObject
{
    public float CreationSpeed { get; set; }
    public float DestructionSpeed { get; set; }
    float creationProgress;
    float destructionProgress;
    public PersistentStorage stroage;
    public ShapeFactory shapeFactory;
    //public PersistableObject prefab;
    public KeyCode createKey = KeyCode.C;
    public KeyCode destroyKey = KeyCode.X;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;
    private List<Shape> shapes;
    private const int saveVersion = 2;
    private string savePath;
    public int lvCnt;
    private int loadedLvIdx;
    private void Awake()
    {
       

    }
    // Use this for initialization
    void Start ()
    {
        shapes = new List<Shape>();
        if (Application.isEditor)
        {
            //只能有一个关卡
            for(int i =0; i < SceneManager.sceneCount; i++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(i);
                if(loadedScene.name.Contains("Level "))
                {
                    Debug.LogError("已经加载的关卡名--->" + loadedScene.name);
                    SceneManager.SetActiveScene(loadedScene);
                    loadedLvIdx = loadedScene.buildIndex;
                    return;
                }
            }
            //Scene loadedLevel = SceneManager.GetSceneByName("Level 1");
            //if (loadedLevel.isLoaded)
            //{
            //    SceneManager.SetActiveScene(loadedLevel);
            //    return;
            //}
            StartCoroutine(LoadLevel(1));
        }
       
    }
    IEnumerator LoadLevel(int lvBuildIdx)
    {
        enabled = false;
        if(loadedLvIdx > 0)
        {
            yield return SceneManager.UnloadSceneAsync(loadedLvIdx);
        }
        //同步加载场景
        //SceneManager.LoadScene("Level 1",LoadSceneMode.Additive);
        //yield return null;
        //异步加载场景
        yield return SceneManager.LoadSceneAsync(lvBuildIdx, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(lvBuildIdx));
        loadedLvIdx = lvBuildIdx;
        enabled = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateShape();
        }
        else if (Input.GetKeyDown(destroyKey))
        {
            DestroyShape();
        }
        else if (Input.GetKeyDown(newGameKey))
        {
            BeginNewGame();
        }
        else if (Input.GetKeyDown(saveKey))
        {
            stroage.Save(this,saveVersion);
        }
        else if (Input.GetKeyDown(loadKey))
        {
            BeginNewGame();
            stroage.Load(this);
        }
        else
        {
            for(int i =1; i <=lvCnt; i++)
            {
     
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    BeginNewGame();
                    StartCoroutine(LoadLevel(i));
                    return;
                }
            }
        }
        creationProgress += Time.deltaTime * CreationSpeed;
        while(creationProgress >= 1f)
        {
            creationProgress -= 1f;
            CreateShape();
        }

        destructionProgress += Time.deltaTime * DestructionSpeed;
        while (destructionProgress >= 1f)
        {
            destructionProgress -= 1f;
            DestroyShape();
        }
    }
    public override void Save(GameDataWriter writer)
    {
        //writer.Write(-saveVersion);
        writer.Write(shapes.Count);
        writer.Write(loadedLvIdx);//保存加载的场景
        for (int i = 0; i < shapes.Count; i++)
        {
            writer.Write(shapes[i].ShapeId);
            writer.Write(shapes[i].MaterialId);
            shapes[i].Save(writer);
        }
    }
    public override void Load(GameDataReader reader)
    {
        ///兼容性处理，因为以前的版本没有保存版本号
        //  int version = -reader.ReadInt();
        int version = reader.version;
        if(version > saveVersion)
        {
            Debug.LogError("Unsupported future save version " + version);
            return;
        }
        int count = version <=0 ? -version:reader.ReadInt();
        StartCoroutine(LoadLevel(version < 2 ? 1 : reader.ReadInt()));
        for (int i = 0; i < count; i++)
        {
            //也是做兼容性处理
            int shapeId = version > 0 ? reader.ReadInt():0;
            int materialId = version > 0 ? reader.ReadInt() : 0;
            Shape shape = shapeFactory.Get(shapeId,materialId);
            shape.Load(reader);
            shapes.Add(shape);
        }
    }
    void BeginNewGame()
    {
        for(int i = 0; i < shapes.Count; i++)
        {
            // Destroy(shapes[i].gameObject);
            shapeFactory.Reclaim(shapes[i]);
        }
        shapes.Clear();
    }

    void CreateShape()
    {
        //   PersistableObject o = Instantiate(prefab);
        Shape shape = shapeFactory.GetRandom();
        Transform t = shape.transform;
        t.localPosition = Random.insideUnitSphere * 5;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        shape.SetColor(Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.25f, 1f, 1f, 1f));
        shapes.Add(shape);
    }

    void DestroyShape()
    {
        if(shapes.Count > 0)
        {
            int index = Random.Range(0, shapes.Count);
            // Destroy(shapes[index].gameObject);
            shapeFactory.Reclaim(shapes[index]);
            //1,RemoveAt删除比较低效率
            //shapes.RemoveAt(index);
            //2，高效删除
            int lastIndex = shapes.Count - 1;
            shapes[index] = shapes[lastIndex];
            shapes.RemoveAt(lastIndex);

        }
       
    }

}
