using UnityEngine;
using UnityEngine.UI;

public class DistanceCounter : MonoBehaviour
{
    public Parallax parallax;

    public Text meterCounter;

    private int counter = 0;
    private int meters = 0;

    // Update the distance counter
    public void UpdateDistance()
    {
        counter++;

        // Reduce the counter update so that it doesn't update every px moved, but rather every 2nd px.
        if (counter % 2 == 0)
        {
            meters++;

            meterCounter.text = meters.ToString();
        }
    }
}
