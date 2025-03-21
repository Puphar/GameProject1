using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MusicStop : MonoBehaviour
{
    public PlayableDirector playableDirector;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopTimeline();
        }
    }

    private void StopTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Stop();
        }
    }
}
