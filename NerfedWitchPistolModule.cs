using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using Gungeon;

namespace NerfedWitchPistol
{
    public class NerfedWitchPistolModule : ETGModule
    {
        public override void Init()
        {
        }

        public override void Start()
        {
            FakePrefabHooks.Init();
            ItemBuilder.Init();
            NerfedWitchPistol.Init();
            ClonedDuelingLaser.Init();
            CustomSynergies.Add("Twelve Shots", new List<string> { "witch_pistol", "spapi:magician_pistol" }, ignoreLichEyeBullets: false);
            CustomSynergies.Add("Cheat against the Impossible", new List<string> { "staff_of_firepower", "spapi:mini_hakkero" }, ignoreLichEyeBullets: false);
            AdvancedAmmoRegenSynergyProcessor processor = Game.Items["staff_of_firepower"].gameObject.AddComponent<AdvancedAmmoRegenSynergyProcessor>();
            processor.PreventGainWhileFiring = false;
            processor.RequiredSynergy = "Cheat against the Impossible";
            processor.AmmoPerSecond = 1f;
        }

        public override void Exit()
        {
        }
    }
}