using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Karakter : MonoBehaviour
{
    public GameManager _GameManager;
    public Kamera _Kamera; 
    public bool SonaGeldikmi;
    public GameObject Gidecegiyer;
    public Slider _Slider;
    public GameObject GecisNoktasi;

    public List<GameObject> Alt_Karakterler;
    
    private Vector2 parmakBaslangicPozisyonu;
    private Vector2 parmakGuncelPozisyonu;
    [SerializeField] private float HIZ_CARPANI = 2f;
    [SerializeField] private float Karakter_Hizi = 1.2f;

    private NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (_Slider != null && GecisNoktasi != null)
        {
            float Fark = Vector3.Distance(transform.position, GecisNoktasi.transform.position);
            _Slider.maxValue = Fark;
        }
    }

    private void Update()
    {
        
        if (!SonaGeldikmi)
        {
            transform.Translate(Vector3.forward * Karakter_Hizi * Time.deltaTime);
        }

        if (SonaGeldikmi)
        {
            if (Gidecegiyer != null)
            {
                agent.SetDestination(Gidecegiyer.transform.position);
            }
               
            //transform.position = Vector3.MoveTowards(transform.position, Gidecegiyer.transform.position, 3 * Time.deltaTime);

            if (_Slider != null && _Slider.value > 0)
                _Slider.value -= 0.01f;
            
            
        }
        else
        {
            if (_Slider != null && GecisNoktasi != null)
            {
                float Fark = Vector3.Distance(transform.position, GecisNoktasi.transform.position);
                _Slider.value = Fark;
            }

            if (Time.timeScale != 0)
            {
                if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (transform.position.x >= 1.25)
                        {
                            print("1.25");
                            transform.position = Vector3.Lerp(transform.position,
                                new Vector3(1f, transform.position.y, transform.position.z), .1f * Time.deltaTime);
                        }
                        else
                        {
                            print("1,25");
                            transform.position = Vector3.Lerp(transform.position,
                                new Vector3(-1f, transform.position.y, transform.position.z), .1f * Time.deltaTime);
                        }

                        float mouseX = Input.GetAxis("Mouse X");
                        transform.position = Vector3.Lerp(transform.position,
                            new Vector3(transform.position.x + (mouseX * HIZ_CARPANI), transform.position.y,
                                transform.position.z), .1f);
                    }
                }
                else
                {
                    if (Input.touchCount > 0)
                    {
                        Touch touch = Input.GetTouch(0);

                        if (touch.phase == TouchPhase.Began)
                        {
                            parmakBaslangicPozisyonu = touch.position;
                        }
                        else if (touch.phase == TouchPhase.Moved)
                        {
                            if (transform.position.x >= 1.25)
                            {
                                print("1.25");
                                transform.position = Vector3.Lerp(transform.position,
                                    new Vector3(1f, transform.position.y, transform.position.z), .1f * Time.deltaTime);
                            }
                            else
                            {
                                print("1,25");
                                transform.position = Vector3.Lerp(transform.position,
                                    new Vector3(-1f, transform.position.y, transform.position.z), .1f * Time.deltaTime);
                            }

                            parmakGuncelPozisyonu = touch.position;
                            float farkX = (parmakGuncelPozisyonu.x - parmakBaslangicPozisyonu.x) / Screen.width;

                            transform.position = Vector3.Lerp(transform.position,
                                new Vector3(transform.position.x + (farkX * HIZ_CARPANI * 10f), transform.position.y,
                                    transform.position.z), .1f);

                            parmakBaslangicPozisyonu = parmakGuncelPozisyonu;
                        }
                    }
                }
            }
        }
    }

    Vector3 PozisyonVer()
    {
        return new Vector3(transform.position.x, .23f, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Carpma") || other.CompareTag("Toplama") || other.CompareTag("Cikartma") ||
            other.CompareTag("Bolme"))
        {
            int sayi = int.Parse(other.name);
            if (_GameManager != null)
                _GameManager.AdamYonetim(other.tag, sayi, other.transform);  
        }
        else if (other.CompareTag("Sontetikleyici"))
        {
            if (_Kamera != null)
                _Kamera.SonaGeldikmi = true;

            if (_GameManager != null)
                _GameManager.DusmanlariTetikle();

            SonaGeldikmi = true;
        }
        else if (other.CompareTag("BosKarakter"))
        {
            if (_GameManager != null)
                _GameManager.Karakterler.Add(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Direk") || other.gameObject.CompareTag("Pervaneigneler"))
        {
            if (transform.position.x > 0f)
            {
                transform.position = new Vector3(transform.position.x - 0.35f, transform.position.y, transform.position.z);
            }
            else if (transform.position.x < 0f)
            {
                transform.position = new Vector3(transform.position.x + 0.35f, transform.position.y, transform.position.z);
            }
            else
            {
                float randomPozisyon = (Random.value < 0.5f) ? -0.35f : 0.35f;
                transform.position = new Vector3(transform.position.x + randomPozisyon, transform.position.y, transform.position.z);
            }
        }
        else if (other.gameObject.CompareTag("igneliKutuObje"))
        {
            if (transform.position.x > 0f)
            {
                transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
            }
            else if (transform.position.x < 0f)
            {
                transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
            }
            else
            {
                float randomPozisyon = (Random.value < 0.5f) ? -0.5f : 0.5f;
                transform.position = new Vector3(transform.position.x + randomPozisyon, transform.position.y, transform.position.z);
            }
        }
    }
}
