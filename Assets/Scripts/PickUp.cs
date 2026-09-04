using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject Flashlight;
    public Transform FlashParent;
    public Light flashlightLight;
    public Vector3 equipRotationOffset = Vector3.zero;

    [Header("Sway settings")]
    public float positionFollowSpeed = 8f;   // cat de repede prinde pozitia camerei
    public float rotationFollowSpeed = 6f;   // cat de repede prinde rotatia camerei (mai mic = mai mult lag)

    private bool isEquipped = false;
    private bool canEquip = false;
    private bool isLightOn = false;

    void Start()
    {
        Flashlight.GetComponent<Rigidbody>().isKinematic = true;
        if (flashlightLight != null) flashlightLight.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && isEquipped)
        {
            Drop();
        }

        if (Input.GetKeyDown(KeyCode.E) && canEquip && !isEquipped)
        {
            Equip();
        }

        if (Input.GetKeyDown(KeyCode.F) && isEquipped)
        {
            ToggleLight();
        }

        if (isEquipped)
        {
            FollowCamera();
        }
    }

    void FollowCamera()
    {
        Quaternion targetRotation = FlashParent.rotation * Quaternion.Euler(equipRotationOffset);

        Flashlight.transform.position = Vector3.Lerp(
            Flashlight.transform.position,
            FlashParent.position,
            positionFollowSpeed * Time.deltaTime
        );

        Flashlight.transform.rotation = Quaternion.Slerp(
            Flashlight.transform.rotation,
            targetRotation,
            rotationFollowSpeed * Time.deltaTime
        );
    }

    void ToggleLight()
    {
        isLightOn = !isLightOn;
        flashlightLight.enabled = isLightOn;
    }

    void Drop()
    {
        Flashlight.transform.SetParent(null);
        Flashlight.GetComponent<Rigidbody>().isKinematic = false;
        Flashlight.GetComponent<MeshCollider>().enabled = true;
        isEquipped = false;

        isLightOn = false;
        flashlightLight.enabled = false;
    }

    void Equip()
    {
        Flashlight.GetComponent<Rigidbody>().isKinematic = true;
        Flashlight.GetComponent<MeshCollider>().enabled = false;

        Flashlight.transform.SetParent(FlashParent);
        isEquipped = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") canEquip = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") canEquip = false;
    }
}