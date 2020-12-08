using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace NerfedWitchPistol
{
	public class AdvancedAmmoRegenSynergyProcessor : MonoBehaviour
	{
		public AdvancedAmmoRegenSynergyProcessor()
		{
			this.AmmoPerSecond = 0.1f;
			this.PreventGainWhileFiring = true;
		}

		private void Awake()
		{
			this.m_gun = base.GetComponent<Gun>();
		}

		private void Update()
		{
			if (this.m_gun.CurrentOwner && this.m_gun.OwnerHasSynergy(this.RequiredSynergy) && (!this.PreventGainWhileFiring || !this.m_gun.IsFiring))
			{
				this.m_ammoCounter += BraveTime.DeltaTime * this.AmmoPerSecond;
				if (this.m_ammoCounter > 1f)
				{
					int num = Mathf.FloorToInt(this.m_ammoCounter);
					this.m_ammoCounter -= (float)num;
					this.m_gun.GainAmmo(num);
				}
			}
		}

		private void OnEnable()
		{
			if (this.m_gameTimeOnDisable > 0f)
			{
				this.m_ammoCounter += (Time.time - this.m_gameTimeOnDisable) * this.AmmoPerSecond;
				this.m_gameTimeOnDisable = 0f;
			}
		}

		private void OnDisable()
		{
			this.m_gameTimeOnDisable = Time.time;
		}

		public string RequiredSynergy;
		public float AmmoPerSecond;
		public bool PreventGainWhileFiring;
		private Gun m_gun;
		private float m_ammoCounter;
		private float m_gameTimeOnDisable;
	}
}
