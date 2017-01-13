using UnityEngine;
using System.Collections;

public class MoviePlayControl : MonoBehaviour
{

    [SerializeField]
    private MovieTexture[] Movie;
    [SerializeField]
    private Camera PlayMoiveCam;
    private float timer = 0;
    private int movieid = 0;
    // Use this for initialization
    public void PlayMovie(int i)
    {

        this.GetComponent<MeshRenderer>().material.mainTexture = Movie[i];
        PlayMoiveCam.gameObject.SetActive(true);
        Movie[i].Play();
        StartCoroutine(Do(Movie[i].duration));
        

    }

    /// <summary>
    /// 当完成播放回调函数
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator Do(float time)
    {
        //while (true) // 还需另外设置跳出循环的条件
        //{
        yield return new WaitForSeconds(time);
        Debug.Log("wancheng");
        //}
    }



    /// <summary>
    /// 当播放完成时回调函数
    /// </summary>
    private void FinishPlay()
    {
        PlayMoiveCam.gameObject.SetActive(false);
        
        
    }


}
