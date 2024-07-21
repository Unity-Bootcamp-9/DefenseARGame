using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject mapPrefab;

    public void StartGame()
    {
        GameObject mapPreview = FindObjectOfType<XRGrabInteractable>().gameObject;

        if (!mapPreview) return;

        if (mapPrefab)
        {
            GameObject temp = Instantiate(mapPrefab, mapPreview.transform.position, mapPreview.transform.rotation);
            temp.transform.localScale = mapPreview.transform.localScale * 1e-2f;
            temp.transform.Rotate(Vector3.up, -45f);
            Destroy(mapPreview);
        }
    }
}
