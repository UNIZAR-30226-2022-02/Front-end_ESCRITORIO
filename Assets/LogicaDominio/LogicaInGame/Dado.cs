using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dado : MonoBehaviour
{
    private GameObject[] caras;
    int ultValor = 0;
    bool rodando;

    // Start is called before the first frame update
    void Awake()
    {
        ultValor = 0;

        int i = 0;
        foreach(Transform cara in this.transform){
            caras[i++] = cara.gameObject;
        }
    }

    public void mostrarTirada(int valorFinal){
        StartCoroutine(mostrarTiradaAux(valorFinal));
    }

    private IEnumerator mostrarTiradaAux(int valorFinal){
        ultValor = 0;
        for (int i = 0; i <= 20; i++)
        {
            int valor = Random.Range(0, 5);
            if(i==20){
                valor = valorFinal;
            }
            
            Debug.Log(this.gameObject.name + ": ultvalor=" + ultValor);
            caras[ultValor].SetActive(false);
            caras[valor].SetActive(true);
            ultValor = valor;
            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(6);
    }

}
