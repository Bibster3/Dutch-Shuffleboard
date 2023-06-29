using UnityEngine;
using System;

public class AimingDiscScript : MonoBehaviour
{
    public GameObject discPrefab;
    public GameObject discLaunchPosition;
    public static bool isPlacedInitially;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    public Vector3 mousePosition;
    public static MeshRenderer initialDiscMesh;
    public static event EventHandler OnDiscPlaced;
    private void Start()
    {
        Debug.Log(isPlacedInitially + "isPlacedInitially");
        initialDiscMesh = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        FollowMouse();
    }
    public void FollowMouse()
    {
        if ((isPlacedInitially == false))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
            {
                transform.position = raycastHit.point;
            }
            mousePosition = transform.position;
            if ((Input.GetButtonDown("Fire1") && (GameManager.Hits > 0)))
            {
                PlaceDisc();
                initialDiscMesh.enabled = false;
            }
        }
    }
    public void PlaceDisc()
    {
        GameObject shootingDisc = ObjectPool.SharedInstance.GetPooledObject();
        if ((shootingDisc != null)&&(!IsPositionOccupied(discLaunchPosition.transform.position)))
        {
            shootingDisc.transform.position = discLaunchPosition.transform.position;
            shootingDisc.transform.rotation = discLaunchPosition.transform.rotation;
            shootingDisc.SetActive(true);
            isPlacedInitially = true;
        }
        OnDiscPlaced?.Invoke(this, EventArgs.Empty);
    }
    private bool IsPositionOccupied(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, discPrefab.transform.localScale.x / 2f);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Disc"))
            {
                return true; // Position is occupied by a disc
            }
        }
        return false; // Position is not occupied
    }
}