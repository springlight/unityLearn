using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoreDropdown : MonoBehaviour
{
    [Header("主按钮")]
    public Button mainButton;
    [Header("dropdownPanel模板")]
    public Image dropdownPanel;
    [Header("dropdown列表")]
    public Image dropdownGrid;
    [Header("dropdownItem模板")]
    public Button dropdownItem;
    [Header("获取dropdownItem模板大小")]
    public RectTransform dropdownItemRT;
    [Header("背景按钮隐藏")]
    public Button hideBG;
    //下拉菜单集
    private List<Image> dropdownPanels = new List<Image>();
    //菜单数据
    private static List<IMoreDropdownInfo> allInfo;
    //记录点击位置顺序
    private int[] clickOrder = new int[10];
    private int orderIndex = 0;
    //是否显示
    private bool isShowFirstPanel = true;
    //选中按钮
    private List<Button> pointerButtonList = new List<Button>();
    //当前选中按钮数据
    private Button enterButton;
    private int enterButtonLevel;
    private IMoreDropdownInfo enterButtonInfo;
    //按钮选中颜色状态
    private enum ButtonColorState
    {
        Normal,
        Enter,
        Exit,
        Click,
    }
    //多下拉菜单辅助Action
    public Action onCreateDropdown;

    void Awake()
    {
        //下拉菜单
        mainButton.onClick.AddListener(delegate ()
        {
            if (isShowFirstPanel)
            {
                isShowFirstPanel = false;
                hideBG.gameObject.SetActive(true);
                //置于顶部
                transform.SetAsLastSibling();
                //开始创建列表
              //  onCreateDropdown?.Invoke();
                CreateDropdown(0, allInfo);
            }
            else
            {
                HideFirstPanel();
            }
        });

        //背景全部隐藏
        hideBG.onClick.AddListener(delegate ()
        {
            HideFirstPanel();
        });
    }

    /// <summary>
    /// 创建下拉菜单
    /// </summary>
    /// <param name="level">第几级菜单</param>
    private void CreateDropdown(int level, List<IMoreDropdownInfo> infoList)
    {
        Image dropdown = Instantiate(dropdownPanel);
        dropdownPanels.Add(dropdown);
        dropdown.transform.parent = dropdownGrid.transform;
        dropdown.transform.localScale = new Vector3(1f, 1f, 1f);
        dropdown.gameObject.SetActive(true);

        dropdownGrid.gameObject.SetActive(true);

        for (int k = 0; k < infoList.Count; k++)
        {
            //二级及以上的第一位不显示（填充到了前一级的位置）
            if ((level > 0) && (k == 0))
                continue;

            IMoreDropdownInfo info = infoList[k];
            Button cloneButton = Instantiate(dropdownItem);
            cloneButton.transform.parent = dropdown.transform;
            cloneButton.transform.localScale = new Vector3(1f, 1f, 1f);
            Image dropdownButton = cloneButton.GetComponent<Image>();
            Text dropdownText = cloneButton.transform.Find("dropdownText").GetComponent<Text>();
            Image dropdownArrow = cloneButton.transform.Find("dropdownArrow").GetComponent<Image>();

            //创建时 选中按钮默认记录第一个
            if (k == 1)
                pointerButtonList.Add(cloneButton);
            //判断是否有下一级
            if (info.str != null)
            {
                dropdownArrow.gameObject.SetActive(false);
                dropdownText.text = info.str;
            }
            else
            {
                dropdownArrow.gameObject.SetActive(true);
                dropdownText.text = info.list[0].str;
            }

            //处理选中状态
            cloneButton.gameObject.SetActive(true);
            MCsUIListener listener = MCsUIListener.Get(cloneButton.gameObject);
            listener.onEnter = (go, eventData) =>
            {
                SetButtonState(cloneButton, ButtonColorState.Enter);
                enterButton = cloneButton;
                enterButtonLevel = level;
                enterButtonInfo = info;
            };
            //这是项目封装的代码 可以继承IPointerClickHandler接口
            listener.onExit = (go, eventData) =>
            {
                SetButtonState(cloneButton, ButtonColorState.Exit);
                enterButton = null;
                enterButtonLevel = -1;
                enterButtonInfo = null;
            };

            listener.onUp = (go, eventData) =>
            {
                if (enterButton != null)
                    OnSelectDropdownItem();
            };
        }
    }

    /// <summary>
    /// 移除第几级及后的菜单
    /// </summary>
    /// <param name="level"></param>
    private void RemovePanelItems(int level)
    {
        //判断是否不是第一级
        if (level < dropdownPanels.Count)
        {
            //点击级之后的全部清除
            for (int k = dropdownPanels.Count - 1; k >= level; k--)
            {
                for (int kk = dropdownPanels[k].transform.childCount - 1; kk >= 0; kk--)
                {
                    Destroy(dropdownPanels[k].transform.GetChild(kk).gameObject);
                }
                //清除背景
                Destroy(dropdownPanels[k].gameObject);
                dropdownPanels.RemoveAt(k);
                //保护防止越界
                orderIndex = orderIndex >= 0 ? orderIndex : 0;
                //清除位置
                clickOrder[orderIndex] = 0;
                orderIndex--;
                //清除记录按钮
                pointerButtonList.RemoveAt(k);
            }
        }
    }

    //设置按钮颜色状态
    private void SetButtonState(Button button, ButtonColorState state)
    {
        Text buttonText = button.transform.Find("dropdownText").GetComponent<Text>();
        Image buttonArrow = button.transform.Find("dropdownArrow").GetComponent<Image>();
        if (state == ButtonColorState.Normal)
        {
            buttonText.color = new Color((111f / 256f), (111f / 256f), (111f / 256f), 1f);
            buttonArrow.color = new Color((111f / 256f), (111f / 256f), (111f / 256f), 1f);
        }
        else if (state == ButtonColorState.Enter)
        {
            Color oldColor = buttonText.color;
            buttonText.color = new Color(oldColor.r, oldColor.g, oldColor.b, (120f / 256f));
            buttonArrow.color = new Color(oldColor.r, oldColor.g, oldColor.b, (120f / 256f));
        }
        else if (state == ButtonColorState.Exit)
        {
            Color oldColor = buttonText.color;
            buttonText.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1f);
            buttonArrow.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1f);
        }
        else if (state == ButtonColorState.Click)
        {
            buttonText.color = new Color((84f / 256f), (145f / 256f), (220f / 256f), 1f);
            buttonArrow.color = new Color((84f / 256f), (145f / 256f), (220f / 256f), 1f);
        }
    }

    //执行最终选中按钮数据处理
    private void OnSelectDropdownItem()
    {
        //记录点击位置
        clickOrder[enterButtonLevel] = enterButtonInfo.index;
        orderIndex = enterButtonLevel + 1;
        if (enterButtonInfo.str != null)
        {
            ChangeMainText(enterButtonInfo.str);
        }
        else
        {
            List<IMoreDropdownInfo> nextList = enterButtonInfo.list;
            nextList = enterButtonInfo.list;
            RemovePanelItems(enterButtonLevel + 1);
            CreateDropdown(enterButtonLevel + 1, nextList);
            //处理选中按钮状态
            if (pointerButtonList.Count >= enterButtonLevel)
                SetButtonState(pointerButtonList[enterButtonLevel], ButtonColorState.Normal);
            pointerButtonList[enterButtonLevel] = enterButton;
            SetButtonState(enterButton, ButtonColorState.Click);
        }
    }

    //隐藏
    private void HideFirstPanel()
    {
        isShowFirstPanel = true;
        hideBG.gameObject.SetActive(false);
        RemovePanelItems(0);
        orderIndex = 0;
    }

    //显示
    private void ShowFirstPanel()
    {
        isShowFirstPanel = true;
        dropdownGrid.gameObject.SetActive(true);
    }

    //选好收回的Action回调
    public Action<String, String> onClickItem;
    //设置主按钮的文字
    private void ChangeMainText(String str)
    {
        Text firstText = mainButton.transform.Find("mainText").GetComponent<Text>();
        firstText.text = str;
        //生成返回字符串
        string orderStr = "";
        for (int i = 0; i < orderIndex; i++)
        {
            if (i == 0)
            {
                orderStr += (clickOrder[i] + 1);
            }
            else
            {
                orderStr += "|" + clickOrder[i];
            }
        }
        //隐藏所有并清空所有临时数据
        HideFirstPanel();
        //回调
      //  onClickItem?.Invoke(str, orderStr);
    }

    //传入值
    public static void SetAllInfo(List<IMoreDropdownInfo> _allInfo)
    {
        allInfo = _allInfo;
    }
}

