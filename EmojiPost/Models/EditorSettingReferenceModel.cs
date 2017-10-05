using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyaStyle.Models
{
    /// <summary>
    /// 編集画面の設定値参照モデル
    /// </summary>
    public class EditorSettingReferenceModel : BindableBase, ISingletonModel
    {
        #region Properties

        private double _magnify;
        /// <summary>
        /// 拡縮率 を取得または設定します。
        /// </summary>
        public double Magnify
        {
            get
            {
                return this._magnify;
            }
            set
            {
                SetProperty(ref this._magnify, value);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// このクラスのインスタンスを生成する、既定のコンストラクタです。
        /// </summary>
        public EditorSettingReferenceModel()
        {
            this._magnify = 1.0d;
        }

        #endregion

    }
}
