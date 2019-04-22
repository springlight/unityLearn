using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour {

    public int rowIdx = 0;
    public int columnIdx = 0;
    public float xOffset = 4.5f;
    public float yOffset = -3f;
    public SpriteRenderer sprite;
    public GameObject [] bgs;
    public GameObject bg;
    public int type;
    public GameController game;
    public int candyNumber = 6;//类型数目
    private SpriteRenderer spr;
    public int selected
    {
        set
        {
            if(spr != null)
            {
                spr.color = value > 0 ? Color.blue : Color.white;
            }
        }
    }
	void Start () {

    }
	private void AddRandBg()
    {
        if (bg != null) return;
        type = Random.Range(0,Mathf.Min(candyNumber, bgs.Length));
        bg = GameObject.Instantiate(bgs[type]);
        bg.transform.parent = this.transform;
        spr = bg.GetComponent<SpriteRenderer>();
        //bg.transform.localPosition = Vector3.zero;
    }
	
    public void UpdatePos()
    {
        AddRandBg();
        transform.position = new Vector3(columnIdx + xOffset, rowIdx + yOffset, 0);
    }

    public void TweenToPos()
    {
        AddRandBg();
        iTween.MoveTo(this.gameObject,
            iTween.Hash("x",columnIdx + xOffset,
                        "y",rowIdx + yOffset,
                         "time",0.3f));

    }
    private void OnMouseDown()
    {
        Debug.LogError("OnMouseDown" + rowIdx + ":" + columnIdx);
        
        game.Select(this);
    }

    private void OnMouseUp()
    {
        Debug.LogError("OnMouseuP");
    }

    public void Dispose()
    {
        game = null;
        Destroy(bg.gameObject);
        Destroy(this.gameObject);
    }
}
