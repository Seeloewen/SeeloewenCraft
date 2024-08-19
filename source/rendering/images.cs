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
            if (Game.selectedTexturepack == "default")
            {
                textureDirectory = "pack://application:,,,/SeeloewenCraft;component/Resources";
                Log.Write($"Set texture directory to 'pack://application:,,,/SeeloewenCraft;component/Resources' (internal resources)", "Info");
            }
            else
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
            Amethyst_Ore = new SealImage(TextureType.Block, "Amethyst_Ore.png");
            Anvil = new SealImage(TextureType.Block, "Anvil.png");
            Apple = new SealImage(TextureType.Item, "Apple.png");
            Archeology_Pot = new SealImage(TextureType.Item, "Archeology_Pot.png");
            Archeology_Pot_Base = new SealImage(TextureType.Connected_Block, "Archeology_Pot_Base.png");
            Archeology_Pot_Top = new SealImage(TextureType.Connected_Block, "Archeology_Pot_Top.png");
            Barrel = new SealImage(TextureType.Block, "Barrel.png");
            Blue_Flower = new SealImage(TextureType.Block, "Blue_Flower.png");
            Bone_Block = new SealImage(TextureType.Block, "Bone_Block.png");
            Bread = new SealImage(TextureType.Item, "Bread.png");
            Bucket_Empty = new SealImage(TextureType.Item, "Bucket_Empty.png");
            Bucket_Water = new SealImage(TextureType.Item, "Bucket_Water.png");
            Cactus_BottomLeft = new SealImage(TextureType.Connected_Block, "Cactus_BottomLeft.png");
            Cactus_BottomRight = new SealImage(TextureType.Connected_Block, "Cactus_BottomRight.png");
            Cactus_Cross = new SealImage(TextureType.Connected_Block, "Cactus_Cross.png");
            Cactus_Fruit = new SealImage(TextureType.Connected_Block, "Cactus_Fruit.png");
            Cactus_Horizontal = new SealImage(TextureType.Connected_Block, "Cactus_Horizontal.png");
            Cactus_Top = new SealImage(TextureType.Connected_Block, "Cactus_Top.png");
            Cactus_TopLeft = new SealImage(TextureType.Connected_Block, "Cactus_TopLeft.png");
            Cactus_TopRight = new SealImage(TextureType.Connected_Block, "Cactus_TopRight.png");
            Cactus_Top_Fruit = new SealImage(TextureType.Connected_Block, "Cactus_Top_Fruit.png");
            Cactus_Vertical = new SealImage(TextureType.Connected_Block, "Cactus_Vertical.png");
            Candle = new SealImage(TextureType.Block, "Candle.png");
            Coal = new SealImage(TextureType.Item, "Coal.png");
            Copper_Bar = new SealImage(TextureType.Item, "Copper_Bar.png");
            Copper_Ore = new SealImage(TextureType.Block, "Copper_Ore.png");
            Croissant = new SealImage(TextureType.Item, "Croissant.png");
            Dead_Bush = new SealImage(TextureType.Block, "Dead_Bush.png");
            Diamond = new SealImage(TextureType.Item, "Diamond.png");
            Diamond_Axe = new SealImage(TextureType.Tool_Item, "Diamond_Axe.png");
            Diamond_Hammer = new SealImage(TextureType.Tool_Item, "Diamond_Hammer.png");
            Diamond_Pickaxe = new SealImage(TextureType.Tool_Item, "Diamond_Pickaxe.png");
            Diamond_Scythe = new SealImage(TextureType.Tool_Item, "Diamond_Scythe.png");
            Diamond_Shovel = new SealImage(TextureType.Tool_Item, "Diamond_Shovel.png");
            Diamond_Sword = new SealImage(TextureType.Tool_Item, "Diamond_Sword.png");
            Emerald = new SealImage(TextureType.Item, "Emerald.png");
            Emerald_Ore = new SealImage(TextureType.Block, "Emerald_Ore.png");
            Flower_Pot = new SealImage(TextureType.Block, "Flower_Pot.png");
            Fossil_Fragment = new SealImage(TextureType.Item, "Fossil_Fragment.png");
            Furnace_Idle = new SealImage(TextureType.States_Block, "Furnace_Idle.png");
            Furnace_Running = new SealImage(TextureType.States_Block, "Furnace_Running.png");
            Gold_Bar = new SealImage(TextureType.Item, "Gold_Bar.png");
            Gold_Ore = new SealImage(TextureType.Block, "Gold_Ore.png");
            Grass = new SealImage(TextureType.Block, "Grass.png");
            Iron_Axe = new SealImage(TextureType.Tool_Item, "Iron_Axe.png");
            Iron_Bar = new SealImage(TextureType.Item, "Iron_Bar.png");
            Iron_Gates = new SealImage(TextureType.Block, "Iron_Gates.png");
            Iron_Hammer = new SealImage(TextureType.Tool_Item, "Iron_Hammer.png");
            Iron_Pickaxe = new SealImage(TextureType.Tool_Item, "Iron_Pickaxe.png");
            Iron_Rod = new SealImage(TextureType.Item, "Iron_Rod.png");
            Iron_Scythe = new SealImage(TextureType.Tool_Item, "Iron_Scythe.png");
            Iron_Shovel = new SealImage(TextureType.Tool_Item, "Iron_Shovel.png");
            Iron_Sword = new SealImage(TextureType.Tool_Item, "Iron_Sword.png");
            Ladder = new SealImage(TextureType.Block, "Ladder.png");
            Mossy_Cobblestone = new SealImage(TextureType.Block, "Mossy_Cobblestone.png");
            Oak_Chair = new SealImage(TextureType.Item, "Oak_Chair.png");
            Oak_Chair_Bottom = new SealImage(TextureType.Connected_Block, "Oak_Chair_Bottom.png");
            Oak_Chair_Top = new SealImage(TextureType.Connected_Block, "Oak_Chair_Top.png");
            Oak_Door_Closed_Base = new SealImage(TextureType.States_Block, "Oak_Door_Closed_Base.png");
            Oak_Door_Closed_Top = new SealImage(TextureType.States_Block, "Oak_Door_Closed_Top.png");
            Oak_Door_Item = new SealImage(TextureType.Item, "Oak_Door_Item.png");
            Oak_Door_Open_Base = new SealImage(TextureType.States_Block, "Oak_Door_Open_Base.png");
            Oak_Door_Open_Top = new SealImage(TextureType.States_Block, "Oak_Door_Open_Top.png");
            Oak_Planks = new SealImage(TextureType.Block, "Oak_Planks.png");
            Oak_Planks_StairBottomLeft = new SealImage(TextureType.Chiseled_Block, "Oak_Planks_StairBottomLeft.png");
            Oak_Planks_StairBottomRight = new SealImage(TextureType.Chiseled_Block, "Oak_Planks_StairBottomRight.png");
            Oak_Planks_StairTopLeft = new SealImage(TextureType.Chiseled_Block, "Oak_Planks_StairTopLeft.png");
            Oak_Planks_StairTopRight = new SealImage(TextureType.Chiseled_Block, "Oak_Planks_StairTopRight.png");
            Oak_Plank_BottomLeft = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_BottomLeft.png");
            Oak_Plank_BottomRight = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_BottomRight.png");
            Oak_Plank_Center = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_Center.png");
            Oak_Plank_SlabBottom = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_SlabBottom.png");
            Oak_Plank_SlabLeft = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_SlabLeft.png");
            Oak_Plank_SlabRight = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_SlabRight.png");
            Oak_Plank_SlabTop = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_SlabTop.png");
            Oak_Plank_TopLeft = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_TopLeft.png");
            Oak_Plank_TopRight = new SealImage(TextureType.Chiseled_Block, "Oak_Plank_TopRight.png");
            Oak_Sapling = new SealImage(TextureType.Block, "Oak_Sapling.png");
            Oak_Table = new SealImage(TextureType.Block, "Oak_Table.png");
            Oak_Trapdoor_Closed = new SealImage(TextureType.States_Block, "Oak_Trapdoor_Closed.png");
            Oak_Trapdoor_Open = new SealImage(TextureType.States_Block, "Oak_Trapdoor_Open.png");
            Paper = new SealImage(TextureType.Item, "Paper.png");
            Pot_Shard = new SealImage(TextureType.Item, "Pot_Shard.png");
            Rock = new SealImage(TextureType.Item, "Rock.png");
            Sand = new SealImage(TextureType.Block, "Sand.png");
            Sand_Stone = new SealImage(TextureType.Block, "Sand_Stone.png");
            Sand_Stone_BottomLeft = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_BottomLeft.png");
            Sand_Stone_BottomRight = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_BottomRight.png");
            Sand_Stone_Bricks = new SealImage(TextureType.Block, "Sand_Stone_Bricks.png");
            Sand_Stone_Center = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_Center.png");
            Sand_Stone_SlabBottom = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_SlabBottom.png");
            Sand_Stone_SlabLeft = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_SlabLeft.png");
            Sand_Stone_SlabRight = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_SlabRight.png");
            Sand_Stone_SlabTop = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_SlabTop.png");
            Sand_Stone_StairBottomLeft = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_StairBottomLeft.png");
            Sand_Stone_StairBottomRight = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_StairBottomRight.png");
            Sand_Stone_StairTopLeft = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_StairTopLeft.png");
            Sand_Stone_StairTopRight = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_StairTopRight.png");
            Sand_Stone_TopLeft = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_TopLeft.png");
            Sand_Stone_TopRight = new SealImage(TextureType.Chiseled_Block, "Sand_Stone_TopRight.png");
            Spruce_Chair = new SealImage(TextureType.Item, "Spruce_Chair.png");
            Spruce_Chair_Bottom = new SealImage(TextureType.Connected_Block, "Spruce_Chair_Bottom.png");
            Spruce_Chair_Top = new SealImage(TextureType.Connected_Block, "Spruce_Chair_Top.png");
            Spruce_Planks = new SealImage(TextureType.Block, "Spruce_Planks.png");
            Spruce_Plank_BottomLeft = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_BottomLeft.png");
            Spruce_Plank_BottomRight = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_BottomRight.png");
            Spruce_Plank_Center = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_Center.png");
            Spruce_Plank_SlabBottom = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_SlabBottom.png");
            Spruce_Plank_SlabLeft = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_SlabLeft.png");
            Spruce_Plank_SlabRight = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_SlabRight.png");
            Spruce_Plank_SlabTop = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_SlabTop.png");
            Spruce_Plank_StairBottomLeft = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_StairBottomLeft.png");
            Spruce_Plank_StairBottomRight = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_StairBottomRight.png");
            Spruce_Plank_StairTopLeft = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_StairTopLeft.png");
            Spruce_Plank_StairTopRight = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_StairTopRight.png");
            Spruce_Plank_TopLeft = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_TopLeft.png");
            Spruce_Plank_TopRight = new SealImage(TextureType.Chiseled_Block, "Spruce_Plank_TopRight.png");
            Spruce_Sapling = new SealImage(TextureType.Block, "Spruce_Sapling.png");
            Spruce_Table = new SealImage(TextureType.Block, "Spruce_Table.png");
            Spruce_Trapdoor_Closed = new SealImage(TextureType.States_Block, "Spruce_Trapdoor_Closed.png");
            Spruce_Trapdoor_Item = new SealImage(TextureType.Item, "Spruce_Trapdoor_Item.png");
            Spruce_Trapdoor_Open = new SealImage(TextureType.States_Block, "Spruce_Trapdoor_Open.png");
            Stick = new SealImage(TextureType.Item, "Stick.png");
            Stone_Axe = new SealImage(TextureType.Tool_Item, "Stone_Axe.png");
            Stone_Bricks = new SealImage(TextureType.Block, "Stone_Bricks.png");
            Stone_Pickaxe = new SealImage(TextureType.Tool_Item, "Stone_Pickaxe.png");
            Stone_Scythe = new SealImage(TextureType.Tool_Item, "Stone_Scythe.png");
            Stone_Shovel = new SealImage(TextureType.Tool_Item, "Stone_Shovel.png");
            Stone_Sword = new SealImage(TextureType.Tool_Item, "Stone_Sword.png");
            Tin_Axe = new SealImage(TextureType.Tool_Item, "Tin_Axe.png");
            Tin_Bar = new SealImage(TextureType.Item, "Tin_Bar.png");
            Tin_Hammer = new SealImage(TextureType.Tool_Item, "Tin_Hammer.png");
            Tin_Ore = new SealImage(TextureType.Block, "Tin_Ore.png");
            Tin_Pickaxe = new SealImage(TextureType.Tool_Item, "Tin_Pickaxe.png");
            Tin_Scythe = new SealImage(TextureType.Tool_Item, "Tin_Scythe.png");
            Tin_Shovel = new SealImage(TextureType.Tool_Item, "Tin_Shovel.png");
            Tin_Sword = new SealImage(TextureType.Tool_Item, "Tin_Sword.png");
            Tungsten_Bar = new SealImage(TextureType.Item, "Tungsten_Bar.png");
            Tungsten_Ore = new SealImage(TextureType.Block, "Tungsten_Ore.png");
            Wax = new SealImage(TextureType.Item, "Wax.png");
            Wood_Axe = new SealImage(TextureType.Tool_Item, "Wood_Axe.png");
            Wood_Hammer = new SealImage(TextureType.Tool_Item, "Wood_Hammer.png");
            Wood_Pickaxe = new SealImage(TextureType.Tool_Item, "Wood_Pickaxe.png");
            Wood_Scythe = new SealImage(TextureType.Tool_Item, "Wood_Scythe.png");
            Wood_Shovel = new SealImage(TextureType.Tool_Item, "Wood_Shovel.png");
            Wood_Sword = new SealImage(TextureType.Tool_Item, "Wood_Sword.png");
            Yellow_Flower = new SealImage(TextureType.Block, "Yellow_Flower.png");

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
        public static SealImage Amethyst_Ore;
        public static SealImage Anvil;
        public static SealImage Apple;
        public static SealImage Archeology_Pot;
        public static SealImage Archeology_Pot_Base;
        public static SealImage Archeology_Pot_Top;
        public static SealImage Barrel;
        public static SealImage Blue_Flower;
        public static SealImage Bone_Block;
        public static SealImage Bread;
        public static SealImage Bucket_Empty;
        public static SealImage Bucket_Water;
        public static SealImage Cactus_BottomLeft;
        public static SealImage Cactus_BottomRight;
        public static SealImage Cactus_Cross;
        public static SealImage Cactus_Fruit;
        public static SealImage Cactus_Horizontal;
        public static SealImage Cactus_Top;
        public static SealImage Cactus_TopLeft;
        public static SealImage Cactus_TopRight;
        public static SealImage Cactus_Top_Fruit;
        public static SealImage Cactus_Vertical;
        public static SealImage Candle;
        public static SealImage Coal;
        public static SealImage Copper_Bar;
        public static SealImage Copper_Ore;
        public static SealImage Croissant;
        public static SealImage Dead_Bush;
        public static SealImage Diamond;
        public static SealImage Diamond_Axe;
        public static SealImage Diamond_Hammer;
        public static SealImage Diamond_Pickaxe;
        public static SealImage Diamond_Scythe;
        public static SealImage Diamond_Shovel;
        public static SealImage Diamond_Sword;
        public static SealImage Emerald;
        public static SealImage Emerald_Ore;
        public static SealImage Flower_Pot;
        public static SealImage Fossil_Fragment;
        public static SealImage Furnace_Idle;
        public static SealImage Furnace_Running;
        public static SealImage Gold_Bar;
        public static SealImage Gold_Ore;
        public static SealImage Grass;
        public static SealImage Iron_Axe;
        public static SealImage Iron_Bar;
        public static SealImage Iron_Gates;
        public static SealImage Iron_Hammer;
        public static SealImage Iron_Pickaxe;
        public static SealImage Iron_Rod;
        public static SealImage Iron_Scythe;
        public static SealImage Iron_Shovel;
        public static SealImage Iron_Sword;
        public static SealImage Ladder;
        public static SealImage Mossy_Cobblestone;
        public static SealImage Oak_Chair;
        public static SealImage Oak_Chair_Bottom;
        public static SealImage Oak_Chair_Top;
        public static SealImage Oak_Door_Closed_Base;
        public static SealImage Oak_Door_Closed_Top;
        public static SealImage Oak_Door_Item;
        public static SealImage Oak_Door_Open_Base;
        public static SealImage Oak_Door_Open_Top;
        public static SealImage Oak_Planks;
        public static SealImage Oak_Planks_StairBottomLeft;
        public static SealImage Oak_Planks_StairBottomRight;
        public static SealImage Oak_Planks_StairTopLeft;
        public static SealImage Oak_Planks_StairTopRight;
        public static SealImage Oak_Plank_BottomLeft;
        public static SealImage Oak_Plank_BottomRight;
        public static SealImage Oak_Plank_Center;
        public static SealImage Oak_Plank_SlabBottom;
        public static SealImage Oak_Plank_SlabLeft;
        public static SealImage Oak_Plank_SlabRight;
        public static SealImage Oak_Plank_SlabTop;
        public static SealImage Oak_Plank_TopLeft;
        public static SealImage Oak_Plank_TopRight;
        public static SealImage Oak_Sapling;
        public static SealImage Oak_Table;
        public static SealImage Oak_Trapdoor_Closed;
        public static SealImage Oak_Trapdoor_Open;
        public static SealImage Paper;
        public static SealImage Pot_Shard;
        public static SealImage Rock;
        public static SealImage Sand;
        public static SealImage Sand_Stone;
        public static SealImage Sand_Stone_BottomLeft;
        public static SealImage Sand_Stone_BottomRight;
        public static SealImage Sand_Stone_Bricks;
        public static SealImage Sand_Stone_Center;
        public static SealImage Sand_Stone_SlabBottom;
        public static SealImage Sand_Stone_SlabLeft;
        public static SealImage Sand_Stone_SlabRight;
        public static SealImage Sand_Stone_SlabTop;
        public static SealImage Sand_Stone_StairBottomLeft;
        public static SealImage Sand_Stone_StairBottomRight;
        public static SealImage Sand_Stone_StairTopLeft;
        public static SealImage Sand_Stone_StairTopRight;
        public static SealImage Sand_Stone_TopLeft;
        public static SealImage Sand_Stone_TopRight;
        public static SealImage Spruce_Chair;
        public static SealImage Spruce_Chair_Bottom;
        public static SealImage Spruce_Chair_Top;
        public static SealImage Spruce_Planks;
        public static SealImage Spruce_Plank_BottomLeft;
        public static SealImage Spruce_Plank_BottomRight;
        public static SealImage Spruce_Plank_Center;
        public static SealImage Spruce_Plank_SlabBottom;
        public static SealImage Spruce_Plank_SlabLeft;
        public static SealImage Spruce_Plank_SlabRight;
        public static SealImage Spruce_Plank_SlabTop;
        public static SealImage Spruce_Plank_StairBottomLeft;
        public static SealImage Spruce_Plank_StairBottomRight;
        public static SealImage Spruce_Plank_StairTopLeft;
        public static SealImage Spruce_Plank_StairTopRight;
        public static SealImage Spruce_Plank_TopLeft;
        public static SealImage Spruce_Plank_TopRight;
        public static SealImage Spruce_Sapling;
        public static SealImage Spruce_Table;
        public static SealImage Spruce_Trapdoor_Closed;
        public static SealImage Spruce_Trapdoor_Item;
        public static SealImage Spruce_Trapdoor_Open;
        public static SealImage Stick;
        public static SealImage Stone_Axe;
        public static SealImage Stone_Bricks;
        public static SealImage Stone_Pickaxe;
        public static SealImage Stone_Scythe;
        public static SealImage Stone_Shovel;
        public static SealImage Stone_Sword;
        public static SealImage Tin_Axe;
        public static SealImage Tin_Bar;
        public static SealImage Tin_Hammer;
        public static SealImage Tin_Ore;
        public static SealImage Tin_Pickaxe;
        public static SealImage Tin_Scythe;
        public static SealImage Tin_Shovel;
        public static SealImage Tin_Sword;
        public static SealImage Tungsten_Bar;
        public static SealImage Tungsten_Ore;
        public static SealImage Wax;
        public static SealImage Wood_Axe;
        public static SealImage Wood_Hammer;
        public static SealImage Wood_Pickaxe;
        public static SealImage Wood_Scythe;
        public static SealImage Wood_Shovel;
        public static SealImage Wood_Sword;
        public static SealImage Yellow_Flower;
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


