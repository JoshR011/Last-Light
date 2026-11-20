/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Date Created: 2026-11-19
 * Date Last Updated: 2026-11-20
 * Summary: Fades between outdoor exploration, indoor exploration, and combat music.
 */

using UnityEngine;

public class AdaptiveMusic : MonoBehaviour
{
    [Header("Outdoor Exploration")]
    public AudioSource outside;

    [Header("Interior Exploration")]
    public AudioSource belowGround;

    [Header("Combat")]
    public AudioSource midTension;

    private const float MusicVolume = 0.5f;
    private const float FadeSeconds = 1.5f;
    private float combatUntil;

    public void RecordCombat()
    {
        // Refresh the timer on each combat event so brief gaps between attacks keep the combat track playing.
        combatUntil = Time.time + 10f;
    }

    private void Update()
    {
        DockHouseTravel travel = GetComponent<DockHouseTravel>();

        AudioSource selectedTrack = outside;
        if (travel != null && travel.IsInside)
        {
            selectedTrack = belowGround;
        }
        if (Time.time < combatUntil)
        {
            selectedTrack = midTension;
        }

        FadeTrackVolume(outside, selectedTrack);
        FadeTrackVolume(belowGround, selectedTrack);
        FadeTrackVolume(midTension, selectedTrack);
    }

    private void FadeTrackVolume(AudioSource track, AudioSource selectedTrack)
    {
        float step = MusicVolume * Time.deltaTime / FadeSeconds;

        if (track == selectedTrack)
        {
            track.volume = Mathf.Min(MusicVolume, track.volume + step);
        }
        else
        {
            track.volume = Mathf.Max(0f, track.volume - step);
        }
    }
}
