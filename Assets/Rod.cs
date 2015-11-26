using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rod : MonoBehaviour {

    //ATTRIBUTION: Drag code thanks to GitHub user r3econ https://gist.github.com/r3econ/9712710

    private Vector3 screenPoint;
    private Vector3 offset;
    private const string StrIdCurrnetArrow = "current";
    public GameObject CurrentArrow;
    public BetterToggleGroup FluxDirectionTglGroup;
    public List<GameObject> CurrentArrows = new List<GameObject>();
	// Use this for initialization
	void Start () {
        CurrentArrow = GameObject.Find(StrIdCurrnetArrow);
        CurrentArrow.SetActive(false);
        GenerateCurrentArrows();
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
    }

    void OnMouseDrag()
    {
        // Get the click location.
        Vector3 newScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

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
        GenerateCurrentArrows();
    }

    private void GenerateCurrentArrows()
    {
        if (FluxDirectionTglGroup == null)
        {
            FluxDirectionTglGroup = GameObject.Find(Main.StrIdTglGroupFluxDirection).GetComponent<BetterToggleGroup>();
        }
        var tglDirection = FluxDirectionTglGroup.Active();
        DeleteCurrentArrows();
        CurrentArrows.Clear();
        // there can only be current if flux directed into or out of page
        //if (FluxDirectionTglGroup.FluxDirectionIsInOrOutOfPage())
        //{
        //}
        // counterclockwise
        var firstArrowPosition = new Vector3(5.12f, -.02f, 3.04f);
        var arrowRotation = Quaternion.Euler(0, 0, 0);
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
        arrowRotation = Quaternion.Euler(0, 270, 0);
        var arrowsToMakeHeightWise = rodHeight/arrowWidth;
        position.Set(transform.position.x,position.y,position.z-arrowWidth);
        for (var i = 0; i < arrowsToMakeHeightWise-1; i++)
        {
            var arrow = (GameObject)Instantiate(CurrentArrow, position,arrowRotation);
            arrow.SetActive(true);
            position.Set(transform.position.x,position.y,position.z-arrowWidth);
            CurrentArrows.Add(arrow);
        }
        // go back left
        arrowRotation = Quaternion.Euler(0, 180, 0);
        position.Set(position.x,position.y,position.z+arrowWidth/4);
        for (var i = 0; i < arrowsToMakeWidthWise; i++)
        {
            var arrow = (GameObject) Instantiate(CurrentArrow, position, arrowRotation);
            arrow.SetActive(true);
            position.Set(position.x+arrowWidth,position.y,position.z);
            CurrentArrows.Add(arrow);
        }
        // go down
        arrowRotation = Quaternion.Euler(0,90,0);
        position.Set(firstArrowPosition.x,position.y,position.z+arrowWidth);
        for (var i = 0; i < arrowsToMakeHeightWise - 1; i++)
        {
            var arrow = (GameObject)Instantiate(CurrentArrow, position, arrowRotation);
            arrow.SetActive(true);
            position.Set(firstArrowPosition.x, position.y, position.z + arrowWidth);
            CurrentArrows.Add(arrow);
        }
    }
    private void DeleteCurrentArrows()
    {
        foreach (GameObject arrow in CurrentArrows)
        {
            Destroy(arrow);
        }
    }
}
