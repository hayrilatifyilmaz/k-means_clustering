//Coded by hayrilatifyilmaz
//https://github.com/hayrilatifyilmaz


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k_ortalamalari_kumelemesi_deneme
{
    public class K_OrtalamaKumeleme
    {
        public class GorselSiniflandirma
        {
            Bitmap girdi;
            int bolumleme_sayisi;
            Color[,] renk_haritasi;
            int[,] siniflandirma_haritasi;
            float[][] merkez_noktalari;
            Color[] sinif_renkleri = new Color[10];
            Bitmap cikti = new Bitmap(1, 1);

            public GorselSiniflandirma(Bitmap girdi, int bolumleme_sayisi)
            {
                this.girdi = girdi;
                this.bolumleme_sayisi = bolumleme_sayisi;

                renk_haritasi = new Color[girdi.Width, girdi.Height];
                siniflandirma_haritasi = new int[girdi.Width, girdi.Height];

                merkez_noktalari = new float[bolumleme_sayisi][];
                for (int i = 0; i < bolumleme_sayisi; i++)
                {
                    merkez_noktalari[i] = new float[3];
                }
            }

            public Bitmap Siniflandir(int iterasyon_sayisi)
            {
                while (true)
                {
                    renk_haritalamasi();
                    rastgele_merkez_secimi();
                    yakin_kume_sinif_haritalama();

                    for (int i = 0; i < iterasyon_sayisi; i++)
                    {
                        merkez_nokta_konum_guncelleme();
                        yakin_kume_sinif_haritalama();
                    }

                    if (iki_boyutlu_dizi_eleman_esitlik_arayici() == true)
                    {
                        break;
                    }
                }

                cikti = gorsele_cevir();

                return cikti;
            }

            private float oklid_uzaklik_bulma(float[] ilk_nokta, float[] ikinci_nokta)
            {
                if (ilk_nokta == null || ikinci_nokta == null)
                {
                    return 0f;
                }
                else if (ilk_nokta.Length != ikinci_nokta.Length)
                {
                    return 0f;
                }
                else if (ilk_nokta.Length > 3)
                {
                    return 0f;
                }

                if (ilk_nokta.Length == 1)
                {
                    return (float)Math.Sqrt(Math.Pow(ilk_nokta[0] - ikinci_nokta[0], 2));
                }
                if (ilk_nokta.Length == 2)
                {
                    return (float)Math.Sqrt(Math.Pow(ilk_nokta[0] - ikinci_nokta[0], 2) + Math.Pow(ilk_nokta[1] - ikinci_nokta[1], 2));
                }
                if (ilk_nokta.Length == 3)
                {
                    return (float)Math.Sqrt(Math.Pow(ilk_nokta[0] - ikinci_nokta[0], 2) + Math.Pow(ilk_nokta[1] - ikinci_nokta[1], 2) + Math.Pow(ilk_nokta[2] - ikinci_nokta[2], 2));
                }
                if (ilk_nokta.Length == 4)
                {
                    return (float)Math.Sqrt(Math.Pow(ilk_nokta[0] - ikinci_nokta[0], 2) + Math.Pow(ilk_nokta[1] - ikinci_nokta[1], 2) + Math.Pow(ilk_nokta[2] - ikinci_nokta[2], 2) + Math.Pow(ilk_nokta[3] - ikinci_nokta[3], 2));
                }
                else
                {
                    return 0f;
                }
            }

            private void renk_haritalamasi()
            {
                for (int x = 0; x < girdi.Width; x++)
                {
                    for (int y = 0; y < girdi.Height; y++)
                    {
                        renk_haritasi[x, y] = girdi.GetPixel(x, y);
                    }
                }
            }

            private void rastgele_merkez_secimi()
            {

                for (int i = 0; i < bolumleme_sayisi; i++)
                {
                    for (int a = 0; a < merkez_noktalari[i].Length; a++)
                    {
                        Random rastgele = new Random(Guid.NewGuid().GetHashCode());
                        merkez_noktalari[i][a] = rastgele.Next(20, 235);
                        sinif_renkleri[a] = Color.FromArgb((int)merkez_noktalari[i][a] - 20, (int)merkez_noktalari[i][a], (int)merkez_noktalari[i][a] + 20);
                    }
                }

            }

            private void yakin_kume_sinif_haritalama()
            {
                for (int x = 0; x < girdi.Width; x++)
                {
                    for (int y = 0; y < girdi.Height; y++)
                    {
                        float[] uzakliklar = new float[bolumleme_sayisi];

                        for (int i = 0; i < bolumleme_sayisi; i++)
                        {
                            Color a = renk_haritasi[x, y];
                            uzakliklar[i] = oklid_uzaklik_bulma(new float[] { a.R, a.G, a.B }, merkez_noktalari[i]);
                        }

                        var sinif = Array.IndexOf(uzakliklar, uzakliklar.Min());

                        siniflandirma_haritasi[x, y] = sinif;
                    }
                }
            }

            private void merkez_nokta_konum_guncelleme()
            {
                for (int i = 0; i < bolumleme_sayisi; i++)
                {
                    float r = 0, g = 0, b = 0;
                    int renk_sayisi = 0;

                    for (int x = 0; x < girdi.Width; x++)
                    {
                        for (int y = 0; y < girdi.Height; y++)
                        {
                            if (siniflandirma_haritasi[x, y] == i)
                            {
                                Color a = renk_haritasi[x, y];
                                r += a.R;
                                g += a.G;
                                b += a.B;

                                renk_sayisi += 1;
                            }
                        }
                    }

                    merkez_noktalari[i] = new float[] { r / renk_sayisi, g / renk_sayisi, b / renk_sayisi };

                }
            }

            private Bitmap gorsele_cevir()
            {

                Bitmap cikti = new Bitmap(girdi.Width, girdi.Height);

                for (int x = 0; x < girdi.Width; x++)
                {
                    for (int y = 0; y < girdi.Height; y++)
                    {
                        cikti.SetPixel(x, y, sinif_renkleri[siniflandirma_haritasi[x, y]]);
                    }
                }

                return cikti;
            }

            private bool iki_boyutlu_dizi_eleman_esitlik_arayici()
            {
                int onceki_deger = siniflandirma_haritasi[0, 0];
                for (int x = 0; x < siniflandirma_haritasi.GetLength(0); x++)
                {
                    for (int y = 0; y < siniflandirma_haritasi.GetLength(1); y++)
                    {
                        if (siniflandirma_haritasi[x, y] != onceki_deger)
                        {
                            return true;
                            throw new Exception();
                        }
                        onceki_deger = siniflandirma_haritasi[x, y];
                    }
                }

                return false;
            }
        }
    }
}
