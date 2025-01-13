
using UnityEngine;

public interface ISelectable
{
    //https://www.redblobgames.com/
    //https://www.youtube.com/watch?v=tIfC00BE6z8&list=PLcRSafycjWFf0a6AneqVfuRcXHcwG8tcr

    public void OnRightClick(Vector3 point);

    public void IsClicked();

    public void Deselect();
}
