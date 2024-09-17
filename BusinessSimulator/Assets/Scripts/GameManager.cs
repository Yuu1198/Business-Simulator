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
    public float playerWaterAmount;

    private float playerMxWaterCapacity = 100;
    private float waterCollectionRate = 10;

    public bool canCollectWater = true;
    private float waterCollectCooldown = 10f;

    public TMP_Text waterAmountText;
    public TMP_Text waterProductionRateText;
    public Slider waterCollectCooldownSlider;

    [Header("Berries")]
    public GameObject berryBushParent;
    public float playerBerryAmount;
    private int berryBushAmount;

    public TMP_Text berryAmountText;
    public TMP_Text berryBushesAmountText;

    [Header("Wheat")]
    public TMP_Text wheatAmountText;
    private int playerWheatAmount;

    private int activeFields;
    public TMP_Text wheatProductionRateText;

    [Header("Flour")]
    public TMP_Text flourAmountText;
    private int playerFlourAmount;

    public TMP_Text flourProductionRateText;

    private void Start()
    {
        waterCollectCooldownSlider.gameObject.SetActive(false);

        // Berry Bushes
        Transform[] berryBushes = berryBushParent.GetComponentsInChildren<Transform>();
        berryBushAmount = berryBushes.Length;
        berryBushesAmountText.text = "Berry Bushes: " + berryBushAmount;
    }

    #region Water
    public void CollectWater(Well well)
    {
        if (canCollectWater)
        {
            if (well.waterAmount >= waterCollectionRate) // Check if the well has enough water to collect
            {
                if (playerWaterAmount + waterCollectionRate <= playerMxWaterCapacity) // Check if max capacity is overshot
                {
                    // Transfer water from the well to the player
                    playerWaterAmount += waterCollectionRate;
                    well.waterAmount -= waterCollectionRate;

                    waterAmountText.text = "Water: " + playerWaterAmount;
                }
                else
                {
                    waterAmountText.text = "Water: " + playerWaterAmount + "(full)";
                }

                // Start cooldown
                StartCoroutine(WaterCooldown());
            }
        }
    }

    private IEnumerator WaterCooldown()
    {
        canCollectWater = false;  // Disable water collection
        // Slider
        waterCollectCooldownSlider.gameObject.SetActive(true);
        waterCollectCooldownSlider.value = 0;

        // Cooldown
        float elapsed = 0f;
        while (elapsed < waterCollectCooldown)
        {
            elapsed += Time.deltaTime;

            // Update slider
            waterCollectCooldownSlider.value = elapsed / waterCollectCooldown;

            yield return null;
        }

        canCollectWater = true;
        waterCollectCooldownSlider.gameObject.SetActive(false);
    }
    #endregion

    #region Berries
    public void CollectBerries(BerryBush berryBush)
    {
        playerBerryAmount += berryBush.berryAmount;
        berryBushAmount--;

        Destroy(berryBush.gameObject);

        berryAmountText.text = "Berries: " + playerBerryAmount;
        berryBushesAmountText.text = "Berry Bushes: " + berryBushAmount;
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

    public void OnProductionUpdated(bool active, float flourProductionTime)
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
}