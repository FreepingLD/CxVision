using LJV7_DllSampleAll;
using System.Collections.Generic;
using System.Linq;

namespace LJV7_IF_Test
{
    /// <summary>
    /// リングバッファクラス
    /// <http://gushwell.ifdef.jp/etude/TailCommand.html>のコピペ
    /// </summary>
    public class RingBuffer<T> : IEnumerable<T>
    {
        #region フィールド
        private int _size;          // サイズ
        private T[] _buffer;        // バッファ
        private bool[] _existence;  // 存在確認フラグ
        private int _writeIndex;    // 書き終わった位置
        private int _readIndex;     // これから読み込む位置
        private object syncObject;  // 排他制御用オブジェクト
		private LJV7IF_PROFILE_INFO _info; 
        #endregion

        #region プロパティ
        /// <summary>
        /// 要素数の取得
        /// </summary>
        public int Count
        {
            get { return _existence.Count(b => b == true); }
        }

        /// <summary>
        /// バッファ
        /// </summary>
        public T[] Buffer
        {
            get { lock (syncObject) return _buffer; }
        }

		/// <summary>
		/// プロファイル情報
		/// </summary>
		public LJV7IF_PROFILE_INFO Info
		{
			get { lock (syncObject) return _info; }
		}
        #endregion

        #region メソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="size">保持したデータの数</param>
        public RingBuffer(int size)
        {
            // フィールドの初期化
            _size = size;
            _buffer = new T[size];
            _existence = new bool[size];
            _writeIndex = -1;
            _readIndex = 0;
            syncObject = new object();
        }

        /// <summary>
        /// 次のバッファインデックス
        /// </summary>
        /// <param name="ix">現在のインデックス</param>
        /// <returns>次のインデックス</returns>
        private int NextIndex(int ix)
        {
            return ++ix % _size;
        }

        /// <summary>
        /// 要素の追加
        /// </summary>
        /// <param name="value">追加要素</param>
        public void Add(T value)
        {
            lock (syncObject)
            {
                _writeIndex = NextIndex(_writeIndex);
                _buffer[_writeIndex] = value;
                if (_existence[_readIndex] && _writeIndex == _readIndex)
                    _readIndex = NextIndex(_readIndex);
                _existence[_writeIndex] = true;
            }
        }

		public void SetProfileInfo(LJV7IF_PROFILE_INFO info)
		{
			lock (syncObject)
			{
				_info = info;
			}
		}

        /// <summary>
        /// 要素のクリア
        /// </summary>
        public void Clear()
        {
            lock (syncObject)
            {
                _writeIndex = -1;
                _readIndex = 0;
                for (int i = 0; i < _size; i++)
                    _existence[i] = false;
            }
        }

        /// <summary>
        /// 要素の取得
        /// </summary>
        /// <returns>要素</returns>
        public IEnumerator<T> GetEnumerator()
        {
            lock (syncObject)
            {
                while (Exists())
                {
                    yield return Get();
                }
            }
        }

        /// <summary>
        /// 要素の取得
        /// </summary>
        /// <returns>要素</returns>
        private T Get()
        {
            if (!Exists())
                return default(T);//バッファにデータがない場合
            T val = _buffer[_readIndex];
            _existence[_readIndex] = false;
            _readIndex = NextIndex(_readIndex);
            return val;
        }

        /// <summary>
        /// 要素の存在確認
        /// </summary>
        /// <returns>true：ある、false：ない</returns>
        private bool Exists()
        {
            return _existence.Any(b => b == true);
        }

        /// <summary>
        /// 要素の取得
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}
