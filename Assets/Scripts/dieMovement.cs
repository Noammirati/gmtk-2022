using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieMovement : MonoBehaviour
{

    Transform p_mx, p_x, die, die2;

    public float rotation_speed;
    
    private Transform transformPivot;
    private Vector3 initial_position;
    private bool is_rotating = false;
    private bool is_sliding = false;
    private int time_start_rotating;
    private float fractionOfJourney;
    private Vector3 rot_dir, mov_dir;


    // Start is called before the first frame update
    void Awake()
    {
        this.p_x = this.transform.Find("pivot_X");
        this.p_mx = this.transform.Find("pivot_mX");
        this.die = this.transform.Find("Die");

        die2 = Instantiate(die, die.transform.position, Quaternion.identity).transform;
        die.transform.parent = this.p_x;
        die2.transform.parent = this.p_mx;
    }

    void prep_rotation(Vector3 rot_dir, Transform transformPivot, Vector3 mov_dir) {
        this.rot_dir  = rot_dir;
        this.transformPivot = transformPivot;
        this.mov_dir = mov_dir;

        this.is_rotating = true;
    }

    void slide(Vector3 direction)
    {
        this.mov_dir = direction;

        this.is_sliding = true;
    }

    bool check_tile(Vector3 origin)
    {
        RaycastHit hit;
        Debug.Log(origin);
        if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Board")))
        {
            Debug.DrawRay(origin, Vector3.down * hit.distance, Color.yellow, 3);
            Debug.Log("Did Hit " + hit.collider.gameObject.name);

            switch ((char) hit.collider.gameObject.name[0])
            {
                case 'X':      
                    Debug.Log("Can't move there");
                    return false;
                default:
                    return true;
            }

        }
        Debug.Log("Can't move there");
        return false;
    }

    char tile_type(Vector3 origin)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Board")))
        {
            Debug.DrawRay(origin, Vector3.down * hit.distance, Color.yellow, 3);
            Debug.Log("Did Hit " + hit.collider.gameObject.name);

            return (char)hit.collider.gameObject.name[0];
        }

        return 'O';
    }

    // Update is called once per frame
    void Update()
    {
        if(is_rotating)
        {
            fractionOfJourney = fractionOfJourney + (Time.deltaTime / rotation_speed);

            Vector3 rotation = Vector3.Lerp(Vector3.zero, new Vector3(rot_dir.x, rot_dir.y, rot_dir.z), fractionOfJourney);

            transformPivot.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
            
            //Debug.Log(transformPivot.transform.rotation);

            if(fractionOfJourney >= 1) {
                fractionOfJourney = 0;
                is_rotating = false;

                this.transform.Translate(this.mov_dir);
                transformPivot.rotation = Quaternion.identity;

                this.initial_position = this.transform.position;

                if (check_tile(this.transform.position + this.mov_dir))
                {
                    if (tile_type(this.transform.position) == 'I') {
                        is_sliding = true;
                    }
                }
            }

            return;
        }

        if (is_sliding)
        {
            Vector3 end = this.initial_position + this.mov_dir;
            Debug.Log("Départ " + this.initial_position);
            Debug.Log("Arrivée " + end);
            fractionOfJourney = fractionOfJourney + (Time.deltaTime / rotation_speed);

            Vector3 translation = Vector3.Lerp(this.initial_position, this.initial_position + this.mov_dir, fractionOfJourney);

            transformPivot.transform.position = translation;

            if (fractionOfJourney >= 1)
            {
                fractionOfJourney = 0;
                is_sliding = false;

                this.initial_position = this.transform.position;
            }

            return;
        }

        if (Input.GetKey("up"))
        {
            if (check_tile(this.transform.position + new Vector3(0, 0, 1)))
            {
                this.die2.gameObject.SetActive(false);
                this.die.gameObject.SetActive(true);
                prep_rotation(new Vector3(90.0f, 0.0f, 0.0f), this.p_x, new Vector3(0, 0, 1));
            }
            
        } else if (Input.GetKey("down"))
        {  
            if (check_tile(this.transform.position + new Vector3(0, 0, -1)))
            {
                this.die2.gameObject.SetActive(true);
                this.die.gameObject.SetActive(false);
                prep_rotation(new Vector3(-90.0f, 0.0f, 0.0f), this.p_mx, new Vector3(0, 0, -1));
            }
        } else if (Input.GetKey("right"))
        {
            if (check_tile(this.transform.position + new Vector3(1, 0, 0)))
            {
                this.die2.gameObject.SetActive(false);
                this.die.gameObject.SetActive(true);
                prep_rotation(new Vector3(0.0f, 0.0f, -90.0f), this.p_x, new Vector3(1, 0, 0));
            }
        } else if (Input.GetKey("left"))
        {
            
            if (check_tile(this.transform.position + new Vector3(-1, 0, 0)))
            {
                this.die2.gameObject.SetActive(true);
                this.die.gameObject.SetActive(false);
                prep_rotation(new Vector3(0.0f, 0.0f, 90.0f), this.p_mx, new Vector3(-1, 0, 0));
            }
        }

        
    }
}
