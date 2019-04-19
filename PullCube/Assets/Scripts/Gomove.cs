using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gomove : MonoBehaviour {

    public Transform[] target;
    public float speed = 0;

	// Use this for initialization
	void Start () {
        //运行时初始化位置
        transform.position = new Vector3(Random.Range(-9, 9), 0.75f, Random.Range(-9, 9));
        StartCoroutine(MoveToPath());
	}
	
	private IEnumerator MoveToPath()
    {
        while (true)
        {
            for(int i = 0; i< target.Length; i++)
            {
                //嵌套协程，依次向寻路点循环
                yield return StartCoroutine(MovToTarget(target[i].position));
            }
        }
    }
    //寻路方法
    IEnumerator MovToTarget(Vector3 target)
    {
        //当和目标距离大于0.1时，向目标寻路
        while((transform.position - target).magnitude > 0.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
