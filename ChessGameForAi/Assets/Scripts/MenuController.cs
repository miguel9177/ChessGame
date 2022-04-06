using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public void VeryEasy()
    {
        SceneManager.LoadScene("VeryEasy");
    }

    public void Easy()
    {
        SceneManager.LoadScene("Easy");
    }

    public void NormalDefensive()
    {
        SceneManager.LoadScene("NormalDefensive");
    }

    public void Normal()
    {
        SceneManager.LoadScene("PlayableScene");
    }
}
