using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GmailParserViewProgram.Act
{
    public class FileParser
    {
        static private FileStream lastFile = null;
        static public FileStream LastFile
        {
            get { return lastFile; }
            private set { lastFile = value; }
        }

        static public void Write (string path , object obj)
        {
            if (File.Exists(path)) File.Delete(path); 
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                try
                {
                    byte[] bytes = ObjectToByteArray(obj);
                    fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.Write(bytes, 0, bytes.Length);
                }
                catch (Exception e)
                {

                }
                fileStream.Close();
            }
        }
        static public T Reads<T>(string path) where T : new()
        {
            T t = new T();
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    byte[] bytes = new byte[fileStream.Length];
                    if (fileStream.Read(bytes, 0, bytes.Length) > 0) t = (T)ByteArrayToObject(bytes);
                }
                catch (Exception e)
                {

                }
                fileStream.Close();
            }
            return t;
        } 

        /// <summary>
        /// Добавить данные в конец файла
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <param name="obj">данные которые нужно добавить</param>
        static public void Add(string path, object obj)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                byte[] bytes = ObjectToByteArray(obj);
                byte[] length = BitConverter.GetBytes(bytes.Length);
                try
                {

                    byte[] c = length.Concat(bytes).ToArray();
                    fileStream.Write(c, 0, c.Length);
                }
                catch (Exception e)
                {

                }
                fileStream.Close();
            }
        }
        /// <summary>
        /// Считываем файл по частям , начиная с указанной позиции
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <param name="position">смещение ( 0 - начало файла)</param>
        /// <param name="isAll"></param>
        /// <returns>возвращаем объект</returns>
        static public T ReadItem<T>(string path, ref long position) where T : new()
        {
            T t = default(T);
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                byte[] length = new byte[4];
                try
                {
                    int offset = 0;
                    for (int i = 0; i <= position; i++)
                    {
                        fileStream.Seek(offset, SeekOrigin.Begin);
                        fileStream.Read(length, 0, length.Length);

                        int blockSize = BitConverter.ToInt32(length, 0);

                        if (i == position)
                        {
                            byte[] data = new byte[blockSize];
                            fileStream.Read(data, 0, data.Length);
                            t = (T)ByteArrayToObject(data);
                        }

                        offset += (blockSize + 4);
                    }
                    position = offset;
                }
                catch (Exception e)
                {

                }
                finally
                {
                    fileStream.Close();
                }
            };
            return t;
        }
        /// <summary>
        /// Удаляет конкретный элемент в списке
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <param name="position">позиция в списке (0 - начало списка)</param>
        static public void RemoveItem(string path , long position)
        {
            FileStream newFile = new FileStream(path + ".tmp" , FileMode.Create , FileAccess.Write);
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                byte[] length = new byte[4];
                try
                {
                    long offset = 0;
                    int i = 0;

                    newFile.Seek(0, SeekOrigin.Begin);
                    while (true)
                    {
                        fileStream.Seek(offset, SeekOrigin.Begin);
                        if (fileStream.Read(length, 0, length.Length) == 0) break;
                        
                        int blockSize = BitConverter.ToInt32(length, 0);

                        if (i != position)
                        {
                            byte[] data = new byte[blockSize];
                            fileStream.Read(data, 0, blockSize);
                            byte[] concat = length.Concat(data).ToArray();
                            newFile.Write(concat, 0, concat.Length);
                        }
                        offset += (blockSize + 4);
                        i++;
                    }
                }
                catch (Exception e)
                {

                }
                finally
                {
                    fileStream.Close();
                    File.Delete(path);
                    newFile.Close();
                    File.Move(path + ".tmp", path);
                }
            };
        }
        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="path">путь к файлу</param>
        static public void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        /// <summary>
        /// Перезапись данных в файле
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <param name="obj">объект для сериализации</param>
        static public void Save(string path, object obj)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            var s = FileParser.CreateOrOpen(path);
            if (s != null)
            {
                s.Close();
                File.WriteAllBytes(path, ObjectToByteArray(obj));
            }
        }
        /// <summary>
        /// Полное считывания файла
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <returns>сериализованный объект</returns>
        static public List<T> Read<T>(string path) where T : new()
            {
            List<T> list = new List<T>();
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                byte[] length = new byte[4];
                try
                {
                    int offset = 0;
                    while (true) {
                        fileStream.Seek(offset, SeekOrigin.Begin);
                        int i = fileStream.Read(length, 0, length.Length);

                        if (i == 0) break;
                        int blockSize = BitConverter.ToInt32(length, 0);
                        byte[] data = new byte[blockSize];
                        i = fileStream.Read(data, 0, data.Length);
                        if (i == 0) break;
                        list.Add((T)ByteArrayToObject(data));
                        offset += (blockSize + 4);
                    }
                }
                catch (Exception e)
                {

                }
                finally
                {
                    fileStream.Close();
                }
            };
            return list;
        }
        /// <summary>
        /// Создает или открывает уже существующий файл
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <returns>поток</returns>
        static public FileStream CreateOrOpen(string path)
        {
            if (File.Exists(path))
            {
                return lastFile = File.Open(path, FileMode.OpenOrCreate);
            }
            else return lastFile = File.Create(path);
        }

        // Convert an object to a byte array
        static public byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        // Convert a byte array to an Object
        static public Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }
    }
}
