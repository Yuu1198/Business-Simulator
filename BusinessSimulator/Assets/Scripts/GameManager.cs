using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [Header("Water")]
    public int playerWaterAmount;
    private int playerMaxWaterCapacity = 100;

    public TMP_Text waterAmountText;
    public TMP_Text waterProductionRateText;

    [Header("Berries")]
    public float playerBerryAmount;
    public TMP_Text berryAmountText;

    [Header("Wheat")]
    public TMP_Text wheatAmountText;
    private int playerWheatAmount;

    private int activeFields;
    public TMP_Text wheatProductionRateText;

    [Header("Flour")]
    public TMP_Text flourAmountText;
    private int playerFlourAmount;

    public TMP_Text flourProductionRateText;

    [Header("Dough")]
    public TMP_Text doughAmountText;
    private int playerDoughAmount;

    public TMP_Text doughProductionRateText;

    [Header("Pie")]
    public TMP_Text pieAmountText;
    private int playerPieAmount;

    public TMP_Text pieProductionRateText;

    #region Water
    public void CollectWater(Well well)
    {
        if (playerWaterAmount < playerMaxWaterCapacity)
        {
            // Transfer water from the well to the player
            playerWaterAmount++;
            well.waterAmount--;

            waterAmountText.text = "Water: " + playerWaterAmount;
        }
    }
    #endregion

    #region Berries
    public void CollectBerries(BerryBush berryBush)
    {
        playerBerryAmount += berryBush.berryAmount;

        Destroy(berryBush.gameObject);

        berryAmountText.text = "Berries: " + playerBerryAmount;
    }
    #endregion

    #region Wheat
    public void CollectWheat(int amount)
    {
        playerWheatAmount += amount;

        wheatAmountText.text = "Wheat: " + playerWheatAmount;
    }

    public void UpdateWheatProductionRate(int wheatPerField, float wheatGrowTime)
    {
        float productionRate = (activeFields * wheatPerField) / wheatGrowTime;
        wheatProductionRateText.text = "Wheat PR: " + productionRate.ToString("F2") + " /s";
    }

    public void OnFieldPlanted(int wheatPerField, float wheatGrowTime)
    {
        activeFields++;
        UpdateWheatProductionRate(wheatPerField, wheatGrowTime);
    }

    public void OnFieldGrown(int wheatPerField, float wheatGrowTime)
    {
        activeFields--;
        UpdateWheatProductionRate(wheatPerField, wheatGrowTime);
    }
    #endregion

    #region Flour

    public bool GetWheat(int neededWheatAmount)
    {
        if (playerWheatAmount >= neededWheatAmount) // Enough wheat
        {
            playerWheatAmount -= neededWheatAmount; // Use wheat
            wheatAmountText.text = "Wheat: " + playerWheatAmount;
            return true;
        }
        else
        {
            return false; // Not enough wheat
        }
    }

    public void CollectFlour()
    {
        playerFlourAmount++;

        flourAmountText.text = "Flour: " + playerFlourAmount;
    }

    public void OnFlourProductionUpdated(bool active, float flourProductionTime)
    {
        if (active)
        {
            float productionTime = 1 / flourProductionTime;
            flourProductionRateText.text = "Flour PR: " + productionTime.ToString("F2") + " /s";
        }
        else
        {
            flourProductionRateText.text = "Flour PR: 0 /s";
        }
    }
    #endregion

    #region Dough
    public bool GetFlourAndWater(int neededFlourAmount, int neededWaterAmount)
    {
        if (playerFlourAmount >= neededFlourAmount && playerWaterAmount >= neededWaterAmount) // Enough resources
        {
            // Use resources
            playerFlourAmount -= neededFlourAmount;
            playerWaterAmount -= neededWaterAmount;
            flourAmountText.text = "Flour: " + playerFlourAmount;
            waterAmountText.text = "Water: " + playerWaterAmount;
            return true;
        }
        else
        {
            return false; // Not enough resources
        }
    }

    public void CollectDough()
    {
        playerDoughAmount++;

        doughAmountText.text = "Dough: " + playerDoughAmount;
    }

    public void OnDoughProductionUpdated(bool active, float doughProductionTime)
    {
        if (active)
        {
            float productionTime = 1 / doughProductionTime;
            doughProductionRateText.text = "Dough PR: " + productionTime.ToString("F2") + " /s";
        }
        else
        {
            doughProductionRateText.text = "Dough PR: 0 /s";
        }
    }
    #endregion

    #region Pie
    public bool GetDoughAndBerries(int neededDoughAmount, int neededBerriesAmount)
    {
        if (playerDoughAmount >= neededDoughAmount && playerBerryAmount >= neededBerriesAmount) // Enough resources
        {
            // Use resources
            playerDoughAmount -= neededDoughAmount;
            playerBerryAmount -= neededBerriesAmount;
            doughAmountText.text = "Dough: " + playerDoughAmount;
            berryAmountText.text = "Berries: " + playerBerryAmount;
            return true;
        }
        else
        {
            return false; // Not enough resources
        }
    }

    public void OnPieProductionUpdated(bool active, float pieProductionTime)
    {
        if (active)
        {
            float productionTime = 1 / pieProductionTime;
            pieProductionRateText.text = "Pie PR: " + productionTime.ToString("F2") + " /s";
        }
        else
        {
            pieProductionRateText.text = "Pie PR: 0 /s";
        }
    }

    public void CollectPie()
    {
        playerPieAmount++;

        pieAmountText.text = "Pie: " + playerPieAmount;
    }
    #endregion
}