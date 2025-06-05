using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Temp : MonoBehaviour
{
    #region ==========Fields==========
    [SerializeField] private PlayableDirector director;
    private TimelineAsset timelineAsset;
    private int curPoint = 0;
    #endregion

    #region ==========Unity Methods==========
    private void Awake()
    {
        timelineAsset = director.playableAsset as TimelineAsset;
        director.Pause();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (director.state == PlayState.Paused) director.Play();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Application.Quit(0);
        }
    }
    #endregion

    #region ==========Methods==========

    #endregion
}