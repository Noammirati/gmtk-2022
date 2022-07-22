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

    [Space(10)]

    public GameObject one;
    public GameObject two;
    public GameObject three;
    public GameObject four;
    public GameObject five;
    public GameObject six;

    private Dictionary<char, GameObject> tiles;
    private Dictionary<char, GameObject> access_obj;

    private Dictionary<string ,string> levels_dict;
    private Dictionary<string, string> access_dict;

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
            {'R', rotate_tile},
            {'H', hole_tile}
        };

        this.access_obj = new Dictionary<char, GameObject>(){
            {'1', one},
            {'2', two},
            {'3', three},
            {'4', four},
            {'5', five},
            {'6', six},
            {'0', new GameObject()}
        };

        levels_dict = new Dictionary<string, string>();
        access_dict = new Dictionary<string, string>();

        levels_dict.Add("test", "0 0 0\n0 R 0\nS 0 R\nF 0 0");
        access_dict.Add("test", "1 2 3\n4 5 6\n1 2 3\n4 6 5");

        levels_dict.Add("intro", "X X X X\nX 0 F X\nX S 0 X\nX X X X");
        access_dict.Add("intro", "0 0 0 0\n0 0 0 0\n0 0 0 0\n0 0 0 0");

        levels_dict.Add("level1", "S 0 0\n0 0 0\n0 0 F");
        access_dict.Add("level1", "0 0 0\n0 0 0\n0 0 0");

        levels_dict.Add("level2", "0 0 0 F\n0 H 0 H\n0 I S 0");
        access_dict.Add("level2", "0 0 0 0\n0 0 0 0\n0 0 0 0");

        levels_dict.Add("level3", "F 0 0 0\n0 H H 0\n0 0 I S\nH 0 0 0");
        access_dict.Add("level3", "0 0 0 0\n0 0 0 0\n0 0 0 0\n0 0 0 0");

        levels_dict.Add("level4", "X R X X\n0 L H K\nH 0 I 0\nH 0 H I\nS 0 0 0\nF X X X");
        access_dict.Add("level4", "0 0 0 0\n0 6 0 6\n0 0 0 0\n0 0 0 0\n0 0 0 0\n2 0 0 0");

        levels_dict.Add("level5", "X 0 C 0 I 0\nX C H 0 H 0\n0 0 0 0 H 0\nK X L 0 H 0\nX X X X F S");
        access_dict.Add("level5", "0 0 0 0 0 0\n0 0 0 0 0 0\n0 0 0 0 0 0\n5 0 0 0 0 0\n0 0 0 0 3 0");

        levels_dict.Add("level6", "X 0 0 0 I 0\nX I X 0 X 0\nF 0 0 I 0 0\nX 0 X 0 X I\nX 0 I 0 0 S");
        access_dict.Add("level6", "0 0 0 0 0 0\n0 0 0 0 0 0\n6 0 0 0 0 0\n0 0 0 0 0 0\n0 0 0 0 0 0");

        levels_dict.Add("level7", "0 C 0 0 0 0 0\n0 H C H 0 H 0\n0 0 0 S I 0 I\nX 0 0 H 0 I 0\nF L 0 C 0 0 K");
        access_dict.Add("level7", "0 0 0 0 0 0 0\n0 0 0 0 0 0 0\n0 0 0 0 0 0 0\n0 0 0 0 0 0 0\n2 0 0 0 0 0 4");
    }

    public void realign(Vector3 pos) {
        this.transform.position = pos;
    }

    public void loadLevel(string path)
    {
        string level_str = levels_dict[path];
        string[] rows = level_str.Split('\n');
        
        int width = rows[0].Split(' ').Length;
        int height = rows.Length;

        string[,] lvl;
        lvl = new string[height,width];
        for(int i = 0; i < height; i++) {
            string[] row = rows[i].Split(' ');
            for(int j = 0; j < width; j++) {
                lvl[i,j] = row[j];
            }
        }
        
        for(int i = 0; i < lvl.GetLength(0); i++){
            for(int j = 0; j < lvl.GetLength(1); j++) {
                GameObject clone = Instantiate(this.tiles[lvl[i, j][0]], new Vector3(i, 0, j), Quaternion.identity);
                clone.transform.parent = this.transform;
                clone.name = lvl[i, j];

                if(lvl[i, j][0] == 'S') {
                    this.die.transform.position = new Vector3(i, this.die.transform.position.y, j);
                    this.die.transform.rotation = Quaternion.identity;

                    this.die.transform.Find("pivot_X/Die").transform.rotation = Quaternion.identity;
                    this.die.transform.Find("pivot_mX/Die").transform.rotation = Quaternion.identity;
                }
            }
        }
    }

    public void loadAccess(string path)
    {
        string level_str = access_dict[path];
        string[] rows = level_str.Split('\n');

        Debug.Log(rows[1]);

        int width = rows[0].Split(' ').Length;
        int height = rows.Length;

        string[,] access;
        access = new string[height,width];
        for(int i = 0; i < height; i++) {
            string[] row = rows[i].Split(' ');
            for(int j = 0; j < width; j++) {
                access[i,j] = row[j];
            }
        }
        
        for(int i = 0; i < access.GetLength(0); i++){
            for(int j = 0; j < access.GetLength(1); j++) {
                if(access[i, j] != "0"){
                    GameObject clone = Instantiate(this.access_obj[access[i, j][0]], new Vector3(i, 0.1f, j), Quaternion.identity);
                    clone.transform.parent = this.transform;
                    clone.name = access[i, j] + "_access";
                }
            }
        }
    }

    public void clearBoard() {
        foreach (Transform child in transform) {
            if (child.gameObject.layer == 3 || child.gameObject.layer == 7) {
                Object.Destroy(child.gameObject);
            }
        }
    }

}
