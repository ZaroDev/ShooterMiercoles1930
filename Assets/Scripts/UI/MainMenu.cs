using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//dbaujwhnbuihbuibuidaw
//Esto solo es accesible desde la rama de test

//dbaujw
//bduiwabduyihwbduiwa
public class MainMenu : MonoBehaviour
{
    bool opciones = false;
    public GameObject menuOpciones;
    void Start()
    {
        menuOpciones.SetActive(false);
    }
    public void Jugar()
    {
        SceneManager.LoadScene(1);
    }
    public void Opciones()
    {
        opciones = !opciones;
        menuOpciones.SetActive(opciones);
    }
    public void Salir()
    {
        Application.Quit();
    }
}
