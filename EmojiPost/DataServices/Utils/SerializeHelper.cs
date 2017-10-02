using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace EmojiPost.DataServices.Utils
{
    /// <summary>
    /// シリアライズヘルパー
    /// </summary>
    public static class SerializeHelper
    {
        /// <summary>
        /// 指定されたオブジェクトをシリアライズした文字列を返します。
        /// </summary>
        /// <typeparam name="T">シリアライズするオブジェクト型。<see cref="DataContractAttribute"/>であること。</typeparam>
        /// <param name="obj">シリアライズするオブジェクト。</param>
        /// <returns>オブジェクトをシリアライズした文字列</returns>
        public static string Serialize<T>(T obj) where T : class
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// 指定された文字列をデシリアライズしてオブジェクトを返します。
        /// </summary>
        /// <typeparam name="T">デシリアライズするオブジェクト型。<see cref="DataContractAttribute"/>であること。</typeparam>
        /// <param name="str">デシリアライズする文字列。</param>
        /// <returns>文字列をデシリアライズしたオブジェクト</returns>
        public static T Deserialize<T>(string str) where T : class
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var reader = XmlReader.Create(new StringReader(str)))
            {
                return serializer.ReadObject(reader) as T;
            }
        }

        #region Constructor

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static SerializeHelper()
        {
        }

        #endregion
    }
}
