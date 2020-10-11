using UnityEngine;

public class Maze : MonoBehaviour
{
    public Transform floor;
    public Texture2D map;
    public GameObject wallPrefab;
    public GameObject endPrefab;
    private Vector3 startPos;

    private int reward = 10;
    private string returnScene = "Main";

    void Start()
    {
        floor.localScale = new Vector3(map.width, 1, map.height);
        floor.position = new Vector3(map.width / 2, -0.5f, map.height / 2);
        print($"w: {map.width} h: {map.height}");
        for(int i = 0; i < map.width; i++)
        {
            for(int j = 0; j < map.height; j++)
            {
                Color c = map.GetPixel(i, j);
                print($"c: {c}");
                if (c == Color.black)
                    Instantiate(wallPrefab, new Vector3(i, 1, j), Quaternion.identity);
                else if (c == Color.red)
                    startPos = new Vector3(i, 0, j);
                else if (c == Color.blue)
                {
                    MazeEndBlock m = Instantiate(endPrefab, new Vector3(i, 0, j), Quaternion.identity).GetComponent<MazeEndBlock>();
                    m.reward = reward;
                    m.returnScene = returnScene;
                }
            }
        }

        print("sp: " + startPos);
        GameObject.Find("Player").transform.position = startPos;
    }
}