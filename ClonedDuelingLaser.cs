using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace NerfedWitchPistol
{
    class ClonedDuelingLaser : GunBehaviour
    {
        public static void Init()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Mini-Hakkero", "mini_hakkero");
            Game.Items.Rename("outdated_gun_mods:mini-hakkero", "spapi:mini_hakkero");
            ClonedDuelingLaser controller = gun.gameObject.AddComponent<ClonedDuelingLaser>();
            GunExt.SetShortDescription(gun, "It's all about firepower!");
            GunExt.SetLongDescription(gun, "Made by a master smith and forged with the mythical hihi'irokane metal, this Magic Furnace takes the form of an octagonal block of wood with the eight Taoist trigrams inscribed on the front.\n\n" +
                "It allows its' wielder to channel and amplify their magic energy into a beam of destructive power.");
            GunExt.SetupSprite(gun, null, "mini_hakkero_idle2_001", 8);
            GunExt.SetAnimationFPS(gun, gun.idleAnimation, 8);
            GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(508) as Gun, true, false);
            Gun duelingLaser = PickupObjectDatabase.GetById(508) as Gun;
            gun.UsesRechargeLikeActiveItem = true;
            gun.ActiveItemStyleRechargeAmount = duelingLaser.ActiveItemStyleRechargeAmount;
            gun.reloadTime = 1.6f;
            controller.idle2Animation = GunExt.UpdateAnimation(gun, "idle2");
            controller.normalIdleAnimation = GunExt.UpdateAnimation(gun, "idle");
            gun.InfiniteAmmo = true;
            gun.gunSwitchGroup = "ChargeLaser";
            gun.barrelOffset.transform.localPosition += new Vector3(-0.55f, -0.160f, 0f);
            gun.quality = PickupObject.ItemQuality.SPECIAL;
            gun.gunClass = GunClass.PISTOL;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        public void Update()
        {
            if (this.gun.RemainingActiveCooldownAmount <= 0f || this.gun.OwnerHasSynergy("Cheat against the Impossible"))
            {
                this.gun.idleAnimation = this.normalIdleAnimation;
            }
            else
            {
                this.gun.idleAnimation = this.idle2Animation;
            }
            if(this.lastIdleAnimation != this.gun.idleAnimation)
            {
                this.gun.PlayIdleAnimation();
            }
            if (this.gun.OwnerHasSynergy("Cheat against the Impossible"))
            {
                this.gun.RemainingActiveCooldownAmount = Mathf.Max(0f, this.gun.RemainingActiveCooldownAmount - 10f * BraveTime.DeltaTime);
            }
            //ClonedDuelingLaser.UpdateSequencerAnimation(this, "idle", null, false);
            this.lastIdleAnimation = this.gun.idleAnimation;
        }

        public void LateUpdate()
        {
            tk2dBaseSprite sprite = this.gun.GetSprite();
            float num = this.gun.CurrentAngle;
            if (this.gun.CurrentOwner is PlayerController)
            {
                PlayerController playerController = this.gun.CurrentOwner as PlayerController;
                num = BraveMathCollege.Atan2Degrees(playerController.unadjustedAimPoint.XY() - this.gun.transform.parent.position.XY());
            }
            int num2 = BraveMathCollege.AngleToOctant(num + 90f);
            if (num2 == 1 || num2 == 2 || num2 == 3)
            {
                sprite.HeightOffGround = 0.075f;
            }
            else
            {
                sprite.HeightOffGround = -0.075f;
            }
            sprite.UpdateZDepth();
        }

        public static string UpdateSequencerAnimation(ClonedDuelingLaser controller, string name, tk2dSpriteCollectionData collection = null, bool returnToIdle = false)
        {
            Gun gun = controller.gun;
            collection = (collection ?? ETGMod.Databases.Items.WeaponCollection);
            string text = gun.name + "_" + name + ((gun.RemainingActiveCooldownAmount <= 0f) ? "2" : "");
            string text2 = text + "_";
            int prefixLength = text2.Length;
            List<tk2dSpriteAnimationFrame> list = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < collection.spriteDefinitions.Length; i++)
            {
                tk2dSpriteDefinition tk2dSpriteDefinition = collection.spriteDefinitions[i];
                if (tk2dSpriteDefinition.Valid && tk2dSpriteDefinition.name.StartsWithInvariant(text2))
                {
                    list.Add(new tk2dSpriteAnimationFrame
                    {
                        spriteCollection = collection,
                        spriteId = i
                    });
                }
            }
            if (list.Count == 0)
            {
                return null;
            }
            tk2dSpriteAnimationClip tk2dSpriteAnimationClip = gun.spriteAnimator.Library.GetClipByName(text);
            if (tk2dSpriteAnimationClip == null)
            {
                tk2dSpriteAnimationClip = new tk2dSpriteAnimationClip();
                tk2dSpriteAnimationClip.name = text;
                tk2dSpriteAnimationClip.fps = 15f;
                if (returnToIdle)
                {
                    tk2dSpriteAnimationClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
                }
                Array.Resize<tk2dSpriteAnimationClip>(ref gun.spriteAnimator.Library.clips, gun.spriteAnimator.Library.clips.Length + 1);
                gun.spriteAnimator.Library.clips[gun.spriteAnimator.Library.clips.Length - 1] = tk2dSpriteAnimationClip;
            }
            list.Sort((tk2dSpriteAnimationFrame x, tk2dSpriteAnimationFrame y) => int.Parse(collection.spriteDefinitions[x.spriteId].name.Substring(prefixLength)) - int.Parse(collection.spriteDefinitions[y.spriteId].name.Substring(prefixLength)));
            tk2dSpriteAnimationClip.frames = list.ToArray();
            return text;
        }

        public string lastIdleAnimation;
        public string normalIdleAnimation;
        public string idle2Animation;
    }
}
