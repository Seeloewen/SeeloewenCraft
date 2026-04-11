using Newtonsoft.Json.Linq;
using SeeloewenCraft.game.core.blocks.components;
using SeeloewenCraft.game.core.crafting;
using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.entities.inventory;
using SeeloewenCraft.game.core.items;
using SeeloewenCraft.game.core.world;
using SeeloewenCraft.game.graphics;
using SeeloewenCraft.game.graphics.ui_lib;
using SeeloewenCraft.game.networking;
using SeeloewenCraft.game.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace SeeloewenCraft.game.core.blocks
{
    public abstract partial class Block : IDebugMenuTargetable
    {
        //Constant block type info
        public readonly string name;
        public readonly string id;
        public readonly string itemId;
        public readonly int breakTime = 150; //Milliseconds
        public readonly Tool effectiveTool;
        public readonly Material? effectiveMaterial;

        //Semi-const attributes, should be only set once at best
        public int posX; //X-pos in chunk (0-7)
        public int posY;
        public Chunk chunk;
        public Collision collision = new EntireBlockCollision();
        public (LootTable lootTable, int minRolls, int maxRolls) lootTable; //TODO: Rework

        //Variable attributes
        private readonly HashSet<string> tags = new HashSet<string>();
        public bool isBase = false;
        public bool isSolid = true;
        public bool isBackground = false;
        public int lightLevel = 7;
        public bool isAirLightSource;
        public bool isForeground = false;
        public BlockState blockState { protected get; set; }
        public bool breaking;
        public double breakProgress;
        public bool hammering;
        public int hammerProgress;
        protected double sinceLastSpecificUpdate = 0;

        //Other block data
        internal readonly List<BlockComponent> components = new List<BlockComponent>();
        private Block foregroundBlock;
        public List<(int xOffset, int yOffset, string blockId)> connectedBlocks = new List<(int, int, string)>();
        public (int x, int y) baseBlockOffset;

        public List<(string id, int min, int max)> drops = new List<(string, int, int)>(); //Can be empty, means that item id will drop x1
        public (bool doesNeed, string tag) needsGround = (false, "");


        //Temporary, only important during generation
        public bool isSurface = false;
        public int breakTimeTicks => breakTime * 12 / 100;  //TODO: 12 is legacy code from old timer, might need adaption


        protected Block(string name, string id, int breakTime, string itemId = null, Tool tool = Tool.None, Material material = Material.None)
        {
            this.name = name;
            this.id = id;
            this.breakTime = breakTime;
            this.itemId = itemId;
            effectiveTool = tool;
        }

        #region lighting (dev)

        double lightTopRight = 1.0;
        double lightTopLeft = 1.0;
        double lightBotLeft = 1.0;
        double lightBotRight = 1.0;

        double lightFactor = 0.3;

        void ResetLight()
        {
            lightBotLeft = 0f;
            lightBotRight = 0f;
            lightTopLeft = 0f;
            lightTopRight = 0f;
        }


        void SetLightTopRight(double light)
        {
            double sqrt2 = Math.Sqrt(2.0);



        }


        #endregion

        #region tags

        public void WriteTag(string tag)
        {
            if (!HasTag(tag)) tags.Add(tag);
        }

        public bool HasTag(string tag)
        {
            return tags.Contains(tag);
        }

        public void RemoveTag(string tag)
        {
            tags.Remove(tag);
        }

        #endregion

        public virtual BlockState GetBlockState() => blockState; //Can be overriden when a block uses some other logic to determine blockstate

        internal T GetComponent<T>()
        {
            foreach (var component in components)
            {
                if (component is T t) return t;
            }

            return default;
        }

        internal BlockComponent GetComponent(BlockComponentType type)
        {
            foreach (var component in components)
            {
                if (component.GetType() == type) return component;
            }

            return default;
        }

        public bool IsLightSource() => HasTag(BlockTags.LIGHTSOURCE) || isAirLightSource;

        public void DoUpdate(double dt) //Warning: does NOT run every gameloop tick - dt is correct regardless
        {
            sinceLastSpecificUpdate += dt;

            if (breaking) UpdateBreaking();
            if (hammering) UpdateHammering();

            LightHandler.UpdateLighting(this);

            //Check if the block is floating even though it's not allowed to
            if (needsGround.doesNeed && !ValidBlockBelow(this, GetBlockBelow()))
            {
                BreakBlock(true, false, true);
            }

            //Call block specific updates         
            if (sinceLastSpecificUpdate >= 1)
            {
                DoSpecificUpdate(1);
                sinceLastSpecificUpdate = 0;
            }

            if(foregroundBlock != null) foregroundBlock.DoUpdate(dt);
        }

        protected virtual void DoSpecificUpdate(double dt) { } //Can be overriden in blocks, for block-specific updates - run every 1s

        public BlockRenderInfo GetBlockRenderInfo()
        {
            Block b = GetForegroundBlock() ?? this; //Animation is depending on the block that gets 
            int animation = (int)(b.breaking || b.hammering ? (6 * b.breakProgress) / b.breakTimeTicks : 0);
            var info = new BlockRenderInfo(posX + chunk.index * 8, posY, id, GetBlockState(), isBackground, animation, hammering, lightLevel);
            if (foregroundBlock != null) info.AddForegroundBlock(foregroundBlock.id, foregroundBlock.GetBlockState());
            return info;
        }

        public virtual bool[] CheckTouch(int startX, int startY, int endX, int endY)
        {
            if (isBackground && foregroundBlock != null)
            {
                return foregroundBlock.CheckTouch(startX, startY, endX, endY);
            }
            bool[] touchingStatus = new bool[Entity.TOUCHING_STATUS_COUNT];
            touchingStatus[Entity.TOUCHING_AIR] = true;
            return touchingStatus;
        }

        public virtual (bool, int) CheckCollision(Direction direction, int startX, int endX, int startY, int endY)
        {
            startX -= (posX + chunk.index * 8) * 1000;
            endX -= (posX + chunk.index * 8) * 1000;
            startY -= posY * 1000;
            endY -= posY * 1000;

            if (!isSolid || isBackground)
            {
                if (foregroundBlock != null && foregroundBlock.isSolid)
                {
                    return foregroundBlock.collision.CheckCollision(direction, startX, endX, startY, endY);
                }
                else
                {
                    return (false, 0);
                }
            }
            else
            {
                return collision.CheckCollision(direction, startX, endX, startY, endY);
            }
        }

        public virtual JObject ToJson()
        {
            //Basic attributes
            JObject root = new JObject
            {
                { "id", id },
                { "pos_x", posX },
                { "pos_y", posY },
                { "is_background", isBackground }
            };

            //Tags
            JArray tags = [.. this.tags];
            root.Add("tags", tags);

            //Connected blocks
            if (connectedBlocks.Count > 0)
            {
                JArray blocksArray = new JArray();
                foreach (var block in connectedBlocks)
                {
                    JObject blockObject = new JObject
                    {
                        { "block_id", block.blockId },
                        { "x_offset", block.xOffset },
                        { "y_offset", block.yOffset }
                    };
                    blocksArray.Add(blockObject);
                }

                root.Add("connected_blocks", blocksArray);
            }

            //Base block
            if (baseBlockOffset.x != 0 || baseBlockOffset.y != 0)
            {
                root.Add("baseblock_x_offset", baseBlockOffset.x);
                root.Add("baseblock_y_offset", baseBlockOffset.y);
            }

            if (GetForegroundBlock() != null)
            {
                root.Add("foreground_block", GetForegroundBlock().ToJson());
            }

            //Save components
            JArray componentArray = new JArray();
            root.Add("components", componentArray);
            components.ForEach(e => componentArray.Add(e.ToJson()));

            return root;
        }

        static public Block FromJson(JObject obj)
        {
            string id = obj.Get<string>("id");
            Block block = BlockRegister.Get(id) ?? new AirBlock(); //Default to air if the id is invalid

            //Load basic attributes
            block.posX = obj.Get<int>("/pos_x");
            block.posY = obj.Get<int>("/pos_y");
            if (obj.Get<bool>("/is_in_background")) block.MoveToBackground();

            //Get blocktags
            foreach (JToken tokenTag in obj.Get<JArray>("tags"))
            {
                block.WriteTag(tokenTag.ToString());
            }

            //Load foreground block
            JObject blockToken = obj.Get<JObject>("foreground_block");
            block.foregroundBlock = blockToken != null ? FromJson(blockToken) : null;

            //Components
            JArray componentArray = obj.Get<JArray>("components");
            foreach (JObject comObj in componentArray)
            {
                var type = comObj.Get<BlockComponentType>("type");
                BlockComponent com = block.GetComponent(type);
                if (com == null) continue;

                com.FromJson(comObj.Get<JObject>("content"));
            }

            //Connected blocks
            if (obj.ContainsKey("connected_blocks"))
            {
                JArray connectedBlocks = obj.Get<JArray>("connected_blocks");
                foreach (JObject blockObj in connectedBlocks)
                {
                    string blockId = blockObj.Get<string>("block_id");
                    int xOffset = blockObj.Get<int>("x_offset");
                    int yOffset = blockObj.Get<int>("y_offset");

                    block.connectedBlocks.Add((xOffset, yOffset, blockId));
                }
            }

            if (obj.ContainsKey("baseblock_x_offset") && obj.ContainsKey("baseblock_y_offset"))
            {
                block.baseBlockOffset = (obj.Get<int>("baseblock_x_offset"), obj.Get<int>("baseblock_y_offset"));
            }

            return block;
        }

        protected virtual void AppendJson(JObject obj) { }
        protected virtual void LoadAdditionalData(JObject obj) { }

        public PositionData GetPosData() => new PositionData(posX, posY, chunk.index);

        public int GetAbsoluteX() => chunk.index * 8 + posX;

        public static bool IsCollidingWithPlayer(int x, int y, int c, bool isSolid)
        {
            if (!isSolid) return false;

            x += 8 * c;
            double px = Player.Get().posX / 1000d;
            double py = Player.Get().posY / 1000d;

            if (x == Math.Floor(px) && (y == Math.Floor(py) || y == Math.Floor(py + 1) || (py % 1 > 0.1 && y == Math.Floor(py + 2)))) return true; ;

            if (px % 1 > 0.6) //If the player is slightly to the right side of the block, also check the block to the right
            {
                if (x == Math.Floor(px + 1) && (y == Math.Floor(py) || y == Math.Floor(py + 1) || (py % 1 > 0.1 && y == Math.Floor(py + 2)))) return true;
            }

            return false;
        }

        public Block MoveToBackground()
        {
            isBackground = true;

            Block blockAbove = GetBlockAbove();
            if (blockAbove != null && blockAbove.HasTag(BlockTags.CAN_FALL))
            {
                blockAbove.BreakBlock(true, true, false);
                World.Get().AddEntity(new FallingBlockEntity(posX + 8 * chunk.index, posY - 1, blockAbove.id));
            }

            return this;
        }

        public void MoveToNormal()
        {
            isBackground = false;
        }

        public bool IsInRange()
        {
            Block playerBlock = World.Get().GetBlock(Player.Get().posX / 1000, (Player.Get().posY / 1000) + 1);
            if (playerBlock != null)
            {
                return (GetXRangeToBlock(playerBlock) < 5 && GetYRangeToBlock(playerBlock) < 5);
            }
            else
            {
                return false;
            }
        }

        public static bool ValidBlockBelow(Block block, Block blockBelow)
        {
            if (block == null && blockBelow == null) return false;

            //If the block doesn't need a specific ground block (besides being solid) or the ground block has the needed tag, the block can stay
            return (string.IsNullOrEmpty(block.needsGround.tag) && blockBelow.isSolid)
                    || blockBelow.HasTag(block.needsGround.tag);
        }


        public void RemoveForegroundBlock()
        {
            foregroundBlock = null;
        }

        public Block GetForegroundBlock()
        {
            return foregroundBlock;
        }

        public Item GetItem()
        {
            //If the item has an id, generate an item and return it
            if (!string.IsNullOrEmpty(itemId))
            {
                return ItemRegister.Get(itemId);
            }

            return null;
        }


        public virtual void RightClickAction()
        {
            return;
        }

        #region Debug menu implementation



        public virtual void AddDebugMenu()
        {
            //Show the debug information for the block in debug menu
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "id", $"{id}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "name", $"{name}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "x", $"{posX + chunk.index * 8}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "y", $"{posY}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "x;chunk", $"{posX};{chunk.index}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "isSolid");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, "isBackground");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"lightLevel");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"isLightSource");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"isBase", $"{isBase}");
            DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"blockState", $"{GetBlockState()}");
            //DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"isSurface", $"{isSurface}");
            if (foregroundBlock != null)
            {
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"foregroundBlock", $"{foregroundBlock.id}");
            }
            if (GetBaseBlock() != null)
            {
                DebugMenu.AddLine(DebugMenu.Section.TARGETED, $"baseBlock", $"{GetBaseBlock().id} [{posX + baseBlockOffset.x}|y{posY + baseBlockOffset.y}]");
            }

            if (tags.Count > 0)
            {
                string s = tags.Count != 0 ? $"{tags.First()}" : "None";
                foreach (string tag in tags) s += ";" + tag;

                DebugMenu.AddLine(DebugMenu.Section.TARGETED, "Tags", s);

            }
        }


        public virtual void UpdateDebugMenu()
        {
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, "isSolid", $"{isSolid} ");
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, "isBackground", $"{isBackground} ");
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, "lightLevel", $"{lightLevel} ");
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, $"isLightSource", $"{IsLightSource()}");
            DebugMenu.UpdateLine(DebugMenu.Section.TARGETED, $"blockState", $"{GetBlockState()}");
        }


        #endregion

        public static List<Block> GetBlocksInRange(Block block, int range)
        {
            List<Block> blocksInRange = new List<Block>();

            //Gets all blocks above 
            for (int yOffset = 0; yOffset < range; yOffset++)
            {
                for (int xOffset = -yOffset; xOffset <= yOffset; xOffset++)
                {
                    blocksInRange.Add(GetBlockFromOffset(block, xOffset, range - yOffset));
                    blocksInRange.Add(GetBlockFromOffset(block, xOffset, -range + yOffset));
                }
            }

            for (int xOffset = 1; xOffset < range + 1; xOffset++)
            {
                blocksInRange.Add(GetBlockFromOffset(block, xOffset, 0));
                blocksInRange.Add(GetBlockFromOffset(block, -xOffset, 0));
            }

            return blocksInRange;
        }

        private static Block GetBlockFromOffset(Block block, int xOffset, int yOffset)
        {
            //Get the new block based from the offset from this block

            //If total y is above 74 or below 0, there isn't any available block
            if (yOffset + block.posY < 0 || yOffset + block.posY > 74)
            {
                return null;
            }

            //If total x is below 0, get the chunk to the left
            if (block.posX + xOffset < 0)
            {
                Chunk chunk = World.Get().GetChunk(block.chunk.index - 1);

                if (chunk != null)
                {
                    return chunk.blockList.Get(block.posX + xOffset + 8, block.posY + yOffset);
                }
                else
                {
                    return null;
                }
            }
            //If total x is above 7, get the chunk to the right
            else if (block.posX + xOffset > 7)
            {
                Chunk chunk = World.Get().GetChunk(block.chunk.index + 1);

                if (chunk != null)
                {
                    return chunk.blockList.Get(block.posX + xOffset - 8, block.posY + yOffset);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                //Else just get the block from the current chunk
                return block.chunk.blockList.Get(block.posX + xOffset, block.posY + yOffset);
            }
        }

        public int GetRangeToBlock(Block block)
        {
            int xDiff = block.posX + block.chunk.index * 8 - posX - chunk.index * 8;
            return Math.Abs(xDiff) + Math.Abs(block.posY - posY);
        }

        public int GetXRangeToBlock(Block block)
        {
            int xDiff = block.posX + block.chunk.index * 8 - posX - chunk.index * 8;
            return Math.Abs(xDiff);
        }

        public int GetYRangeToBlock(Block block)
        {
            int yDiff = block.posY - posY;
            return Math.Abs(yDiff);
        }

        public virtual void Drop()
        {
            //Get the block that should drop
            if (HasTag(BlockTags.DOESNT_DROP)) return;

            if ((Player.Get().inventory.GetSelectedItem() is ToolItem tool && tool.type == effectiveTool && ToolIsCorrectMaterial(tool.material) && HasTag(BlockTags.TOOL_SPECIFIC) || !HasTag(BlockTags.TOOL_SPECIFIC) || World.Get().gamemode == Gamemode.Creative))
            {
                //If the block has a loot table, roll an entry and give the items to player
                if (lootTable.lootTable != null)
                {
                    //Get the amount of times the item gets dropped
                    int rolls = Game.rnd.Next(lootTable.minRolls, lootTable.maxRolls + 1);

                    for (int i = 0; i < rolls; i++)
                    {
                        List<Item> items = lootTable.lootTable.RollEntry().RollItems();
                        foreach (Item item in items)
                        {
                            SpawnItem(item);
                        }
                    }
                }
                //If it has a specified droplist
                else if (drops.Count > 0)
                {
                    foreach (var entry in drops)
                    {
                        //Roll an amount of drops for each item and drop that amount of that item
                        int amount = Game.rnd.Next(entry.min, entry.max + 1);

                        for (int j = 0; j < amount; j++)
                        {
                            SpawnItem(ItemRegister.Get(entry.id));
                        }
                    }
                }
                //If it has only an item, only give that item
                else if (GetItem() != null)
                {
                    SpawnItem(GetItem());
                }
            }
        }

        private void SpawnItem(Item item)
        {
            if (item != null)
            {
                //Spawn the item entity in the world
                World.Get().AddEntity(new ItemEntity(item, item.tag, //item type
                    (posX + 8 * chunk.index) * 1000 + 500 - ItemEntity.itemSizeX / 2, //posX
                    posY * 1000 + 500 - ItemEntity.itemSizeY / 2, //posY
                Game.rnd.Next(-6000, 6000), Game.rnd.Next(-15000, -10000))); //velX and velY 
            }
        }

        public void BreakBlock(bool skipRangeCheck, bool skipBreakableCheck, bool dropItems)
        {
            //Check if is in range
            if (!IsInRange() && !skipRangeCheck) return;

            //If it has no foreground block, check if the normal block is breakable
            if (!HasTag(BlockTags.UNBREAKABLE) || skipBreakableCheck)
            {
                //Remove the block from the chunks blocklist and add an airblock
                Block block = new AirBlock();
                World.Get().SetBlock(GetPosData(), block);

                if (dropItems)
                {
                    Drop();

                    if (HasTag(BlockTags.HAS_INVENTORY))
                    {
                        BlockInventory invComponent = (BlockInventory)GetComponent(BlockComponentType.Inventory);
                        invComponent.inventory.DropAll((posX + 8 * chunk.index) * 1000 + 500 - ItemEntity.itemSizeX / 2, posY * 1000 + 500 - ItemEntity.itemSizeY / 2);
                    }
                }

                Block blockAbove = GetBlockAbove();
                if (blockAbove.HasTag(BlockTags.CAN_FALL))
                {
                    blockAbove.BreakBlock(true, true, false);
                    World.Get().AddEntity(new FallingBlockEntity(posX + 8 * chunk.index, posY - 1, blockAbove.id));
                }
            }
        }

        public virtual void OnSetBlock() //Gets called when this block is placed somewhere
        {
            if (foregroundBlock != null) SetForegroundBlock(foregroundBlock); //Replace the foreground block to update the coords

            //Make the block fall if it has nothing below and can fall
            Block blockBelow = GetBlockBelow();
            if (HasTag(BlockTags.CAN_FALL)
                && blockBelow != null && (blockBelow.HasTag(BlockTags.REPLACEABLE) || blockBelow.isBackground))
            {
                BreakBlock(true, true, false);
                World.Get().AddEntity(new FallingBlockEntity(posX + 8 * chunk.index, posY, id));
            }

            if (this == DebugMenu.target) chunk.GetBlock(posX, posY).AddDebugMenu();

            //Send the data on the network if it's multiplayer
            NetworkHandler.SendData(MultiplayerPacketType.SET_BLOCK, id, chunk.index.ToString(), posX.ToString(), posY.ToString());
        }

        public Block GetBlockBelow()
        {
            if (chunk == null) return null;

            return World.Get().GetBlock(GetAbsoluteX(), posY + 1);
        }

        public Block GetBlockAbove()
        {
            if (chunk == null) return null;

            return World.Get().GetBlock(GetAbsoluteX(), posY - 1);
        }

        public Block GetBlockRight()
        {
            if (chunk == null) return null;

            PositionData posData = GetPosData();

            return World.Get().GetBlock(GetAbsoluteX() + 1, posY);
        }

        public Block GetBlockLeft()
        {
            if (chunk == null) return null;

            return World.Get().GetBlock(GetAbsoluteX() - 1, posY);
        }

        public void SetForegroundBlock(Block block)
        {
            if (block == null)
            {
                return;
            }

            foregroundBlock = block;
            block.isForeground = true;
            block.posX = posX;
            block.posY = posY;
            block.chunk = chunk;
        }

        public bool ConBlocksHaveSpace(Block baseBlock, bool isForeground)
        {
            if (!isForeground)
            {
                foreach (var conBlockInfo in baseBlock.connectedBlocks)
                {
                    //Goes through all connected blocks and checks whether the block at the location, that they should be placed, at is solid
                    Block block = World.Get().GetBlock(posX + 8 * chunk.index + conBlockInfo.xOffset, posY + conBlockInfo.yOffset);
                    if (block != null && (block.isSolid || block.isBackground))
                    {
                        return false;
                    }
                }
                return true;
            }
            else if (isForeground)
            {
                foreach (var conBlockInfo in baseBlock.connectedBlocks)
                {
                    //Goes through all connected blocks and checks whether the block at the location, that they should be placed, has a foreground block
                    Block block = World.Get().GetBlock(posX + 8 * chunk.index + conBlockInfo.xOffset, posY + conBlockInfo.yOffset);
                    if (block != null && (block.foregroundBlock != null || !block.isBackground))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetCoords(int posX, int posY, Chunk chunk) //TODO: This seems sketchy, rework it
        {
            //Warning: Only sets coords inside blocks, not inside chunk/blocklist
            this.chunk = chunk;
            this.posX = posX;
            this.posY = posY;
        }

        public List<Block> GetConnectedBlocks(bool inForeground) //Assumes this is a base block
        {
            //Get all connected blocks from the given coordinates
            List<Block> connectedBlocks = new List<Block>();
            foreach (var entry in this.connectedBlocks)
            {
                if (!inForeground)
                {
                    connectedBlocks.Add(World.Get().GetBlock(posX + 8 * chunk.index + entry.xOffset, posY + entry.yOffset));
                }
                else
                {
                    connectedBlocks.Add(World.Get().GetBlock(posX + 8 * chunk.index + entry.xOffset, posY + entry.yOffset).foregroundBlock);
                }
            }

            return connectedBlocks;
        }

        public Block GetBaseBlock()
        {
            if (baseBlockOffset.x != 0 && baseBlockOffset.y != 0)
            {
                return World.Get().GetBlock(posX + chunk.index * 8 + (int)baseBlockOffset.x, posY + (int)baseBlockOffset.y);
            }
            return null;
        }

        public void PlaceConnectedForegroundBlocks(Block baseBlock)
        {
            foreach (var conBlock in baseBlock.connectedBlocks)
            {
                //Place the connected block
                Block oldBlock = World.Get().GetBlock(posX + 8 * chunk.index + conBlock.xOffset, posY + conBlock.yOffset);
                Block newBlock = BlockRegister.Get(conBlock.blockId);
                newBlock.baseBlockOffset = (-conBlock.xOffset, -conBlock.yOffset);

                oldBlock.SetForegroundBlock(newBlock);
            }
        }

        public void PlaceConnectedBlocks(Block baseBlock)
        {
            foreach (var conBlock in baseBlock.connectedBlocks)
            {
                //Place the connected block
                Block oldBlock = World.Get().GetBlock(posX + 8 * chunk.index + conBlock.xOffset, posY + conBlock.yOffset);
                Block newBlock = BlockRegister.Get(conBlock.blockId);
                newBlock.baseBlockOffset = (-conBlock.xOffset, -conBlock.yOffset);

                World.Get().SetBlock(oldBlock.GetPosData(), newBlock);
            }
        }


        public void RegisterTool(Tool tool)
        {
            switch (tool)
            {
                case Tool.Hammer:
                    tags.Add("effectiveTools/hammer");
                    break;
                case Tool.Scythe:
                    tags.Add("effectiveTools/scythe");
                    break;
                case Tool.Shovel:
                    tags.Add("effectiveTools/shovel");
                    break;
                case Tool.Sword:
                    tags.Add("effectiveTools/sword");
                    break;
                case Tool.Pickaxe:
                    tags.Add("effectiveTools/pickaxe");
                    break;
                case Tool.Axe:
                    tags.Add("effectiveTools/axe");
                    break;
            }
        }

        public bool ToolIsCorrectMaterial(Material toolMaterial)
        {
            //Check if the effective material is supported by the tool
            if (effectiveMaterial != null)
            {
                switch (effectiveMaterial)
                {
                    case Material.Wood:
                        return toolMaterial == Material.Wood
                            || toolMaterial == Material.Stone
                            || toolMaterial == Material.Tin
                            || toolMaterial == Material.Iron
                            || toolMaterial == Material.Diamond;

                    case Material.Stone:
                        return toolMaterial == Material.Stone
                            || toolMaterial == Material.Tin
                            || toolMaterial == Material.Iron
                            || toolMaterial == Material.Diamond;

                    case Material.Tin:
                        return toolMaterial == Material.Tin
                            || toolMaterial == Material.Iron
                            || toolMaterial == Material.Diamond;

                    case Material.Iron:
                        return toolMaterial == Material.Iron
                            || toolMaterial == Material.Diamond;

                    case Material.Diamond:
                        return toolMaterial == Material.Diamond;

                    default:
                        return false;
                }
            }
            else
            {
                return true;
            }
        }

        //Normally you'd need to access this over the component, however since it's used a lot I opted for a shortcut
        public Inventory GetInventory()
        {
            BlockInventory invComp = GetComponent<BlockInventory>();
            return invComp.inventory;
        }

        //-- Event Handlers --//
        public void HandleMouseEnter()
        {

        }

        public void HandleMouseLeave()
        {
            hammering = false;
            breaking = false;
        }

        public void HandleMouseLeftDown()
        {
            breakProgress = 0;
            breaking = true;
        }

        public void HandleMouseLeftUp()
        {
            breaking = false;
        }

        public void HandleMouseRightDown()
        {
            breakProgress = 0;

            if (Player.Get().inventory.GetSelectedItem() is ToolItem tool && tool.type == Tool.Hammer)
            {
                hammering = true;
            }
            else
            {
                ClickHandler.DoRightClick(this);
            }
        }

        public void HandleMouseRightUp()
        {
            hammering = false;
        }

        private void UpdateBreaking()
        {
            hammering = false;

            if (!IsInRange() || HasTag(BlockTags.UNBREAKABLE)) return;

            Block effectiveBlock = foregroundBlock ?? this; //The block which the break get's performed on

            if (World.Get().gamemode == Gamemode.Creative || breakTime == 0)
            {
                //Instantly perform the break when in creative or when time is 0
                ClickHandler.DoLeftClick(this);
            }
            else
            {
                //Default breakpower to 1. If the right tool is selected, apply that breakpower
                double breakPower = 1;
                if (Player.Get().inventory.GetSelectedItem() is ToolItem tool && effectiveBlock.effectiveTool == tool.type && effectiveBlock.ToolIsCorrectMaterial(tool.material))
                {
                    breakPower = tool.breakPower == 0 ? 1 : tool.breakPower;
                }

                effectiveBlock.breakProgress += 1 * breakPower;

                if (effectiveBlock.breakProgress >= effectiveBlock.breakTimeTicks) ClickHandler.DoLeftClick(effectiveBlock);
            }
        }

        public void UpdateHammering()
        {
            breaking = false;

            if (Player.Get().inventory.GetSelectedItem() is ToolItem tool && tool.type == Tool.Hammer)
            {
                //If the player holds a hammer, is in gamemode survival, the block is in range and doesn't have a foreground block
                if (IsInRange() && foregroundBlock == null && !HasTag(BlockTags.CANT_BE_BACKGROUND))
                {
                    //Get the break power from the selected tool
                    double breakPower = tool.breakPower == 0 ? 1 : tool.breakPower;
                    breakProgress += 1 * breakPower;

                    if (breakProgress >= breakTimeTicks || World.Get().gamemode == Gamemode.Creative)
                    {
                        ClickHandler.DoRightClick(this);
                        hammering = false;
                    }
                }
            }
        }

    }

    public enum Tool
    {
        Pickaxe,
        Axe,
        Shovel,
        Scythe,
        Sword,
        Hammer,
        None
    }

    public enum Material
    {
        Wood,
        Stone,
        Tin,
        Iron,
        Diamond,
        None
    }
}