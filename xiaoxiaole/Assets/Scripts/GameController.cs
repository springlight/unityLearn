using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	// Use this for initialization
	void Start ()
    {
        candyArr = new ArrayList();
        InitCandys();
	}
	//初始化
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
        AddEffect(c.transform.position);
        audioSource.PlayOneShot(explodeClip);
        //删除自己
        c.Dispose();
       
        int cIdx = c.columnIdx;
        Debug.LogErrorFormat("选中的列数---》{0}/行数-->{1}", cIdx, c.rowIdx);

        //遍历选中的candy同列，上面所有行的candy，每个都下移一格
        for (int rIdx = c.rowIdx + 1; rIdx <rowNum; rIdx++)
        {
            Candy c2 = GetCandy(rIdx, cIdx);
            //y下移一行
            c2.rowIdx--;
            // c2.UpdatePos();
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
        //Remove(c);return;
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
        SetCandy(c1.rowIdx, c2.columnIdx, c2);
        SetCandy(c2.rowIdx, c2.columnIdx, c1);
        int r = c1.rowIdx;
        int c = c1.columnIdx;
        c1.rowIdx = c2.rowIdx;
        c1.columnIdx = c2.columnIdx;
        c1.TweenToPos();
        c2.rowIdx = r;
        c2.columnIdx = c;
        c2.TweenToPos();
        //交换完成后置空当前选择



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
                //判断第一个和第二个是否一样，以及第二个第三个是否一样
                if (GetCandy(rowIdx, cIdx).type == GetCandy(rowIdx, cIdx + 1).type &&
                   GetCandy(rowIdx, cIdx + 1).type == GetCandy(rowIdx, cIdx + 2).type)
                {
                    audioSource.PlayOneShot(matchClip);
                    Debug.LogErrorFormat("列数--{0}/{1}/{2}", cIdx, cIdx + 1, cIdx + 2);
                    AddMath(GetCandy(rowIdx, cIdx));
                    AddMath(GetCandy(rowIdx, cIdx + 1));
                    AddMath(GetCandy(rowIdx, cIdx + 2));
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
                //判断第一个和第二个是否一样，以及第二个第三个是否一样
                if (GetCandy(rowIdx, cIdx).type == GetCandy(rowIdx + 1, cIdx).type &&
                   GetCandy(rowIdx +1, cIdx).type == GetCandy(rowIdx + 2, cIdx).type)
                {
                    audioSource.PlayOneShot(matchClip);
                    // Debug.LogErrorFormat("列数--{0}/{1}/{2}", cIdx, cIdx + 1, cIdx + 2);
                    AddMath(GetCandy(rowIdx, cIdx));
                    AddMath(GetCandy(rowIdx + 1, cIdx));
                    AddMath(GetCandy(rowIdx + 2, cIdx));
                    result = true;
                }
            }
        }
        return result;
    }

    private ArrayList mathes;
    private void AddMath(Candy c)
    {
        if(mathes == null)
            mathes = new ArrayList();
        int idx = mathes.IndexOf(c);//判断是存在
        if (idx == -1)
            mathes.Add(c);
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
        mathes.Clear();
        //第一次检测
        StartCoroutine(WaitAndCheck());
    }
    /// <summary>
    /// 检测是否需要删除
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitAndCheck()
    {
        yield return new WaitForSeconds(0.5F);
        if (CheckMathes())
        {
            RemoveMathes();
        }
    }
    
}
