using Assets.Common;
using System;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class GlobalGenerator:MonoBehaviour
    {

        private void Start()
        {
            InitGameMgr();
        }
        public void InitGameMgr()
        {
            GameObject gameObject = IO.mgr;
            if(gameObject == null)
            {
                GameObject original = Resources.Load(Const.GameDir + "GameMgr") as GameObject;
                gameObject = Instantiate(original);
                gameObject.name = "GameManager";
                
            }
        }
    }
}
