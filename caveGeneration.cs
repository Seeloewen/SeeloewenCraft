using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class CaveComponent
    {
        public wndGame wndGame;
        public List<StructureComponent> structureComponents = new List<StructureComponent>();
        public List<BorderComponent> borderComponents = new List<BorderComponent>();
        public int xOffset = 0;
        public int yOffset = 0;
        public string previousDirection;

        public CaveComponent(int xOffset, int yOffset, Chunk chunk, wndGame wndGame, string previousDirection, string chunkDirection)
        {
            this.wndGame = wndGame;
            this.previousDirection = previousDirection;
            this.xOffset = xOffset;
            this.yOffset = yOffset;

            int dirSpecificXfactor = 1;
            int dirSpecificYfactor = 1;




            //structureComponents.Add(new StructureComponent(wndGame, xOffset * dirSpecificXfactor, yOffset * dirSpecificYfactor, new BedrockItem(wndGame, 0).GenerateBlock(xOffset, yOffset, chunk)));
            structureComponents.Add(new StructureComponent(wndGame, xOffset * dirSpecificXfactor, yOffset + (1 * dirSpecificYfactor), new AirBlock(wndGame, xOffset, yOffset, chunk, null)));
            structureComponents.Add(new StructureComponent(wndGame, xOffset * dirSpecificXfactor, yOffset + (2 * dirSpecificYfactor), new AirBlock(wndGame, xOffset, yOffset, chunk, null)));
            structureComponents.Add(new StructureComponent(wndGame, xOffset + (1 * dirSpecificXfactor), yOffset + (1 * dirSpecificYfactor), new AirBlock(wndGame, xOffset, yOffset, chunk, null)));
            structureComponents.Add(new StructureComponent(wndGame, xOffset + (1 * dirSpecificXfactor), yOffset * dirSpecificYfactor, new AirBlock(wndGame, xOffset, yOffset, chunk, null)));
            structureComponents.Add(new StructureComponent(wndGame, xOffset + (1 * dirSpecificXfactor), yOffset + (2 * dirSpecificYfactor), new AirBlock(wndGame, xOffset, yOffset, chunk, null)));
            structureComponents.Add(new StructureComponent(wndGame, xOffset + (1 * dirSpecificXfactor), yOffset + (3 * dirSpecificYfactor), new AirBlock(wndGame, xOffset, yOffset, chunk, null)));
            structureComponents.Add(new StructureComponent(wndGame, xOffset + (2 * dirSpecificXfactor), yOffset * dirSpecificYfactor, new AirBlock(wndGame, xOffset, yOffset, chunk, null)));
            structureComponents.Add(new StructureComponent(wndGame, xOffset + (2 * dirSpecificXfactor), yOffset + (1 * dirSpecificYfactor), new AirBlock(wndGame, xOffset, yOffset, chunk, null)));
            structureComponents.Add(new StructureComponent(wndGame, xOffset + (2 * dirSpecificXfactor), yOffset + (2 * dirSpecificYfactor), new AirBlock(wndGame, xOffset, yOffset, chunk, null)));
            structureComponents.Add(new StructureComponent(wndGame, xOffset + (2 * dirSpecificXfactor), yOffset + (3 * dirSpecificYfactor), new AirBlock(wndGame, xOffset, yOffset, chunk, null)));
            structureComponents.Add(new StructureComponent(wndGame, xOffset + (3 * dirSpecificXfactor), yOffset + (1 * dirSpecificYfactor), new AirBlock(wndGame, xOffset, yOffset, chunk, null)));
            structureComponents.Add(new StructureComponent(wndGame, xOffset + (3 * dirSpecificXfactor), yOffset + (2 * dirSpecificYfactor), new AirBlock(wndGame, xOffset, yOffset, chunk, null)));

            borderComponents.Add(new BorderComponent("left", xOffset * dirSpecificXfactor, yOffset + (1 * dirSpecificYfactor)));
            borderComponents.Add(new BorderComponent("left", xOffset * dirSpecificXfactor, yOffset + (2 * dirSpecificYfactor)));
            borderComponents.Add(new BorderComponent("down", xOffset + (1 * dirSpecificXfactor), yOffset + (3 * dirSpecificYfactor)));
            borderComponents.Add(new BorderComponent("down", xOffset + (2 * dirSpecificXfactor), yOffset + (3 * dirSpecificYfactor)));
            borderComponents.Add(new BorderComponent("right", xOffset + (3 * dirSpecificXfactor), yOffset + (1 * dirSpecificYfactor)));
            borderComponents.Add(new BorderComponent("right", xOffset + (3 * dirSpecificXfactor), yOffset + (2 * dirSpecificYfactor)));
            borderComponents.Add(new BorderComponent("up", xOffset + (1 * dirSpecificXfactor), yOffset * dirSpecificYfactor));
            borderComponents.Add(new BorderComponent("up", xOffset + (2 * dirSpecificXfactor), yOffset * dirSpecificYfactor));

        }

    }

    public class BorderComponent
    {
        public int x;
        public int y;
        public string direction = "";

        public BorderComponent(string direction, int x, int y)
        {
            this.direction = direction;
            this.x = x;
            this.y = y;
        }
    }

    public class Cave : Structure
    {
        public Cave(wndGame wndGame, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(wndGame, chunk, canFloat)
        {
            string direction = "";

            if (index >= 0)
            {
                direction = "right";
            }
            else if (index < 0)
            {
                direction = "left";
            }

            structureComponents.Add(new StructureComponent(wndGame, 0, 0, new OakLogBlock(wndGame, x, y, chunk, null)));


            List<CaveComponent> caveComponents = new List<CaveComponent>();
            if (direction == "right")
            {
                caveComponents.Add(new CaveComponent(0, 0, chunk, wndGame, "right", direction));
            }
            else if (direction == "left")
            {
                caveComponents.Add(new CaveComponent(0, 0, chunk, wndGame, "left", direction));
            }

            for (int i = 0; i < 50; i++)
            {
                string newDirection = "";

                if (caveComponents[caveComponents.Count - 1].previousDirection == "up" || caveComponents[caveComponents.Count - 1].previousDirection == "down")
                {
                    //int random = 1;
                    int random = rnd.Next(1, 101);

                    if (random > 0 && random <= 50)
                    {
                        newDirection = "down";
                    }
                    else if (random > 50 && random <= 70)
                    {
                        newDirection = "up";
                    }
                    else if (random > 70 && random <= 90)
                    {
                        newDirection = "right";
                    }
                    else if (random > 90 && random <= 100)
                    {
                        newDirection = "left";
                    }
                }
                else if (caveComponents[caveComponents.Count - 1].previousDirection == "left" || caveComponents[caveComponents.Count - 1].previousDirection == "right")
                {
                    //int random = 71;
                    int random = rnd.Next(1, 101);

                    if (direction == "right")
                    {
                        if (random > 0 && random <= 50)
                        {
                            newDirection = "right";
                        }
                        else if (random > 50 && random <= 70)
                        {
                            newDirection = "up";
                        }
                        else if (random > 70 && random <= 90)
                        {
                            newDirection = "down";
                        }
                        else if (random > 90 && random <= 100)
                        {
                            newDirection = "left";
                        }
                    }
                    else if (direction == "left")
                    {
                        if (random > 0 && random <= 50)
                        {
                            newDirection = "left";
                        }
                        else if (random > 50 && random <= 70)
                        {
                            newDirection = "up";
                        }
                        else if (random > 70 && random <= 90)
                        {
                            newDirection = "down";
                        }
                        else if (random > 90 && random <= 100)
                        {
                            newDirection = "right";
                        }
                    }


                }

                List<BorderComponent> potentialBorders = new List<BorderComponent>();
                foreach (BorderComponent borderComponent in caveComponents[caveComponents.Count - 1].borderComponents)
                {
                    if (borderComponent.direction == newDirection)
                    {
                        potentialBorders.Add(borderComponent);
                        //MessageBox.Show(borderComponent.direction);
                    }
                }

                int random2 = rnd.Next(0, 2);

                foreach (StructureComponent structureComponent in caveComponents[caveComponents.Count - 1].structureComponents)
                {
                    if (potentialBorders[random2].x == structureComponent.xOffset && potentialBorders[random2].y == structureComponent.yOffset)
                    {
                        //Console.WriteLine($"X: {structureComponent.xOffset} Y: {structureComponent.yOffset}");

                        if (direction == "right")
                        {
                            if (newDirection == "right")
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset, structureComponent.yOffset + rnd.Next(-1, 1), chunk, wndGame, newDirection, direction));
                            }
                            else if (newDirection == "left")
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset - 2, structureComponent.yOffset, chunk, wndGame, newDirection, direction));
                            }
                            else if (newDirection == "up")
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset, structureComponent.yOffset, chunk, wndGame, newDirection, direction));
                            }
                            else if (newDirection == "down")
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset + rnd.Next(-2, 0), structureComponent.yOffset - 5, chunk, wndGame, newDirection, direction));
                            }
                        }
                        else if (direction == "left")
                        {
                            if (newDirection == "right")
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset, structureComponent.yOffset + rnd.Next(-1, 1), chunk, wndGame, newDirection, direction));
                            }
                            else if (newDirection == "left")
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset + 2, structureComponent.yOffset, chunk, wndGame, newDirection, direction));
                            }
                            else if (newDirection == "up")
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset, structureComponent.yOffset, chunk, wndGame, newDirection, direction));
                            }
                            else if (newDirection == "down")
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset + rnd.Next(0, 2), structureComponent.yOffset - 5, chunk, wndGame, newDirection, direction));
                            }
                        }

                    }
                }
            }

            foreach (CaveComponent caveComponent in caveComponents)
            {

                foreach (StructureComponent structureComponent in caveComponent.structureComponents)
                {
                    structureComponents.Add(structureComponent);
                }
            }

            Console.WriteLine(structureComponents.Count);

            List<int> handledX = new List<int>();
            foreach (StructureComponent structureComponent in structureComponents)
            {
                if (!handledX.Contains(structureComponent.xOffset))
                {
                    handledX.Add(structureComponent.xOffset);
                }
            }
            totalWidth = handledX.Count;

            BeginGeneration(x, y, index, isNew);

        }
    }
}
