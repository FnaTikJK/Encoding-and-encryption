using Encryption.Vigener;

var str = "Метод Array.IndexOf в C# выполняет поиск указанного объекта и возвращает индекс первого найденного совпадения в одномерном массиве или диапазоне элементов массива";
var key = "ВАСЯ";

var encrypted = Vigener.Encrypt(str, key);
var decrypted = Vigener.Decrypt(encrypted, key);
var a = 1;