using System;

public class Beverage
{
    public Beverage(BeverageTypes typeOfBeverage, string name, int levelOfEffect, int possibilityOfSpawning)
    {
        TypeOfBeverage = typeOfBeverage;
        Name = name;
        LevelOfEffect = levelOfEffect;
        PossibilityOfSpawning = possibilityOfSpawning;
    }

    public BeverageTypes TypeOfBeverage
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }

    public int LevelOfEffect
    {
        get;
        set;
    }

    public int PossibilityOfSpawning
    {
        get;
        set;
    }
}