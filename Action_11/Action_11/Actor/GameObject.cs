using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action_11.Device;
using Microsoft.Xna.Framework;

namespace Action_11.Actor
{
    /// <summary>
    /// 当たった時の方向
    /// </summary>
    enum Direction
    {
        //上　下　　左　右
        Top, Buttom, Left, Right
    };

    /// <summary>
    /// 抽象ゲームオブジェクトクラス
    /// </summary>
    abstract class GameObject : ICloneable
    {
        protected string name;
        protected Vector2 position;
        protected int width;
        protected int height;
        protected bool isDeadFlag = false;
        protected GameDevice gameDevice;
        protected GameObjectID id =
            GameObjectID.NONE;//個別に見分ける（デフォルト値は識別なし）

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="gameDevice"></param>
        public GameObject(string name, Vector2 position, int width, int height, GameDevice gameDevice)
        {
            this.name = name;
            this.position = position;
            this.width = width;
            this.height = height;
            this.gameDevice = gameDevice;
        }

        /// <summary>
        /// 位置の決定
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        /// <summary>
        /// 位置の取得
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition()
        {
            return position;
        }

        /// <summary>
        /// オブジェクト幅の取得
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            return width;
        }

        /// <summary>
        /// オブジェクトの高さの取得
        /// </summary>
        /// <returns></returns>
        public int GetHeight()
        {
            return height;
        }

        //抽象メソッド
        public abstract object Clone();
        public abstract void Update(GameTime gameTime);
        public abstract void Hit(GameObject gameObject);

        //仮想メソッド
        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position + gameDevice.GetDisplayModify());
        }

        /// <summary>
        /// 死んでいるか？
        /// </summary>
        /// <returns></returns>
        public bool IsDead()
        {
            return isDeadFlag;
        }

        /// <summary>
        /// 当たり判定用、矩形情報の取得
        /// </summary>
        /// <returns></returns>
        public Rectangle getRectangle()
        {
            //矩形の生成
            Rectangle area = new Rectangle();

            //位置と幅、高さを設定
            area.X = (int)position.X;
            area.Y = (int)position.Y;
            area.Height = height;
            area.Width = width;

            return area;
        }

        /// <summary>
        /// 矩形同士の当たり判定
        /// </summary>
        /// <param name="otherObj"></param>
        /// <returns></returns>
        public bool IsCollision(GameObject otherObj)
        {
            return this.getRectangle().Intersects(otherObj.getRectangle());
        }

        public Direction CheckDirection(GameObject otherObj)
        {
            Point thisCenter = this.getRectangle().Center;
            Point otherCenter = otherObj.getRectangle().Center;

            Vector2 dir =
                new Vector2(thisCenter.X, thisCenter.Y) -
                new Vector2(otherCenter.X, otherCenter.Y);

            if (Math.Abs(dir.X) > Math.Abs(dir.Y))
            {
                if (dir.X > 0)
                {
                    return Direction.Right;
                }
                return Direction.Left;
            }

            if (dir.Y > 0)
            {
                return Direction.Buttom;
            }

            return Direction.Top;
        }

        /// <summary>
        /// 当たった面からの位置補正
        /// </summary>
        /// <param name="other"></param>
        public virtual void CorrectPosition(GameObject other)
        {
            //当たった面の取得
            Direction dir = this.CheckDirection(other);

            if (dir == Direction.Top)
            {
                position.Y = other.getRectangle().Top - this.height;
            }
            else if (dir == Direction.Right)
            {
                position.X = other.getRectangle().Right;
            }
            else if (dir == Direction.Left)
            {
                position.X = other.getRectangle().Left - this.width;
            }
            else if (dir == Direction.Buttom)
            {
                position.Y = other.getRectangle().Bottom;
            }

        }

        /// <summary>
        /// 識別用IDの取得
        /// </summary>
        /// <returns></returns>
        public GameObjectID GetID()
        {
            return id;
        }

        /// <summary>
        /// 識別IDの設定
        /// </summary>
        /// <param name="id"></param>
        public void SetID(GameObjectID id)
        {
            this.id = id;
        }

    }
}
