using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class playerscript : MonoBehaviour
{
    public bool Paused;
    public GameObject pausemenu;
    public GameObject pressetointeract;
    public bool dialoguing;
    Dialoguesystem ds;

    public bool horror;
    public GameObject sun;
    public GameObject flashlight;
    public float battery = 1.5f;
    public GameObject batterybar;
    float lastnoise;

    public float Speed;
    public Rigidbody rb;
    new public GameObject camera;
    private float horizontalSpeed = 2.0F;
    private float verticalSpeed = 2.0F;
    float yaw;
    float pitch;

    //inventorty
    bool inventoryopen;
    public GameObject inventory;
    public int[] items;
    public int selecteditem;
    public itemids itemids;
    public Image itemimage;
    public TMP_Text itemnametextthingy;
    public TMP_Text itemdescriptiontextthingy;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Movement
        float movv = Input.GetAxisRaw("Vertical");
        float movh = Input.GetAxisRaw("Horizontal");
        transform.position += transform.forward * Time.deltaTime * Speed * movv;
        transform.position += transform.right * Time.deltaTime * Speed * movh;

        float h = horizontalSpeed * Input.GetAxis("Mouse X");
        float v = verticalSpeed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        if (Cursor.lockState == CursorLockMode.Locked && !Paused && !dialoguing)
        {
            yaw += horizontalSpeed * h;
            pitch -= verticalSpeed * v;
            transform.transform.eulerAngles = new Vector3(0, yaw, 0);
            camera.transform.eulerAngles = new Vector3(pitch, transform.eulerAngles.y, transform.eulerAngles.z);

        }

        // Noise shaenanigans
        if(movh > 0 || movv > 0)
        {
            MakeNoise(5);
        }
        lastnoise--;
        if(lastnoise <= 0)
        {
            lastnoise = 0;
        }

        // Flashlight
        if(!Paused)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                flashlight.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                flashlight.SetActive(false);
            }
            if (Input.GetKey(KeyCode.R) && battery < 1.50)
            {
                battery = battery + 0.0004f;
                Speed = 2;
                MakeNoise(10);
            }
            if (Input.GetKeyUp(KeyCode.R) || battery == 1.50)
            {
                Speed = 5;
                MakeNoise(30);
            }

            if (battery > 1.5f)
            {
                battery = 1.5f;
            }

            if (flashlight.active == true && battery > 0)
            {
                battery = battery - 0.0005f;
            }

            if (battery <= 0)
            {
                battery = 0;
                flashlight.SetActive(false);
            }

            batterybar.transform.localScale = new Vector3(battery, batterybar.transform.localScale.y, batterybar.transform.localScale.z);
        }
        // Horror toggle
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (horror == false)
            {
                horror = true;
            }
            else
            {
                horror = false;
            }
        }
        if (horror==true)
        {
            sun.SetActive(false);
        }
        else if(horror==false)
        {
            sun.SetActive(true);
        }

        // Pause
        if (Input.GetKeyDown(KeyCode.Escape) && Paused == false)
        {
            Pause(true, true,true);
            Paused = true;

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Paused == true)
        {
            Pause(false, true,true);
            Paused = false;
        }

        // Interacting
        RaycastHit hit;
        LayerMask interactable = LayerMask.GetMask("interactable");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit,2.5f,interactable))
        {
            ds = hit.transform.gameObject.GetComponent<Dialoguesystem>();
            Debug.Log("you can interact with something probably");
            pressetointeract.SetActive(true);
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(dialoguing == false)
                {
                    Debug.Log("you interacted :3");
                    ds.dialoguable = true;
                    ds.Begin();
                    batterybar.SetActive(false);
                    Pause(true, false,true);
                }
                dialoguing = true;
            } 
        }
        else
        {
            pressetointeract.SetActive(false);
        }

        // Inventory system
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(inventoryopen)
            {
                inventoryopen = false;
            }
            else
            {
                inventoryopen = true;
            }    
        }

        if(inventoryopen)
        {
            inventory.SetActive(true);
            batterybar.SetActive(false);
            Pause(true, false, true);
            itemimage.sprite = itemids.itemsprite[items[selecteditem]];
            itemnametextthingy.text = itemids.itemname[items[selecteditem]];
            itemdescriptiontextthingy.text = itemids.itemdescription[items[selecteditem]];
        }
        else
        {
            inventory.SetActive(false);
            batterybar.SetActive(true);
            Pause(false, false, true);
        }
    }

    public void MakeNoise(float noise)
    {
    
        if(noise >= lastnoise)
        {
            lastnoise = noise;
        }
    }

    public void Resume()
    {
        Paused = false;
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScreen");
    }

    public void Pause(bool paused, bool pausemenud,bool lockc)
    {
        if(paused)
        {
            Time.timeScale = 0;
            if(lockc)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            if(pausemenud)
            {
                pausemenu.SetActive(true);
            }
        }
        else
        {
            Time.timeScale = 1;
            if(lockc)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            if(pausemenud)
            {
                pausemenu.SetActive(false);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, lastnoise);
    }

    public void switchitem(bool left)
    {
        if (left)
        {
            selecteditem = selecteditem - 1;
        }
        else
        {
            selecteditem = selecteditem + 1;
        }
        if (selecteditem >= items.Length)
        {
            selecteditem = 0;
        }
        else if (selecteditem < 0)
        {
            selecteditem = items.Length-1;
        }
    }
}
