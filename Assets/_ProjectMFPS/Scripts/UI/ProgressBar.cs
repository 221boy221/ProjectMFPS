using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    #region Vars

    [SerializeField]
    private RectTransform _bg;

    [SerializeField]
    private RectTransform _fill;

    #endregion

    #region Methods
    
    /// <summary>
    /// Updates the fill rate on the progress bar UI element.
    /// </summary>
    /// <param name="normal"></param>
    public void SetProgress(float normal)
    {
        _fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _bg.rect.width * normal);
    }

    #endregion
}