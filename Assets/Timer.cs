using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    #region Zamanlayıcı Değişkenleri
    [SerializeField] public float zamanOyuncu1 = 60;
    [SerializeField] public float zamanOyuncu2 = 60;
    [SerializeField] public float kullanılanZamanOyuncu1 = 3;
    [SerializeField] public float kullanılanZamanOyuncu2 = 3;

    [SerializeField] public float increment = 0;
    public bool sıra;
    public bool durduruldu;
    public bool başladı;
    public bool oyunBitti;
    #endregion

    #region UI değişkenleri

    [SerializeField] public Button butonOyuncu1;
    [SerializeField] public Button butonOyuncu2;
    [SerializeField] public Button durdurButon;
    [SerializeField] public Text zamanOyuncu1Text;
    [SerializeField] public Text zamanOyuncu2Text;
    [SerializeField] public Slider timeSlider;
    [SerializeField] public Slider eklemeSlider;
    [SerializeField] public AudioClip değiştirme_ses;
    [SerializeField] public AudioSource _audioSource;

    [SerializeField] public InputField oyuncu1isim;
    [SerializeField] public InputField oyuncu2isim;

    #endregion

    

    #region Zamanlayıcı Fonksiyonları

    void Start()
    {
        ZamanGüncelle();
        ZamanGüncelle();
    }
    void Update(){

        if (zamanOyuncu1 < 0 || zamanOyuncu2 < 0)
        {
            durduruldu = true;
            oyunBitti = true;
        }


        if (durduruldu){
            butonOyuncu1.GetComponent<Image>().color = Color.gray;
            butonOyuncu2.GetComponent<Image>().color = Color.gray;
            oyuncu1isim.interactable = true;
            oyuncu2isim.interactable = true;
            return;
        }
        oyuncu1isim.interactable = false;
        oyuncu2isim.interactable = false;
        if (sıra){
            zamanOyuncu1 -= Time.deltaTime;
            kullanılanZamanOyuncu1 += Time.deltaTime;
            butonOyuncu1.GetComponent<Image>().color = Color.green;
            butonOyuncu2.GetComponent<Image>().color = Color.red;
        } else{
            zamanOyuncu2 -= Time.deltaTime;
            kullanılanZamanOyuncu2 += Time.deltaTime;
            butonOyuncu2.GetComponent<Image>().color = Color.green;
            butonOyuncu1.GetComponent<Image>().color = Color.red;
        }


        ZamanGüncelle();

    }


    public void SıraDeğiştir(bool oyuncu){
        if (oyunBitti) return;
        if (!başladı) {
            başladı = true;
            _audioSource.PlayOneShot(değiştirme_ses); // ilk başladığında çalması için
        }
        durduruldu = false;
        if (oyuncu != sıra){ // sırası olmayan oyuncu basıyor
            sıra = oyuncu;
            _audioSource.PlayOneShot(değiştirme_ses);
        }
        else return;
        if (!sıra){
            zamanOyuncu1 += Math.Max(0, increment - kullanılanZamanOyuncu1);
            kullanılanZamanOyuncu1 = 0;
        }
        else{
            zamanOyuncu2 += Math.Max(0, increment - kullanılanZamanOyuncu2);
            kullanılanZamanOyuncu2 = 0;
        }

        
    }

    Dictionary<float, float> zamanDict = new Dictionary<float, float>()
    {
        {0, 60},
        {1, 180},
        {2, 300},
        {3, 600},
        {4, 1200},
        {5, 1800},
        {6, 3600}
    };
    Dictionary<float, float> eklemeDict = new Dictionary<float, float>()
    {
        {0, 0},
        {1, 5},
        {2, 10},
        {3, 20},
        {4, 30},
        {5, 60},
        {6, 120}
    };

    public void ZamanDeğiştir(){
        if (!durduruldu) return;
        oyunBitti = false;
        float zaman = timeSlider.value;
        zamanOyuncu1 = zamanDict[zaman];
        zamanOyuncu2 = zamanDict[zaman];
        ZamanGüncelle();
    }
    public void IncrementDeğiştir(){
        if (!durduruldu) return;
        oyunBitti = false;
        increment = eklemeDict[eklemeSlider.value];
        ZamanGüncelle();
    }

    public void Durdur(){
        if (!başladı) return;
        durduruldu = !durduruldu;
    }

    public void Sıfırla(){
        zamanOyuncu1 = zamanDict[timeSlider.value];
        zamanOyuncu2 = zamanDict[timeSlider.value];
        durduruldu = true;
        oyunBitti = false;
        başladı = false;
        ZamanGüncelle();
    }

    void ZamanGüncelle(){
        zamanOyuncu1Text.text = ZamanHesapla(zamanOyuncu1);
        zamanOyuncu2Text.text = ZamanHesapla(zamanOyuncu2);



    }

    IEnumerator Bekle(int saniye){
        yield return new WaitForSeconds(saniye);
    }

    #endregion

    #region Aritmetik Fonksiyonlar
    string ZamanHesapla(float zaman){ // saat:dakika.saniye
        int saat = (int)zaman / 3600;
        int dakika = (int)zaman / 60 - saat * 60;
        int saniye = (int)zaman - saat * 3600 - dakika * 60;
        return $"{saat:00}:{dakika:00}:{saniye:00}";
    }
    #endregion
}
