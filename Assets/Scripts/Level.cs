using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public Camera cam;
    public GameObject ballPrefab, anvilPrefab;
    public GameObject bman;
    public GameObject decorations;
    public Transform weapons; // To hold weapons before despawning
    public Material boxMaterial;
    public int numBoxes = 500, xBoxLength = 1000, zBoxLength = 40;
    public float ballSpeed = 20f, minBoxSize = 1, maxBoxSize = 5, boxBicycleDistance = 6, zScaling = 10, ballAdjust = 5;
    public float anvilHeight = 30;
    public GameObject pauseScreen;

    public static bool frozen = false;
    public static bool paused = false;

    bool pauseCalled = false;
    Bicycle b;
    Vector3 prevCam, camVector;
    int weaponSelected = 1; // 1=balls, 2=anvils, etc.
    KeyCode[] keyCodes = new KeyCode[] {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };
    

    private void Start()
    {
        generateDecorations(true);
        generateDecorations(false);

        b = FindObjectOfType<Bicycle>();

        prevCam = cam.transform.position;

        InvokeRepeating("despawnCheck", 1f, 1f);

    }


    
   
    void Update()
    {
        // Reload game
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            paused = false;
            frozen = false;
            Time.timeScale = 1;
        }

        // Check paused, get value on previous late update so this is early
        if (pauseCalled)
        {
            if (paused)
                PauseGame();
            else
                ResumeGame();
            pauseCalled = false;
        }
        


        if (!paused) // Disable all controls when paused
        {
            // Select weapon
            for (int i = 0; i < keyCodes.Length; ++i)
                if (Input.GetKeyDown(keyCodes[i]))
                    weaponSelected = i + 1;

            // Use weapon
            if (Input.GetMouseButtonDown(0))
            {
                switch (weaponSelected)
                {
                    case 1:
                        throwBall();
                        break;
                    case 2:
                        dropAnvil();
                        break;
                    default:
                        throwBall();
                        break;
                }
            }

            // Freeze Mode
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (frozen)
                    Time.timeScale = 1;
                else
                    Time.timeScale = 0;
                frozen = !frozen;
            }
        }

        

    }

    // For early pause
    private void LateUpdate()
    {
        // Check for pause here, perform pause at beginning of next frame
        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;
            pauseCalled = true;
        }
            
    }

    private void FixedUpdate()
    {
        // For ball adjust
        camVector = (cam.transform.position - prevCam);
        prevCam = cam.transform.position;
    }

    void despawnCheck()
    {
        foreach(Transform i in weapons.Find("Anvils")){
            if(i.position.x < bman.transform.position.x - 50)
            {
                Destroy(i.gameObject);
            }
        }
    }

    void throwBall() // weapon 1
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);

        Vector3 dir = r.GetPoint(5) - r.GetPoint(0);

        GameObject bullet = Instantiate(ballPrefab, r.GetPoint(2), Quaternion.LookRotation(dir));

        Vector3 newCamVector = Vector3.zero;
        // Adjust for camera movement only if not turning camera
        if (!(Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W)))
            newCamVector = new Vector3(camVector.x, 0, camVector.z) * ballAdjust;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * ballSpeed + newCamVector;
        bullet.transform.SetParent(weapons.Find("Balls"));
        Destroy(bullet, 3);
    }

    void dropAnvil() // weapon 2
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 spawnPos;
        if (Physics.Raycast(r, out hit, 100))
            spawnPos = new Vector3(hit.point.x, anvilHeight, hit.point.z);
        else
            spawnPos = new Vector3(bman.transform.position.x, anvilHeight, bman.transform.position.z);
        GameObject anvil = Instantiate(anvilPrefab, spawnPos, Quaternion.identity);
        anvil.transform.SetParent(weapons.Find("Anvils"));
    }

    void generateDecorations(bool behindBicycle)
    {
        var zSide = 1;
        if (behindBicycle)
            zSide = -1;

        for(var i=0; i<numBoxes; i++)
        {
            var boxZ = zSide * (Random.value * (zBoxLength - boxBicycleDistance) + boxBicycleDistance);
            var boxScale = Mathf.Max(1, Mathf.Abs(boxZ / zScaling)) * Random.value * (maxBoxSize - 1) + minBoxSize;
            boxZ -= boxScale / 2;
            Vector3 boxPos = new Vector3(Random.value * xBoxLength - (xBoxLength/2), boxScale / 2, boxZ);
            var yRotation = Random.value * 90f;

            GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.transform.position = boxPos;
            box.transform.eulerAngles = new Vector3(0, yRotation, 0);
            box.transform.localScale = Vector3.one * boxScale;
            box.transform.parent = decorations.transform;
            box.GetComponent<Renderer>().material = boxMaterial;
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    void ResumeGame()
    {
        if(!frozen)
            Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }
}
