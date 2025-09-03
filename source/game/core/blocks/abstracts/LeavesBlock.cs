using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.core.blocks
{
    public abstract class LeavesBlock : Block
    {
        public LeavesBlock(string name, string id, string itemId) : base(name, id, 125, itemId, Tool.None)
        {
            WriteTag(BlockTags.TYPES_LEAF);
        }

        protected override void DoSpecificUpdate()
        {
            //Don't try to despawn the leaves if they were placed manually or by a structure that isn't done generating
            if (HasTag(BlockTags.PLACED_MANUALLY)) return;

            //Check recursively if it has a connection to a log
            if (!HasAdjacentLog(this, new List<Block>()))
            {
                if (HasTag(BlockTags.STRUCTURE_LEAF)) return; //This means that it's part of a structure that isn't done generating
            }
            else
            {
                //If it has adjacent logs but still the structure tag, the tag can be safely removed
                if (HasTag(BlockTags.STRUCTURE_LEAF)) RemoveTag(BlockTags.STRUCTURE_LEAF);

                return;
            }

            //Leaves that should decay have a 33% chance to do so
            if (Game.rnd.Next(0, 3) == 0) BreakBlock(true, false, true);
        }

        public bool HasAdjacentLog(Block block, List<Block> visitedBlocks) //Check recursively if a connection to a log is found
        {
            //If the block is null or it was already visited, don't check it
            if (block == null || visitedBlocks.Contains(block))
            {
                return false;
            }

            visitedBlocks.Add(block);

            //If it's a log, stop the search
            if (block.HasTag(BlockTags.TYPES_LOG))
            {
                return true;
            }

            //If it's not a leaf, stop searching on this branch
            if (!block.HasTag(BlockTags.TYPES_LEAF))
            {
                return false;
            }

            Block blockBelow = block.GetBlockBelow();
            Block blockAbove = block.GetBlockAbove();
            Block blockRight = block.GetBlockRight();
            Block blockLeft = block.GetBlockLeft();

            //If it's a leaf, check if the adjacent blocks are connected to a log
            return HasAdjacentLog(blockBelow, visitedBlocks) || HasAdjacentLog(blockAbove, visitedBlocks) || HasAdjacentLog(blockRight, visitedBlocks) || HasAdjacentLog(blockLeft, visitedBlocks);

        }
    }
}
