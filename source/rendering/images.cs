using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;

namespace SeeloewenCraft
{
    public static class Images //This class contains all the references to the image resources
    {
        //-- Custom Methods --//

        public static string textureDirectory;

        public static void Init()
        {
            //Check which texurepack is selected and set the directory based on that
            if (Game.selectedTexturepack == "default" || string.IsNullOrEmpty(Game.selectedTexturepack))
            {
                textureDirectory = "pack://application:,,,/SeeloewenCraft;component/Resources";
                Log.Write($"Set texture directory to 'pack://application:,,,/SeeloewenCraft;component/Resources' (internal resources)", "Info");
            }
            else if (Game.selectedTexturepack != "default" && !string.IsNullOrEmpty(Game.selectedTexturepack))
            {
                textureDirectory = Game.selectedTexturepack;
                Log.Write($"Set texture directory to '{Game.selectedTexturepack}'", "Info");
            }

            //Actually load the resources
            CreateResources();
        }

        public static ImageSource GetImageSource(string resourceName, TextureType type)
        {
            Uri imageUri;

            //Try to get the image file if a texturepack is loaded
            imageUri = GetTexturepackUri(resourceName, type);

            //If no texturepack is loaded or it's not found in the texturepack, fallback to default ones
            if (imageUri == null)
            {
                imageUri = GetInternalUri(resourceName, type);
            }

            //Get an image from an imagesource from a uri
            return BitmapFrame.Create(imageUri);
        }

        public static bool ResourceExists(Uri resourceUri)
        {
            //Check if the resource exists
            try
            {
                var resourceInfo = Application.GetResourceStream(resourceUri);
                return resourceInfo != null;
            }
            catch
            {
                return false;
            }
        }

