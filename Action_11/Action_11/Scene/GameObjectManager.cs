using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action_11.Actor;
using Action_11.Device;
using Microsoft.Xna.Framework;

namespace Action_11.Scene
{
    class GameObjectManager : IGameObjectMediator
    {
        private List<GameObject> gameObjectList;//プレイヤーグループ
        private List<GameObject> addGameObjects;//追加するキャラクターリスト

        private Map map;//マップ


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameObjectManager()
        {
            Initialize();//初期化
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            //リストの実体があればクリアし、なければ実体生成
            if (gameObjectList != null)
            {
                gameObjectList.Clear();
            }
            else
            {
                gameObjectList = new List<GameObject>();
            }

            if (addGameObjects != null)
            {
                addGameObjects.Clear();
            }
            else
            {
                addGameObjects = new List<GameObject>();
            }
        }

        /// <summary>
        /// ゲームオブジェクトの追加
        /// </summary>
        /// <param name="gameObject"></param>
        public void Add(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }
            addGameObjects.Add(gameObject);
        }

        /// <summary>
        /// マップの追加
        /// </summary>
        /// <param name="map"></param>
        public void Add(Map map)
        {
            if (map == null)
            {
                return;
            }
            this.map = map;
        }

        /// <summary>
        /// マップとの当たり判定
        /// </summary>
        private void hitToMap()
        {
            if (map == null)
            {
                return;
            }
            //すべてのオブジェクトマップとのヒット通知
            foreach (var obj in gameObjectList)
            {
                map.Hit(obj);
            }
        }

        /// <summary>
        /// ゲームオブジェクトとのヒット通知
        /// </summary>
        private void hitToGameObject()
        {
            //ゲームオブジェクとリストを繰り返し
            foreach (var c1 in gameObjectList)
            {
                //同じゲームオブジェクトリストを繰り返し
                foreach (var c2 in gameObjectList)
                {
                    if (c1.Equals(c2) || c1.IsDead() || c2.IsDead())
                    {
                        //同じキャラか、キャラが死んでたら次へ
                        continue;
                    }

                    //衝突判定
                    if (c1.IsCollision(c2))
                    {
                        //ヒット通知
                        c1.Hit(c2);
                        c2.Hit(c1);
                    }
                }
            }
        }

        /// <summary>
        /// 死亡キャラをリストから削除
        /// </summary>
        private void removeDeadCharacters()
        {
            gameObjectList.RemoveAll(c => c.IsDead());
        }

        public void Update(GameTime gameTime)
        {
            //全キャラ更新 
            foreach (var c in gameObjectList)
            {
                c.Update(gameTime);
            }

            //キャラクタの追加
            foreach (var c in addGameObjects)
            {
                gameObjectList.Add(c);
            }

            //追加終了後、追加リストはクリア
            addGameObjects.Clear();

            //当たり判定
            hitToMap();
            hitToGameObject();

            //死亡フラグが立っているキャラをすべて削除
            removeDeadCharacters();
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            foreach (var c in gameObjectList)
            {
                c.Draw(renderer);
            }
        }

        /// <summary>
        /// ゲームオブジェクトの追加
        /// </summary>
        /// <param name="gameObject"></param>
        public void AddGameObject(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }
            addGameObjects.Add(gameObject);
        }

        /// <summary>
        /// プレイヤーの取得
        /// </summary>
        /// <returns></returns>
        public GameObject GetPlayer()
        {
            GameObject find = gameObjectList.Find(c => c is Player);
            if (find != null && !find.IsDead())
            {
                return find;
            }
            return null;//プレイヤーがいない
        }

        public bool IsPlayerDead()
        {
            GameObject find = gameObjectList.Find(c => c is Player);

            return (find == null || find.IsDead());
        }


        public GameObject GetGameObject(GameObjectID id)
        {
            //引数のidがリストのGameObjectにあるか探す
            GameObject find = gameObjectList.Find(c => c.GetID() == id);

            //発見したオブジェクトがnullでない時かつ、死んでないとき
            if (find != null && !find.IsDead())
            {
                return find;
            }
            return null;//発見できなかった
        }

        public List<GameObject> GetGameObjectList(GameObjectID id)
        {
            //引数で指定されたオブジェクトを複数探す
            List<GameObject> list = gameObjectList.FindAll(c => c.GetID() == id);
            //発見したオブジェクトが生きているか確認
           　List<GameObject> aliveList = new List<GameObject>();//生存リスト
            foreach (var c in list)
            {
                //生きていたらリストに追加
                if (!c.IsDead())
                {
                    //生存リストに登録
                    aliveList.Add(c);
                }
            }
            //生存リストを返す
            return aliveList;
        }

        /// <summary>
        /// マップ全体のサイズ取得
        /// </summary>
        /// <returns></returns>
        public Vector2 MapSize()
        {
            return new Vector2(
                map.GetWidth(),
                map.GetHeight());
        }

        private Vector2 respawnPos;
        public Vector2 RespawnPos
        {
            get { return respawnPos; }
            set { respawnPos = value; }
        }

        public void SetRespawnPos(Vector2 respawnPos)
        {
            RespawnPos = respawnPos;
        }

    }
}
