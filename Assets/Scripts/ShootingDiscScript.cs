using System.Collections;
using UnityEngine;
using System;
using Constants;
public class ShootingDiscScript : MonoBehaviour
{
    private float currentDistance; //the distance from the mouse and ball
    private float goodSpace; //the right amount of space between 0 - max distance
    private float shootPower; //the power when release mouse click
    private Vector3 shootDirection; //derection to shoot
    private LineRenderer line; //use to generate line
    private RaycastHit hitInfo; //for raycasting. enable mouse position to be 3D and not 2D
    private Vector3 currentMousePosition; //the current mouse position
    private Vector3 temp;
    public LayerMask groundLayers;
    public float maxPullDistance = 3f; //("Max pull distance")]
    public float power;
    public float speed = 1f;
    public bool isShot;
    Renderer[] pointIndicators;
    [SerializeField] private static int countOfOnes;
    [SerializeField] private static int countOfTwos;
    [SerializeField] private static int countOfThrees;
    [SerializeField] private static int countOfFours;
    public delegate void Scored(int scoreAmount);
    public static event Scored OnScored;
    public static event EventHandler OnAddBonusPoints;
    public static event EventHandler OnShooting;
    public bool hasScored = false;
    private Rigidbody rb;

    private void Awake()
    {
        line = GetComponent<LineRenderer>(); //set line renderer
        line.enabled = false;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        isShot = false;
        GameManager.DiscResetForAnotherTurn += ShootingDiscScript_OnDiscResetForAnotherTurn;
        GameManager.SwitchToNextPlayer += ShootingDiscScript_OnSwitchToSecondPlayer;
    }

   
    private void ShootingDiscScript_OnSwitchToSecondPlayer(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
        countOfOnes = 0;
        countOfTwos = 0;
        countOfThrees = 0;
        countOfFours = 0;
        hasScored = false; 
        isShot = false; 
        ShowAimingDisc();
    }
    private void ShowAimingDisc()
    {
        AimingDiscScript.isPlacedInitially = false;
        AimingDiscScript.initialDiscMesh.enabled = true;
    }
    private void ShootingDiscScript_OnDiscResetForAnotherTurn(object sender, EventArgs e)
    {
        if ((hasScored == false) && (isShot == true))
        {
            gameObject.SetActive(false);
            isShot = false;
        }
    }
    private void OnMouseDown()
    {
        if (isShot)
            return;
        line.enabled = true; //enable toe first point of the line
        line.SetPosition(0, transform.position);  //the line begins at this target position
    }
    private void OnMouseDrag()
    {
        currentDistance = Vector3.Distance(currentMousePosition * speed, transform.position);         //update the current distcance
        if (currentDistance <= maxPullDistance)
        {
            temp = currentMousePosition * speed; //saving the current possion while dragin is  allowed
            goodSpace = currentDistance;
        }
        else
        {
            temp = new Vector3(currentMousePosition.x * speed, currentMousePosition.y * speed, temp.z); // dont go any further
            goodSpace = maxPullDistance;
        }

        shootPower = Mathf.Abs(goodSpace) * power;         //assign the shoot power and times it by your desired power
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);         //get mouse position over the floor - when we drag the mouse position will be allow the x y and Z in 3D :) Yay!

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundLayers))
        {
            currentMousePosition = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
        }
        shootDirection = Vector3.Normalize(currentMousePosition - transform.position);         //calculate the shoot Direction
        line.SetPosition(1, temp);         ///update the line while we drag
    }
    private void OnMouseUp()
    {
        if (isShot)
            return;
        Vector3 push = shootDirection * shootPower * -1; //force in the correct direction
        rb.AddForce(push, ForceMode.Impulse);
        OnShooting?.Invoke(this, EventArgs.Empty);
        line.enabled = false; //remove the line
        isShot = true;
        if (GameManager.Hits == AppConstants.LastHitOfATurn)
        {
            StartCoroutine(DelayedExecution());
        }
        else if (GameManager.Hits > 0)
        {
            GameManager.Hits--;
            ShowAimingDisc();
        }
    }
    private IEnumerator DelayedExecution()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Hits--;
        ShowAimingDisc();
    }
    private void OnTriggerEnter(Collider other)
    {
        pointIndicators = other.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in pointIndicators)
        {
            StartCoroutine(SwitchColor(rend));
        }
        hasScored = true;
        GameManager.discsToReshoot--;
        int.TryParse(other.gameObject.tag, out int scoreToAdd);
        GameManager.Score = GameManager.Score + scoreToAdd;
        if (OnScored != null)
        {
            Debug.Log("OnScored " + scoreToAdd);
            OnScored(scoreToAdd);
        }
        switch (scoreToAdd)
        {
            case 1:
                countOfOnes++;
                break;
            case 2:
                countOfTwos++;
                break;
            case 3:
                countOfThrees++;
                break;
            case 4:
                countOfFours++;
                break;
        }
        if (Array.TrueForAll<int>(new int[] { countOfOnes, countOfTwos, countOfThrees, countOfFours },
       val => (countOfOnes == val) && (val != 0)))
        {
            OnAddBonusPoints?.Invoke(this, EventArgs.Empty);
        }
    }
    IEnumerator SwitchColor(Renderer renderer)
    {
        renderer.material.color = Color.green;
        yield return new WaitForSeconds(0.3f);
        renderer.material.color = Color.white;
    }
    private void OnDestroy()
    {
        GameManager.DiscResetForAnotherTurn -= ShootingDiscScript_OnDiscResetForAnotherTurn;
        GameManager.SwitchToNextPlayer -= ShootingDiscScript_OnSwitchToSecondPlayer;
    }
}