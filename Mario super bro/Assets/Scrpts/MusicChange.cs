using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MusicChange : MonoBehaviour
{
    public PlayableDirector playableDirector;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartTimeline();
        }
    }

    private void StartTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
        }
    }
}
