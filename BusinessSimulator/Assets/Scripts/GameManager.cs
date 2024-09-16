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
    
    private void Start()
    {
        waterCollectCooldownSlider.gameObject.SetActive(false);
    }

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
}
