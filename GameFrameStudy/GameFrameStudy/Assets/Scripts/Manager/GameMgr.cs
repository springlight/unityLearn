using Assets.Common;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Manager
{
    public class GameMgr:MonoBehaviour
    {
        private void Start()
        {
            AddManager();
            DontDestroyOnLoad(this);
        }        
        public void InitGui()
        {
            GameObject gameObject = IO.Gui;
            if(gameObject == null)
            {
                GameObject original = Util.LoadPrefab(Const.PanelPrefabDir + "MainUI.prefab");
                gameObject = Instantiate(original) as GameObject;
                gameObject.name = "MainUI";
            }
        }

        private void AddManager()
        {
            Util.AddComponent<DialogMgr>(gameObject);
            Util.AddComponent<PanelMgr>(gameObject);
            Util.AddComponent<MusicMgr>(gameObject);
        }
    }
}
