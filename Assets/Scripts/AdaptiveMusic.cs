using UnityEngine;

public class AdaptiveMusic : MonoBehaviour
{
    public AudioSource outside;
    public AudioSource belowGround;
    public AudioSource midTension;

    private const float MusicVolume = 0.5f;
    private const float FadeSeconds = 1.5f;
    private float combatUntil;

    public void RecordCombat()
    {
        combatUntil = Time.time + 10f;
    }

    private void Update()
    {
        // The door script adds this component the first time you use a dock door
        DockHouseTravel travel = GetComponent<DockHouseTravel>();

        AudioSource target = outside;
        if (travel != null && travel.IsInside)
        {
            target = belowGround;
        }
        if (Time.time < combatUntil)
        {
            target = midTension;
        }

        Fade(outside, target);
        Fade(belowGround, target);
        Fade(midTension, target);
    }

    private void Fade(AudioSource source, AudioSource target)
    {
        float step = MusicVolume * Time.deltaTime / FadeSeconds;

        if (source == target)
        {
            source.volume = Mathf.Min(MusicVolume, source.volume + step);
        }
        else
        {
            source.volume = Mathf.Max(0f, source.volume - step);
        }
    }
}