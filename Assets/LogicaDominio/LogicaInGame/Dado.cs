using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dado : MonoBehaviour
{
    public const int duracionTirada = 6;

    private GameObject[] caras;
    int ultValor = 0;

    // Start is called before the first frame update
    void Awake()
    {
        ultValor = 0;

        int i = 0;
        caras = new GameObject[6];
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
            int valor = Random.Range(1, 6);
            if(i==20){
                valor = valorFinal;
            }
            
            caras[ultValor].SetActive(false);
            caras[valor-1].SetActive(true);
            ultValor = valor-1;
            // Pause before next iteration
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(duracionTirada - 2f);
        caras[ultValor].SetActive(false);
    }

}
