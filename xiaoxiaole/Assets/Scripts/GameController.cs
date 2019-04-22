using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Candy candy;
    public int columnNum = 10;//列数
    public int rowNum = 7;//行数
    private ArrayList candyArr;
    public AudioClip swapClip;
    public AudioClip explodeClip;
    public AudioClip matchClip;
    public AudioSource audioSource;
    public AudioClip wrongClip;
    public Text txt;
    public GameObject successPanel;
    private int cnt = 0;
    public SpriteRenderer[] bgs;

    public SpriteRenderer bg;
	// Use this for initialization
	void Start ()
    {
        ChangeBg();
        candyArr = new ArrayList();
        InitCandys();

	}
    void ChangeBg()
    {

        bg.gameObject.SetActive(false);
        bg = bgs[Random.Range(0, bgs.Length)];
        bg.gameObject.SetActive(true);
    }
	//初始化糖果
    private void InitCandys()
    {
        for(int rIdx = 0; rIdx < rowNum; rIdx++)
        {
            ArrayList tmp = new ArrayList();
            for(int cIdx = 0; cIdx < columnNum; cIdx++)
            {
                tmp.Add(CreateCandy(rIdx, cIdx));
            }
            //二维数组
            candyArr.Add(tmp);
         
        }
        //第一次检测
        if (CheckMathes())
        {
            RemoveMathes();
        }
    }
    private Candy CreateCandy(int rIdx,int cIdx)
    {
        Candy c = GameObject.Instantiate(candy, this.transform) as Candy;
        c.rowIdx = rIdx;
        c.columnIdx = cIdx;
        c.game = this;
        //更新位置
        c.UpdatePos();
        return c;
    }
    /// <summary>
    /// 从二维数组中获取一个Candy
    /// </summary>
    /// <param name="rIdx"></param>
    /// <param name="cIdx"></param>
    /// <returns></returns>
    private Candy GetCandy(int rIdx,int cIdx)
    {
        ArrayList tmp = candyArr[rIdx] as ArrayList;
        Candy c = tmp[cIdx] as Candy;
        return c;

    }

    
    private void SetCandy(int rIdx,int cIdx,Candy c)
    {
        ArrayList tmp = candyArr[rIdx] as ArrayList;
        tmp[cIdx] = c;
    }

    private void AddEffect(Vector3 pos)
    {
        Instantiate(Resources.Load("Prefabs/Explosion"),pos,Quaternion.identity);
        CameraShake.shakeFor(0.5f, 0.1f);
    }
    /// <summary>
    /// 删除逻辑
    /// </summary>
    /// <param name="c"></param>
    private void Remove(Candy c)
    {
        Vector3 pos = c.transform.position;
        int cIdx = c.columnIdx;
        int rIdx = c.rowIdx + 1;
        //删除自己
        c.Dispose();
        AddEffect(pos);
        audioSource.PlayOneShot(explodeClip);
        //遍历选中的candy同列，上面所有行的candy，每个都下移一格
        for (; rIdx <rowNum; rIdx++)
        {
            Candy c2 = GetCandy(rIdx, cIdx);
            //y下移一行
            c2.rowIdx--;
            c2.TweenToPos();
            SetCandy(rIdx - 1, cIdx, c2);
        }
        //顶部添加新candy
        Candy newC = CreateCandy(rowNum-1, cIdx);
        newC.rowIdx = rowNum;//放在最上面一个
        newC.UpdatePos();
        newC.rowIdx--;//下移一格
        newC.TweenToPos();
        SetCandy(rowNum - 1, cIdx, newC);
    }

    private Candy curCany;
    public void Select(Candy c)
    {
        if (curCany == null)
        {
            curCany = c;
            curCany.selected = 1;
            return;
        }
        else
        {
            if (Mathf.Abs(curCany.rowIdx - c.rowIdx) + Mathf.Abs(curCany.columnIdx - c.columnIdx) == 1)
            {
                StartCoroutine(Swap2(curCany, c));

            }
            else
            {
                audioSource.PlayOneShot(wrongClip);
            }
            curCany.selected = 0;
            curCany = null;
        }
    }
    IEnumerator Swap2(Candy c1,Candy c2)
    {
        Swap(c1, c2);
        yield return new WaitForSeconds(0.4F);
        //如果能消除就消除
        if (CheckMathes())
        {
            RemoveMathes();
        }
        else//不能消除在交换回来
        {
            Swap(c1, c2);
        }
    }
    /// <summary>
    /// 交换
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    private void Swap(Candy c1,Candy c2)
    {
        audioSource.PlayOneShot(swapClip);
        SetCandy(c1.rowIdx, c1.columnIdx, c2);
        SetCandy(c2.rowIdx, c2.columnIdx, c1);
        int r = c1.rowIdx;
        int c = c1.columnIdx;
        c1.rowIdx = c2.rowIdx;
        c1.columnIdx = c2.columnIdx;
        c1.TweenToPos();
        c2.rowIdx = r;
        c2.columnIdx = c;
        c2.TweenToPos();

    }
    /// <summary>
    /// 检测有没有可以删除的
    /// </summary>
    /// <returns></returns>
    private bool CheckMathes()
    {
        //CheckHorizontalMath();
        //CheckVerticalMath();
        return CheckHorizontalMath() || CheckVerticalMath();
    }
    /// <summary>
    /// 检测水平方向
    /// </summary>
    /// <returns></returns>
    private bool CheckHorizontalMath()
    {
        bool result = false;
        for (int rowIdx = 0; rowIdx <rowNum; rowIdx++)
        {
            for (int cIdx = 0; cIdx < columnNum - 2; cIdx++)
            {
                Candy c0 = GetCandy(rowIdx, cIdx);
                Candy c1 = GetCandy(rowIdx, cIdx + 1);
                Candy c2 = GetCandy(rowIdx, cIdx + 2);
                //判断第一个和第二个是否一样，以及第二个第三个是否一样
                if (c0.type == c1.type &&
                   c1.type == c2.type)
                {
                    audioSource.PlayOneShot(matchClip);
                   
                    AddMath(c0);
                    AddMath(c1);
                    AddMath(c2);
                    result = true;
                }
            }
        }
       
        return result;
    }

    /// <summary>
    /// 检测垂直方向
    /// </summary>
    /// <returns></returns>
    private bool CheckVerticalMath()
    {
        bool result = false;
        for (int cIdx = 0; cIdx < columnNum; cIdx++)
        {
            for (int rowIdx = 0; rowIdx < rowNum - 2; rowIdx++)
            {
                Candy c0 = GetCandy(rowIdx, cIdx);
                Candy c1 = GetCandy(rowIdx + 1, cIdx);
                Candy c2 = GetCandy(rowIdx + 2, cIdx);
                //判断第一个和第二个是否一样，以及第二个第三个是否一样
                if (c0.type == c1.type &&
                   c1.type == c2.type)
                {
                    audioSource.PlayOneShot(matchClip);
                    // Debug.LogErrorFormat("列数--{0}/{1}/{2}", cIdx, cIdx + 1, cIdx + 2);
                    AddMath(c0);
                    AddMath(c1);
                    AddMath(c2);
                    result = true;
                }
            }
        }
        return result;
    }

    private ArrayList mathes;
    /// <summary>
    /// 加入匹配列表
    /// </summary>
    /// <param name="c"></param>
    private void AddMath(Candy c)
    {
        if(mathes == null)
            mathes = new ArrayList();
        int idx = mathes.IndexOf(c);//判断是存在
        if (idx == -1)
        {
            mathes.Add(c);
        }
        else
        {
            //Debug.LogError("匹配池中已经存在--->" + c.GetHashCode());
        }
          
    }
    /// <summary>
    /// 删除所有可以消除的candy
    /// </summary>
    private void RemoveMathes()
    {
        if (mathes == null) return;
        Candy tmp;
        for(int i=0; i < mathes.Count; i++)
        {
            tmp = mathes[i] as Candy;
            Remove(tmp);
            
        }
        cnt++;
        if (cnt % 10 == 0 && cnt != 0)
        {
            successPanel.SetActive(true);
            StartCoroutine(Success());
            ChangeBg();
        }
        txt.text = cnt.ToString();
        
        mathes.Clear();
        //第一次检测
        StartCoroutine(WaitAndCheck());
    }
    IEnumerator Success()
    {
       
        yield return new WaitForSeconds(2.5f);
      //  txt.text = cnt.ToString();
        successPanel.gameObject.SetActive(false);
    }
    /// <summary>
    /// 检测是否需要删除
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitAndCheck()
    {
        yield return new WaitForSeconds(1.0F);
        if (CheckMathes())
        {
            RemoveMathes();
        }
    }
    
}
