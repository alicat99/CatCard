using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelector : MonoBehaviour
{
    public ISelectable selection { get; private set; }

    public void Select(ISelectable selection)
    {
        if (this.selection != null)
        {
            this.selection.OnDeselect();

            selection.OnSelect();
            this.selection = selection;

            OnSelectionMove();
        }
        else
        {
            selection.OnSelect();
            this.selection = selection;

            OnSelectionCreate();
        }
    }

    public void Deselect()
    {
        if (selection != null)
            selection.OnDeselect();
        selection = null;
        OnSelectionEnd();
    }

    protected virtual void OnSelectionCreate()
    {

    }

    protected virtual void OnSelectionMove()
    {

    }

    protected virtual void OnSelectionEnd()
    {

    }
}
