using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;

namespace DoubleDCore.OS.Compression
{
    public static class CompressionService
    {
        /// <summary>
        /// Сериализует объект в JSON, сжимает его GZip-ом и возвращает результат в виде Base64-строки.
        /// </summary>
        /// <typeparam name="T">Тип сериализуемого объекта.</typeparam>
        /// <param name="data">Объект для сериализации и сжатия.</param>
        /// <returns>Base64-строка, содержащая GZip-сжатый JSON.</returns>
        public static string CompressBase64<T>(T data)
        {
            var json = JsonUtility.ToJson(data);
            var jsonBytes = Encoding.UTF8.GetBytes(json);

            using var memoryStream = new MemoryStream();

            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
            {
                gzipStream.Write(jsonBytes, 0, jsonBytes.Length);
            }

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        /// <summary>
        /// Принимает Base64-строку с GZip-сжатыми JSON-данными, 
        /// распаковывает и десериализует в объект типа T.
        /// </summary>
        /// <typeparam name="T">Тип объекта для десериализации.</typeparam>
        /// <param name="data">Base64-строка с GZip-сжатыми JSON-данными.</param>
        /// <returns>Восстановленный объект типа T.</returns>
        public static T DecompressBase64<T>(string data)
        {
            var compressedData = Convert.FromBase64String(data);

            using var memoryStream = new MemoryStream(compressedData);
            using var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
            using var reader = new StreamReader(gzipStream, Encoding.UTF8);
            
            var json = reader.ReadToEnd();
            return JsonUtility.FromJson<T>(json);
        }
    }
}