using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shelter : MonoBehaviour
{
    [Header("Parameters")]
    private ShelterState state;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI provisionUI;
    [SerializeField] private TextMeshProUGUI fiberUI;
    [SerializeField] private TextMeshProUGUI oreUI;
    [SerializeField] private TextMeshProUGUI oilUI;
    [SerializeField] private TextMeshProUGUI crystalShardUI;
    [SerializeField] private TextMeshProUGUI elderTreeFlowerUI;

    public ShelterState State
    {
        get => state;
        set
        {
            state = value;
            UpdateUI(state);
        }
    }

    private void Start()
    {
        RequestShelterState();
    }

    private void RequestShelterState()
    {
        State = GameManager.Instance.shelterState;
    }

    private void UpdateUI(ShelterState state)
    {
        provisionUI.text = state.provision.ToString();
        fiberUI.text = state.fiber.ToString();
        oreUI.text = state.ore.ToString();
        oilUI.text = state.oil.ToString();
        crystalShardUI.text = state.crystalShard.ToString();
        elderTreeFlowerUI.text = state.elderTreeFlower.ToString();
    }
}
