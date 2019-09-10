using Assets.Manager;
using Assets.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Common
{
    //负责管理Mgrs
    public class IO:MonoBehaviour
    {
        private static GameObject _mgr;
        private static GameMgr _gameMgr;
        private static PanelMgr _panelMgr;
        private static DialogMgr _dialogMgr;
        private static MusicMgr _musicMgr;
        private static UIContainer _uiContainer;

        public static GameObject mgr
        {
            get
            {
                if(_mgr == null)
                {
                    _mgr = GameObject.FindWithTag("GameManager");
                }
                return _mgr;
            }
        }

        public static GameMgr gameMgr
        {
            get
            {
                if (_gameMgr == null)
                {
                    _gameMgr =  mgr.GetComponent<GameMgr>();
                }
                return _gameMgr;
            }
        }


        public static MusicMgr musicMgr
        {
            get
            {
                if (_musicMgr == null)
                {
                    _musicMgr = mgr.GetComponent<MusicMgr>();
                }
                return _musicMgr;
            }
        }


        public static PanelMgr panelMgr
        {
            get
            {
                if (_panelMgr == null)
                {
                    _panelMgr = mgr.GetComponent<PanelMgr>();
                }
                return _panelMgr;
            }
        }


        public static DialogMgr dialogMgr
        {
            get
            {
                if (_dialogMgr == null)
                {
                    _dialogMgr = mgr.GetComponent<DialogMgr>();
                }
                return _dialogMgr;
            }
        }

        public static GameObject Gui
        {
            get
            {
                return GameObject.FindWithTag("GUI");
            }
        }

        public static UIContainer uiContainer
        {
            get
            {
                if(_uiContainer == null)
                {
                    _uiContainer = Gui.GetComponent<UIContainer>();
                }
                return _uiContainer;
            }
        }
    }
}
