using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Well : MonoBehaviour
{
    private float regenInterval = 5f;
    private float regenTimer = 0f;

    public int waterAmount;
    private int maxWaterCapacity = 100;

    public Material activeMaterial;
    public Material inactiveMaterial;
    private Renderer wellRenderer;

    private bool inProduction = true;

    private void Start()
    {
        wellRenderer = GetComponent<Renderer>();
        activeMaterial = wellRenderer.material;

        UpdateWaterProductionRateUI();
    }

    // Production
    private void Update()
    {
        if (inProduction)
        {
            // Increment the timer
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenInterval) // Check if regenInterval seconds have passed
            {
                if (waterAmount < maxWaterCapacity)
                {
                    // Increment water amount of well
                    waterAmount++;

                    UpdateWaterProductionRateUI();
                }
                else
                {
                    UpdateWaterProductionRateUI(true);
                }

                // Reset timer for next regenInterval
                regenTimer = 0f;
            }
        }

        // Collect water if there is any
        if (waterAmount > 0)
        {
            GameManager.Instance.CollectWater(this);
        }
    }

    private void OnMouseDown()
    {
        if (inProduction)
        {
            // Turn production off
            inProduction = false;
            wellRenderer.material = inactiveMaterial;
        }
        else
        {
            // Turn production on
            inProduction = true;
            wellRenderer.material = activeMaterial;
        }
    }

    // UI
    private void UpdateWaterProductionRateUI(bool wellFull = false)
    {
        if (wellFull)
        {
            GameManager.Instance.waterProductionRateText.text = "Water PR: 0 units/s";
        }
        else
        {
            float waterProductionRatePerSecond = 1 / regenInterval;
            GameManager.Instance.waterProductionRateText.text = "Water PR: " + waterProductionRatePerSecond.ToString("F2") + " units/s";
        }
    }
}
