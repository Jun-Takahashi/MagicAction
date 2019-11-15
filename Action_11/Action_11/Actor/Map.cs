using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Action_11.Device;
using Action_11.Util;

namespace Action_11.Actor
{
    class Map
    {
        private List<List<GameObject>>
    mapList;
        private GameDevice gameDevice;

        public Map(GameDevice gameDevice)
        {
            mapList = new List<List<GameObject>>();
            this.gameDevice = gameDevice;
        }

        private List<GameObject> addBlock(int lineCnt, string[] line)
        {
            Dictionary<string, GameObject> objectDict = new Dictionary<string, GameObject>();
            objectDict.Add("0", new Space(Vector2.Zero, gameDevice));
            objectDict.Add("1", new Block(Vector2.Zero, gameDevice));
            //ギミック用
            //objectDict.Add("2", new Pitfall(Vector2.Zero, gameDevice));//落とし穴
            //objectDict.Add("3", new CheckPoint(Vector2.Zero, gameDevice));
            //objectDict.Add("9", new DeathBlock(Vector2.Zero, gameDevice));//死亡ブロック

            List<GameObject> workList = new List<GameObject>();

            int colCnt = 0;

            //渡された1行から1つずつ作業リストに登録
            foreach (var s in line)
            {
                try
                {
                    //ディクショナリから元データを取り出し、クローン機能で複製
                    GameObject work = (GameObject)objectDict[s].Clone();
                    work.SetPosition(new Vector2(colCnt * work.GetWidth(),
                        lineCnt * work.GetHeight()));
                    workList.Add(work);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                //列カウンタを増やす
                colCnt += 1;
            }
            return workList;
        }

        public void Load(string filename, string path = "./")
        {
            CSVReader csvReader = new CSVReader();
            csvReader.Read(filename, path);

            //List<string[]>型で取得
            var date = csvReader.GetDate();

            //1行ごとmapListに追加していく
            for (int lineCnt = 0; lineCnt < date.Count(); lineCnt++)
            {
                mapList.Add(addBlock(lineCnt, date[lineCnt]));
            }
        }

        public void Unload()
        {
            mapList.Clear();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var list in mapList)
            {
                foreach (var obj in list)
                {
                    if (obj is Space)
                    {
                        continue;
                    }

                    //更新
                    obj.Update(gameTime);
                }
            }
        }

        public void Hit(GameObject gameObject)
        {
            Point work = gameObject.getRectangle().Location;

            int x = work.X / 64;
            int y = work.Y / 64;

            if (x < 1)
            {
                x = 1;
            }
            if (y < 1)
            {
                y = 1;
            }

            Range yRange = new Range(0, mapList.Count() - 1);
            Range xRange = new Range(0, mapList[0].Count() - 1);

            for (int row = y - 1; row <= (y + 1); row++)
            {
                for (int col = x - 1; col <= (x + 1); col++)
                {
                    if (xRange.IsOutOfRange(col) || yRange.IsOutOfRange(row))
                    {
                        continue;
                    }
                    GameObject obj = mapList[row][col];

                    if (obj is Space)
                    {
                        continue;
                    }

                    if (obj.IsCollision(gameObject))
                    {
                        gameObject.Hit(obj);
                    }
                }
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            foreach (var list in mapList)
            {
                foreach (var obj in list)
                {
                    obj.Draw(renderer);
                }
            }
        }

        public int GetWidth()
        {
            int col = mapList[0].Count;
            int width = col * mapList[0][0].GetWidth();
            return width;
        }

        public int GetHeight()
        {
            int row = mapList.Count;
            int height = row * mapList[0][0].GetHeight();
            return height;
        }

    }
}
