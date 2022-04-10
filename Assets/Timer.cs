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

    #endregion

    #region Zamanlayıcı Fonksiyonları
    void Update(){

        if (zamanOyuncu1 < 0 || zamanOyuncu2 < 0)
        {
            durduruldu = true;
            oyunBitti = true;
        }


        if (durduruldu){
            butonOyuncu1.GetComponent<Image>().color = Color.gray;
            butonOyuncu2.GetComponent<Image>().color = Color.gray;
        }
        else if (sıra){
            zamanOyuncu1 -= Time.deltaTime;
            butonOyuncu1.GetComponent<Image>().color = Color.green;
            butonOyuncu2.GetComponent<Image>().color = Color.red;
        } else{
            zamanOyuncu2 -= Time.deltaTime;
            butonOyuncu2.GetComponent<Image>().color = Color.green;
            butonOyuncu1.GetComponent<Image>().color = Color.red;
        }

        ZamanGüncelle();

    }

    public void SıraDeğiştir(int oyuncu){
        if (oyunBitti) return;
        if (!başladı) başladı = true;
        durduruldu = false;
        if (Convert.ToInt32(sıra) == oyuncu){
            return;
        }
        else{            
            sıra = Convert.ToBoolean(oyuncu);
        }
        if (zamanOyuncu1 <= 5){
            zamanOyuncu1 = 5.8f;
        }
        if (zamanOyuncu2 <= 5){
            zamanOyuncu2 = 5.8f;
        }
        ZamanGüncelle();
        
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

    public void ZamanDeğiştir(){
        if (!durduruldu) return;
        oyunBitti = false;
        float zaman = timeSlider.value;
        zamanOyuncu1 = zamanDict[zaman];
        zamanOyuncu2 = zamanDict[zaman];
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
