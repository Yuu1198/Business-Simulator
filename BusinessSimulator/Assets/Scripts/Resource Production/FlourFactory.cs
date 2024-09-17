using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlourFactory : MonoBehaviour
{
    private int neededWheat = 10;

    private float flourProductionTime = 20f;
    private float productionTimer = 0f;

    private bool inProduction = false;
    private bool finishedProduction = false;

    public Slider productionProgressBar;

    private void Start()
    {
        productionProgressBar.gameObject.SetActive(false);
    }

    // Player Input
    private void OnMouseDown()
    {
        if (!inProduction && !finishedProduction)
        {
            StartProduction(); // Produce
        }
        else if (finishedProduction)
        {
            CollectFlour(); // Collect
        }
    }

    private void StartProduction()
    {
        if (GameManager.Instance.GetWheat(neededWheat))
        {
            inProduction = true;

            productionProgressBar.gameObject.SetActive(true);
            productionProgressBar.value = 0f;

            GameManager.Instance.OnProductionUpdated(true, flourProductionTime);

            StartCoroutine(ProduceFlour());
        }
        else
        {
            return; // Production not possible
        }
    }

    private IEnumerator ProduceFlour()
    {
        productionTimer = 0f;
        while (productionTimer < flourProductionTime)
        {
            // Update timer and progress bar
            productionTimer += Time.deltaTime;
            productionProgressBar.value = productionTimer / flourProductionTime;

            yield return null;
        }

        // Finished
        finishedProduction = true;
        inProduction = false;

        GameManager.Instance.OnProductionUpdated(false, flourProductionTime);
    }

    private void CollectFlour()
    {
        // Reset
        finishedProduction = false;
        productionProgressBar.gameObject.SetActive(false);
        productionProgressBar.value = 0f;

        GameManager.Instance.CollectFlour();
    }

}
