using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
//todo知识点 C#Hashtable 和Dict的区别
namespace Assets.Scripts.Common
{
    public static class Resource
    {
        private static Hashtable texts = new Hashtable();
        private static Hashtable images = new Hashtable();
        private static Hashtable prefabs = new Hashtable();

        public static GameObject LoadPrefab(string path)
        {
            object obj = prefabs[path];

            if(obj == null)
            {
                prefabs.Remove(path);
                GameObject gameObject = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                prefabs.Add(path, gameObject);
                return gameObject;
            }
            return obj as GameObject;
        }

        public static string LoadTextFile(string path,string ext)
        {
            object obj = texts[path];

            if (obj == null)
            {
                texts.Remove(path);
                string text = string.Empty;
#if UNITY_EDITOR
                string pathstr = Util.AppPersistentDataUri + "/StreamingAssets/" + path + ext;
#else
                string pathstr = Util.AppContentDataUri + path + ext;
#endif        
                text = File.ReadAllText(pathstr);
                texts.Add(pathstr, ext);
                return text;

            }
            return obj as string;
        }


        public static Texture2D LoadTexture(string path)
        {
            object obj = images[path];

            if (obj == null)
            {
                images.Remove(path);
                Texture2D texture2D = (Texture2D)Resources.Load(path, typeof(Texture2D));
                images.Add(path, texture2D);
                return texture2D;
            }
            return obj as Texture2D;
        }




    }
}
