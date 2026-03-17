using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform pivot;
    [SerializeField]
    Transform cam;

    [Header("마우스 감도")]
    [Range(100,300)]
    [SerializeField]
    private float mouse_Sensitivity = 150f;

    float x_Rotation = 0f;

    private Vector3 rpg_Offset = new Vector3(0f, 6f, -5f);

    [SerializeField] private Vector3 fps_Position = new Vector3(0f, -0.1f, 0.3f);

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Base_Manager.game_Mng.current_Mode = Camera_Mode.TPS;
    }

    void Update()
    {
        Change_Mode();
    }
    void LateUpdate()
    {
        if (Base_Manager.game_Mng.current_Mode == Camera_Mode.RPG && player != null)
        {
            cam.position = player.position + rpg_Offset;
            cam.rotation = Quaternion.Euler(45f, 0f, 0f);
        }
    }
    private void Change_Mode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            Base_Manager.game_Mng.current_Mode = Camera_Mode.TPS;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            Base_Manager.game_Mng.current_Mode = Camera_Mode.RPG;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Base_Manager.game_Mng.current_Mode == Camera_Mode.TPS && scroll > 0f)
        {
            Base_Manager.game_Mng.current_Mode = Camera_Mode.FPS;
        }

        if (Base_Manager.game_Mng.current_Mode == Camera_Mode.FPS && scroll < 0f)
        {
            Base_Manager.game_Mng.current_Mode = Camera_Mode.TPS;
        }

        switch (Base_Manager.game_Mng.current_Mode)
        {
            case Camera_Mode.TPS:
                Set_TPS_Camera();
                break;
            case Camera_Mode.RPG:
                Set_RPG_Camera();
                break;
            case Camera_Mode.FPS:
                Set_FPS_Camera();
                break;
        }
    }

    #region CAMERA_SET
    private void Set_RPG_Camera()
    {
        pivot.localPosition = rpg_Offset;
        pivot.localRotation = Quaternion.Euler(45f, 0f, 0f);
        cam.SetParent(null, true);
    }

    private void Set_TPS_Camera()
    {
        cam.SetParent(pivot, true);

        float mouse_X = Input.GetAxis("Mouse X") * mouse_Sensitivity * Time.deltaTime;
        float mouse_Y = Input.GetAxis("Mouse Y") * mouse_Sensitivity * Time.deltaTime;

        pivot.localPosition = new Vector3(0f, 1.7f, 0f);
        cam.transform.localPosition = new Vector3(0f, 1f, -4f);
        cam.transform.localRotation = Quaternion.identity;

        x_Rotation -= mouse_Y;
        x_Rotation = Mathf.Clamp(x_Rotation, -40f, 70f);
        pivot.localRotation = Quaternion.Euler(x_Rotation, 0f, 0f);

        player.Rotate(Vector3.up * mouse_X);

    }

    private void Set_FPS_Camera()
    {
        cam.SetParent(pivot, true);

        cam.localPosition = fps_Position;
        cam.localRotation = Quaternion.identity;

        float mouseX = Input.GetAxis("Mouse X") * mouse_Sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouse_Sensitivity * Time.deltaTime;

        x_Rotation -= mouseY;
        x_Rotation = Mathf.Clamp(x_Rotation, -40f, 70f);
        pivot.localRotation = Quaternion.Euler(x_Rotation, 0f, 0f);

        player.Rotate(Vector3.up * mouseX);
    }
    #endregion
}
