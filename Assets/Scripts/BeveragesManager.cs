using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BeveragesManager : MonoBehaviour
{
    private List<GameObject> beverageGameObjects;

    [Tooltip("The parallax manager script.")]
    public Parallax parallaxManager;

    [Tooltip("The ballance meter script.")]
    public BeverageBallanceMeter ballanceMeter;

    [Tooltip("Reference to the game manager script.")]
    public GameManager gameManager;

    public bool shouldSpawn = false;
    
    internal List<List<Beverage>> beveragesList;
    internal List<Beverage> foodList;
    internal List<Beverage> drinksList;

    private float minTimeBetweenSpawn = 1.0f;
    private float maxTimeBetweenSpawn = 3.5f;

    private float startTimeBetweenSpawn;
    private float timeBetweenSpawn;

    /// <summary>
    /// Initialize the beverage lists and instantiate all types and variations of beverages available.
    /// </summary>
    private void Awake()
    {
        beveragesList = new List<List<Beverage>>();
        foodList = new List<Beverage>();
        drinksList = new List<Beverage>();

        drinksList.Add(new Beverage(BeverageTypes.Drink, "specialBeer", 4, 2));
        drinksList.Add(new Beverage(BeverageTypes.Drink, "beer", 2, 98));

        foodList.Add(new Beverage(BeverageTypes.Food, "burger", -1, 100));

        beveragesList.Add(foodList);
        beveragesList.Add(drinksList);
    }

    // Use this for initialization
    void Start()
    {
        float randomTimeBetweenSpawn = Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);

        startTimeBetweenSpawn = randomTimeBetweenSpawn;
        timeBetweenSpawn = startTimeBetweenSpawn;
    }

    /// <summary>
    /// Generate a random number and based on possibility spawn the corresponding beverage.
    /// </summary>
    public void RandomSpawnBeverage()
    {
        List<Beverage> beverageGroup = RandomBeverageGroupPicker();

        int randomPercentageValue = Random.Range(0, 101);
        int cumulative = 0;
        foreach (Beverage currentBeverage in beverageGroup)
        {
            cumulative += currentBeverage.PossibilityOfSpawning;
            if (randomPercentageValue < cumulative)
            {
                // Get the needed game object based on its tag.
                GameObject beverageGameObject = GameObject.FindGameObjectWithTag(currentBeverage.Name);

                // Instantiate a new beverage game object.
                GameObject spawnNewBeverage = GameObject.Instantiate(beverageGameObject);

                spawnNewBeverage.transform.parent = transform;
                spawnNewBeverage.transform.position = beverageGameObject.transform.position;

                // Add a new box collider 2D to the game object.
                BoxCollider2D newBoxCollider2D = spawnNewBeverage.AddComponent<BoxCollider2D>();

                newBoxCollider2D.isTrigger = true;
                newBoxCollider2D.enabled = true;

                // Enable the renderer of the game object and its children.
                Renderer newBeverageRenderer = spawnNewBeverage.GetComponent<Renderer>();
                newBeverageRenderer.enabled = true;

                int childrenCount = spawnNewBeverage.transform.childCount;
                for (int index = 0; index < childrenCount; index++)
                {
                    Transform currentChild = spawnNewBeverage.transform.GetChild(index);
                    Renderer currentChildRenderer = currentChild.GetComponent<Renderer>();

                    currentChildRenderer.enabled = true;
                }

                // Enable the script attached to the game object.
                spawnNewBeverage.GetComponent<BeverageRender>().enabled = true;

                break;
            }
        }
    }

    /// <summary>
    /// Randomly select one of all beverage groups.
    /// </summary>
    public List<Beverage> RandomBeverageGroupPicker()
    {
        int numberOfBeverageTypes = beveragesList.Count;
        int randomBeverageTypeIndex = Random.Range(0, numberOfBeverageTypes);

        return beveragesList[randomBeverageTypeIndex];
    }

    public void Update()
    {
        if (shouldSpawn && !gameManager.gameOver)
        {
            if (timeBetweenSpawn <= 0)
            {
                RandomSpawnBeverage();

                float randomTimeBetweenSpawn = Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);

                startTimeBetweenSpawn = randomTimeBetweenSpawn;

                timeBetweenSpawn = startTimeBetweenSpawn;
            }
            else
            {
                timeBetweenSpawn -= Time.deltaTime;
            }
        }
    }
}
