using UnityEditor;
using UnityEngine;

public class RegisterLevelObjectMenuItem
{
    const string menuItem = "GameObject/Register Level Object";

    [MenuItem(menuItem,true)]
    static bool ValidateRegisterLevelObject()
    {
        if (Selection.objects.Length == 0)
        {
            return false;
        }

        foreach (Object o in Selection.objects)
        {
            if (!(o is GameObject))
            {
                return false;
            }
        }
        return true;
    }
    [MenuItem(menuItem)]
    static void RegisterLevelObject()
    {
        foreach (Object o in Selection.objects)
        {
            Register(o as GameObject);
        }
    }

    private static void Register(GameObject o)
    {
       // GameObject o = Selection.activeGameObject;
        //if (o == null)
        //{
        //    Debug.LogWarning("No level object selected.");
        //    return;
        //}
        if (PrefabUtility.GetPrefabType(o) == PrefabType.Prefab)
        {
            Debug.LogWarning(o.name + " is a prefab asset.", o);
            return;
        }
        var levelObject = o.GetComponent<GameLevelObject>();
        if (levelObject == null)
        {
            Debug.LogWarning(o.name + " isn't a game level object.", o);
            return;
        }
        foreach (GameObject rootObj in o.scene.GetRootGameObjects())
        {
            var gameLv = rootObj.GetComponent<GameLevel>();
            if (gameLv != null)
            {
                if (gameLv.HasLevelObject(levelObject))
                {
                    Debug.LogWarning(o.name + " is already registered.", o);
                    return;
                }
                Undo.RecordObject(gameLv, "Register Level Object.");
                gameLv.RegisterLevelObject(levelObject);
                Debug.Log(
                    o.name + " registered to game level " +
                    gameLv.name + " in scene " + o.scene.name + ".", o
                );
                return;
            }
        }
        Debug.LogWarning(o.name + " isn't part of a game level.", o);
    }
}
