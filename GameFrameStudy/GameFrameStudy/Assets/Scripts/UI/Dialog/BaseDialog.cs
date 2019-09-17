using Assets.UI.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.Scripts.UI.Dialog
{
    public class BaseDialog
    {
        private string name;
        //private UILabel title;
        //private UILabel prompt;
        private GameObject container;
        protected DialogType _type;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }


        public DialogType type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        public GameObject Container
        {
            get
            {
                return this.container;
            }
            set
            {
                this.container = value;
            }
        }

        public void InitDialog(GameObject _container)
        {
            GameObject topObj = Util.Child(_container, "TopName");
            if(topObj != null)
            {
              //  this.prompt = topobj.GetComponent<UILabel>();
            }
        }

        protected void Open(GameObject container,string data)
        {
            this.container = container;
            InitDialog(container);
            //if (!data.Equals(string.Empty) && this.title != null)
            //{
            //    this.title.text = data;
            //}
        }

        protected void Close()
        {

        }
    }


}
