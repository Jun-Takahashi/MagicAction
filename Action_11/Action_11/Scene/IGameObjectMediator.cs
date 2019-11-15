using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action_11.Actor;
using Microsoft.Xna.Framework;


namespace Action_11.Scene
{
    interface IGameObjectMediator
    {
        //ゲームオブジェクト追加
        void AddGameObject(GameObject gameObject);
        //プレイヤー取得
        GameObject GetPlayer();
        //プレイヤーが死んでいるかどうか
        bool IsPlayerDead();
        //特定のオブジェクトを取得する
        GameObject GetGameObject(GameObjectID id);

        //複数のゲームオブジェクトの取得
        List<GameObject> GetGameObjectList(GameObjectID id);

        //マップ全体のサイズ取得
        Vector2 MapSize();

        void SetRespawnPos(Vector2 respawnPos);
    }
}
