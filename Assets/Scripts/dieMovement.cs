using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieMovement : MonoBehaviour
{

    Transform p_mx, p_x, die, die2;

    public float rotation_speed;
    public GameObject gm_object;
    public GameObject board_object;
    [Space(10)]

    private Transform transformPivot;
    private Vector3 initial_position;
    private Quaternion initial_rotation;

    private bool is_rotating = false;
    private bool is_sliding = false;
    private bool is_turning = false;

    private float fractionOfJourney;
    private Vector3 rot_dir, mov_dir;

    private GameManager gm;

    private boardBuilder board;
    private bool haskey;

    private static int soundIteration = 0;
    AudioSource[] movementSource;

    // Start is called before the first frame update
    void Awake()
    {
        this.gm = gm_object.GetComponent<GameManager>();
        this.board = board_object.GetComponent<boardBuilder>();
        this.haskey = false;

        this.p_x = this.transform.Find("pivot_X");
        this.p_mx = this.transform.Find("pivot_mX");
        this.die = this.transform.Find("Die");

        die2 = Instantiate(die, die.transform.position, Quaternion.identity).transform;
        die2.name = "Die";
        die.transform.parent = this.p_x;
        die2.transform.parent = this.p_mx;

        movementSource = this.GetComponents<AudioSource>();
    }

    void prep_rotation(Vector3 rot_dir, Transform transformPivot, Vector3 mov_dir) {
        this.rot_dir  = rot_dir;
        this.transformPivot = transformPivot;
        this.mov_dir = mov_dir;

        this.is_rotating = true;
    }

    void apply_tile(Vector3 origin)
    {
        GameObject go;
        Debug.DrawRay(origin, Vector3.down, Color.red, 1f, false);
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Board")))
        {
            int ignored;
            if (int.TryParse(hit.collider.gameObject.name, out ignored) && hit.collider.gameObject.name[0] != '0')
            {
                return;
            } else
            {
                switch ((char) hit.collider.gameObject.name[0])
                {
                    case 'X':      
                        break;
                    
                    case 'F':
                        gm.NextBoard();
                        break;
                    
                    case 'H':
                        break;
                    
                    case 'I':
                        if (can_move(this.transform.position + this.mov_dir))
                        {
                            this.initial_position = this.transform.position;
                            is_sliding = true;
                        }
                        break;

                    case 'C':
                        go = Instantiate(board.regular_tile, hit.collider.gameObject.transform.position, Quaternion.identity);
                        go.transform.parent = board.transform;
                        go.name = "0";
                        Destroy(hit.collider.gameObject);
                        Destroy(get_access(origin));
                        if (can_move(this.transform.position + this.mov_dir))
                        {
                            this.initial_position = this.transform.position;
                            is_sliding = true;
                        }
                        break;

                    case 'K':
                        go = Instantiate(board.regular_tile, hit.collider.gameObject.transform.position, Quaternion.identity);
                        go.transform.parent = board.transform;
                        go.name = "0";
                        Destroy(hit.collider.gameObject);
                        Destroy(get_access(origin));
                        this.haskey = true;
                        break;
                        
                    case 'L':
                        break;

                    case 'R':
                        this.initial_rotation = die.transform.rotation;
                        hit.collider.gameObject.transform.Rotate(0, 180, 0);
                        is_turning = true;
                        break;

                    default:
                        break;
                }
            }
        }
    }

    GameObject get_access(Vector3 origin)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Access")))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    bool can_move(Vector3 origin)
    {
        GameObject go;
        RaycastHit hit;
        Debug.DrawRay(origin, Vector3.down, Color.blue, 1f, false);
        if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Board")))
        {
            int ignored;
            if (int.TryParse(hit.collider.gameObject.name, out ignored) && hit.collider.gameObject.name[0] != '0')
            {
                Debug.Log("Finish - " + hit.collider.gameObject.name[0]);
            } else
            {
                switch ((char) hit.collider.gameObject.name[0])
                {
                    case 'H':      
                        return false;

                    case 'X':      
                        return false;

                    case 'L':
                        if(haskey){
                            movementSource[7].Play();
                            go = Instantiate(board.regular_tile, hit.collider.gameObject.transform.position, Quaternion.identity);
                            go.transform.parent = board.transform;
                            go.name = "0";
                            Destroy(hit.collider.gameObject);
                            Destroy(get_access(origin));
                            this.haskey = false;
                            return true;
                        } else {
                            return false;
                        }

                    default:
                        return hasaccess(origin);
                }
            }
        }
        return false;
    }

    bool hasaccess(Vector3 origin)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Access")))
        {
            int ignored;
            if (int.TryParse(hit.collider.gameObject.name, out ignored) && hit.collider.gameObject.name[0] != '0')
            {
                return true;
            } else
            {
                float[] distances = new float[6];

                char access_char = (char) hit.collider.gameObject.name[0];
                int access_int;
                int.TryParse(""+access_char, out access_int);

                //for every child of die, find the closest to origin
                foreach (Transform child in die.transform)
                {
                    //array of int of size 6, each int is the distance to the child of die
                    int index;
                    int.TryParse(""+child.name[0], out index);
                    distances[index-1] = Vector3.Distance(child.position, origin);
                }

                //find the index of the lowest value in the array
                int min_index = 0;
                for (int i = 0; i < distances.Length; i++)
                {
                    if (distances[i] < distances[min_index])
                    {
                        min_index = i;
                    }
                }
                
                return (min_index+1 == access_int);
            }
        }

        return true;
    }

    char tile_type(Vector3 origin)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Board")))
        {
            Debug.DrawRay(origin, Vector3.down * hit.distance, Color.yellow, 3);

            return (char)hit.collider.gameObject.name[0];
        }

        return 'O';
    }

    // Update is called once per frame
    void Update()
    {
        if(is_turning)
        {
            fractionOfJourney = fractionOfJourney + (Time.deltaTime / rotation_speed);
            Vector3 rotation = Vector3.Lerp(Vector3.zero, new Vector3(0, 180f, 0), fractionOfJourney);

            Vector3 r_new = initial_rotation.eulerAngles + rotation;

            die.transform.rotation = Quaternion.Euler(r_new);
            die2.transform.rotation = Quaternion.Euler(r_new);

            if (fractionOfJourney >= 1)
            {
                movementSource[soundIteration % 7].Play();
                soundIteration++;
                fractionOfJourney = 0;
                is_turning = false;
            }

            return;
        }

        if(is_rotating)
        {
            fractionOfJourney = fractionOfJourney + (Time.deltaTime / rotation_speed);

            Vector3 rotation = Vector3.Lerp(Vector3.zero, new Vector3(rot_dir.x, rot_dir.y, rot_dir.z), fractionOfJourney);

            transformPivot.transform.rotation = Quaternion.Euler(rotation.x,rotation.y, rotation.z);
            
            if(fractionOfJourney >= 1) {
                movementSource[soundIteration % 7].Play();
                soundIteration++;
                fractionOfJourney = 0;
                is_rotating = false;

                die.transform.parent = null;
                die2.transform.parent = null;

                transformPivot.rotation = Quaternion.identity;
                this.transform.localPosition += this.mov_dir;
                
                die.transform.parent = this.transform;
                die2.transform.parent = this.transform;
                die.transform.localPosition = Vector3.zero;
                die2.transform.localPosition = Vector3.zero;
                
                die.transform.parent = this.p_x;
                die2.transform.parent = this.p_mx;
                
                if(transformPivot == this.p_x)
                {
                    die2.transform.rotation = die.transform.rotation;
                } else {
                    die.transform.rotation = die2.transform.rotation;
                }

                apply_tile(this.transform.position);
            }

            return;
        }

        if (is_sliding)
        {
            Vector3 end = this.initial_position + this.mov_dir;

            fractionOfJourney = fractionOfJourney + (Time.deltaTime / rotation_speed);

            Vector3 translation = Vector3.Lerp(this.initial_position, this.initial_position + this.mov_dir, fractionOfJourney);

            this.transform.position = translation;

            if (fractionOfJourney >= 1)
            {
                fractionOfJourney = 0;
                is_sliding = false;

                apply_tile(this.transform.position);
            }

            return;
        }

        if (!gm.canPlay())
        {
            return;
        }

        if (Input.GetKey("up"))
        {
            if (can_move(this.transform.position + new Vector3(0, 0, 1)))
            {
                GameManager.score += 1;
                this.die2.gameObject.SetActive(false);
                this.die.gameObject.SetActive(true);
                prep_rotation(new Vector3(90.0f, 0.0f, 0.0f), this.p_x, new Vector3(0, 0, 1));
            }
            
        } else if (Input.GetKey("down"))
        {
            if (can_move(this.transform.position + new Vector3(0, 0, -1)))
            {
                GameManager.score += 1;
                this.die2.gameObject.SetActive(true);
                this.die.gameObject.SetActive(false);
                prep_rotation(new Vector3(-90.0f, 0.0f, 0.0f), this.p_mx, new Vector3(0, 0, -1));
            }
        } else if (Input.GetKey("right"))
        {
            if (can_move(this.transform.position + new Vector3(1, 0, 0)))
            {
                GameManager.score += 1;
                this.die2.gameObject.SetActive(false);
                this.die.gameObject.SetActive(true);
                prep_rotation(new Vector3(0.0f, 0.0f, -90.0f), this.p_x, new Vector3(1, 0, 0));
            }
        } else if (Input.GetKey("left"))
        {
            if (can_move(this.transform.position + new Vector3(-1, 0, 0)))
            {
                GameManager.score += 1;
                this.die2.gameObject.SetActive(true);
                this.die.gameObject.SetActive(false);
                prep_rotation(new Vector3(0.0f, 0.0f, 90.0f), this.p_mx, new Vector3(-1, 0, 0));
            }
        }
    }
}
