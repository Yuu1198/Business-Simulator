using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BerryBushParent : MonoBehaviour
{
    private float collectionRate = 10f;
    private float timer = 0f;
    public Slider progressBar;

    public TMP_Text berryProductionRateText;

    private bool berryBushExist = true;
    private bool inProduction = true;

    public Material activeMaterial;
    public Material inactiveMaterial;

    private void Start()
    {
        activeMaterial = transform.GetChild(0).GetComponent<Renderer>().material;

        UpdateProductionRateUI();
    }

    private void Update()
    {
        if (inProduction)
        {
            timer += Time.deltaTime;
            progressBar.value = timer / collectionRate;

            if (timer >= collectionRate)
            {
                if (transform.childCount > 0) // At least one berry bush in scene
                {
                    BerryBush bushToCollectFrom = transform.GetChild(0).GetComponent<BerryBush>(); // Get bush

                    // Collect berries
                    if (bushToCollectFrom != null)
                    {
                        GameManager.Instance.CollectBerries(bushToCollectFrom);
                    }
                    else
                    {
                        berryBushExist = false; // No more berries
                        UpdateProductionRateUI();
                    }
                }

                timer = 0f; // Reset
            }
        }
    }

    public void TurnProductionOnOrOff()
    {
        Renderer[] berryBushRenderer = transform.GetComponentsInChildren<Renderer>();

        if (inProduction)
        {
            inProduction = false;
            progressBar.gameObject.SetActive(false);

            foreach (Renderer r in berryBushRenderer)
            {
                r.material = inactiveMaterial;
            }
        }
        else
        {
            inProduction = true;
            progressBar.gameObject.SetActive(true);

            foreach (Renderer r in berryBushRenderer)
            {
                r.material = activeMaterial;
            }
        }

        UpdateProductionRateUI();
    }

    // UI
    private void UpdateProductionRateUI()
    {
        if (!berryBushExist || !inProduction)
        {
            berryProductionRateText.text = "Berry PR: 0 /s";
        }
        else
        {
            float berryProductionRatePerSecond = 10 / collectionRate;
            berryProductionRateText.text = "Berry PR: " + berryProductionRatePerSecond.ToString("F2") + " /s";
        }
    }
}
