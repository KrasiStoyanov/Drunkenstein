using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BeverageRender : MonoBehaviour
{
    [Tooltip("The parallax manager script.")]
    public Parallax parallaxManager;

    [Tooltip("The beverage manager script.")]
    public BeveragesManager beveragesManager;

    [Tooltip("The ballance slide game object.")]
    public BeverageBallanceMeter ballanceSlider;

    [Tooltip("Reference to the game manager script.")]
    public GameManager gameManager;

    [Tooltip("Select the type of the current beverage.")]
    public BeverageTypes beverageType;

    [Tooltip("Select the type of the current beverage.")]
    public NamesOfBeverages beverageName;

    private Beverage beverage;
    private int levelOfEffect;

    private Vector3 maxFloatingPositionUp;
    private Vector3 maxFloatingPositionDown;
    private bool isGoingUp = true;

    private Vector3 initialShadowPosition;
    private float smallShadowSize = 0.5f;
    private float bigShadowSize = 1.5f;

    private const float cameraWidth = 19.2f;
    private const float floatingSpeed = 0.4f;
    [SerializeField]
    private const float shadowScaleSpeed = 2.4f;

    // Use this for initialization
    void Start()
    {
        switch (beverageType)
        {
            case BeverageTypes.Drink:
                beverage = beveragesManager.drinksList.Find(b => b.Name == beverageName.ToString());
                break;
            case BeverageTypes.Food:
                beverage = beveragesManager.foodList.Find(b => b.Name == beverageName.ToString());
                break;
        }

        levelOfEffect = beverage.LevelOfEffect;

        maxFloatingPositionUp = new Vector3(0, -2.5f, 0);
        maxFloatingPositionDown = new Vector3(0, -2.7f, 0);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !gameManager.gameOver)
        {
            Destroy(gameObject);

            ballanceSlider.ChangeBalanceLevel(levelOfEffect);
        }
    }

    private void Move()
    {
        float deltaTime = Time.deltaTime;
        float scrollSpeed = parallaxManager.scrollSpeed;
        Transform shadow = transform.GetChild(0);

        if (transform.position.y >= maxFloatingPositionUp.y)
        {
            isGoingUp = false;
        }
        else if (transform.position.y <= maxFloatingPositionDown.y)
        {
            isGoingUp = true;
        }

        Vector3 updatedPosition = new Vector3();
        updatedPosition = Vector3.left * scrollSpeed * deltaTime;
        if (isGoingUp)
        {
            updatedPosition = updatedPosition + (Vector3.up * deltaTime * floatingSpeed);
            shadow.localScale = Vector3.Lerp(shadow.localScale, new Vector3(smallShadowSize, smallShadowSize, 0.0f), shadowScaleSpeed * deltaTime);
        }
        else
        {
            updatedPosition = updatedPosition + (Vector3.down * deltaTime * floatingSpeed);
            shadow.localScale = Vector3.Lerp(shadow.localScale, new Vector3(bigShadowSize, bigShadowSize, 0.0f), shadowScaleSpeed * deltaTime);
        }

        transform.Translate(updatedPosition, Space.World);
        foreach (Transform child in transform)
        {
            child.position = new Vector3(child.position.x, child.position.y - updatedPosition.y, child.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        float widthOfSprite = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        float borderPosition = -((cameraWidth / 2) + widthOfSprite);
        if (transform.position.x <= borderPosition)
        {
            Destroy(gameObject);
        }
    }
}
