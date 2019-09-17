using Assets.Common;
using Assets.Scripts.UI.Dialog;
using Assets.UI.Dialog;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Manager
{
    public class DialogMgr:MonoBehaviour
    {
        private Hashtable dialogs = new Hashtable();
        public bool DialogExist(DialogType type)
        {
            return dialogs.Contains(type);
        }

        public DialogInfo Add(DialogType type)
        {
            DialogInfo dialogInfo = new DialogInfo();
            dialogInfo.type = type;
            dialogs.Add(type, dialogInfo);
            return dialogInfo;
        }

        public void ResetDialog()
        {
            IDictionaryEnumerator enumerator = dialogs.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DialogInfo dialoginfo = enumerator.Value as DialogInfo;
                dialoginfo.AsyncState = AsyncState.Completed;
            }
        }

        public DialogInfo GetDialogInfo(DialogType type)
        {
            if (!DialogExist(type))
            {
                return Add(type);
            }
            return dialogs[type] as DialogInfo;
        }

        public void Remove(DialogType type)
        {
            if (DialogExist(type))
            {
                dialogs.Remove(type);
            }
        }

        public void ClearDialog()
        {
            dialogs.Clear();
        }

        public Transform GetDialog(DialogType type)
        {
            if (type == DialogType.None) return null;
            string str = Util.ConvertPanelName(type);
            return IO.Gui.transform.Find(str);
        }
    }

 
}
