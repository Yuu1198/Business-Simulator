using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheatField : MonoBehaviour
{
    private int wheatAmount = 6;

    private bool isPlanted = false;
    private bool isHarvestable = false;

    private float growthTime = 10f;
    private float growTimer = 0f;

    public Slider growProgressSlider;

    private void Start()
    {
        growProgressSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Harvesting
        if (isHarvestable)
        {
            HarvestWheat();
        }
    }

    // Player Input
    private void OnMouseDown()
    {
        // Planting
        if (!isPlanted)
        {
            PlantWheat();
        }
    }

    private void PlantWheat()
    {
        isPlanted = true;
        this.transform.GetChild(0).gameObject.SetActive(true);

        growProgressSlider.gameObject.SetActive(true);
        growProgressSlider.value = 0f;

        GameManager.Instance.OnFieldPlanted(wheatAmount, growthTime);

        StartCoroutine(GrowWheat());
    }

    // Production
    private IEnumerator GrowWheat()
    {
        growTimer = 0f;

        // Growing
        while (growTimer < growthTime)
        {
            growTimer += Time.deltaTime;
            growProgressSlider.value = growTimer / growthTime; // Update Progress Bar

            yield return null;
        }

        // Finished growing
        isHarvestable = true;
        
        GameManager.Instance.OnFieldGrown(wheatAmount, growthTime);

        growProgressSlider.gameObject.SetActive(false);
    }

    private void HarvestWheat()
    {
        // Reset
        isPlanted = false;
        isHarvestable = false;

        GameManager.Instance.CollectWheat(wheatAmount);

        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
