using RPG.TimeOfDay;
using UnityEngine;

public class CheckTimeOfDayLights : MonoBehaviour
{
    GameObject sun;

    private void Awake()
    {
        sun = GameObject.FindWithTag("TimeOfDay");
    }
    private void Update()
    {
        if (sun.GetComponent<TimeOfDay>().IsNight())
        {
            gameObject.GetComponent<Light>().enabled = true;
        }
        else { gameObject.GetComponent<Light>().enabled = false; }
    }
}
