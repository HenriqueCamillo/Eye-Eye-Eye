using UnityEngine;

public class EyeCameraController : MonoBehaviour
{
    public static EyeCameraController instance;
    [SerializeField] open_close eyeLidTop;
    [SerializeField] open_close eyeLidBottom;
    public bool isClosed;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    public void OpenEyes()
    {
        if (isClosed)
        {
            eyeLidBottom.OpenEyes();
            eyeLidTop.OpenEyes();
            isClosed = false;
        }
    }

    public void CloseEyes()
    {
        if (!isClosed)
        {
            eyeLidBottom.CloseEyes();
            eyeLidTop.CloseEyes();
            isClosed = true;
        }
    }
}