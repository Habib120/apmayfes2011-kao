using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataVariation
{
    public interface IDataVariator
    {
        /// <summary>
        /// バリエーションの名前
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 説明
        /// </summary>
        string Description { get; }
        /// <summary>
        /// バリエーションの識別ID(一意である必要がある)
        /// </summary>
        string ID { get; set; }

        /// <summary>
        /// プロパティを入力するためのフォームを取得する
        /// </summary>
        /// <returns></returns>
        VariatorForm GetForm();

        /// <summary>
        /// フォームへの入力結果を反映させる
        /// </summary>
        /// <param name="form"></param>
        void Bind(VariatorForm form);

        /// <summary>
        /// プロパティに基づいて引数で指定された顔画像
        /// に変形を加え、バリエーションを複数作成する
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        IEnumerable<FaceData> GetVariation(FaceData src);

    }
}
