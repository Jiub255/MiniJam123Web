using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int width = 20;
    public int height = 40;

    [SerializeField]
    private int numberOfPaths = 5;

	[SerializeField]
	private int minPathLength = 1;
	[SerializeField]
	private int maxPathLength = 5;

    [SerializeField]
    private int numberOfSpiders = 10;
    [SerializeField]
    private int bossNumberOfSpiders = 10;
    [SerializeField]
    private GameObject spiderPrefab;
    [SerializeField]
    private GameObject bossPrefab;

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
        // First Level
        MakeStickyGrid();
        SprinkleWithSpiders(numberOfSpiders);

        for (int i = 0; i < numberOfPaths; i++)
        {
            MakePath();
        }

        // Make safe zone on bottom to start on
        for (int x = 0; x < width; x++)
        {
            Instantiate(leftRightPrefab, new Vector3(x, -2, 0), Quaternion.identity);
            Instantiate(upDownPrefab, new Vector3(x, -2, 0), Quaternion.identity);

            Instantiate(leftRightPrefab, new Vector3(x, -1, 0), Quaternion.identity);
            Instantiate(upDownPrefab, new Vector3(x, -1, 0), Quaternion.identity);

            Instantiate(leftRightPrefab, new Vector3(x, 0, 0), Quaternion.identity);
            Instantiate(upDownPrefab, new Vector3(x, 0, 0), Quaternion.identity);

            Instantiate(leftRightPrefab, new Vector3(x, 1, 0), Quaternion.identity);
            Instantiate(upDownPrefab, new Vector3(x, 1, 0), Quaternion.identity);

            Instantiate(leftRightPrefab, new Vector3(x, 2, 0), Quaternion.identity);
        }


        // Boss Level
        MakeStickyGrid(height * -4);
        SprinkleWithSpiders(bossNumberOfSpiders, height * -4);

        for (int i = 0; i < numberOfPaths; i++)
        {
            MakePath(height * -4);
        }

        // Make safe zone on top to start on
        for (int x = 0; x < width; x++)
        {
            Instantiate(leftRightPrefab, new Vector3(x, (-3 * height) - 4, 0), Quaternion.identity);

            Instantiate(leftRightPrefab, new Vector3(x, (-3 * height) - 5, 0), Quaternion.identity);
            Instantiate(upDownPrefab, new Vector3(x, (-3 * height) - 5, 0), Quaternion.identity);

            Instantiate(leftRightPrefab, new Vector3(x, (-3 * height) - 6, 0), Quaternion.identity);
            Instantiate(upDownPrefab, new Vector3(x, (-3 * height) - 6, 0), Quaternion.identity);

            Instantiate(leftRightPrefab, new Vector3(x, (-3 * height) - 7, 0), Quaternion.identity);
            Instantiate(upDownPrefab, new Vector3(x, (-3 * height) - 7, 0), Quaternion.identity);

            Instantiate(leftRightPrefab, new Vector3(x, (-3 * height) - 8, 0), Quaternion.identity);
            Instantiate(upDownPrefab, new Vector3(x, (-3 * height) - 8, 0), Quaternion.identity);
        }

        // Instantiate boss spider
        Instantiate(bossPrefab, new Vector3(Mathf.RoundToInt(width / 2), (-3 * height) + 5, 0), Quaternion.identity);
    }
    
    private void MakeStickyGrid(int yOffset = 0)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Instantiate(leftRightStickyPrefab, new Vector3(x, y + yOffset, 0), Quaternion.identity);
                Instantiate(upDownStickyPrefab, new Vector3(x, y + yOffset, 0), Quaternion.identity);
            }
        }
    }

    // At each step, choose a random straight length, and a random x between 0 and width.
    // Then build path straight from where you are random length units long, and then left or right to the random x.
    // If you are within say 5 units from the end, just build a straight path to the end.
    private void MakePath(int yOffset = 0)
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
                Instantiate(upDownPrefab, new Vector3(x, i + yOffset, 0), Quaternion.identity);

            }
            
            y += randomLength;

            // Build left/right part (if any)
            if (randomX < x)
            {
                for (int j = randomX; j < x; j++)
                {
                    Instantiate(leftRightPrefab, new Vector3(j, y + yOffset, 0), Quaternion.identity);
                }
            }
            else if (randomX > x)
            {
                for (int k = x; k < randomX; k++)
                {
                    Instantiate(leftRightPrefab, new Vector3(k, y + yOffset, 0), Quaternion.identity);
                }
            }

            x = randomX;
        }

        // Build last 5 or less straight pieces
        for (int n = y; n < height; n++)
        {
            Instantiate(upDownPrefab, new Vector3(x, n + yOffset, 0), Quaternion.identity);
        }
    }

    private void SprinkleWithSpiders(int numberOfSpiders, int yOffset = 0)
    {
        for (int i = 1; i <= numberOfSpiders; i++)
        {
            int x = Random.Range(0, width);
            int y = i * Mathf.FloorToInt(height / numberOfSpiders);

            Instantiate(spiderPrefab, new Vector3(x, y + yOffset, 0), Quaternion.identity);
        }
    }
}