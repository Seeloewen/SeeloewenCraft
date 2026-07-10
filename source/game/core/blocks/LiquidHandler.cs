using SeeloewenCraft.game.util;
using SeeloewenCraft.game.core.world;

namespace SeeloewenCraft.game.core.blocks
{
    internal static class LiquidHandler
    {
        /*
         * Warning: To avoid possible confusion, I need to clarify that block can be both a normal or a foreground block,
         * it does not matter and will work either way
         */

        internal static void DoUpdate(LiquidBlock block)
        {
            if (block == null) return;

            //If it's not a permanent source block, check if it still has its source
            //If not, replace it with air
            if (!SourceExists(block))
            {
                World.Get().SetBlock(block.GetPosData(), BlockRegister.Get("sc:air_block"));
                return;
            }

            //Try expanding the liquid
            foreach (var n in TryExpand(block))
            {
                if (n.b != null) World.Get().SetBlock(n.pos, n.b);
            }
        }

        private static bool SourceExists(LiquidBlock block)
        {
            if (block.liquidSource.x == -1 || block.liquidSource.y == -1) return true;

            Block source = World.Get().GetBlock(block.liquidSource.x, block.liquidSource.y, block.liquidSource.ci);
            Block sourceForeground = source.GetForegroundBlock();

            //Check that the block at the specified location is still a liquid of the same type
            return (source != null && source is LiquidBlock l && l.liquidTag == block.liquidTag //Source Normal
                || sourceForeground != null && sourceForeground is LiquidBlock lf && lf.liquidTag == block.liquidTag); //Source foreground
        }

        private static (LiquidBlock b, PositionData pos)[] TryExpand(LiquidBlock block)
        {
            var newBlocks = new (LiquidBlock b, PositionData pos)[2];

            bool canExpandDown = CanExpandTowards(block, Direction.DOWN);

            if (canExpandDown) //Prioritize downwards flow
            {
                PositionData source = block.GetPosData();
                PositionData newBlock = source.Move(0, 1);
                newBlocks[0] = (block.GetLiquid(6, Direction.DOWN, block.GetPosData()), newBlock);
            }
            else if (!canExpandDown && !block.GetBlockBelow().HasTag(block.liquidTag)) //If no expansion downwards was possible, try to expand to the sides
            {
                if (block.liquidLevel == 1 || block.chunk == null) return newBlocks; //Only expand if the water hasn't reached the end of the stream

                if (CanExpandTowards(block, Direction.RIGHT))
                {
                    PositionData source = block.GetPosData();
                    PositionData newBlock = source.Move(1, 0);
                    newBlocks[0] = (block.GetLiquid(block.liquidLevel - 1, Direction.RIGHT, source), newBlock);
                }

                if (CanExpandTowards(block, Direction.LEFT))
                {
                    PositionData source = block.GetPosData();
                    PositionData newBlock = source.Move(-1, 0);
                    int i = newBlocks[0].b == null ? 0 : 1; //Index in array may be different, depending on whether extension to the right worked
                    newBlocks[i] = (block.GetLiquid(block.liquidLevel - 1, Direction.LEFT, source), newBlock);
                }
            }

            return newBlocks;
        }

        /*
         * Warning: The following checks are absolutely scuffed and hard to read. Please forgive me.
         * Best wishes to anyone working on this in the future.
         * PS: It was even worse before the rewrite
         */

        private static bool CanExpandTowards(LiquidBlock block, Direction dir)
        {
            Block b = dir switch
            {
                Direction.DOWN => block.GetBlockBelow(),
                Direction.RIGHT => block.GetBlockRight(),
                Direction.LEFT => block.GetBlockLeft(),
                _ => null
            };

            if (b == null) return false;

            LiquidBlock l; //Used in case the regarded block is liquid
            bool replaceable;
            bool sameLiquidButLower;

            if (!b.isBackground) //Differentiate check if block below is background
            {
                l = b is LiquidBlock liquid ? liquid : null;

                replaceable = b.HasTag(BlockTags.REPLACEABLE);
                sameLiquidButLower = l != null
                    && l.liquidTag == block.liquidTag
                    && ((l.liquidLevel - block.liquidLevel < -1) || (dir == Direction.DOWN && l.liquidLevel != 6)); //The liquid needs to be at least one lower for the expansion to make sense (if expanding to sides)
            }
            else //If the block below is in background, the background block can be ignored and only the foreground block matters
            {
                Block f = b.GetForegroundBlock();
                l = (f != null && f is LiquidBlock liquid) ? liquid : null;

                replaceable = f == null
                    || f.HasTag(BlockTags.REPLACEABLE);
                sameLiquidButLower = l != null
                    && l.liquidTag == block.liquidTag
                    && ((l.liquidLevel - block.liquidLevel < -1) || (dir == Direction.DOWN && l.liquidLevel != 6));
            }

            return sameLiquidButLower || (l == null && replaceable);
        }
    }
}
