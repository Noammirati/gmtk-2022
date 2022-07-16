using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boardBuilder : MonoBehaviour
{
    public GameObject die;

    [Space(10)]

    public GameObject finish_tile;
    public GameObject start_tile;
    public GameObject regular_tile;
    public GameObject wall_tile;
    public GameObject icy_tile;

    private Dictionary<char, GameObject> tiles;

    private string[,] level;

    // Start is called before the first frame update
    void Awake()
    {
        this.tiles = new Dictionary<char, GameObject>(){
            {'F', finish_tile},
            {'S', start_tile},
            {'0', regular_tile},
            {'X', wall_tile},
            {'I', icy_tile}
        };

        string level_str = System.IO.File.ReadAllText("Assets/Levels/test.lvl");
        string[] rows = level_str.Split('\n');
        int dim = rows[0].Split(' ').Length;

        level = new string[dim,dim];
        for(int i = 0; i < dim; i++) {
            string[] row = rows[i].Split(' ');
            for(int j = 0; j < dim; j++) {
                level[i,j] = row[j];
            }
        }
    }

    void Start()
    {
        for(int i = 0; i < level.GetLength(0); i++){
            for(int j = 0; j < level.GetLength(1); j++) {
                GameObject clone = Instantiate(this.tiles[level[i, j][0]], new Vector3(i, 0, j), Quaternion.identity);
                clone.transform.parent = this.transform;
                clone.name = level[i, j];

                if(level[i, j][0] == 'S') {
                    this.die.transform.position = new Vector3(i, this.die.transform.position.y, j);
                }
            }
        }
        this.transform.localPosition = new Vector3(0.5f + level.GetLength(0)/-2, 1, 0.5f + level.GetLength(1)/-2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
