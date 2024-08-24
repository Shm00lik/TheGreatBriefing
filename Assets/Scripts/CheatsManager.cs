using UnityEngine;

public class CheatsManager : MonoBehaviour
{
    public string cheatCode = "GARIBI";  // Replace with your desired cheat code
    private string input = "";
    private bool enabledCheats = false;


    private void Start()
    {
        PlayerPrefs.SetInt("enableCheats", 0);  // Set cheat code to deactivated (0)
    }

    public void ToggleCheats()
    {

        if (!enabledCheats)
        {
            PlayerPrefs.SetInt("enableCheats", 1);
            enabledCheats = true;
            Debug.Log("Cheat activated!");
        } else
        {
            PlayerPrefs.SetInt("enableCheats", 0);
            enabledCheats = false;
            Debug.Log("Cheat disabled!");
        }
    }

    private void Update()
    {
        foreach (char c in Input.inputString)
        {
            input += c.ToString().ToUpper();

            if (input.Length > cheatCode.Length)
            {
                input = input.Substring(input.Length - cheatCode.Length);
            }

            if (input == cheatCode)
            {
                ToggleCheats();
                input = "";
            }
        }
    }
}
