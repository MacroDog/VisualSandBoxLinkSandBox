using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
public class MoviePlayUI : BaseUI {


    public delegate void finishMoviePlay();
    public finishMoviePlay EventHandler;
    public MovieTexture movie;
    private bool isplay;
    public RawImage moviePlayer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!movie.isPlaying&&isplay)
        {
            ClosePanel();
        }
	
	}
    public override EnumUIPlaneType GetUIType()
    {
        return EnumUIPlaneType.MoviePlay;
    }

    public void palyerMovie()
    {
        if (movie!=null)
        {
            movie.Play();
            Debug.Log("Start Play Movie");
            isplay = true;
        }
    }
    protected override void OnLoadData()
    {
        base.OnLoadData();
        
       
        
    }
    protected override void SetUI(params object[] UIParams)
    {
        base.SetUI(UIParams);
        movie = UIParams[0] as MovieTexture;
        Debug.Log(movie.name);
        moviePlayer.texture = movie;
        palyerMovie();

    }
    protected override void OnRelease()
    {
        base.OnRelease();
        EventHandler();

    }
    
}
