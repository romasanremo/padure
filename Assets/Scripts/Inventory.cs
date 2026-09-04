using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform handParent;
    public int maxSlots = 3;
    public float positionFollowSpeed = 8f;
    public float rotationFollowSpeed = 6f;

    private GameObject[] slots;
    private int currentIndex = -1;
    private bool canPickup = false;
    private GameObject nearbyItem;

    void Start()
    {
        slots = new GameObject[maxSlots];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPickup && nearbyItem != null)
            TryPickUp(nearbyItem);

        if (Input.GetKeyDown(KeyCode.Q))
            SwitchItem();

        if (Input.GetKeyDown(KeyCode.G) && currentIndex != -1)
            DropCurrent();

        if (Input.GetKeyDown(KeyCode.F) && currentIndex != -1)
            ToggleCurrentLight();

        if (currentIndex != -1)
            FollowHand();
    }

    void FollowHand()
    {
        GameObject current = slots[currentIndex];
        Item itemData = current.GetComponent<Item>();
        Quaternion offset = itemData != null ? Quaternion.Euler(itemData.equipRotationOffset) : Quaternion.identity;
        Quaternion targetRotation = handParent.rotation * offset;

        current.transform.position = Vector3.Lerp(
            current.transform.position,
            handParent.position,
            positionFollowSpeed * Time.deltaTime
        );

        current.transform.rotation = Quaternion.Slerp(
            current.transform.rotation,
            targetRotation,
            rotationFollowSpeed * Time.deltaTime
        );
    }

    void TryPickUp(GameObject item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;
                item.GetComponent<Rigidbody>().isKinematic = true;

                MeshCollider mc = item.GetComponent<MeshCollider>();
                if (mc != null) mc.enabled = false;

                currentIndex = i;
                ShowOnly(item);

                canPickup = false;
                nearbyItem = null;
                return;
            }
        }

        Debug.Log("Inventar plin!");
    }

    void ShowOnly(GameObject item)
    {
        foreach (GameObject slot in slots)
            if (slot != null) slot.SetActive(false);

        item.SetActive(true);
        item.transform.SetParent(handParent);
    }

    void SwitchItem()
    {
        int next = currentIndex;
        for (int i = 0; i < maxSlots; i++)
        {
            next = (next + 1) % maxSlots;
            if (slots[next] != null)
            {
                currentIndex = next;
                ShowOnly(slots[currentIndex]);
                return;
            }
        }
    }

  void DropCurrent()
{
    if (slots[currentIndex] == null) return;

    GameObject item = slots[currentIndex];
    item.transform.SetParent(null);

    // muta obiectul putin in fata, ca sa nu se suprapuna cu capsula Player-ului
    item.transform.position = handParent.position + handParent.forward * 0.5f;

   Rigidbody rb = item.GetComponent<Rigidbody>();
if (rb != null)
{
    rb.isKinematic = false; // 1. Treci mai întâi obiectul pe fizică dinamică
    rb.linearVelocity = Vector3.zero; // 2. Acum poți reseta viteza fără eroare
    rb.angularVelocity = Vector3.zero;
}

    MeshCollider mc = item.GetComponent<MeshCollider>();
    if (mc != null) mc.enabled = true;

    Light light = item.GetComponentInChildren<Light>();
    if (light != null) light.enabled = false;

    slots[currentIndex] = null;
    currentIndex = -1;

    for (int i = 0; i < slots.Length; i++)
    {
        if (slots[i] != null)
        {
            currentIndex = i;
            ShowOnly(slots[i]);
            break;
        }
    }
}

    void ToggleCurrentLight()
    {
        Light light = slots[currentIndex].GetComponentInChildren<Light>();
        if (light != null) light.enabled = !light.enabled;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Item>() != null)
        {
            canPickup = true;
            nearbyItem = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == nearbyItem)
        {
            canPickup = false;
            nearbyItem = null;
        }
    }
}