using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera camArmario;
    public Camera camMesa;
    public Camera camBanheiro;

    public VerificacaoLista verificacaoLista;

    private void ActivateCamera(Camera targetCamera, string nomeCamera)
    {
        mainCamera.gameObject.SetActive(false);
        camArmario.gameObject.SetActive(false);
        camMesa.gameObject.SetActive(false);
        camBanheiro.gameObject.SetActive(false);

        if (targetCamera != null)
            targetCamera.gameObject.SetActive(true);

        if (verificacaoLista != null)
            verificacaoLista.AtualizarVisibilidadePorCamera(nomeCamera);
    }

    public void ShowMainCamera()
    {
        ActivateCamera(mainCamera, "Principal");
    }

    public void ShowCamArmario()
    {
        ActivateCamera(camArmario, "Armario");
    }

    public void ShowCamMesa()
    {
        ActivateCamera(camMesa, "Mesa");
    }

    public void ShowCamBanheiro()
    {
        ActivateCamera(camBanheiro, "Banheiro");
    }

    private void Start()
    {
        ShowMainCamera();
    }
}
