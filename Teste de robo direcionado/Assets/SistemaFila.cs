using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaFila : MonoBehaviour
{
    SistemaFila sistemaFila;
    GameObject objetoRemovido;

    public List<GameObject> listaDeObjetos = new List<GameObject>();
 

    public void AdicionarComTagNaFila(string Coin)
        {
            GameObject[] objetosComTag = GameObject.FindGameObjectsWithTag(Coin);
            
            foreach (GameObject objeto in objetosComTag)
                {
                    listaDeObjetos.Add(objeto);
                }
                
                //Direcionado para qualaquer um independente
                var dronePos = transform.position;
                
                //Calculo individual (independente da posição) de cada objeto na lista
                var dist0 = listaDeObjetos[0].transform.position - dronePos;

                
                // Informando lista
                GameObject Coin_obj = listaDeObjetos[0];

                //Debug.Log(dist.magnitude);

                //Econtrando o objeto na lista
                foreach (GameObject CoinRef in listaDeObjetos)
                {
                    //Calculando para o mais proximo
                    if ((CoinRef.transform.position - dronePos).magnitude < dist0.magnitude)
                    {
                        dist0 = CoinRef.transform.position - dronePos;
                        Coin_obj = CoinRef;
                    }

                    Debug.Log(dist0.magnitude);
                }
        }


    public GameObject RemoverDaFila()
    {
        if (listaDeObjetos.Count == 0) return null;
        
        GameObject Coin_obj = listaDeObjetos[0];
        listaDeObjetos.RemoveAt(0);
        
        return Coin_obj;
    }

    // Start is called before the first frame update
    void Start()
    {
        sistemaFila = GetComponent<SistemaFila>();
        sistemaFila.AdicionarComTagNaFila("Coin");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            {
                objetoRemovido = sistemaFila.RemoverDaFila();
                if (objetoRemovido != null)
                    {

                    }
            }
    }  
}
