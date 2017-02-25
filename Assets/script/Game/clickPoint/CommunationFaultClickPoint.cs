using UnityEngine;
using System.Collections;

public class CommunationFaultClickPoint : CarWayPoint
{

    public MovieTexture Movie;

    private bool needPlayMovie = true;
    private CarControl player;
    public override void RunOnTrigger(CarControl carControl)
    {
        player = carControl;
        carControl.Push();
        PlayMovie();
    }

    /// <summary>
    /// 播放视屏
    /// </summary>
    public void PlayMovie()
    {
        if (Movie != null)
        {
            UIManager.Instence.OpenUI(EnumUIPlaneType.MoviePlay, false, Movie);
            UIManager.Instence.GetUIGameObject(EnumUIPlaneType.MoviePlay).GetComponent<MoviePlayUI>().EventHandler += finishMoviePlay;
        }

    }
    private void finishMoviePlay()
    {
        player.Continue();
        UIManager.Instence.CloseUI(EnumUIPlaneType.MoviePlay);
    }
    
}
