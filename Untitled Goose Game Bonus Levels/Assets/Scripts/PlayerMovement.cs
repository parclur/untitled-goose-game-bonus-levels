using Cinemachine;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /*
     * public float movementSpeed;
     * public float runSpeed;
     * public bool isRunning;
     * public bool isSwimming;
     * grab
     * honk
     * crouch
     * flap
     * zoom in
     * zoom out
    */

    //Public Variables
    public Canvas tasklist;
    //bool toggleTaskList;
    // task list with the objectives
    private bool isTaskListOpen = false;

    [Tooltip("Speed player accelerates")]
    public float movementSpeed;
    public float rotationSpeed;
    //[Tooltip("Speed player can go at full force")]
    //public float topSpeed;
    //[Tooltip("Modifier to apply to topSpeed while sprinting")]
    //public float topSpeedModifier = 0.15f;
    //
    //[Tooltip("Is the player running")]
    //public bool isRunning;
    //[Tooltip("Is the player swimming")]
    //public bool isSwimming;
    ////grab
    //[Tooltip("Honk")]
    //public bool honk;
    ////crouch
    ////flap
    //
    [HideInInspector]
    public Rigidbody rb; // Rigidbody reference
    //
    //[Tooltip("Speed the player actually moves (CHANGES)")]
    //public float trueSpeed;
    //
    //// task list with the objectives
    //private bool isTaskListOpen = false;
    //
    //// Tell the code to start decelerating
    //public bool decelerate = false;
    //
    //#region AUDIO
    //private bool canPlayFootstepSound; // If foot step can be played (only 1 can be heard at a time)
    //
    //private bool canPlaySwimmingSound; // If a ladder footstep can be played
    //
    //private int currentGroundType = 0; // Flag for what ground is currently being collided with
    //
    //[Tooltip("Grass footstep sounds")]
    ////public SoundDataClass snowFootsteps;
    //
    //[Tooltip("Dirt footstep sounds")]
    ////public SoundDataClass woodFootsteps;
    //
    //[Tooltip("Wood footstep sounds")]
    ////public SoundDataClass stoneFootsteps;
    //
    //[Tooltip("Rock footstep sounds")]
    ////public SoundDataClass ladderFootsteps;
    //
    //[Tooltip("Swimming sound")]
    ////public SoundDataClass jumpSound;
    //
    //[Tooltip("Honk sound")]
    ////public SoundDataClass jumpSound;
    //
    //#endregion
    //
    ///// <summary>
    ///// 
    ///// Start listening to these events
    ///// 
    ///// </summary>
    //private void OnEnable()
    //{
    //    GameManager.StartListening("ToggleTaskList", ToggleTaskList);
    //}
    //
    ///// <summary>
    ///// 
    ///// Stop listening to these events
    ///// 
    ///// </summary>
    //private void OnDisable()
    //{
    //    GameManager.StopListening("ToggleTaskList", ToggleTaskList);
    //}
    //
    //public void ToggleTaskList(string emptyData)
    //{
    //    isTaskListOpen = !isTaskListOpen;
    //
    //    // Toggle cursor states
    //    if (isTaskListOpen)
    //    {
    //        Cursor.lockState = CursorLockMode.None;
    //        Cursor.visible = true;
    //
    //        // hide the inventory as menu is opened
    //        if (isInventoryOpen)
    //        {
    //            GameManager.TriggerEvent("ToggleTaskList");
    //        }
    //
    //        Time.timeScale = 0;
    //    }
    //    else
    //    {
    //        Cursor.lockState = CursorLockMode.Locked;
    //        Cursor.visible = false;
    //
    //        if ((isRightyControls && Input.GetKey(KeyCode.E) || (!isRightyControls && Input.GetKey(KeyCode.U))))
    //        {
    //            GameManager.TriggerEvent("ToggleTaskList");
    //        }
    //
    //        Time.timeScale = 1;
    //    }
    //
    //}

    /// <summary>
    /// 
    /// Start listening to these events
    /// 
    /// </summary>
    private void OnEnable()
    {
        //GameManager.StartListening("ToggleTaskList", ToggleTaskList);
    }

    /// <summary>
    /// 
    /// Stop listening to these events
    /// 
    /// </summary>
    private void OnDisable()
    {
        //GameManager.StopListening("ToggleTaskList", ToggleTaskList);
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>(); //rb equals the rigidbody on the player
        tasklist.gameObject.SetActive(isTaskListOpen);
    }

    void Update()
    {
        float xMove = Input.GetAxisRaw("Horizontal"); // d key changes value to 1, a key changes value to -1
        float zMove = Input.GetAxisRaw("Vertical"); // w key changes value to 1, s key changes value to -1

        rb.velocity = new Vector3(xMove, rb.velocity.y, zMove) * movementSpeed; // Creates velocity in direction of value equal to keypress (WASD). rb.velocity.y deals with falling + jumping by setting velocity to y. 

        Vector3 movementDirection = new Vector3(xMove, 0, zMove);
        movementDirection.Normalize();

        transform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //GameManager.TriggerEvent("ToggleTaskList");
            ToggleTaskList();
        }

    }

    public void ToggleTaskList()// string emptyData)
    {
        isTaskListOpen = !isTaskListOpen;
        tasklist.gameObject.SetActive(isTaskListOpen);

        // Toggle cursor states
        if (isTaskListOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1;
        }
    }
}

