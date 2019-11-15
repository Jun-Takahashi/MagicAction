using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action_11.Device;
using Microsoft.Xna.Framework;


namespace Action_11.Actor
{
    class Block : GameObject
    {
        public Block(Vector2 position, GameDevice gameDevice)
            : base("Block", position, 64, 64, gameDevice)
        { }

        public Block(Block other)
            : this(other.position, other.gameDevice)
        { }
        public override object Clone()
        {
            return new Block(this);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Hit(GameObject gameObject)
        {
        }
    }
}
