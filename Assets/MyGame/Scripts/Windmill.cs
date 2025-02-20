

public class Windmill : MonoBehaviour
{
    [SerializeField] private GameObject windmill;
    [SerializeField] private Slider windmillSlider;
    [SerializeField] private Button lockAndApplyButton;
    [SerializeField] private Light windmillLight;

    private GameObject colorTarget;
    private float windmillSpeed = 0f;
    private bool isLocked = false;
    private bool isActive = false;
    private float maxRotationSpeed = 255f;
    private float decreaseRate = 100f;

    private WindmillManager manager;
    private int windmillIndex;

    private void Start()
    {
        if (windmill == null)
        {
            windmill = this.gameObject;
        }

        lockAndApplyButton.onClick.AddListener(LockAndApplyColor);
        windmillSlider.value = windmillSpeed;
    }

    private void Update()
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (!isLocked && isActive)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                IncreaseWindmillValue(Time.deltaTime);
            }
            else
            {
                DecreaseWindmillValue(Time.deltaTime);
            }
        }

        RotateWindmill();
    }

    private void IncreaseWindmillValue(float deltaTime)
    {
        windmillSpeed = Mathf.Clamp(windmillSpeed + (deltaTime * 100f), 0, maxRotationSpeed);
        windmillSlider.value = windmillSpeed;
    }

    private void DecreaseWindmillValue(float deltaTime)
    {
        windmillSpeed = Mathf.Clamp(windmillSpeed - (deltaTime * decreaseRate), 0, maxRotationSpeed);
        windmillSlider.value = windmillSpeed;
    }

    private void RotateWindmill()
    {
        if (windmill != null)
        {
            windmill.transform.Rotate(0, 0, windmillSpeed * Time.deltaTime);
        }
    }

    public void LockAndApplyColor()
    {
        if (!isLocked)
        {
            isLocked = true;
            isActive = false;
            lockAndApplyButton.interactable = false;

            Color appliedColor = GetAppliedColor();
            if (manager != null)
            {
                manager.NotifyWindmillLocked(windmillIndex, appliedColor, windmillSpeed);
            }
        }
    }

    private Color GetAppliedColor()
    {
        Color lampColor = windmillLight.color;
        float speedFactor = Mathf.Clamp(windmillSpeed / maxRotationSpeed, 0f, 1f);
        return lampColor * speedFactor;
    }

    public void SetColorTarget(GameObject target)
    {
        colorTarget = target;
    }

    public void SetManager(WindmillManager windmillManager, int index)
    {
        manager = windmillManager;
        windmillIndex = index;
    }

    public void SetActiveState(bool state)
    {
        isActive = state;
    }
}
