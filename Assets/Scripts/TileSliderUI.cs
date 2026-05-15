using UnityEngine;
using UnityEngine.UI;

public class TileSliderUI : MonoBehaviour
{
    public TileSlider tileSlider;
    public Button slideRightButton;
    public Button slideLeftButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slideRightButton.onClick.AddListener(() =>
        {
            tileSlider.SlideRight();
        });

        slideLeftButton.onClick.AddListener(() =>
        {
            tileSlider.SlideLeft();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
