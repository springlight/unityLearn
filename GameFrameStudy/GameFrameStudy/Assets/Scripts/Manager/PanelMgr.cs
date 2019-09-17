using Assets.Common;
using Assets.Scripts.Common;
using Assets.UI;
using Assets.UI.Dialog;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Manager
{
    public class PanelMgr:MonoBehaviour
    {
        private Transform parent;
        private string path;

        public Transform Parent
        {
            get
            {
                if(parent == null)
                {
                    GameObject gui = IO.Gui;
                    if (gui)
                    {
                        parent = gui.transform.Find("Camera");
                    }
                }
                return parent;
            }
        }

        public void CreatePanel(DialogType type)
        {
#if UNITY_EDITOR
            string typename = Util.ConvertPanelName(type);
            CreatePanel(typename);
#else
            StartCoroutine(OnCreatePanel(type));
#endif
        }

        private IEnumerator OnCreatePanel(DialogType type)
        {
            path = Util.AppContentDataUri + "UI/" + type.ToString() + "Panel.unity3d";
            GameObject go = null;
            WWW bundle = new WWW(path);
            yield return bundle;

            try
            {
                if(bundle.assetBundle.Contains(type.ToString() + "Panel"))
                {
                    go = Instantiate(bundle.assetBundle.LoadAsset(type.ToString() + "Panel", typeof(GameObject))) as GameObject;
                }
            }
            catch (Exception e)
            {

                Debug.LogError("catch go..." +e.ToString());
            }
            go.name = type.ToString() + "Panel";
            go.transform.parent = UIContainer.ins.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            bundle.assetBundle.Unload(true);
        }

        private void CreatePanel(string name)
        {
            if (Parent.FindChild(name) != null) return;
            GameObject go = Util.LoadPrefab(Const.PanelPrefabDir + name + ".prefab");
            if (go == null) return;
            GameObject go2 = Util.AddChild(go, UIContainer.ins.transform);
            go2.name = name;
            go.transform.localPosition = Vector3.zero;
            this.OnCreatePanel(name,go2);
        }

        private void OnCreatePanel(string name, GameObject go)
        {
            switch (name)
            {
                case "LoginPanel":
                    this.OnLoginPanel(go);
                    break;
                case "CharacterPanel":
                    this.OnCharacterPanel(go);
                    break;
                case "MainPanel":
                    this.OnMainPanel(go);
                    break;
                case "WorldPanel":
                    this.OnWorldPanel(go);
                    break;
                case "DuplicatePanel":
                    this.OnDuplicatePanel(go);
                    break;
            }
        }

        private void OnLoginPanel(GameObject go)
        {
            go.transform.localPosition = new Vector3(0f, 0f, 0f);
            IO.uiContainer.loinPanel = go;
        }

        private void OnCharacterPanel(GameObject go)
        {
            go.transform.localPosition = new Vector3(0f, 0f, 0f);
            IO.uiContainer.characterPanel = go;
        }

        private void OnMainPanel(GameObject go)
        {
            go.transform.localPosition = new Vector3(0f, 0f, 0f);
            IO.uiContainer.mainPanel = go;
        }

        private void OnDuplicatePanel(GameObject go)
        {
            go.transform.localPosition = new Vector3(0f, 0f, 0f);
            IO.uiContainer.duplicatePanel = go;
        }

        private void OnWorldPanel(GameObject go)
        {
            go.transform.localPosition = new Vector3(0f, 0f, 0f);
            IO.uiContainer.worldPanel = go;
        }

    }
}
