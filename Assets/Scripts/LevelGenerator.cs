using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public int width = 20;
	[SerializeField]
	private int height = 40;

	[SerializeField]
	private int minPathLength = 1;
	[SerializeField]
	private int maxPathLength = 5;

    [SerializeField]
    private int numberOfSpiders = 10;
    [SerializeField]
    private GameObject spiderPrefab;

	[SerializeField]
	private GameObject upDownPrefab;
	[SerializeField]
	private GameObject upDownStickyPrefab;
	[SerializeField]
	private GameObject leftRightPrefab;
	[SerializeField]
	private GameObject leftRightStickyPrefab;

    private void Start()
    {
        MakeStickyGrid();
        MakePath();
        SprinkleWithSpiders(numberOfSpiders);
    }
    
    private void MakeStickyGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Instantiate(leftRightStickyPrefab, new Vector3(x, y, 0), Quaternion.identity);
                Instantiate(upDownStickyPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

    // At each step, choose a random straight length, and a random x between 0 and width.
    // Then build path straight from where you are random length units long, and then left or right to the random x.
    // If you are within say 5 units from the end, just build a straight path to the end.
    private void MakePath()
    {
        int x = Mathf.RoundToInt(width / 2);
        int y = 0;

        while (y + 5 < height)
        {
            int randomLength = Random.Range(minPathLength, maxPathLength + 1);
            int randomX = Random.Range(1, width - 1);

            // Build straight part
            for (int i = y; i < y + randomLength; i++)
            {
                Instantiate(upDownPrefab, new Vector3(x, i, 0), Quaternion.identity);

            }
            
            y += randomLength;

            // Build left/right part (if any)
            if (randomX < x)
            {
                for (int j = randomX; j < x; j++)
                {
                    Instantiate(leftRightPrefab, new Vector3(j, y, 0), Quaternion.identity);
                }
            }
            else if (randomX > x)
            {
                for (int k = x; k < randomX; k++)
                {
                    Instantiate(leftRightPrefab, new Vector3(k, y, 0), Quaternion.identity);
                }
            }

            x = randomX;
        }

        // Build last 5 or less straight pieces
        for (int n = y; n < height; n++)
        {
            Instantiate(upDownPrefab, new Vector3(x, n, 0), Quaternion.identity);
        }
    }

    private void SprinkleWithSpiders(int numberOfSpiders)
    {
        for (int i = 1; i <= numberOfSpiders; i++)
        {
            int x = Random.Range(0, width);
            int y = i * Mathf.FloorToInt(height / numberOfSpiders);

            Instantiate(spiderPrefab, new Vector3(x, y, 0), Quaternion.identity);
        }
    }
}