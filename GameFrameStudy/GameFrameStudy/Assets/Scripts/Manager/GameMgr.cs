using UnityEngine;

namespace Assets.Manager
{
    public class GameMgr:MonoBehaviour
    {
        private void Start()
        {
            AddManager();
        }

        private void AddManager()
        {
            Util.AddComponent<DialogMgr>(this.gameObject);
            Util.AddComponent<PanelMgr>(this.gameObject);
            Util.AddComponent<MusicMgr>(this.gameObject);
        }
    }
}
