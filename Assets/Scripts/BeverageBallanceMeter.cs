using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BeverageBallanceMeter : MonoBehaviour
{
    public GameManager gameManager;

    [Tooltip("The ballance slide game object.")]
    public Slider ballanceSlider;

    [Tooltip("The beverage manager script.")]
    public BeveragesManager beveragesManager;

    public Parallax parallaxManager;

    private int ballanceSliderValue;

    [SerializeField]
    [Tooltip("The wait time before the player gets caught if staying slow for too long.")]
    private float maxGetCaughtWaitTime = 25.0f;

    [SerializeField]
    [Tooltip("The wait time before the player dies from alcohol if staying drunk for too long.")]
    private float maxDieWaitTime = 10.0f;

    private float dieCounter = 0.0f;
    private float getCaughtCounter = 0.0f;

    // Use this for initialization
    void Start()
    {
        ballanceSliderValue = 0;

        // TODO: Check if this is appropriate to do or should better use a variable for the value
        ChangeBalanceLevel(2);
    }

    public void ChangeBalanceLevel(int additionalBeverageLevel)
    {
        ballanceSliderValue += additionalBeverageLevel;

        // Limit the alcohol count to be within the range of 0 and the maxValue of ballanceSlider
        ballanceSliderValue = (int)Mathf.Clamp(ballanceSliderValue, 0, ballanceSlider.maxValue);
        
        ballanceSlider.value = ballanceSliderValue;
        parallaxManager.ChangeSpeed(ballanceSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        if (ballanceSliderValue >= ballanceSlider.maxValue - 3)
        {
            dieCounter += 1 * Time.deltaTime;
        }
        else
        {
            dieCounter = 0;
        }

        if (dieCounter >= maxDieWaitTime)
        {
            gameManager.GameOver(false, true);
        }

        if (ballanceSliderValue <= 3)
        {
            getCaughtCounter += 1 * Time.deltaTime;
        }
        else
        {
            getCaughtCounter = 0;
        }

        if (getCaughtCounter >= maxGetCaughtWaitTime)
        {
            gameManager.GameOver(true, false);
        }
    }
}
