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
    class leftMagic : GameObject
    {
        private IGameObjectMediator mediator;
        private Vector2 velocity;

        public leftMagic(Vector2 position, GameDevice gameDevice, IGameObjectMediator mediator)
            : base("ice_magic_l", position, 32, 32, gameDevice)
        {
            this.mediator = mediator;
            velocity = new Vector2(0.5f, 0);

        }

        public leftMagic(leftMagic other)
            : this(other.position, other.gameDevice, other.mediator)
        {

        }

        public override object Clone()
        {
            return new leftMagic(this);
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Block)
            {
                isDeadFlag = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            position -= velocity * 20;

        }
    }
}
