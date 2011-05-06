using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataVariation
{
    /// <summary>
    /// ディレクトリにある画像ファイル/Xmlファイルに対して、
    /// FaceData配列の読み書きを行うクラス
    /// </summary>
    public class FaceDataSchema : List<FaceData>
    {
        public LabelDefinition LabelDefinition;
    }
}
