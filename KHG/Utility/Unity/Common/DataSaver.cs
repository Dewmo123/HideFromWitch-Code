using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace Utility.Unity.Common
{
    public class SaveData//내용물을 수1정
    {
        public string Name;
        public int value;
    }
    public class DataSaver
    {
        private static readonly string _privateKey = "B2yG7UoXxL16lwlcmIPJiJeb3tC25WP8";//프로젝트마다 변환 권1장
        public static void Save(SaveData data)
        {
            SaveData saveData = data;
            //SaveData data = new SaveData();

            //data.Name = "testItem";
            //data.value = 10;

            string jsonString = DataToJson(saveData);
            string encryptString = Encrypt(jsonString);
            SaveFile(encryptString);
        }
        public static SaveData Load()
        {
            if (!File.Exists(GetPath()))
            {
                Debug.LogWarning("세이브 파일이 존재하지 않음.");
                return null;
            }

            string encryptData = LoadFile(GetPath());
            string decryptData = Decrypt(encryptData);

            Debug.Log(decryptData);

            SaveData sd = JsonToData(decryptData);
            return sd;
        }





        static string DataToJson(SaveData sd)
        {
            string jsonData = JsonUtility.ToJson(sd);
            return jsonData;
        }
        static SaveData JsonToData(string jsonData)
        {
            SaveData sd = JsonUtility.FromJson<SaveData>(jsonData);
            return sd;
        }

        static void SaveFile(string jsonData) //저장
        {
            using (FileStream fs = new FileStream(GetPath(), FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData); //파일로 저장할수있게 변환

                fs.Write(bytes, 0, bytes.Length);//최대길이까지 복사
            }
        }

        static string LoadFile(string path) //파일 불러오기
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[(int)fs.Length]; //바이트화해서 담을곳

                fs.Read(bytes, 0, (int)fs.Length); //추출

                //추출한 바이트를 json string으로 인코딩
                string jsonString = System.Text.Encoding.UTF8.GetString(bytes);
                return jsonString;
            }
        }

        //-----
        private static string Encrypt(string data)
        {

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            RijndaelManaged rm = CreateRijndaelManaged();
            ICryptoTransform ct = rm.CreateEncryptor();
            byte[] results = ct.TransformFinalBlock(bytes, 0, bytes.Length);
            return System.Convert.ToBase64String(results, 0, results.Length);
        }
        private static string Decrypt(string data)
        {
            byte[] bytes = System.Convert.FromBase64String(data);
            RijndaelManaged rm = CreateRijndaelManaged();
            ICryptoTransform ct = rm.CreateDecryptor();
            byte[] resultArray = ct.TransformFinalBlock(bytes, 0, bytes.Length);
            return System.Text.Encoding.UTF8.GetString(resultArray);
        }


        private static RijndaelManaged CreateRijndaelManaged() //Rijndael은 AES계열 알고리즘 구현체
        {
            byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(_privateKey);
            RijndaelManaged result = new RijndaelManaged();

            byte[] newKeysArray = new byte[16];
            System.Array.Copy(keyArray, 0, newKeysArray, 0, 16);

            result.Key = newKeysArray;
            result.Mode = CipherMode.ECB;
            result.Padding = PaddingMode.PKCS7;
            return result;
        }

        static string GetPath() //저장주소반환 (AppData/LocalLow)
        {
            return Path.Combine(Application.persistentDataPath, "save.abcd");
        }
    }
}
