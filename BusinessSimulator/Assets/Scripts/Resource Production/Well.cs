using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : MonoBehaviour
{
    private float waterRegenRate = 5;
    private float maxWaterCapacity = 100;
    private float regenInterval = 10f;
    private float regenTimer = 0f;

    public float waterAmount;

    public Material activeMaterial;
    public Material cooldownMaterial;
    private Renderer wellRenderer;

    private void Start()
    {
        wellRenderer = GetComponent<Renderer>();
        activeMaterial = wellRenderer.material;

        UpdateProductionRateDisplay();
    }

    private void Update()
    {
        // Increment the timer
        regenTimer += Time.deltaTime;

        if (regenTimer >= regenInterval) // Check if regenInterval seconds have passed
        {
            // Only generate water if there's space for more
            if (waterAmount + waterRegenRate <= maxWaterCapacity)
            {
                waterAmount += waterRegenRate;
            }
            else
            {
                waterAmount = maxWaterCapacity; // Maximum capacity
            }

            // Reset timer for next regenInterval
            regenTimer = 0f;
        }

        // Well material to indicate wheter water can be collected
        if (waterAmount >= 10)
        {
            wellRenderer.material = activeMaterial; 
        }
        else
        {
            wellRenderer.material = cooldownMaterial;
        }
    }

    private void OnMouseDown()
    {
        // Call the function to collect water when the player clicks on the well
        GameManager.Instance.CollectWater(this);
    }

    private void UpdateProductionRateDisplay()
    {
        GameManager.Instance.waterProductionRateText.text = "Water: " + waterRegenRate / regenInterval + " units / s";
    }
}
