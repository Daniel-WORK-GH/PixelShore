using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public float time = 0.75f;
    public float fishChace = 0.9f;

    private float randomNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Start generating a random number every 0.5 seconds
        InvokeRepeating("GenerateRandomNumber", 0f, time);
    }

    void GenerateRandomNumber()
    {
        // Generate a random number (for example, between 0 and 100)
        randomNumber = Random.Range(0f, 100f);
    }
    public bool TryCatchFish()
    {
        Debug.Log(randomNumber > (100 - fishChace * 100));
        return randomNumber > (100 - fishChace * 100);
    }
}
