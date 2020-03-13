using System;
using System.IO;
using System.Threading;

namespace RetroLauncher.ServiceTools.Download
{
    public class ThrottledStream : Stream
    {
        /// <summary>
        /// Константа, используемая для указания бесконечного числа байтов, которые могут передаваться в секунду.
        /// </summary>
        public const long Infinite = 0;

        #region Private members
        /// <summary>
        /// Базовый поток
        /// </summary>
        private Stream _baseStream;

        /// <summary>
        /// Количество байтов, которые были переданы с момента последнего чтения
        /// </summary>
        private long _byteCount;

        /// <summary>
        /// Время начала в миллисекундах последнего чтения.
        /// </summary>
        private long _start;

        /// <summary>
        /// Максимальное количество байтов в секунду, которое может быть передано через основной поток.
        /// </summary>
        private long _maximumBytesPerSecond;
        #endregion

        #region Properties

        protected long CurrentMilliseconds
        {
            get
            {
                return Environment.TickCount;
            }
        }

        /// <summary>
        /// Получить или задать максимальное количество байтов в секунду, которое может быть передано через базовый поток.
        /// </summary>
        /// <value>The maximum bytes per second.</value>
        public long MaximumBytesPerSecond
        {
            get
            {
                return _maximumBytesPerSecond;
            }
            set
            {
                if (MaximumBytesPerSecond != value)
                {
                    _maximumBytesPerSecond = value;
                    Reset();
                }
            }
        }

        /// <summary>
        /// Значение указывает поддерживает ли текущий поток чтение
        /// </summary>
        /// <returns></returns>
        public override bool CanRead
        {
            get
            {
                return _baseStream.CanRead;
            }
        }

        /// <summary>
        /// Значение указывает поддерживает ли текущий поток поиск
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public override bool CanSeek
        {
            get
            {
                return _baseStream.CanSeek;
            }
        }

        /// <summary>
        /// Значение указывает поддерживает ли текущий поток запись
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public override bool CanWrite
        {
            get
            {
                return _baseStream.CanWrite;
            }
        }

        /// <summary>
        /// Длина потока в байтах
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <exception cref="T:System.NotSupportedException">Базовый поток не поддерживает поиск. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Методы были вызваны после того, как поток был закрыт. </exception>
        public override long Length
        {
            get
            {
                return _baseStream.Length;
            }
        }

        /// <summary>
        /// Значение указывает позицию в текущем потоке
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <exception cref="T:System.IO.IOException">Ошибка ввода/вывода. </exception>
        /// <exception cref="T:System.NotSupportedException">Базовый поток не поддерживает поиск.</exception>
        /// <exception cref="T:System.ObjectDisposedException">Методы были вызваны после того, как поток был закрыт. </exception>
        public override long Position
        {
            get
            {
                return _baseStream.Position;
            }
            set
            {
                _baseStream.Position = value;
            }
        }
        #endregion

        #region Ctor

        public ThrottledStream(Stream baseStream)
            : this(baseStream, ThrottledStream.Infinite)
        {
            // Nothing todo.
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        /// <param name="baseStream">Базовый поток</param>
        /// <param name="maximumBytesPerSecond">Максимальное количество байтов в секунду, которое может быть передано через основной поток.</param>
        public ThrottledStream(Stream baseStream, long maximumBytesPerSecond)
        {
            if (baseStream == null)
            {
                throw new ArgumentNullException("baseStream");
            }

            if (maximumBytesPerSecond < 0)
            {
                throw new ArgumentOutOfRangeException("maximumBytesPerSecond",
                    maximumBytesPerSecond, "Максимальное количество байтов в секунду не может быть отрицательным.");
            }

            _baseStream = baseStream;
            _maximumBytesPerSecond = maximumBytesPerSecond;
            _start = CurrentMilliseconds;
            _byteCount = 0;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Очищает все буферы для этого потока и приводит к записи любых буферизованных данных
        /// </summary>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        public override void Flush()
        {
            _baseStream.Flush();
        }

        /// <summary>
        /// Считывает последовательность байтов из текущего потока
        /// </summary>
        /// <param name="buffer">Массив байтов для чтения</param>
        /// <param name="offset">Смещение байта в буфере, с которого начинается сохранение данных</param>
        /// <param name="count">Количество байтов для чтения из текущего потока.</param>
        /// <returns>Общее количество байтов, считанных в буфер </returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            Throttle(count);

            return _baseStream.Read(buffer, offset, count);
        }

        /// <summary>
        /// Устанавливить позицию в текущем потоке.
        /// </summary>
        /// <param name="offset">Смещение байта относительно источника.</param>
        /// <param name="origin">Контрольная точка для получения новой позиции.</param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _baseStream.Seek(offset, origin);
        }

        /// <summary>
        /// Устанавливает длину текущего потока.
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            _baseStream.SetLength(value);
        }

        /// <summary>
        /// Записывает последовательность байтов в текущий поток
        /// </summary>
        /// <param name="buffer">Массив байтов, которые копируются из буфера в текущий поток.</param>
        /// <param name="offset">Смещение байта в буфере, с которого начинается сохранение данных</param>
        /// <param name="count">Количество байтов для записи в текущий поток.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            Throttle(count);

            _baseStream.Write(buffer, offset, count);
        }


        public override string ToString()
        {
            return _baseStream.ToString();
        }
        #endregion

        #region Protected methods
        /// <summary>
        /// Регулировка для указанного размера буфера в байтах.
        /// </summary>
        /// <param name="bufferSizeInBytes"></param>
        protected void Throttle(int bufferSizeInBytes)
        {
            // буфер точно не пустой
            if (_maximumBytesPerSecond <= 0 || bufferSizeInBytes <= 0)
            {
                return;
            }

            _byteCount += bufferSizeInBytes;
            long elapsedMilliseconds = CurrentMilliseconds - _start;

            if (elapsedMilliseconds > 0)
            {
                // Рассчитываем скорость
                long bps = _byteCount * 1000L / elapsedMilliseconds;

                // Если скорость превышает максимальную, то ограничиваем
                if (bps > _maximumBytesPerSecond)
                {
                    // время для сна
                    long wakeElapsed = _byteCount * 1000L / _maximumBytesPerSecond;
                    int toSleep = (int)(wakeElapsed - elapsedMilliseconds);

                    if (toSleep > 1)
                    {
                        try
                        {
                            // Время сна больше миллисекунды, так что спать.
                            Thread.Sleep(toSleep);
                        }
                        catch (ThreadAbortException)
                        {
                            // сюда надо что-то написать
                        }

                        Reset();
                    }
                }
            }
        }

        /// <summary>
        /// Сброс счета до 0 и сброс времени начала к текущему времени.
        /// </summary>
        protected void Reset()
        {
            long difference = CurrentMilliseconds - _start;

            // Сбрасывает счетчик, если разница больше 1 секунды
            if (difference > 1000)
            {
                _byteCount = 0;
                _start = CurrentMilliseconds;
            }
        }
        #endregion
    }
}
