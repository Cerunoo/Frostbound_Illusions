using UnityEngine;

[System.Serializable]
public class IncludeObjectsController
{
    public IncludeObjectsAfter includeObjectsAfter;
    public enum IncludeObjectsAfter { None, AfterGot, AfterPass }
    public GameObject[] includeObjects;

    public void TryInclude(IncludeObjectsAfter after)
    {
        if (after == includeObjectsAfter)
        {
            foreach (GameObject item in includeObjects) item.SetActive(true);
        }
    }

    public void Exclude()
    {
        foreach (GameObject item in includeObjects) item.SetActive(false);
    }
}