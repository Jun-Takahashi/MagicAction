using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Action_11.Device;
using Action_11.Def;
using Action_11.Scene;

namespace Action_11.Actor
{
    class Player : GameObject
    {
        private Vector2 velocity;//Yだけ利用
        private bool isJump;//ジャンプの状態管理

        private IGameObjectMediator mediator;//ゲームオブジェクト仲介者

        //private Magic magic;

        public bool rightFlag;

        float a = 0;
        Vector2 chage;

        //private Vector2 slideModify;//移動ブロック同調用


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="gameDevice">ゲームデバイス</param>
        public Player(Vector2 position, GameDevice gameDevice, IGameObjectMediator mediator)
            : base("player_idle_1", position, 64, 64, gameDevice)
        {
            velocity = Vector2.Zero;
            isJump = true;
            this.mediator = mediator;
            //slideModify = Vector2.Zero;
        }

        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="other"></param>
        public Player(Player other)
            : this(other.position, other.gameDevice, other.mediator)
        {

        }

        /// <summary>
        /// 複製
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {   //自分と同じ型のオブジェクトを自分の情報で生成
            return new Player(this);
        }

        /// <summary>
        /// ヒット通知
        /// </summary>
        /// <param name="gameObject"></param>
        public override void Hit(GameObject gameObject)
        {
            ////ゲームオブジェクトが死亡ブロックか？
            //if (gameObject is DeathBlock)
            //{
            //    isDeadFlag = true;
            //}

            //if (gameObject is ChaseEnemy || gameObject is JumpingEnemy)
            //{
            //    Direction dir = this.CheckDirection(gameObject);

            //    if (dir != Direction.Top)
            //    {

            //        isDeadFlag = true;
            //    }
            //    else
            //    {
            //        velocity.Y = -8;
            //    }
            //}

            //if (gameObject is CheckPoint)
            //{
            //    mediator.SetRespawnPos(gameObject.GetPosition());
            //}

            //ゲームオブジェクトがブロックのオブジェクトか？
            //またはドアが開いていないg時は移動を妨げる
            if (gameObject is Block)
            {
                //プレイヤーとブロックの衝突面処理
                hitBlock(gameObject);
            }
            ////ドアが開いていない時はブロック同様移動を妨げる
            //if (gameObject is Door && !((Door)gameObject).GetStatus())
            //{
            //    //プレイヤーとブロックの衝突面処理
            //    hitBlock(gameObject);
            //}

            //移動量ブロックの移動量設定
            setSlideModify(gameObject);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if ((isJump == false) &&
                (Input.GetKeyTrigger(Keys.Space) ||
                Input.GetKeyTrigger(PlayerIndex.One, Buttons.B)))
            {
                velocity.Y = -10.0f;
                isJump = true;
            }
            else
            {
                //ジャンプ中だけ落下
                velocity.Y = velocity.Y + 0.4f;
                //落下速度制限
                velocity.Y = (velocity.Y > 16.0f) ? (16.0f) : (velocity.Y);
            }
            float speed = 4.0f;
            //横方向の移動量の計算
            velocity.X = Input.Velocity().X * speed + Input.Velocity(PlayerIndex.One).X *
                speed;

            if (velocity.X > 0.0f)
            {
                rightFlag = true;
            }
            else if (velocity.X < 0.0f) 
            {
                rightFlag = false;
            }

            if(Input.GetKeyState(Keys.Z))
            {
                chage = new Vector2(0, a--);
            }

            if(Input.GetKeyRelease(Keys.Z))
            {
                if (rightFlag == true)
                {
                    mediator.AddGameObject(new Magic(position + chage + new Vector2(40, 25), gameDevice, mediator));
                }
                if (rightFlag == false)
                {
                    mediator.AddGameObject(new leftMagic(position + chage + new Vector2(-10, 25), gameDevice, mediator));
                }
                a = 0;
            }

            //位置の計算
            position = position + velocity;

            //プレイヤーの位置を画面の中心に位置補正する
            setDisplayModify();
        }

        /// <summary>
        /// プレイヤーとブロックとの衝突面処理
        /// </summary>
        /// <param name="gameObject"></param>
        private void hitBlock(GameObject gameObject)
        {
            //当たった方向の取得
            Direction dir = this.CheckDirection(gameObject);

            //ブロックの上面と衝突
            if (dir == Direction.Top)
            {
                //プレイヤーがブロックの上に載った
                if (velocity.Y > 0.0f)
                {
                    position.Y = gameObject.getRectangle().Top - this.height;
                    velocity.Y = 0.0f;
                    isJump = false;
                }
            }
            else if (dir == Direction.Right)
            {
                position.X = gameObject.getRectangle().Right;
            }
            else if (dir == Direction.Left)
            {
                position.X = gameObject.getRectangle().Left - this.width;
            }
            else if (dir == Direction.Buttom)
            {
                position.Y = gameObject.getRectangle().Bottom;
                //ジャンプ中で、ぷろっくの下とぶつかったら上方向の移動量をなくす
                if (isJump)
                {
                    velocity.Y = 0.0f;
                }
            }
            //プレイヤーの位置を画面の中心に位置補正する
            setDisplayModify();
        }

        /// <summary>
        /// 位置補正
        /// </summary>
        private void setDisplayModify()
        {
            //中心で描画するよう補正値を設定
            gameDevice.SetDisplayModify(new Vector2(-position.X + (Screen.Width / 2 - width / 2), 0.0f));
            //Playerのx座標が画面の中心より左なら見切れてるので、Vector2.Zeroで設定しなおす
            if (position.X < Screen.Width / 2 - width / 2)
            {
                gameDevice.SetDisplayModify(Vector2.Zero);
            }

            //右端は画面2.5画面を越えたら3画面目が出るよう2画面分のx座標で補正する
            //if (position.X > Screen.Width * 2 + (Screen.Width / 2 - width / 2))
            //{
            //gameDevice.SetDisplayModify(new Vector2(
            //    -(48 * 32 - Screen.Width / 2 - width / 2) + (Screen.Width / 2 - width / 2),
            //    0.0f));
            //    gameDevice.SetDisplayModify(new Vector2(-Screen.Width * 2, 0));
            //}
            if (position.X > mediator.MapSize().X - Screen.Width / 2 - width / 2)
            {
                gameDevice.SetDisplayModify(new Vector2(-(mediator.MapSize().X - Screen.Width / 2 - width / 2) + (Screen.Width / 2 - width / 2), 0.0f));
            }
        }

        /// <summary>
        /// 移動ブロックの移動量設定
        /// </summary>
        /// <param name="gameObject"></param>
        private void setSlideModify(GameObject gameObject)
        {
            //当たった方向の取得
            Direction dir = this.CheckDirection(gameObject);

            //if ((gameObject is SlidingBlock) && (dir == Direction.Top))
            //{
            //    //一緒に移動する（GameObject型なので、SlidingBlock型にキャストして移動量を取得
            //    slideModify = ((SlidingBlock)gameObject).GetVelocity();
            //}
            ////移動ブロック以外に接触したら移動しない
            //else if (!(gameObject is SlidingBlock))
            //{
            //    slideModify = Vector2.Zero;
            //}
        }
    }
}
