using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineControl : MonoBehaviour
{
    #region ==========Fields==========
    [SerializeField] private PlayableDirector director;
    private TimelineAsset timelineAsset;
    private IMarker[] markers;
    private int curPoint = 0;
    #endregion

    #region ==========Unity Methods==========
    private void Awake()
    {
        timelineAsset = director.playableAsset as TimelineAsset;
        markers = timelineAsset.markerTrack.GetMarkers().ToArray();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            JumpToNextPoint();
        }
    }
    #endregion

    #region ==========Methods==========
    public void JumpToNextPoint()
    {
        if (curPoint >= markers.Length) return;
        IMarker curMarker = markers[curPoint];
        if (director.time >= curMarker.time)
        {
            curPoint++;
            JumpToNextPoint();
        }
        else
        {
            director.time = curMarker.time;
        }
    }
    #endregion
}
