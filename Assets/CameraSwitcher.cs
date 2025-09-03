using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera camArmario;
    public Camera camMesa;
    public Camera camBanheiro;

    // Desativa todas e ativa a escolhida
    private void ActivateCamera(Camera targetCamera)
    {
        mainCamera.gameObject.SetActive(false);
        camArmario.gameObject.SetActive(false);
        camMesa.gameObject.SetActive(false);
        camBanheiro.gameObject.SetActive(false);

        if (targetCamera != null)
            targetCamera.gameObject.SetActive(true);
    }

    // Métodos públicos para os botões
    public void ShowMainCamera()
    {
        ActivateCamera(mainCamera);
    }

    public void ShowCamArmario()
    {
        ActivateCamera(camArmario);
    }

    public void ShowCamMesa()
    {
        ActivateCamera(camMesa);
    }

    public void ShowCamBanheiro()
    {
        ActivateCamera(camBanheiro);
    }

    // Inicia com a câmera principal ativa
    private void Start()
    {
        ShowMainCamera();
    }
}