        public static Uri GetTexturepackUri(string resourceName, TextureType type)
        {
            Uri imageUri;

            //Get the uri from texturepack
            switch (type)
            {
                case TextureType.Block:
                    imageUri = new Uri($"{textureDirectory}/Blocks/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Item:
                    imageUri = new Uri($"{textureDirectory}/Items/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Gui:
                    imageUri = new Uri($"{textureDirectory}/Gui/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Entity:
                    imageUri = new Uri($"{textureDirectory}/Entities/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Chiseled_Block:
                    imageUri = new Uri($"{textureDirectory}/Blocks/Chiseled/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Connected_Block:
                    imageUri = new Uri($"{textureDirectory}/Blocks/Connected/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.States_Block:
                    imageUri = new Uri($"{textureDirectory}/Blocks/States/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Tool_Item:
                    imageUri = new Uri($"{textureDirectory}/Items/Tools/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.General:
                    imageUri = new Uri($"{textureDirectory}/{resourceName}", UriKind.Absolute);
                    break;
                default:
                    imageUri = new Uri($"{textureDirectory}/{resourceName}", UriKind.Absolute);
                    break;
            }

            if (File.Exists(imageUri.LocalPath))
            {
                return imageUri;
            }
            else
            {
                return null;
            }
        }

        public static Uri GetInternalUri(string resourceName, TextureType type)
        {
            Uri imageUri;

            //Get the internal resource
            switch (type)
            {
                case TextureType.Block:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Blocks/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Item:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Items/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Gui:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Gui/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Entity:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Entities/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Chiseled_Block:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Blocks/Chiseled/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Connected_Block:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Blocks/Connected/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.States_Block:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Blocks/States/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.Tool_Item:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Items/Tools/{resourceName}", UriKind.Absolute);
                    break;
                case TextureType.General:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/{resourceName}", UriKind.Absolute);
                    break;
                default:
                    imageUri = new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/{resourceName}", UriKind.Absolute);
                    break;
            }

            //Check if the resource exists and return it
            if (ResourceExists(imageUri))
            {
                return imageUri;
            }
            else
            {
                //If default one doesn't exist, use missing texture image
                Log.Write($"Could not load texture {resourceName} (Type '{type}'): No image file in texturepack or internally found. Is the type and file name correct?", "Error");
                return new Uri($"pack://application:,,,/SeeloewenCraft;component/Resources/textures/Missing_Texture.png", UriKind.Absolute);
            }
        }

        private static void CreateResources()
        {
            Background = new SealImage(TextureType.Gui, "Background.png");
            GrassBlock = new SealImage(TextureType.Block, "Grass_Block.png");
            StoneBlock = new SealImage(TextureType.Block, "Stone_Block.png");
            Dirt = new SealImage(TextureType.Block, "Dirt.png");
            Air = new SealImage(TextureType.Block, "Air.png");
            Bedrock = new SealImage(TextureType.Block, "Bedrockdsf.png");
            CoalOre = new SealImage(TextureType.Block, "Coal_Ore.png");
            DiamondOre = new SealImage(TextureType.Block, "Diamond_Ore.png");
            IronOre = new SealImage(TextureType.Block, "Iron_Ore.png");
            OakLog = new SealImage(TextureType.Block, "Oak_Log.png");
            OakLeaves = new SealImage(TextureType.Block, "Oak_Leaves.png");
            Chest = new SealImage(TextureType.Block, "Chest.png");
            MagmaBlock = new SealImage(TextureType.Block, "Magma_Block.png");
            MissingTexture = new SealImage(TextureType.General, "Missing_Texture.png");
            StoneHammer = new SealImage(TextureType.Item, "Stone_Hammer.png");
            SpruceLog = new SealImage(TextureType.Block, "Spruce_Log.png");
            SpruceLeaves = new SealImage(TextureType.Block, "Spruce_Leaves.png");
            Torch = new SealImage(TextureType.Block, "Torch.png");
            PottedCactus_Base = new SealImage(TextureType.Connected_Block, "Potted_Cactus_Base.png");
            PottedCactus_Top = new SealImage(TextureType.Connected_Block, "Potted_Cactus_Top.png");
            PottedCactus = new SealImage(TextureType.Connected_Block, "Potted_Cactus.png");
            Water_1_Right = new SealImage(TextureType.Block, "Water_1_Right.png");
            Water_1_Left = new SealImage(TextureType.Block, "Water_1_Left.png");
            Water_2_Right = new SealImage(TextureType.Block, "Water_2_Right.png");
            Water_2_Left = new SealImage(TextureType.Block, "Water_2_Left.png");
            Water_3_Right = new SealImage(TextureType.Block, "Water_3_Right.png");
            Water_3_Left = new SealImage(TextureType.Block, "Water_3_Left.png");
            Water_4_Right = new SealImage(TextureType.Block, "Water_4_Right.png");
            Water_4_Left = new SealImage(TextureType.Block, "Water_4_Left.png");
            Water_5_Right = new SealImage(TextureType.Block, "Water_5_Right.png");
            Water_5_Left = new SealImage(TextureType.Block, "Water_5_Left.png");
            Water_6 = new SealImage(TextureType.Block, "Water_6.png");
            Gui = new SealImage(TextureType.Gui, "Gui.png");
            Heart_Full = new SealImage(TextureType.Gui, "Heart_Full.png");
            Heart_Half = new SealImage(TextureType.Gui, "Heart_Half.png");
            Heart_Empty = new SealImage(TextureType.Gui, "Heart_Empty.png");
            CraftingTable = new SealImage(TextureType.Block, "Crafting_Table.png");
            CobbleStoneBlock = new SealImage(TextureType.Block, "Cobblestone.png");
            CobbleStoneBlock_BottomRight = new SealImage(TextureType.Chiseled_Block, "Cobblestone_BottomRight.png");
            CobbleStoneBlock_BottomLeft = new SealImage(TextureType.Chiseled_Block, "Cobblestone_BottomLeft.png");
            CobbleStoneBlock_TopRight = new SealImage(TextureType.Chiseled_Block, "Cobblestone_TopRight.png");
            CobbleStoneBlock_TopLeft = new SealImage(TextureType.Chiseled_Block, "Cobblestone_TopLeft.png");
            CobbleStoneBlock_SlabBottom = new SealImage(TextureType.Chiseled_Block, "Cobblestone_SlabBottom.png");
            CobbleStoneBlock_SlabTop = new SealImage(TextureType.Chiseled_Block, "Cobblestone_SlabTop.png");
            CobbleStoneBlock_SlabLeft = new SealImage(TextureType.Chiseled_Block, "Cobblestone_SlabLeft.png");
            CobbleStoneBlock_SlabRight = new SealImage(TextureType.Chiseled_Block, "Cobblestone_SlabRight.png");
            CobbleStoneBlock_StairBottomRight = new SealImage(TextureType.Chiseled_Block, "Cobblestone_StairBottomRight.png");
            CobbleStoneBlock_StairBottomLeft = new SealImage(TextureType.Chiseled_Block, "Cobblestone_StairBottomLeft.png");
            CobbleStoneBlock_StairTopRight = new SealImage(TextureType.Chiseled_Block, "Cobblestone_StairTopRight.png");
            CobbleStoneBlock_StairTopLeft = new SealImage(TextureType.Chiseled_Block, "Cobblestone_StairTopLeft.png");
            CobbleStoneBlock_Center = new SealImage(TextureType.Chiseled_Block, "Cobblestone_Center.png");
            Chiseler = new SealImage(TextureType.Block, "Chiseler.png");
            Unchiseler = new SealImage(TextureType.Block, "Unchiseler.png");
            Break_1 = new SealImage(TextureType.General, "Break_1.png");
            Break_2 = new SealImage(TextureType.General, "Break_2.png");
            Break_3 = new SealImage(TextureType.General, "Break_3.png");
            Break_4 = new SealImage(TextureType.General, "Break_4.png");
            Break_5 = new SealImage(TextureType.General, "Break_5.png");
            Slime_Green = new SealImage(TextureType.Entity, "Slime_Green.png");
            Slime_Blue = new SealImage(TextureType.Entity, "Slime_Blue.png");
            Slime_Red = new SealImage(TextureType.Entity, "Slime_Red.png");
            Slime_Magenta = new SealImage(TextureType.Entity, "Slime_Magenta.png");
            Slime_Yellow = new SealImage(TextureType.Entity, "Slime_Yellow.png");
            Hammer_1 = new SealImage(TextureType.General, "Hammer_1.png");
            Hammer_2 = new SealImage(TextureType.General, "Hammer_2.png");
            Hammer_3 = new SealImage(TextureType.General, "Hammer_3.png");
            Hammer_4 = new SealImage(TextureType.General, "Hammer_4.png");
            Hammer_5 = new SealImage(TextureType.General, "Hammer_5.png");
            Bone = new SealImage(TextureType.Item, "Bone.png");
            Arrow = new SealImage(TextureType.Item, "Arrow.png");
            Snowball = new SealImage(TextureType.Item, "Snowball.png");
            SpruceDoor_Closed_Top = new SealImage(TextureType.States_Block, "Spruce_Door_Closed_Top.png");
            SpruceDoor_Closed_Base = new SealImage(TextureType.States_Block, "Spruce_Door_Closed_Base.png");
            SpruceDoor_Open_Top = new SealImage(TextureType.States_Block, "Spruce_Door_Open_Top.png");
            SpruceDoor_Open_Base = new SealImage(TextureType.States_Block, "Spruce_Door_Open_Base.png");
            SpruceDoor = new SealImage(TextureType.Item, "Spruce_Door_Item.png");
            Amethyst = new SealImage(TextureType.Item, "Amethyst.png");
            AmethystOre = new SealImage(TextureType.Block, "Amethyst_Ore.png");
            Anvil = new SealImage(TextureType.Block, "Anvil.png");
            Apple = new SealImage(TextureType.Item, "Apple.png");
            ArcheologyPot = new SealImage(TextureType.Item, "Archeology_Pot.png");
            ArcheologyPot_Base = new SealImage(TextureType.Connected_Block, "Archeology_Pot_Base.png");
            ArcheologyPot_Top = new SealImage(TextureType.Connected_Block, "Archeology_Pot_Top.png");
            Barrel = new SealImage(TextureType.Block, "Barrel.png");
            BlueFlower = new SealImage(TextureType.Block, "Blue_Flower.png");
            BoneBlock = new SealImage(TextureType.Block, "Bone_Block.png");
            Bread = new SealImage(TextureType.Item, "Bread.png");
            BucketEmpty = new SealImage(TextureType.Item, "Bucket_Empty.png");
            BucketWater = new SealImage(TextureType.Item, "Bucket_Water.png");
            Cactus_BottomLeft = new SealImage(TextureType.Connected_Block, "Cactus_BottomLeft.png");
            Cactus_BottomRight = new SealImage(TextureType.Connected_Block, "Cactus_BottomRight.png");
            Cactus_Cross = new SealImage(TextureType.Connected_Block, "Cactus_Cross.png");
            CactusFruit = new SealImage(TextureType.Block, "Cactus_Fruit.png");
            Cactus_Horizontal = new SealImage(TextureType.Connected_Block, "Cactus_Horizontal.png");
            Cactus_Top = new SealImage(TextureType.Connected_Block, "Cactus_Top.png");
            Cactus_TopLeft = new SealImage(TextureType.Connected_Block, "Cactus_TopLeft.png");
            Cactus_TopRight = new SealImage(TextureType.Connected_Block, "Cactus_TopRight.png");
            Cactus_Top_Fruit = new SealImage(TextureType.Connected_Block, "Cactus_Top_Fruit.png");
            Cactus_Vertical = new SealImage(TextureType.Connected_Block, "Cactus_Vertical.png");
            Candle = new SealImage(TextureType.Block, "Candle.png");
            Coal = new SealImage(TextureType.Item, "Coal.png");
            CopperBar = new SealImage(TextureType.Item, "Copper_Bar.png");
            CopperOre = new SealImage(TextureType.Block, "Copper_Ore.png");
            Croissant = new SealImage(TextureType.Item, "Croissant.png");
            DeadBush = new SealImage(TextureType.Block, "Dead_Bush.png");
            Diamond = new SealImage(TextureType.Item, "Diamond.png");
            DiamondAxe = new SealImage(TextureType.Tool_Item, "Diamond_Axe.png");
            DiamondHammer = new SealImage(TextureType.Tool_Item, "Diamond_Hammer.png");
            DiamondPickaxe = new SealImage(TextureType.Tool_Item, "Diamond_Pickaxe.png");
            DiamondScythe = new SealImage(TextureType.Tool_Item, "Diamond_Scythe.png");
            DiamondShovel = new SealImage(TextureType.Tool_Item, "Diamond_Shovel.png");
            DiamondSword = new SealImage(TextureType.Tool_Item, "Diamond_Sword.png");
            Emerald = new SealImage(TextureType.Item, "Emerald.png");
            EmeraldOre = new SealImage(TextureType.Block, "Emerald_Ore.png");
            FlowerPot = new SealImage(TextureType.Block, "Flower_Pot.png");
            FossilFragment = new SealImage(TextureType.Item, "Fossil_Fragment.png");
            Furnace_Idle = new SealImage(TextureType.States_Block, "Furnace_Idle.png");
            Furnace_Running = new SealImage(TextureType.States_Block, "Furnace_Running.png");
            GoldBar = new SealImage(TextureType.Item, "Gold_Bar.png");
            GoldOre = new SealImage(TextureType.Block, "Gold_Ore.png");
            Grass = new SealImage(TextureType.Block, "Grass.png");
            IronAxe = new SealImage(TextureType.Tool_Item, "Iron_Axe.png");
            IronBar = new SealImage(TextureType.Item, "Iron_Bar.png");
            IronGates = new SealImage(TextureType.Block, "Iron_Gates.png");
            IronHammer = new SealImage(TextureType.Tool_Item, "Iron_Hammer.png");
            IronPickaxe = new SealImage(TextureType.Tool_Item, "Iron_Pickaxe.png");
            IronRod = new SealImage(TextureType.Item, "Iron_Rod.png");
            IronScythe = new SealImage(TextureType.Tool_Item, "Iron_Scythe.png");
            IronShovel = new SealImage(TextureType.Tool_Item, "Iron_Shovel.png");
            IronSword = new SealImage(TextureType.Tool_Item, "Iron_Sword.png");
            Ladder = new SealImage(TextureType.Block, "Ladder.png");
            MossyCobblestone = new SealImage(TextureType.Block, "Mossy_Cobblestone.png");
            OakChair = new SealImage(TextureType.Item, "Oak_Chair.png");
            OakChair_Bottom = new SealImage(TextureType.Connected_Block, "Oak_Chair_Bottom.png");
            OakChair_Top = new SealImage(TextureType.Connected_Block, "Oak_Chair_Top.png");
            OakDoor_Closed_Base = new SealImage(TextureType.States_Block, "Oak_Door_Closed_Base.png");
            OakDoor_Closed_Top = new SealImage(TextureType.States_Block, "Oak_Door_Closed_Top.png");
            OakDoorItem = new SealImage(TextureType.Item, "Oak_Door_Item.png");
            OakDoor_Open_Base = new SealImage(TextureType.States_Block, "Oak_Door_Open_Base.png");
            OakDoor_Open_Top = new SealImage(TextureType.States_Block, "Oak_Door_Open_Top.png");
            OakPlanks = new SealImage(TextureType.Block, "Oak_Planks.png");
            OakPlanksBlock_StairBottomLeft = new SealImage(TextureType.Chiseled_Block, "Oak_Planks_StairBottomLeft.png");
            OakPlanksBlock_StairBottomRight = new SealImage(TextureType.Chiseled_Block, "Oak_Planks_StairBottomRight.png");
            OakPlanksBlock_StairTopLeft = new SealImage(TextureType.Chiseled_Block, "Oak_Planks_StairTopLeft.png");
            OakPlanksBlock_StairTopRight = new SealImage(TextureType.Chiseled_Block, "Oak_Planks_StairTopRight.png");
            OakPlanksBlock_BottomLeft = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_BottomLeft.png");
            OakPlanksBlock_BottomRight = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_BottomRight.png");
            OakPlanksBlock_Center = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_Center.png");
            OakPlanksBlock_SlabBottom = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_SlabBottom.png");
            OakPlanksBlock_SlabLeft = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_SlabLeft.png");
            OakPlanksBlock_SlabRight = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_SlabRight.png");
            OakPlanksBlock_SlabTop = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_SlabTop.png");
            OakPlanksBlock_TopLeft = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_TopLeft.png");
            OakPlanksBlock_TopRight = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_TopRight.png");
            OakSapling = new SealImage(TextureType.Block, "Oak_Sapling.png");
            OakTable = new SealImage(TextureType.Block, "Oak_Table.png");
            OakTrapdoor_Closed = new SealImage(TextureType.States_Block, "Oak_Trapdoor_Closed.png");
            OakTrapdoor_Open = new SealImage(TextureType.States_Block, "Oak_Trapdoor_Open.png");
            Paper = new SealImage(TextureType.Item, "Paper.png");
            PotShard = new SealImage(TextureType.Item, "Pot_Shard.png");
            Rock = new SealImage(TextureType.Item, "Rock.png");
            Sand = new SealImage(TextureType.Block, "Sand.png");
            SandStone = new SealImage(TextureType.Block, "Sand_Stone.png");
            SandStoneBlock_BottomLeft = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_BottomLeft.png");
            SandStoneBlock_BottomRight = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_BottomRight.png");
            SandStoneBricks = new SealImage(TextureType.Block, "Sand_Stone_Bricks.png");
            SandStoneBlock_Center = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_Center.png");
            SandStoneBlock_SlabBottom = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_SlabBottom.png");
            SandStoneBlock_SlabLeft = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_SlabLeft.png");
            SandStoneBlock_SlabRight = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_SlabRight.png");
            SandStoneBlock_SlabTop = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_SlabTop.png");
            SandStoneBlock_StairBottomLeft = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_StairBottomLeft.png");
            SandStoneBlock_StairBottomRight = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_StairBottomRight.png");
            SandStoneBlock_StairTopLeft = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_StairTopLeft.png");
            SandStoneBlock_StairTopRight = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_StairTopRight.png");
            SandStoneBlock_TopLeft = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_TopLeft.png");
            SandStoneBlock_TopRight = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_TopRight.png");
            SpruceChair = new SealImage(TextureType.Item, "Spruce_Chair.png");
            SpruceChair_Bottom = new SealImage(TextureType.Connected_Block, "Spruce_Chair_Bottom.png");
            SpruceChair_Top = new SealImage(TextureType.Connected_Block, "Spruce_Chair_Top.png");
            SprucePlanks = new SealImage(TextureType.Block, "Spruce_Planks.png");
            SprucePlanksBlock_BottomLeft = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_BottomLeft.png");
            SprucePlanksBlock_BottomRight = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_BottomRight.png");
            SprucePlanksBlock_Center = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_Center.png");
            SprucePlanksBlock_SlabBottom = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_SlabBottom.png");
            SprucePlanksBlock_SlabLeft = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_SlabLeft.png");
            SprucePlanksBlock_SlabRight = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_SlabRight.png");
            SprucePlanksBlock_SlabTop = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_SlabTop.png");
            SprucePlanksBlock_StairBottomLeft = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_StairBottomLeft.png");
            SprucePlanksBlock_StairBottomRight = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_StairBottomRight.png");
            SprucePlanksBlock_StairTopLeft = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_StairTopLeft.png");
            SprucePlanksBlock_StairTopRight = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_StairTopRight.png");
            SprucePlanksBlock_TopLeft = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_TopLeft.png");
            SprucePlanksBlock_TopRight = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_TopRight.png");
            SpruceSapling = new SealImage(TextureType.Block, "Spruce_Sapling.png");
            SpruceTable = new SealImage(TextureType.Block, "Spruce_Table.png");
            SpruceTrapdoor_Closed = new SealImage(TextureType.States_Block, "Spruce_Trapdoor_Closed.png");
            SpruceTrapdoor_Item = new SealImage(TextureType.Item, "Spruce_Trapdoor_Item.png");
            SpruceTrapdoor_Open = new SealImage(TextureType.States_Block, "Spruce_Trapdoor_Open.png");
            Stick = new SealImage(TextureType.Item, "Stick.png");
            StoneAxe = new SealImage(TextureType.Tool_Item, "Stone_Axe.png");
            StoneBricks = new SealImage(TextureType.Block, "Stone_Bricks.png");
            StonePickaxe = new SealImage(TextureType.Tool_Item, "Stone_Pickaxe.png");
            StoneScythe = new SealImage(TextureType.Tool_Item, "Stone_Scythe.png");
            StoneShovel = new SealImage(TextureType.Tool_Item, "Stone_Shovel.png");
            StoneSword = new SealImage(TextureType.Tool_Item, "Stone_Sword.png");
            TinAxe = new SealImage(TextureType.Tool_Item, "Tin_Axe.png");
            TinBar = new SealImage(TextureType.Item, "Tin_Bar.png");
            TinHammer = new SealImage(TextureType.Tool_Item, "Tin_Hammer.png");
            TinOre = new SealImage(TextureType.Block, "Tin_Ore.png");
            TinPickaxe = new SealImage(TextureType.Tool_Item, "Tin_Pickaxe.png");
            TinScythe = new SealImage(TextureType.Tool_Item, "Tin_Scythe.png");
            TinShovel = new SealImage(TextureType.Tool_Item, "Tin_Shovel.png");
            TinSword = new SealImage(TextureType.Tool_Item, "Tin_Sword.png");
            TungstenBar = new SealImage(TextureType.Item, "Tungsten_Bar.png");
            TungstenOre = new SealImage(TextureType.Block, "Tungsten_Ore.png");
            Wax = new SealImage(TextureType.Item, "Wax.png");
            WoodAxe = new SealImage(TextureType.Tool_Item, "Wood_Axe.png");
            WoodHammer = new SealImage(TextureType.Tool_Item, "Wood_Hammer.png");
            WoodPickaxe = new SealImage(TextureType.Tool_Item, "Wood_Pickaxe.png");
            WoodScythe = new SealImage(TextureType.Tool_Item, "Wood_Scythe.png");
            WoodShovel = new SealImage(TextureType.Tool_Item, "Wood_Shovel.png");
            WoodSword = new SealImage(TextureType.Tool_Item, "Wood_Sword.png");
            YellowFlower = new SealImage(TextureType.Block, "Yellow_Flower.png");

        }

        //-- Images --//

        public static SealImage Background;
        public static SealImage GrassBlock;
        public static SealImage StoneBlock;
        public static SealImage Dirt;
        public static SealImage Air;
        public static SealImage Bedrock;
        public static SealImage CoalOre;
        public static SealImage DiamondOre;
        public static SealImage IronOre;
        public static SealImage OakLog;
        public static SealImage OakLeaves;
        public static SealImage Chest;
        public static SealImage MagmaBlock;
        public static SealImage MissingTexture;
        public static SealImage StoneHammer;
        public static SealImage SpruceLog;
        public static SealImage SpruceLeaves;
        public static SealImage Torch;
        public static SealImage PottedCactus_Top;
        public static SealImage PottedCactus_Base;
        public static SealImage PottedCactus;
        public static SealImage Water_1_Right;
        public static SealImage Water_1_Left;
        public static SealImage Water_2_Right;
        public static SealImage Water_2_Left;
        public static SealImage Water_3_Right;
        public static SealImage Water_3_Left;
        public static SealImage Water_4_Right;
        public static SealImage Water_4_Left;
        public static SealImage Water_5_Right;
        public static SealImage Water_5_Left;
        public static SealImage Water_6;
        public static SealImage Gui;
        public static SealImage Heart_Full;
        public static SealImage Heart_Half;
        public static SealImage Heart_Empty;
        public static SealImage CraftingTable;
        public static SealImage CobbleStoneBlock;
        public static SealImage CobbleStoneBlock_BottomRight;
        public static SealImage CobbleStoneBlock_BottomLeft;
        public static SealImage CobbleStoneBlock_TopRight;
        public static SealImage CobbleStoneBlock_TopLeft;
        public static SealImage CobbleStoneBlock_SlabBottom;
        public static SealImage CobbleStoneBlock_SlabTop;
        public static SealImage CobbleStoneBlock_SlabLeft;
        public static SealImage CobbleStoneBlock_SlabRight;
        public static SealImage CobbleStoneBlock_StairBottomRight;
        public static SealImage CobbleStoneBlock_StairBottomLeft;
        public static SealImage CobbleStoneBlock_StairTopRight;
        public static SealImage CobbleStoneBlock_StairTopLeft;
        public static SealImage CobbleStoneBlock_Center;
        public static SealImage Chiseler;
        public static SealImage Unchiseler;
        public static SealImage Break_1;
        public static SealImage Break_2;
        public static SealImage Break_3;
        public static SealImage Break_4;
        public static SealImage Break_5;
        public static SealImage Slime_Green;
        public static SealImage Slime_Blue;
        public static SealImage Slime_Magenta;
        public static SealImage Slime_Yellow;
        public static SealImage Slime_Red;
        public static SealImage Hammer_1;
        public static SealImage Hammer_2;
        public static SealImage Hammer_3;
        public static SealImage Hammer_4;
        public static SealImage Hammer_5;
        public static SealImage Bone;
        public static SealImage Arrow;
        public static SealImage Snowball;
        public static SealImage SpruceDoor_Open_Top;
        public static SealImage SpruceDoor_Open_Base;
        public static SealImage SpruceDoor_Closed_Top;
        public static SealImage SpruceDoor_Closed_Base;
        public static SealImage SpruceDoor;
        public static SealImage Amethyst;
        public static SealImage AmethystOre;
        public static SealImage Anvil;
        public static SealImage Apple;
        public static SealImage ArcheologyPot;
        public static SealImage ArcheologyPot_Base;
        public static SealImage ArcheologyPot_Top;
        public static SealImage Barrel;
        public static SealImage BlueFlower;
        public static SealImage BoneBlock;
        public static SealImage Bread;
        public static SealImage BucketEmpty;
        public static SealImage BucketWater;
        public static SealImage Cactus_BottomLeft;
        public static SealImage Cactus_BottomRight;
        public static SealImage Cactus_Cross;
        public static SealImage CactusFruit;
        public static SealImage Cactus_Horizontal;
        public static SealImage Cactus_Top;
        public static SealImage Cactus_TopLeft;
        public static SealImage Cactus_TopRight;
        public static SealImage Cactus_Top_Fruit;
        public static SealImage Cactus_Vertical;
        public static SealImage Candle;
        public static SealImage Coal;
        public static SealImage CopperBar;
        public static SealImage CopperOre;
        public static SealImage Croissant;
        public static SealImage DeadBush;
        public static SealImage Diamond;
        public static SealImage DiamondAxe;
        public static SealImage DiamondHammer;
        public static SealImage DiamondPickaxe;
        public static SealImage DiamondScythe;
        public static SealImage DiamondShovel;
        public static SealImage DiamondSword;
        public static SealImage Emerald;
        public static SealImage EmeraldOre;
        public static SealImage FlowerPot;
        public static SealImage FossilFragment;
        public static SealImage Furnace_Idle;
        public static SealImage Furnace_Running;
        public static SealImage GoldBar;
        public static SealImage GoldOre;
        public static SealImage Grass;
        public static SealImage IronAxe;
        public static SealImage IronBar;
        public static SealImage IronGates;
        public static SealImage IronHammer;
        public static SealImage IronPickaxe;
        public static SealImage IronRod;
        public static SealImage IronScythe;
        public static SealImage IronShovel;
        public static SealImage IronSword;
        public static SealImage Ladder;
        public static SealImage MossyCobblestone;
        public static SealImage OakChair;
        public static SealImage OakChair_Bottom;
        public static SealImage OakChair_Top;
        public static SealImage OakDoor_Closed_Base;
        public static SealImage OakDoor_Closed_Top;
        public static SealImage OakDoorItem;
        public static SealImage OakDoor_Open_Base;
        public static SealImage OakDoor_Open_Top;
        public static SealImage OakPlanks;
        public static SealImage OakPlanksBlock_StairBottomLeft;
        public static SealImage OakPlanksBlock_StairBottomRight;
        public static SealImage OakPlanksBlock_StairTopLeft;
        public static SealImage OakPlanksBlock_StairTopRight;
        public static SealImage OakPlanksBlock_BottomLeft;
        public static SealImage OakPlanksBlock_BottomRight;
        public static SealImage OakPlanksBlock_Center;
        public static SealImage OakPlanksBlock_SlabBottom;
        public static SealImage OakPlanksBlock_SlabLeft;
        public static SealImage OakPlanksBlock_SlabRight;
        public static SealImage OakPlanksBlock_SlabTop;
        public static SealImage OakPlanksBlock_TopLeft;
        public static SealImage OakPlanksBlock_TopRight;
        public static SealImage OakSapling;
        public static SealImage OakTable;
        public static SealImage OakTrapdoor_Closed;
        public static SealImage OakTrapdoor_Open;
        public static SealImage Paper;
        public static SealImage PotShard;
        public static SealImage Rock;
        public static SealImage Sand;
        public static SealImage SandStone;
        public static SealImage SandStoneBlock_BottomLeft;
        public static SealImage SandStoneBlock_BottomRight;
        public static SealImage SandStoneBricks;
        public static SealImage SandStoneBlock_Center;
        public static SealImage SandStoneBlock_SlabBottom;
        public static SealImage SandStoneBlock_SlabLeft;
        public static SealImage SandStoneBlock_SlabRight;
        public static SealImage SandStoneBlock_SlabTop;
        public static SealImage SandStoneBlock_StairBottomLeft;
        public static SealImage SandStoneBlock_StairBottomRight;
        public static SealImage SandStoneBlock_StairTopLeft;
        public static SealImage SandStoneBlock_StairTopRight;
        public static SealImage SandStoneBlock_TopLeft;
        public static SealImage SandStoneBlock_TopRight;
        public static SealImage SpruceChair;
        public static SealImage SpruceChair_Bottom;
        public static SealImage SpruceChair_Top;
        public static SealImage SprucePlanks;
        public static SealImage SprucePlanksBlock_BottomLeft;
        public static SealImage SprucePlanksBlock_BottomRight;
        public static SealImage SprucePlanksBlock_Center;
        public static SealImage SprucePlanksBlock_SlabBottom;
        public static SealImage SprucePlanksBlock_SlabLeft;
        public static SealImage SprucePlanksBlock_SlabRight;
        public static SealImage SprucePlanksBlock_SlabTop;
        public static SealImage SprucePlanksBlock_StairBottomLeft;
        public static SealImage SprucePlanksBlock_StairBottomRight;
        public static SealImage SprucePlanksBlock_StairTopLeft;
        public static SealImage SprucePlanksBlock_StairTopRight;
        public static SealImage SprucePlanksBlock_TopLeft;
        public static SealImage SprucePlanksBlock_TopRight;
        public static SealImage SpruceSapling;
        public static SealImage SpruceTable;
        public static SealImage SpruceTrapdoor_Closed;
        public static SealImage SpruceTrapdoor_Item;
        public static SealImage SpruceTrapdoor_Open;
        public static SealImage Stick;
        public static SealImage StoneAxe;
        public static SealImage StoneBricks;
        public static SealImage StonePickaxe;
        public static SealImage StoneScythe;
        public static SealImage StoneShovel;
        public static SealImage StoneSword;
        public static SealImage TinAxe;
        public static SealImage TinBar;
        public static SealImage TinHammer;
        public static SealImage TinOre;
        public static SealImage TinPickaxe;
        public static SealImage TinScythe;
        public static SealImage TinShovel;
        public static SealImage TinSword;
        public static SealImage TungstenBar;
        public static SealImage TungstenOre;
        public static SealImage Wax;
        public static SealImage WoodAxe;
        public static SealImage WoodHammer;
        public static SealImage WoodPickaxe;
        public static SealImage WoodScythe;
        public static SealImage WoodShovel;
        public static SealImage WoodSword;
        public static SealImage YellowFlower;
    }

    public class SealImage
    {
        public ImageBrush texture = new ImageBrush();
        public TextureType type;
        public string name;
        public bool isCreated = false;

        public SealImage(TextureType type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public void CreateImage()
        {
            texture.ImageSource = Images.GetImageSource(name, type);
            isCreated = true;
        }

        public ImageBrush GetTexture()
        {
            if (!isCreated)
            {
                CreateImage();
            }

            return texture;
        }
    }

    public enum TextureType
    {
        Block,
        Item,
        Gui,
        Entity,
        Tool_Item,
        Chiseled_Block,
        Connected_Block,
        States_Block,
        General
    }
}


