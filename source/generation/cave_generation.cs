using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft
{
    public class CaveComponent
    {
        //References
        public World world;
        public List<StructureComponent> structureComponents = new List<StructureComponent>();
        public List<BorderComponent> borderComponents = new List<BorderComponent>();

        //Variables
        public int xOffset = 0;
        public int yOffset = 0;
        public Direction previousDirection;

        //This class contains the structure components used by the component
        public CaveComponent(int xOffset, int yOffset, Chunk chunk, World world, Direction previousDirection)
        {
            //Pass the variables
            this.world = world;
            this.previousDirection = previousDirection;
            this.xOffset = xOffset;
            this.yOffset = yOffset;

            //Add all structurecomponents to the list
            structureComponents.Add(new StructureComponent(world, xOffset, yOffset + 1, new StoneBlock(world, true)));
            structureComponents.Add(new StructureComponent(world, xOffset, yOffset + 2, new StoneBlock(world, true)));
            structureComponents.Add(new StructureComponent(world, xOffset + 1, yOffset + 1, new StoneBlock(world, true)));
            structureComponents.Add(new StructureComponent(world, xOffset + 1, yOffset, new StoneBlock(world, true)));
            structureComponents.Add(new StructureComponent(world, xOffset + 1, yOffset + 2, new StoneBlock(world, true)));
            structureComponents.Add(new StructureComponent(world, xOffset + 1, yOffset + 3, new StoneBlock(world, true)));
            structureComponents.Add(new StructureComponent(world, xOffset + 2, yOffset, new StoneBlock(world, true)));
            structureComponents.Add(new StructureComponent(world, xOffset + 2, yOffset + 1, new StoneBlock(world, true)));
            structureComponents.Add(new StructureComponent(world, xOffset + 2, yOffset + 2, new StoneBlock(world, true)));
            structureComponents.Add(new StructureComponent(world, xOffset + 2, yOffset + 3, new StoneBlock(world, true)));
            structureComponents.Add(new StructureComponent(world, xOffset + 3, yOffset + 1, new StoneBlock(world, true)));
            structureComponents.Add(new StructureComponent(world, xOffset + 3, yOffset + 2, new StoneBlock(world, true)));

            //Add the bordercomponents to the list
            borderComponents.Add(new BorderComponent(Direction.LEFT, xOffset, yOffset + 1));
            borderComponents.Add(new BorderComponent(Direction.LEFT, xOffset, yOffset + 2));
            borderComponents.Add(new BorderComponent(Direction.DOWN, xOffset + 1, yOffset + 3));
            borderComponents.Add(new BorderComponent(Direction.DOWN, xOffset + 2, yOffset + 3));
            borderComponents.Add(new BorderComponent(Direction.RIGHT, xOffset + 3, yOffset + 1));
            borderComponents.Add(new BorderComponent(Direction.RIGHT, xOffset + 3, yOffset + 2));
            borderComponents.Add(new BorderComponent(Direction.UP, xOffset + 1, yOffset));
            borderComponents.Add(new BorderComponent(Direction.UP, xOffset + 2, yOffset));
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
        public Cave(World world, int x, int y, int index, bool isNew, Chunk chunk, bool canFloat) : base(world, chunk, canFloat)
        {
            canReplaceSolidBlocks = false;
            id = "sc:cave_1_structure";
            name = "Cave1";

            //Determine the generation direction
            Direction direction = Direction.RIGHT;
            if (index >= 0)
            {
                direction.IsRight();
            }
            else if (index < 0)
            {
                direction.IsLeft();
            }

            //Add the starter cave component to the list
            List<CaveComponent> caveComponents = new List<CaveComponent>();
            if (direction.IsRight())
            {
                caveComponents.Add(new CaveComponent(0, 0, chunk, world, Direction.RIGHT));
            }
            else if (direction.IsLeft())
            {
                caveComponents.Add(new CaveComponent(0, 0, chunk, world, Direction.LEFT));
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
                        newDirection.IsDown();
                    }
                    else if (random > 50 && random <= 70)
                    {
                        newDirection.IsUp();
                    }
                    else if (random > 70 && random <= 90)
                    {
                        newDirection.IsRight();
                    }
                    else if (random > 90 && random <= 100)
                    {
                        newDirection.IsLeft();
                    }
                }
                //If the previous direction was right or left, the chances for getting another right or left are significantly higher
                else if (caveComponents[caveComponents.Count - 1].previousDirection.IsLeft() || caveComponents[caveComponents.Count - 1].previousDirection.IsRight())
                {
                    int random = rnd.Next(1, 101);

                    if (direction.IsRight())
                    {
                        if (random > 0 && random <= 50)
                        {
                            newDirection.IsRight();
                        }
                        else if (random > 50 && random <= 70)
                        {
                            newDirection.IsUp();
                        }
                        else if (random > 70 && random <= 90)
                        {
                            newDirection.IsDown();
                        }
                        else if (random > 90 && random <= 100)
                        {
                            newDirection.IsLeft();
                        }
                    }
                    else if (direction.IsLeft())
                    {
                        if (random > 0 && random <= 50)
                        {
                            newDirection.IsLeft();
                        }
                        else if (random > 50 && random <= 70)
                        {
                            newDirection.IsUp();
                        }
                        else if (random > 70 && random <= 90)
                        {
                            newDirection.IsDown();
                        }
                        else if (random > 90 && random <= 100)
                        {
                            newDirection.IsRight();
                        }
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

                //Randomly select one of the potential border blocks
                int random2 = rnd.Next(0, potentialBorders.Count);

                //Get the correct border block
                foreach (StructureComponent structureComponent in caveComponents[caveComponents.Count - 1].structureComponents)
                {
                    if (potentialBorders[random2].x == structureComponent.xOffset && potentialBorders[random2].y == structureComponent.yOffset)
                    {
                        //Add the new cave component to the list, depending on the direction. Some offsets are used here to make the cave generation feel less linear.
                        if (direction.IsRight())
                        {
                            if (newDirection.IsRight())
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset, structureComponent.yOffset + rnd.Next(-1, 1), chunk, world, newDirection));
                            }
                            else if (newDirection.IsLeft())
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset - 2, structureComponent.yOffset, chunk, world, newDirection));
                            }
                            else if (newDirection.IsUp())
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset, structureComponent.yOffset, chunk, world, newDirection));
                            }
                            else if (newDirection.IsDown())
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset + rnd.Next(-2, 0), structureComponent.yOffset - 5, chunk, world, newDirection));
                            }
                        }
                        else if (direction.IsLeft())
                        {
                            if (newDirection.IsRight())
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset, structureComponent.yOffset + rnd.Next(-1, 1), chunk, world, newDirection));
                            }
                            else if (newDirection.IsLeft())
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset + 2, structureComponent.yOffset, chunk, world, newDirection));
                            }
                            else if (newDirection.IsUp())
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset, structureComponent.yOffset, chunk, world, newDirection));
                            }
                            else if (newDirection.IsDown())
                            {
                                caveComponents.Add(new CaveComponent(structureComponent.xOffset + rnd.Next(0, 2), structureComponent.yOffset - 5, chunk, world, newDirection));
                            }
                        }

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
            List<int> handledX = new List<int>();
            foreach (StructureComponent structureComponent in structureComponents)
            {
                if (!handledX.Contains(structureComponent.xOffset))
                {
                    handledX.Add(structureComponent.xOffset);
                }
            }
            totalWidth = handledX.Count;

            //Actually generate the structure now that everything is prepared
            BeginGeneration(x, y, index, isNew);
        }
    }
}
