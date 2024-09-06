using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace SeeloewenCraft
{
    public class CaveComponent
    {
        //References
        public List<StructureComponent> structureComponents = new List<StructureComponent>();
        public List<BorderComponent> borderComponents = new List<BorderComponent>();
        public Random rnd;

        //Variables
        public int xOffset = 0;
        public int yOffset = 0;
        private static int rndOffset = 0;
        public Direction previousDirection;

        //This class contains the structure components used by the component
        public CaveComponent(int xOffset, int yOffset, Chunk chunk, Direction previousDirection, Direction offsetDirection)
        {
            //Pass the variables
            this.previousDirection = previousDirection;
            rnd = new Random(DateTime.Now.Millisecond + rndOffset);
            rndOffset++;
        }
    }

    public class CaveComponent1 : CaveComponent
    {
        public CaveComponent1(int xOffset, int yOffset, Chunk chunk, Direction previousDirection, Direction offsetDirection) : base(xOffset, yOffset, chunk, previousDirection, offsetDirection)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;

            //Get offsets based on directions
            switch (previousDirection)
            {
                case Direction.RIGHT:
                    this.yOffset += rnd.Next(-2, 2);
                    break;
                case Direction.LEFT:
                    this.xOffset += offsetDirection.IsRight() ? -2 : 2;
                    this.yOffset += rnd.Next(-2, 2);
                    break;
                case Direction.DOWN:
                    this.xOffset += offsetDirection.IsRight() ? rnd.Next(-3, -1) : rnd.Next(-1, 3);
                    this.yOffset -= 5;
                    break;
            }

            //Add all structurecomponents to the list
            structureComponents.Add(new StructureComponent(this.xOffset, this.yOffset + 1, new CobblestoneBlock(true)));
            structureComponents.Add(new StructureComponent(this.xOffset, this.yOffset + 2, new CobblestoneBlock(true)));
            structureComponents.Add(new StructureComponent(this.xOffset + 1, this.yOffset + 1, new CobblestoneBlock(true)));
            structureComponents.Add(new StructureComponent(this.xOffset + 1, this.yOffset, new CobblestoneBlock(true)));
            structureComponents.Add(new StructureComponent(this.xOffset + 1, this.yOffset + 2, new CobblestoneBlock(true)));
            structureComponents.Add(new StructureComponent(this.xOffset + 1, this.yOffset + 3, new CobblestoneBlock(true)));
            structureComponents.Add(new StructureComponent(this.xOffset + 2, this.yOffset, new CobblestoneBlock(true)));
            structureComponents.Add(new StructureComponent(this.xOffset + 2, this.yOffset + 1, new CobblestoneBlock(true)));
            structureComponents.Add(new StructureComponent(this.xOffset + 2, this.yOffset + 2, new CobblestoneBlock(true)));
            structureComponents.Add(new StructureComponent(this.xOffset + 2, this.yOffset + 3, new CobblestoneBlock(true)));
            structureComponents.Add(new StructureComponent(this.xOffset + 3, this.yOffset + 1, new CobblestoneBlock(true)));
            structureComponents.Add(new StructureComponent(this.xOffset + 3, this.yOffset + 2, new CobblestoneBlock(true)));

            //Add the bordercomponents to the list
            borderComponents.Add(new BorderComponent(Direction.LEFT, this.xOffset, this.yOffset + 1));
            borderComponents.Add(new BorderComponent(Direction.LEFT, this.xOffset, this.yOffset + 2));
            borderComponents.Add(new BorderComponent(Direction.DOWN, this.xOffset + 1, this.yOffset + 3));
            borderComponents.Add(new BorderComponent(Direction.DOWN, this.xOffset + 2, this.yOffset + 3));
            borderComponents.Add(new BorderComponent(Direction.RIGHT, this.xOffset + 3, this.yOffset + 1));
            borderComponents.Add(new BorderComponent(Direction.RIGHT, this.xOffset + 3, this.yOffset + 2));
            borderComponents.Add(new BorderComponent(Direction.UP, this.xOffset + 1, this.yOffset));
            borderComponents.Add(new BorderComponent(Direction.UP, this.xOffset + 2, this.yOffset));
        }
    }


    //This class is used to let the game know the position and direction of borders of the components
    public struct BorderComponent
    {
        public Direction direction;
        public int x;
        public int y;

        public BorderComponent(Direction direction, int x, int y)
        {
            //Pass the variables
            this.direction = direction;
            this.x = x;
            this.y = y;
        }
    }

    //These are the actual caves, made up of the components above
    public class Cave : Structure
    {

        public Cave(int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(chunk, canFloat)
        {
            canReplaceSolidBlocks = false;
            id = "sc:cave_1_structure";
            name = "Cave1";

            //Determine the generation direction
            Direction direction = Direction.RIGHT;
            if (index >= 0)
            {
                direction = Direction.RIGHT;
            }
            else if (index < 0)
            {
                direction = Direction.LEFT;
            }

            //Add the starter cave component to the list
            List<CaveComponent> caveComponents = new List<CaveComponent>();
            if (direction.IsRight())
            {
                caveComponents.Add(GetCaveComponent(0, 0, chunk, Direction.RIGHT, Direction.RIGHT));
            }
            else if (direction.IsLeft())
            {
                caveComponents.Add(GetCaveComponent(0, 0, chunk, Direction.LEFT, Direction.LEFT));
            }

            //Use random numbers to add new cave components to random sides
            for (int i = 0; i < 50; i++)
            {
                //First, determine the new direction
                Direction newDirection = Direction.RIGHT;

                //If the previos direction was up or down, the chances for getting another down are significantly higher
                if (caveComponents[caveComponents.Count - 1].previousDirection.IsUp() || caveComponents[caveComponents.Count - 1].previousDirection.IsDown())
                {
                    int random = rnd.Next(1, 101);

                    if (random > 0 && random <= 50)
                    {
                        newDirection = Direction.DOWN;
                    }
                    else if (random > 50 && random <= 70)
                    {
                        newDirection = Direction.UP;
                    }
                    else if (random > 70 && random <= 90)
                    {
                        newDirection = Direction.RIGHT;
                    }
                    else if (random > 90 && random <= 100)
                    {
                        newDirection = Direction.LEFT;
                    }
                }
                //If the previous direction was right or left, the chances for getting another right or left are significantly higher
                else if (caveComponents[caveComponents.Count - 1].previousDirection.IsLeft() || caveComponents[caveComponents.Count - 1].previousDirection.IsRight())
                {
                    int random = rnd.Next(1, 101);

                    //Determine new direction based on random value
                    if (random > 0 && random <= 50)
                    {
                        newDirection = direction.IsRight() ? Direction.RIGHT : Direction.LEFT;
                    }
                    else if (random > 50 && random <= 70)
                    {
                        newDirection = Direction.UP;
                    }
                    else if (random > 70 && random <= 90)
                    {
                        newDirection = Direction.DOWN;
                    }
                    else if (random > 90 && random <= 100)
                    {
                        newDirection = direction.IsRight() ? Direction.LEFT : Direction.RIGHT;
                    }
                }

                //Get a list of all possible border blocks in the determined direction that the cave can append to
                List<BorderComponent> potentialBorders = new List<BorderComponent>();
                foreach (BorderComponent borderComponent in caveComponents[caveComponents.Count - 1].borderComponents)
                {
                    if (borderComponent.direction == newDirection)
                    {
                        potentialBorders.Add(borderComponent);
                    }
                }

                //Randomly select one of the potential border blocks to append the new cave component
                int random2 = rnd.Next(0, potentialBorders.Count);

                //Get the correct border block
                foreach (StructureComponent structureComponent in caveComponents[caveComponents.Count - 1].structureComponents)
                {
                    if (potentialBorders[random2].x == structureComponent.xOffset && potentialBorders[random2].y == structureComponent.yOffset)
                    {
                        caveComponents.Add(GetCaveComponent(structureComponent.xOffset, structureComponent.yOffset, chunk, newDirection, direction));
                    }
                }
            }

            //When all cave components are generated, add all structure components of the generated cave components to the list
            foreach (CaveComponent caveComponent in caveComponents)
            {
                foreach (StructureComponent structureComponent in caveComponent.structureComponents)
                {
                    structureComponents.Add(structureComponent);
                }
            }

            //Get the total width by checking the amount of different X coordinates
            totalWidth = GetTotalWidth();

            //Actually generate the structure now that everything is prepared
            BeginGeneration(x, y, index, isNew);
        }

        public CaveComponent GetCaveComponent(int xOffset, int yOffset, Chunk chunk, Direction previousDirection, Direction offsetDirection)
        {
            //Will later return a random component
            return new CaveComponent1(xOffset, yOffset, chunk, previousDirection, offsetDirection);
        }
    }
}
