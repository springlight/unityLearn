using Assets.Common;
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
                
            }
        }

        private void AddManager()
        {
            Util.AddComponent<DialogMgr>(this.gameObject);
            Util.AddComponent<PanelMgr>(this.gameObject);
            Util.AddComponent<MusicMgr>(this.gameObject);
        }
    }
}
