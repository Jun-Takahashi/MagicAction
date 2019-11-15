using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action_11.Actor;
using Action_11.Device;
using Action_11.Scene;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Action_11.Util
{
    class GameObjectCSVParser
    {
        private CSVReader csvReader;//CSV読み込み用オブジェクト
        private List<GameObject> gameObjects;//ゲームオブジェクトのリスト
        private IGameObjectMediator mediator;

        //デリケート宣言（メソッドを変数に保存するための型宣言）
        //戻り値の型がGameObject,引数はList<string>のメソッドを
        //保存できるiFunction方を宣言
        private delegate GameObject iFunction(List<string> data);

        //文字列とiFunction肩をディクショナリで保存
        private Dictionary<string, iFunction> functionTable;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameObjectCSVParser(IGameObjectMediator mediator)
        {
            //仲介者の設定
            this.mediator = mediator;
            //CSV読み込みの実体生成
            csvReader = new CSVReader();
            //ゲームオブジェクトリストの実体生成
            gameObjects = new List<GameObject>();
            //文字列とメソッドを保存するディクショナリの実体生成
            functionTable = new Dictionary<string, iFunction>();
            //ディクショナリにデータを追加
            //文字列はクラス名の文字列と実行用メソッド名
            //functionTable.Add("SlidingBlock", NewSlidingBlock);
            functionTable.Add("Block", NewBlock);
            //functionTable.Add("ChaseEnemy", NewChaseEnemy);//追いかけてくる敵
            //functionTable.Add("JumpingEnemy", NewJumpingEnemy);
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<GameObject> Parse(string filename, string path = "./")
        {
            //リストおクリア
            gameObjects.Clear();

            //CSV読み込み
            csvReader.Read(filename, path);
            //List<string[]>でCSVデータを取得
            var data = csvReader.GetDate();

            //1行ごと解析
            foreach (var line in data)
            {
                //1行目が#の時はコメント行だったら次へ
                if (line[0] == "#")
                {
                    continue;
                }
                //1行目が空文字だった場合もコメントとして次へ
                if (line[0] == "")
                {
                    continue;
                }

                //空白文字削除処理
                var temp = line.ToList();//配列からListへ変換
                temp.RemoveAll(s => s == "");//List内にある空文字を削除

                //ゲームオブジェクトリストに解析後作られたゲームオブジェクトを追加
                gameObjects.Add(functionTable[line[0]](temp));
            }

            return gameObjects;
        }

        /// <summary>
        /// 移動ブロックの解析と生成
        /// </summary>
        /// <param name="data"></param>
        ///// <returns></returns>
        //private SlidingBlock NewSlidingBlock(List<string> data)
        //{
        //    //読み込んだ行の列数が正しいかチェック。おかしければAssertでエラー渓谷
        //    Debug.Assert(
        //        (data.Count == 5) || (data.Count == 6) || (data.Count == 7 || (data.Count == 8)),
        //        "CSVデータを確認してください。");

        //    if (data.Count == 5)//移動なし版
        //    {
        //        return new SlidingBlock(
        //            new Vector2(float.Parse(data[1]), float.Parse(data[2])) *
        //            32,
        //            new Vector2(float.Parse(data[3]), float.Parse(data[4])) *
        //            32,
        //            GameDevice.Instance());

        //    }

        //    if (data.Count == 6)//移動なし版
        //    {
        //        SlidingBlock slidingBlock = new SlidingBlock(
        //            new Vector2(float.Parse(data[1]), float.Parse(data[2])) *
        //            32,
        //            new Vector2(float.Parse(data[3]), float.Parse(data[4])) *
        //            32,
        //            GameDevice.Instance());
        //        //GameObjectID id = (GameObjectID)Enum.Parse(typeof(GameObjectID), data[5]);
        //        slidingBlock.SetID(stringToGameObjectID_Enum(data[5]));
        //        return slidingBlock;

        //    }

        //    if (data.Count == 7)//移動量あり版
        //    {
        //        return new SlidingBlock(
        //            new Vector2(float.Parse(data[1]), float.Parse(data[2])) *
        //            32,
        //            new Vector2(float.Parse(data[3]), float.Parse(data[4])) *
        //            32,
        //            new Vector2(float.Parse(data[5]), float.Parse(data[6])),
        //            GameDevice.Instance());

        //    }
        //    if (data.Count == 8)//移動量あり版
        //    {
        //        SlidingBlock slidingBlock = new SlidingBlock(
        //            new Vector2(float.Parse(data[1]), float.Parse(data[2])) *
        //            32,
        //            new Vector2(float.Parse(data[3]), float.Parse(data[4])) *
        //            32,
        //            new Vector2(float.Parse(data[5]), float.Parse(data[6])),

        //            GameDevice.Instance());
        //        //GameObjectID id = (GameObjectID)Enum.Parse(typeof(GameObjectID), data[7]);
        //        slidingBlock.SetID(stringToGameObjectID_Enum(data[7]));
        //        return slidingBlock;

        //    }
        //    return null;
        //}

        /// <summary>
        /// 通常ブロックの解析と生成
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Block NewBlock(List<string> data)
        {
            Debug.Assert(
                (data.Count == 3),
                "CSVデータを確認してください。");

            return new Block(
                new Vector2(float.Parse(data[1]), float.Parse(data[2])) * 32,
                GameDevice.Instance());
        }

        /// <summary>
        /// 追いかけてくる敵の生成
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        //private ChaseEnemy NewChaseEnemy(List<string> data)
        //{
        //    Debug.Assert(
        //        (data.Count == 3) || (data.Count == 4),
        //        "CSVデータを確認してください。");

        //    if (data.Count == 3)
        //    {
        //        return new ChaseEnemy(
        //            new Vector2(float.Parse(data[1]), float.Parse(data[2])) * 32,
        //            GameDevice.Instance(),
        //            mediator);
        //    }
        //    else if (data.Count == 4)
        //    {
        //        ChaseEnemy enemy = new ChaseEnemy(
        //            new Vector2(float.Parse(data[1]), float.Parse(data[2])) * 32,
        //            GameDevice.Instance(),
        //            mediator);

        //        //GameObjectID id = (GameObjectID)Enum.Parse(typeof(GameObjectID), data[3]);
        //        enemy.SetID(stringToGameObjectID_Enum(data[3]));

        //        return enemy;
        //    }
        //    return null;
        //}

        /// <summary>
        /// ジャンプする敵の生成
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        //private JumpingEnemy NewJumpingEnemy(List<string> data)
        //{
        //    Debug.Assert(
        //        (data.Count == 3) || (data.Count == 4),
        //        "CSVデータを確認してください。");

        //    if (data.Count == 3)
        //    {
        //        return new JumpingEnemy(
        //            new Vector2(float.Parse(data[1]), float.Parse(data[2])) * 32,
        //            GameDevice.Instance(),
        //            mediator);
        //    }
        //    else if (data.Count == 4)
        //    {
        //        JumpingEnemy enemy = new JumpingEnemy(new Vector2(float.Parse(data[1]), float.Parse(data[2])) * 32,
        //            GameDevice.Instance(),
        //            mediator);

        //        enemy.SetID(stringToGameObjectID_Enum(data[3]));

        //        return enemy;
        //    }
        //    return null;
        //}

        /// <summary>
        /// （上級）Enumクラスを利用して、GameObjectIDを取得
        /// </summary>
        /// <param name="stringID"></param>
        /// <returns></returns>
        private GameObjectID stringToGameObjectID_Enum(string stringID)
        {
            GameObjectID id;
            Debug.Assert(
                Enum.TryParse(stringID, false, out id) &&
                Enum.IsDefined(typeof(GameObjectID), id),
                "CSVデータのIDと列挙型の名前が合いません");

            return (GameObjectID)Enum.Parse(typeof(GameObjectID), stringID);
        }

    }
}
