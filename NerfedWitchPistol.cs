using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace NerfedWitchPistol
{
    class NerfedWitchPistol : GunBehaviour
    {
        public static void Init()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Magician Pistol", "nerfed_witch_pistol");
            Game.Items.Rename("outdated_gun_mods:magician_pistol", "spapi:magician_pistol");
            gun.gameObject.AddComponent<NerfedWitchPistol>();
            GunExt.SetShortDescription(gun, "Fancy Schmancy");
            GunExt.SetLongDescription(gun, "Well this is just a really messy attempt at a Witch Pistol.\nThe shoddy craftsmanship makes it particularly awkward to reload, but you'll sure as hell look cool doing it!\n\n" +
                "The \"Magic\" part of this weapon is purely for cosmetic purposes, it's really just a standard magnum.");
            GunExt.AddProjectileModuleFrom(gun, "klobb", true, false);
            gun.TransformToTargetGun(PickupObjectDatabase.GetById(145) as Gun);
            GunExt.SetupSprite(gun, null, "nerfed_witch_pistol_idle_001", 16);
            Projectile projectile = Instantiate((PickupObjectDatabase.GetById(145) as Gun).DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            DontDestroyOnLoad(projectile);
            projectile.CanTransmogrify = false;
            projectile.ChanceToTransmogrify = -1f;
            gun.reloadTime = 1.6f;
            gun.StarterGunForAchievement = true;
            gun.DefaultModule.projectiles[0] = projectile;
            gun.InfiniteAmmo = true;
            gun.quality = PickupObject.ItemQuality.SPECIAL;
            gun.gunClass = GunClass.PISTOL;
            int index = 0;
            foreach (tk2dSpriteAnimationFrame frame in gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.idleAnimation).frames)
            {
                tk2dSpriteDefinition def = frame.spriteCollection.spriteDefinitions[frame.spriteId];
                if (def != null)
                {
                    RemoveOffset(def);
                    MakeOffset(def, offsets[0][index]);
                }
                index++;
            }
            index = 0;
            foreach (tk2dSpriteAnimationFrame frame in gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames)
            {
                tk2dSpriteDefinition def = frame.spriteCollection.spriteDefinitions[frame.spriteId];
                if (def != null)
                {
                    RemoveOffset(def);
                    MakeOffset(def, offsets[1][index]);
                }
                index++;
            }
            index = 0;
            foreach (tk2dSpriteAnimationFrame frame in gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).frames)
            {
                tk2dSpriteDefinition def = frame.spriteCollection.spriteDefinitions[frame.spriteId];
                if (def != null)
                {
                    RemoveOffset(def);
                    MakeOffset(def, offsets[2][index]);
                }
                index++;
            }
            ChangeReloadSpeedSynergyProcessor processor = gun.gameObject.AddComponent<ChangeReloadSpeedSynergyProcessor>();
            processor.SynergyReloadTime = 1.2f;
            processor.RequiredSynergy = "Twelve Shots";
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            AddDualWieldSynergyProcessor(gun.PickupObjectId, 145, "Twelve Shots");
        }

        public static void AddDualWieldSynergyProcessor(int id1, int id2, string synergyName)
        {
            AdvancedDualWieldSynergyProcessor dualWieldController = PickupObjectDatabase.GetById(id1).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            dualWieldController.SynergyNameToCheck = synergyName;
            dualWieldController.PartnerGunID = id2;
            AdvancedDualWieldSynergyProcessor dualWieldController2 = PickupObjectDatabase.GetById(id2).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            dualWieldController2.SynergyNameToCheck = synergyName;
            dualWieldController2.PartnerGunID = id1;
        }

        public static void RemoveOffset(tk2dSpriteDefinition def)
        {
            MakeOffset(def, -def.position0);
        }

        public static void MakeOffset(tk2dSpriteDefinition def, Vector2 offset)
        {
            float xOffset = offset.x;
            float yOffset = offset.y;
            def.position0 += new Vector3(xOffset, yOffset, 0);
            def.position1 += new Vector3(xOffset, yOffset, 0);
            def.position2 += new Vector3(xOffset, yOffset, 0);
            def.position3 += new Vector3(xOffset, yOffset, 0);
            def.boundsDataCenter += new Vector3(xOffset, yOffset, 0);
            def.boundsDataExtents += new Vector3(xOffset, yOffset, 0);
            def.untrimmedBoundsDataCenter += new Vector3(xOffset, yOffset, 0);
            def.untrimmedBoundsDataExtents += new Vector3(xOffset, yOffset, 0);
        }

        public static List<List<Vector2>> offsets = new List<List<Vector2>>
        {
            new List<Vector2>
            {
                new Vector2(0f, 0f)
            },
            new List<Vector2>
            {
                new Vector2(0f, 0f),
                new Vector2(-0.0625f, 0.0625f),
                new Vector2(0f, -0.0625f),
                new Vector2(0f, 0f),
            },
            new List<Vector2>
            {
                new Vector2(0f, -0.0625f),
                new Vector2(0.125f, 0f),
                new Vector2(0.125f, 0.0625f),
                new Vector2(0.125f, 0.125f),
                new Vector2(0.125f, 0.0625f),
                new Vector2(0.125f, 0.125f),
                new Vector2(0f, 0.0625f),
                new Vector2(0f, -0.0625f)
            }
        };
    }
}
