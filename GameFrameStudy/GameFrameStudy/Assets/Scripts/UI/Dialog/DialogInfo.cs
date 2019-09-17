using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI.Dialog
{
    public class DialogInfo:BaseDialog
    {

        private AsyncState asynState = AsyncState.Completed;
        public AsyncState AsyncState
        {
            get { return asynState; }
            set { asynState = value; }
        }
        public new void Close()
        {
            base.Close();
        }

        public void OpenDialog(GameObject container)
        {
            base.Open(container, string.Empty);
        }
    }
}

    
