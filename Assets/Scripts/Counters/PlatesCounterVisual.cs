using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlateCounter plateCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> platesVisualGameObjectList;


    private void Awake() => platesVisualGameObjectList = new List<GameObject>();

    private void Start()
    {
        plateCounter.OnPlateSpawned += PlateCounter_OnPlateSpawned;
        plateCounter.OnPlateRemoved += PlateCounter_OnPlateRemoved;
    }

    private void PlateCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateVisualGameObject = platesVisualGameObjectList[platesVisualGameObjectList.Count - 1];
        platesVisualGameObjectList.Remove(plateVisualGameObject);
        Destroy(plateVisualGameObject);
    }

    private void PlateCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * platesVisualGameObjectList.Count, 0);

        platesVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
