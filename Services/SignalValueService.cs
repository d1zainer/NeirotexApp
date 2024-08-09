using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;



namespace NeirotexApp.Services
{
    public class SignalValueService
    {
        private double _mean; //мат ожидание
        private double _min; //мин значение
        private double _max; //макс значение
        private long _totalCount; //колличесвто символов
        private double _sum; // сумма

      
        private const int _blockSize = 100;

        private int _samplingRate = 0; // Частота дискретизации

        


        public SignalValueService(int samplingRate)
        {
            _mean = 0;
            _min = double.NaN;
            _max = double.NaN;
            _sum = 0;
            _totalCount = 0;
            _samplingRate = samplingRate;
            
        }

        /// <summary>
        /// считывает файл 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task ProcessFileAsync(string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    var buffer = new double[_blockSize];

                    bool isFirstBlock = true;

                    while (fileStream.Position < fileStream.Length)
                    {
                        int bytesRead = ReadBlock(binaryReader, buffer);

                        if (isFirstBlock && bytesRead > 0)
                        {
                            // Инициализация min и max значений первым прочитанным значением
                            _min = _max = buffer[0];
                            isFirstBlock = false;
                        }

                        CalculateStatistics(buffer, bytesRead);
                    }
                }
            }
            catch (Exception ex)
            {
              
                throw new Exception($"Ошибка при обработке файла {filePath}: {ex.Message}");
               
            }
        }

        /// <summary>
        /// возращает результаты
        /// </summary>
        /// <returns></returns>
        public (double Mean, double Min, double Max) GetResults()
        {
            _mean = _totalCount > 0 ? _sum / _totalCount : 0;
            return (_mean, _min, _max);
        }

        /// <summary>
        /// считает блок в 100 символов, возращает число обработ символов
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private int ReadBlock(BinaryReader reader, double[] buffer)
        {
            int index = 0;
            try
            {
                while (index < buffer.Length && reader.BaseStream.Position < reader.BaseStream.Length)
                {                  
                    buffer[index++] = reader.ReadDouble();
                }
            }
            catch (EndOfStreamException)
            {
                throw new Exception();
            }
            return index; 
        }

        /// <summary>
        /// считает значения
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count"></param>
        private void CalculateStatistics(double[] data, int count)
        {
            if (count == 0) return;

            _sum += data.Take(count).Sum();
         
            _totalCount += count;
           _min = Math.Min(_min, data.Take(count).Min());
           _max = Math.Max(_max, data.Take(count).Max());    
        }



    }
}
