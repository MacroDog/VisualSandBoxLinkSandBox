using UnityEngine;
using System.Collections;
using System;

public class MoviePlayUI : BaseUI {


    public delegate void finishMoviePlay();
    public finishMoviePlay EventHandler;
    public MovieTexture movie;
    private bool isplay;

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
            isplay = true;
        }
    }
    protected override void OnLoadData()
    {
        base.OnLoadData();
        
        palyerMovie();
        Debug.Log("playmovie");
    }
    protected override void SetUI(params object[] UIParams)
    {
        base.SetUI(UIParams);
        movie = UIParams[0] as MovieTexture;

    }
    protected override void OnRelease()
    {
        base.OnRelease();
        EventHandler();

    }
    
}
