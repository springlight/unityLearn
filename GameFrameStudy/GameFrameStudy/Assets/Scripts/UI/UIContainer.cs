using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.UI
{
    public class UIContainer:MonoBehaviour
    {
        public GameObject loinPanel;
        public GameObject mainPanel;
        public GameObject fightPanel;
        public GameObject duplicatePanel;
        public GameObject worldPanel;
        public GameObject taskPanel;
        public GameObject characterPanel;
        public List<GameObject> panels = new List<GameObject>();
        public List<GameObject> allPanels
        {
            get
            {
                this.panels.Clear();
                AddPanel(loinPanel);
                AddPanel(mainPanel);
                AddPanel(fightPanel);
                AddPanel(duplicatePanel);
                AddPanel(worldPanel);
                AddPanel(taskPanel);
                AddPanel(this.characterPanel);
                return panels;
            }
        }


        private void AddPanel(GameObject panel)
        {
            if(panel != null)
             panels.Add(panel);
        }

        public void ClearAll()
        {
            foreach(GameObject obj in panels)
            {
                if(obj != null)
                {
                    Util.SafeDestroy(obj);
                }
            }
            panels.Clear();
        }

        public static UIContainer ins;
        private void Awake()
        {
            ins = this;
        }
        private void Start()
        {
            DontDestroyOnLoad(this);
        }
    }
}
