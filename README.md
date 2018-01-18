# T.C Kimlik Doğrulaması
T.C kimlik numarası kontrol kurallarına uyarak önce kontrol edilir. Ardından nüfus müdürlüğü web servisi ile kontrol edilir.

## İlk Kontrol
- T.C kimlik numarasının 11 haneli ve sayısal değerde olduğu kontrol edilir.
İlk 9 rakam arasındaki formül 10.cu rakamı, ilk 10 rakam arasındaki formülasyon ise 11.ci rakamı oluşturur.
İlk rakam 0 olamaz.

- 1,3,5,7 ve 9.cu hanelerin toplamının 7 ile çarpımından 2,4,6, ve 8. haneler çıkartıldığında geriye kalan sayının 10'a göre modu 10. haneyi verir. (çıkarma işleminden elde edilen sonucun 10'a bölümünden kalan)
1,2,3,4,5,6,7,8,9 ve 10. sayıların toplamının 10'a göre modu (10'a bölümünden kalan) 11. rakamı sağlar.

## İkinci Kontrol
- Nüfus müdürlüğünün https://tckimlik.nvi.gov.tr/Service/KPSPublic.asmx?op=TCKimlikNoDogrula soap servisi kullanılarak doğrulama yapılır.

# Kullanımı

```
Identity control = new Identity(tc numarası, isminiz, soy isminiz, doğum yılı);
``` 
olacak şekilde yeni bir kimlik objesi oluşturuyoruz.

```
control.check();
``` 
şeklinde doğrulamayı yapabiliriz. Bu fonksiyon bize 1,2, ve 3 (byte) cinsinden geri değer döndürür.

### Değer sonuçları
- 1 : T.C numarası nüfus müdürlüğü tarafından teyit edilmiştir.
- 2 : T.C kimlik numarası geçerli değildir.
- 3 : T.C kimlik numarası kişi ile uyumlu değildir.
