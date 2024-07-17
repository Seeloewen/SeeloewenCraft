using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SeeloewenCraft
{
    //-- Blocks --//

    public class GrassBlock : Block
    {
        public GrassBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Grass Block";
            id = "sc:grass_block";
        }

        override public void GenerateItem(World world)
        {
            item = new GrassItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.GrassBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class StoneBlock : Block
    {
        public StoneBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            //lootTable = world.lootTables.stoneLootTable;
            SetTexture();
            name = "Stone Block";
            id = "sc:stone_block";
        }

        override public void GenerateItem(World world)
        {
            item = new StoneItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.StoneBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class DirtBlock : Block
    {
        public DirtBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Dirt";
            id = "sc:dirt_block";
        }

        override public void GenerateItem(World world)
        {
            item = new DirtItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.DirtBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class AirBlock : Block
    {
        public AirBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            isBreakable = false;
            isSolid = false;
            isReplacable = true;
            canBeMovedToBackground = false;
            SetTexture();
            name = "Air";
            id = "sc:air_block";
            isLightSource = true;
        }

        override public void GenerateItem(World world)
        {
            item = new AirItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.AirBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class BedrockBlock : Block
    {
        public BedrockBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            isBreakable = false;
            canBeMovedToBackground = false;
            SetTexture();
            name = "Bedrock";
            id = "sc:bedrock_block";
        }

        override public void GenerateItem(World world)
        {
            item = new BedrockItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.BedrockBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class CoalOreBlock : Block
    {
        public CoalOreBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Coal Ore";
            id = "sc:coal_ore_block";
        }

        override public void GenerateItem(World world)
        {
            item = new CoalOreItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.CoalOreBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class DiamondOreBlock : Block
    {
        public DiamondOreBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Diamond Ore";
            id = "sc:coal_ore_block";
        }

        override public void GenerateItem(World world)
        {
            item = new DiamondOreItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.DiamondOreBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class IronOreBlock : Block
    {
        public IronOreBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Iron Ore";
            id = "sc:iron_ore_block";
        }

        override public void GenerateItem(World world)
        {
            item = new IronOreItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.IronOreBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class OakLogBlock : Block
    {
        public OakLogBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Oak Log";
            id = "sc:oak_log_block";
        }

        override public void GenerateItem(World world)
        {
            item = new OakLogItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.OakLogBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class OakLeavesBlock : Block
    {
        public OakLeavesBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Oak Leaves";
            id = "sc:oak_leaves_block";
        }

        override public void GenerateItem(World world)
        {
            item = new OakLeavesItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.OakLeavesBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class SpruceLogBlock : Block
    {
        public SpruceLogBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Spruce Log";
            id = "sc:spruce_log_block";
        }

        override public void GenerateItem(World world)
        {
            item = new SpruceLogItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.SpruceLogBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class SpruceLeavesBlock : Block
    {
        public SpruceLeavesBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Spruce Leaves";
            id = "sc:spruce_leaves_block";
        }

        override public void GenerateItem(World world)
        {
            item = new SpruceLeavesItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.SpruceLeavesBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class ChestBlock : Block
    {
        public ChestBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            hasInventory = true;
            blockInventory = new Inventory(world, false);
            world.inventoryList.Add(blockInventory);
            SetTexture();
            name = "Chest";
            id = "sc:chest_block";
            hasRightClickAction = true;
        }

        override public void GenerateItem(World world)
        {
            item = new ChestItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.ChestBlock;
        }
        public override void RightClickAction(object sender)
        {
            //If the block is solid and has inventory
            if (IsInRange() && isSolid && hasInventory)
            {
                //If the block has an inventory, open it as well as the players inventory
                world.player.inventory.inventoryGui.SetTop(0);
                world.player.inventory.ShowInventory();
                blockInventory.inventoryGui.SetTop(400);
                blockInventory.inventoryGui.tblHeader.Text = "Chest";
                blockInventory.ShowInventory();
            }
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class MagmaBlock : Block
    {
        public MagmaBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Magma Block";
            id = "sc:magma_block";
        }

        override public void GenerateItem(World world)
        {
            item = new MagmaBlockItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.MagmaBlock;
        }
        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class TorchBlock : Block
    {
        public TorchBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            isSolid = false;
            canBeMovedToBackground = false;
            isLightSource = true;
            SetTexture();
            name = "Torch";
            id = "sc:torch_block";
        }

        override public void GenerateItem(World world)
        {
            item = new TorchItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.Torch;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }
    public class Plant2Block_Base : Block
    {
        public Plant2Block_Base(World world, bool isInBackground) : base(world, isInBackground)
        {
            isSolid = false;
            isBase = true;
            SetTexture();
            name = "Cactus Plant Base";
            id = "sc:cactus_plant_base_block";
            connectedBlocks.Add(new Plant2Block_Top(world, isInBackground));
            connectedBlocks[0].yOffset = -1;
            connectedBlocks[0].baseBlock = this;
        }

        override public void GenerateItem(World world)
        {
            item = new Plant2Item(world);
        }

        public override void SetTexture()
        {
            image = world.images.Plant2_Base;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class Plant2Block_Top : Block
    {
        public Plant2Block_Top(World world, bool isInBackground) : base(world, isInBackground)
        {
            isSolid = false;
            SetTexture();
            name = "Cactus Plant Top";
            id = "sc:cactus_plant_top_block";
        }

        override public void GenerateItem(World world)
        {
            item = null;
        }

        public override void SetTexture()
        {
            image = world.images.Plant2_Top;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

    public class AlphaCrafterBlock : Block
    {
        public AlphaCrafterBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Alpha Crafter";
            id = "sc:alpha_crafter_block";
            tags.Add("workstation");
            hasRightClickAction = true;

            craftingHandler = new CraftingHandler(world, this);
            gui = new AlphaCrafterGui(world, 535, 720, 120, 200, "sc:alpha_crafter", null, this);
        }

        override public void GenerateItem(World world)
        {
            item = new AlphaCrafterItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.AlphaCrafter;
        }

        public override void RightClickAction(object sender)
        {
            gui.Show();
        }

        public override void ShowAdditionalDebugInfo()
        {
            world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"recipeClaimable={craftingHandler.recipeClaimable}");
            world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"recipeRunning={craftingHandler.recipeRunning}");

            if (craftingHandler.recipeRunning)
            {
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"selectedRecipe={craftingHandler.selectedRecipe}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"recipeProgress={craftingHandler.recipeProgress}");
                world.debugMenu.AddLine(world.debugMenu.tblBlockStats, $"amount={craftingHandler.amount}");
            }
        }
    }

    public class QuarterOakPlankBlock : Block
    {
        public QuarterOakPlankBlock(World world, bool isInBackground) : base(world, isInBackground)
        {
            SetTexture();
            name = "Quarter Oak Plank";
            id = "sc:quarter_oak_plank_block";
        }

        public override (bool, int) CheckCollision(Direction direction, int startX, int endX, int startY, int endY)
        {
            if (!isSolid || isBackground)
            {
                return (false, 0);
            }

            startX -= (xPos + chunk.index * 8) * 1000;
            endX -= (xPos + chunk.index * 8) * 1000;
            startY -= yPos * 1000;
            endY -= yPos * 1000;

            if (direction == Direction.RIGHT)
            {
                if (startY < 500 && endY > 0)
                {
                    if (startX >= 500 && startX < 1000)
                    {
                        //if movement starts in block
                        //return collision: true and max movement: 0
                        return (true, 0);
                    }
                    else if (startX <= 500 && endX > 500)
                    {
                        //if movement starts before block and ends in or after block
                        //return collision:true and max movement: distance from startX to 0
                        return (true, 500 - startX);
                    }
                    {
                        //if movement start and end are both before or after block
                        //return collision:false and maxmovement: irrelevant
                        return (false, 0);
                    }
                }
                else
                {
                    return (false, 0);
                }
            }

            if (direction == Direction.LEFT)
            {
                if (startY < 500 && endY > 0)
                {
                    if (startX > 500 && startX <= 1000)
                    {
                        //if movement starts in block
                        //return collision: true and max movement: 0
                        return (true, 0);
                    }
                    else if (startX > 1000 && endX < 1000)
                    {
                        //if movement starts before block and ends in or after block 
                        //return collision:true and max movement: distance from startX to 1000
                        return (true, startX - 1000);
                    }
                    {
                        //if movement start and end are both before or after block
                        //return collision:false and maxmovement: irrelevant
                        return (false, 0);
                    }
                }
                else
                {
                    return (false, 0);
                }
            }


            if (direction == Direction.DOWN)
            {
                if (startX < 1000 && endX > 500)
                {
                    if (startY >= 0 && startY < 500)
                    {
                        //if movement starts in block
                        //return collision: true and max movement: 0
                        return (true, 0);
                    }
                    else if (startY <= 0 && endY > 0)
                    {
                        //if movement starts before block and ends in or after block
                        //return collision:true and max movement: distance from startY to 0
                        return (true, 0 - startY);
                    }
                    {
                        //if movement start and end are both before or after block
                        //return collision:false and maxmovement: irrelevant
                        return (false, 0);
                    }
                }
                else
                {
                    return (false, 0);
                }
            }

            if (direction == Direction.UP)
            {
                if (startX < 1000 && endX > 500)
                {
                    if (startY > 0 && startY <= 500)
                    {
                        //if movement starts in block
                        //return collision: true and max movement: 0
                        return (true, 0);
                    }
                    else if (startY > 500 && endY < 500)
                    {
                        //if movement starts before block and ends in or after block 
                        //return collision:true and max movement: distance from startY to 1000
                        return (true, startY - 500);
                    }
                    {
                        //if movement start and end are both before or after block
                        //return collision:false and maxmovement: irrelevant
                        return (false, 0);
                    }
                }
                else
                {
                    return (false, 0);
                }
            }

            //if no direction , maybe throw exception
            return (false, 0);

        }

        override public void GenerateItem(World world)
        {
            item = new QuarterOakPlankItem(world);
        }

        public override void SetTexture()
        {
            image = world.images.QuarterOakPlankBlock;
        }

        public override void RightClickAction(object sender)
        {
            throw new NotImplementedException();
        }

        public override void ShowAdditionalDebugInfo()
        {
            throw new NotImplementedException();
        }
    }

}
