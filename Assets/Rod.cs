using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rod : MonoBehaviour {

    //ATTRIBUTION: Drag code thanks to GitHub user r3econ https://gist.github.com/r3econ/9712710

    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 previousPoint;
    private const string StrIdCurrnetArrow = "current";
    public GameObject CurrentArrow;
    public BetterToggleGroup FluxDirectionTglGroup;
    public List<GameObject> CurrentArrows = new List<GameObject>();
	// Use this for initialization
	void Start () {
        CurrentArrow = GameObject.Find(StrIdCurrnetArrow);
        CurrentArrow.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        // Get the click location.
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        // Get the offset of the point inside the object.
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        previousPoint = screenPoint;
    }

    void OnMouseDrag()
    {
        // Get the click location.
        Vector3 newScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        if (newScreenPoint.x == previousPoint.x) return;
        var isAreaIncreasing = newScreenPoint.x > previousPoint.x;
        Debug.Log("Area is "+ (isAreaIncreasing ? "increasing" : "decreasing"));
        // Adjust the location by adding an offset.
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(newScreenPoint) + offset;
        if (newPosition.x >= 4.82f)
        {
            newPosition.Set(4.82f, 0, 0);
        }
        if (newPosition.x <= -5f)
        {
            newPosition.Set(-5f, 0, 0);
        }
        newPosition.Set(newPosition.x, 0, 0);
        // Assign new position.
        transform.position = newPosition;
        if (FluxDirectionTglGroup == null)
        {
            FluxDirectionTglGroup = GameObject.Find(Main.StrIdTglGroupFluxDirection).GetComponent<BetterToggleGroup>();
        }
        var tglDirection = FluxDirectionTglGroup.Active();
        if (!FluxDirectionTglGroup.FluxDirectionIsInOrOutOfPage()) {
            return;
        }
        // Debug.Log("Flux is directed " + (FluxDirectionTglGroup.IsFluxDirectedIntoPage() ? 
        //     "into" : "out of") + "the page");
        // if area is increasing
        if (isAreaIncreasing) {
            // if the arrows are pointing down
            if (FluxDirectionTglGroup.IsFluxDirectedIntoPage()) {
                // current is counter clockwise
                GenerateCurrentArrows(false);
            }
            else {
                GenerateCurrentArrows(true);
            }
        }
        else {
            // area is decreasing
            // if the arrows are pointing down
            if (FluxDirectionTglGroup.IsFluxDirectedIntoPage()) {
                // current is counter clockwise
                GenerateCurrentArrows(true);
            }
            else {
                GenerateCurrentArrows(false);
            }
        }
        previousPoint = newScreenPoint;
    }

    void OnMouseUp() {
        DeleteCurrentArrows(2f);
    }

    private void GenerateCurrentArrows(bool isClockwise)
    {
        // Debug.Log("current going " +(isClockwise ? "clockwise" : "counterclockwise"));
        DeleteCurrentArrows();
        CurrentArrows.Clear();
        // there can only be current if flux directed into or out of page
        //if (FluxDirectionTglGroup.FluxDirectionIsInOrOutOfPage())
        //{
        //}
        // counterclockwise
        var firstArrowPosition = new Vector3(5.12f, -.02f, 3.04f);
        var rotationRight = Quaternion.Euler(0, 0, 0);
        var rotationUp = Quaternion.Euler(0, 270, 0); 
        var rotationLeft = Quaternion.Euler(0, 180, 0);
        var rotationDown = Quaternion.Euler(0,90,0);
        var arrowRotation = isClockwise ? 
            rotationLeft :
            rotationRight;
        var arrowWidth = CurrentArrow.GetComponent<Renderer>().bounds.size.x;
        var rodHeight = GetComponent<Renderer>().bounds.size.z;
        var distanceFromFirstToRod = Vector3.Distance(new Vector3(transform.position.x,0,0), new Vector3(firstArrowPosition.x,0,0));
        var arrowsToMakeWidthWise = distanceFromFirstToRod/arrowWidth;
        var position = firstArrowPosition;
        for (var i = 0; i < arrowsToMakeWidthWise; i++)
        {
            var arrow = (GameObject)Instantiate(CurrentArrow, position,arrowRotation);
            arrow.SetActive(true);
            position.Set(position.x-arrowWidth,position.y,position.z);
            CurrentArrows.Add(arrow);
        }
        // made it to bar.
        // go up the rod
        arrowRotation = isClockwise ?
            rotationDown : rotationUp;
        var arrowsToMakeHeightWise = rodHeight/arrowWidth;
        position.Set(transform.position.x,position.y,position.z-arrowWidth);
        for (var i = 0; i < arrowsToMakeHeightWise-1; i++)
        {
            var arrow = (GameObject)Instantiate(CurrentArrow, position,arrowRotation);
            arrow.SetActive(true);
            position.Set(transform.position.x,position.y,position.z-arrowWidth);
            CurrentArrows.Add(arrow);
        }
        // top of circuit
        arrowRotation = isClockwise ? rotationRight : rotationLeft;
        position.Set(position.x,position.y,position.z+arrowWidth/4);
        if (isClockwise) {
            position.Set(position.x,position.y,position.z-arrowWidth/4);
        }
        for (var i = 0; i < arrowsToMakeWidthWise; i++)
        {
            var arrow = (GameObject) Instantiate(CurrentArrow, position, arrowRotation);
            arrow.SetActive(true);
            position.Set(position.x+arrowWidth,position.y,position.z);
            CurrentArrows.Add(arrow);
        }
        // left part of circuit
        arrowRotation = isClockwise ? rotationUp : rotationDown;
        position.Set(firstArrowPosition.x,position.y,position.z+arrowWidth);
        for (var i = 0; i < arrowsToMakeHeightWise - 1; i++)
        {
            var arrow = (GameObject)Instantiate(CurrentArrow, position, arrowRotation);
            arrow.SetActive(true);
            position.Set(firstArrowPosition.x, position.y, position.z + arrowWidth);
            CurrentArrows.Add(arrow);
        }
    }
    private void DeleteCurrentArrows(float seconds = 0)
    {
        foreach (GameObject arrow in CurrentArrows)
        {
            Destroy(arrow,seconds);
        }
    }
}
