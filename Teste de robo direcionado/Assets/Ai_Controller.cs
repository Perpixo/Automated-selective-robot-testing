using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Controller : MonoBehaviour
{
    GameObject targetObject;

    [SerializeField] List<GameObject> listaDeObjetos = new List<GameObject>();

    //public float proximidade;

    public float speed = 1.0f;
    public float altitude = 1.0f;

    public float altitudeFromBase;
    public Vector3 posicaoRelativa;

    private bool isFlying = false;
    public bool isOverTarget = false;
    private float currentAltitude = 0.0f;

    public float amplitude = 1.5f;
    
    private float startTime;

    [SerializeField] GameObject basefinal;
    public Transform basefinalTransf;
    public bool ChegouFinalBase;

    SistemaFila sistemaFila;
    [SerializeField] GameObject Player;

    //Coin "Capiturando" comunicação
    Capiturado capiturado;

    public int indiceAtual = 0;

    void AddLista()
        {
            foreach (GameObject objetosEmMim in GameObject.FindGameObjectsWithTag("Coin"))
            {

                listaDeObjetos.Add(objetosEmMim);
                
            }
            //Direcionado para qualaquer um independente
            var dronePos = transform.position;
            var dist = listaDeObjetos[0].transform.position - dronePos;
            //Debug.Log(dist.magnitude);

            // Identificando o primeiro Coin da lista
            GameObject FirstCoin = listaDeObjetos[0];

            //Econtrando o objeto na lista
            foreach (GameObject Coin in listaDeObjetos)
            {
                //Indo para o mais proximo
                if ((Coin.transform.position - dronePos).magnitude < dist.magnitude)
                {
                    dist = Coin.transform.position - dronePos;
                    FirstCoin = Coin;
                }

            }

            targetObject = FirstCoin;
        }
    
    /* void Tracking()
        {
            //Direcionado para qualaquer um independente
            var dronePos = transform.position;
            var dist = listaDeObjetos[0].transform.position - dronePos;
            
            // Informando lista
            GameObject Coin_obj = listaDeObjetos[0];

            //Debug.Log(dist.magnitude);

            //Econtrando o objeto na lista
            foreach (GameObject Coin in listaDeObjetos)
            {
                //Indo para o mais proximo
                if ((Coin.transform.position - dronePos).magnitude < dist.magnitude)
                {
                    dist = Coin.transform.position - dronePos;
                    Coin_obj = Coin; 
                }    
            }
            
            targetObject = Coin_obj;
        } */

    void Awake()
    {
        sistemaFila = Player.GetComponent<SistemaFila>();
    }
    
    void Start()
        {
            //pegando informação da altitude altual no eixo y
            currentAltitude = transform.position.y;

            AddLista();
            
            listaDeObjetos.Reverse();

        }

    void Update()
    {

        void Direcionamento()
            {
                // Calcula a direção e a distância para o objeto
                Vector3 direction = targetObject.transform.position - transform.position;
                float distance = direction.magnitude;

                // Normaliza a direção e multiplica pela velocidade
                Vector3 velocity = direction.normalized * speed;

                // Move o cubo na direção e velocidade calculadas
                transform.Translate(velocity * Time.deltaTime);

                // Verifica se o cubo chegou ao objeto
                if (distance < 0.5f)
                {
                    isOverTarget = true;

                    //Debug.Log(isOverTarget);
                }
            }
        void OnTopOf()
            {
            // Faz o cubo apontar para o objeto
            //transform.LookAt(targetObject.transform);

            // Move o cubo para cima
            transform.Translate(Vector3.up * altitude * Time.deltaTime);

            // Move o cubo sobre o objeto
            Vector3 direction = targetObject.transform.position - transform.position;
            Vector3 velocity = direction.normalized * speed;
            direction.y = Mathf.Sin(Time.time * speed) * amplitude;
            transform.Translate(velocity * Time.deltaTime);
            }

        void NotFlying()
            {
            // Faz o cubo levantar voo
            currentAltitude += altitude * Time.deltaTime;
            transform.Translate(Vector3.up * altitude * Time.deltaTime);

            // Verifica se atingiu a altitude desejada
            if (currentAltitude >= altitude)
                {
                    isFlying = true;
                }
            }

        if (!isFlying)
            {
            NotFlying();
            }
        
        else if (!isOverTarget)
            {
                Direcionamento();
            }
        else
            {
                OnTopOf();
            }

        //Reconhcendo o coin da lista e verificando o bool que capiturou no outro script
        capiturado = targetObject.GetComponent<Capiturado>();

        if (!ChegouFinalBase)
            {

                if (capiturado.DroneGotMe)
                    {
                    //Debug.Log("Peguei");

                    //subir levando o objeto
                    transform.Translate(Vector3.up * altitudeFromBase * Time.deltaTime);

                    //calcular posição RELATIVA da base final
                    Vector3 posicaoSobre = basefinalTransf.TransformPoint(posicaoRelativa);
                    
                    //calcular posição da base final
                    Vector3 direction = basefinal.transform.position - transform.position + posicaoRelativa;
                    float distanceFromBase = direction.magnitude;
                    
                    // Normaliza a direção e multiplica pela velocidade
                    Vector3 velocity = direction.normalized * speed;

                    // Move o cubo na direção e velocidade calculadas
                    transform.Translate(velocity * Time.deltaTime);

                        if (distanceFromBase < 0.1f)
                        {
                            
                            if (startTime == 0f)
                            {
                                startTime = Time.time;
                            }
                        
                            float elapsedTime = Time.time - startTime;

                            if (elapsedTime > 1.0f)
                            {
                                ChegouFinalBase = true;  
                                
                            }

                        }

                }

            }

        if (capiturado.Depositado)
            {

                if (indiceAtual < listaDeObjetos.Count - 1)
                    {
                        indiceAtual++;
                        targetObject = listaDeObjetos[indiceAtual];
                        ChegouFinalBase = false;
                        isOverTarget = false;
                                    
                        Debug.Log(targetObject);
                    }
            }
    }
        
        
}