//按钮数据
public class IMoreDropdownInfo
{
    //记录位置
    public int index;
    //字符串或list
    public string str;
    public List<IMoreDropdownInfo> list;

    public IMoreDropdownInfo(String _str) { str = _str; }
    public IMoreDropdownInfo(List<IMoreDropdownInfo> _list) { list = _list; }
}

//按钮数据处理逻辑
public class MoreDropdownItem
{
    //创建一个独立按钮
    public static IMoreDropdownInfo CreateInfo(String str)
    {
        return new IMoreDropdownInfo(str);
    }

    //创建一个菜单
    public static List<IMoreDropdownInfo> CreateList()
    {
        List<IMoreDropdownInfo> _list = new List<IMoreDropdownInfo>();
        return _list;
    }

    //独立按钮添加到菜单中
    public static List<IMoreDropdownInfo> AddInfo(List<IMoreDropdownInfo> _list, IMoreDropdownInfo _info)
    {
        _info.index = _list.Count;
        _list.Add(_info);
        return _list;
    }

    //子级菜单添加到菜单中
    public static List<IMoreDropdownInfo> AddInfo(List<IMoreDropdownInfo> _list, List<IMoreDropdownInfo> _info)
    {
        IMoreDropdownInfo info = new IMoreDropdownInfo(_info);
        info.index = _list.Count;
        _list.Add(info);
        return _list;
    }

}
public delegate void  OnPointer(GameObject go ,PointerEventData evtData);
public class MCsUIListener : EventTrigger
{
    //  public UIEventDelegate onClick;
    public OnPointer onEnter;
    public OnPointer onExit;
    public OnPointer onUp;
    public static MCsUIListener Get(GameObject go)
    {
        if (!go)
        {
            return null;
        }
        MCsUIListener listener = go.GetComponent<MCsUIListener>();
        if (!listener)
        {
            listener = go.AddComponent<MCsUIListener>();
        }
        return listener;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (onEnter != null) onEnter(gameObject, eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (onExit != null) onExit(gameObject, eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (onUp != null) onUp(gameObject, eventData);
    }
}