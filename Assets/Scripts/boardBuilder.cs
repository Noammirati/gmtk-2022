using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boardBuilder : MonoBehaviour
{
    public GameObject die;

    [Space(10)]

    public GameObject finish_tile;
    public GameObject finish_1_tile;
    public GameObject start_tile;
    public GameObject regular_tile;
    public GameObject wall_tile;
    public GameObject hole_tile;
    public GameObject icy_tile;
    public GameObject cracked_tile;
    public GameObject key_tile;
    public GameObject lock_tile;
    public GameObject rotate_tile;

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
            {'I', icy_tile},
            {'1', finish_1_tile},
            {'C', cracked_tile},
            {'K', key_tile},
            {'L', lock_tile},
            {'R', rotate_tile}
        };
    }

    public void loadLevel(string path, Vector3 pos)
    {
        string level_str = System.IO.File.ReadAllText(path);
        string[] rows = level_str.Split('\n');
        
        int width = rows[0].Split(' ').Length;
        int height = rows.Length;

        level = new string[height,width];
        for(int i = 0; i < height; i++) {
            string[] row = rows[i].Split(' ');
            for(int j = 0; j < width; j++) {
                level[i,j] = row[j];
            }
        }
        
        for(int i = 0; i < level.GetLength(0); i++){
            for(int j = 0; j < level.GetLength(1); j++) {
                GameObject clone = Instantiate(this.tiles[level[i, j][0]], new Vector3(i, 0, j), Quaternion.identity);
                clone.transform.parent = this.transform;
                clone.name = level[i, j];

                if(level[i, j][0] == 'S') {
                    this.die.transform.position = new Vector3(i, this.die.transform.position.y, j);
                    this.die.transform.rotation = Quaternion.identity;

                    this.die.transform.Find("pivot_X/Die").transform.rotation = Quaternion.identity;
                    this.die.transform.Find("pivot_mX/Die").transform.rotation = Quaternion.identity;
                }
            }
        }

        this.transform.position = pos;
    
    }

    public void clearBoard() {
        foreach (Transform child in transform) {
            if(child.gameObject.layer == 3) {
                Object.Destroy(child.gameObject);
            }
        }
    }

}
